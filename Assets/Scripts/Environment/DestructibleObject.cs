using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Stage 8 — минимальное расширяемое ядро разрушаемости (без второй параллельной системы).
/// Существует только этот скрипт; специфики (забор, бочка, ящик) задаются данными в Inspector,
/// а не новыми классами. Совместим с Bullet/EnemyBullet через TakeDamage(int).
/// Стадии: INTACT -> DAMAGED -> DESTROYED. Частичное контролируемое разрушение,
/// пригодно для Android (без сотен осколков, без Update-тика).
/// </summary>
public class DestructibleObject : MonoBehaviour
{
    public enum DestructionStage
    {
        Intact = 0,
        Damaged = 1,
        Destroyed = 2
    }

    [Header("Health")]
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int currentHealth;

    [Header("State Groups (внутри prefab, без переделки FBX)")]
    [SerializeField] private GameObject stateIntactRoot;
    [SerializeField] private GameObject stateDamagedRoot;
    [SerializeField] private GameObject stateDestroyedRoot;

    [Header("Partial destruction mapping")]
    [Tooltip("Объекты INTACT, которые гаснут при DAMAGED (напр. Rail_Top, Plank_03, Plank_05)")]
    [SerializeField] private GameObject[] hideOnDamaged;
    [Tooltip("Объекты DAMAGED, которые загораются при DAMAGED (напр. Rail_Top_Broken, Plank_03_Broken)")]
    [SerializeField] private GameObject[] showOnDamaged;
    [Tooltip("Дополнительно гаснет при DESTROYED (напр. оставшиеся целые доски)")]
    [SerializeField] private GameObject[] hideOnDestroyed;
    [Tooltip("Загорается при DESTROYED (напр. Debris_01/02, оставшиеся broken)")]
    [SerializeField] private GameObject[] showOnDestroyed;

    [Header("Cover integration (существующий CoverPoint/CoverSystem, без ломки)")]
    [Tooltip("CoverPoint-маркеры забора. При DESTROYED выключаются, AI перестаёт идти в разрушенное укрытие.")]
    [SerializeField] private CoverPoint[] coverPoints;
    [SerializeField] private bool disableCoverOnDestroyed = true;
    [SerializeField] private bool keepCoverOnDamaged = true;

    [Header("Debris physics (контролируемая, max 2-3 тела)")]
    [SerializeField] private bool enableDebrisPhysics = true;
    [SerializeField] private Rigidbody[] debrisBodies;
    [SerializeField] private float debrisImpulse = 2.5f;
    [SerializeField] private float debrisTorque = 1.5f;

    [Header("Collider behaviour")]
    [SerializeField] private Collider mainCollider;
    [SerializeField] private bool disableColliderOnDestroyed = false;
    [SerializeField] private bool shrinkColliderOnDamaged = false;
    [SerializeField] private Vector3 damagedColliderSize = new Vector3(3.2f, 0.9f, 0.5f);

    [Header("Events (feedback подключается снаружи, частиц по умолчанию нет)")]
    public UnityEvent onDamaged;
    public UnityEvent onDestroyed;

    public DestructionStage Stage { get; private set; } = DestructionStage.Intact;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDestroyed => Stage == DestructionStage.Destroyed;

    private Vector3 intactColliderSize;

    private void Awake()
    {
        if (currentHealth <= 0 || currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (mainCollider == null)
        {
            mainCollider = GetComponent<Collider>();
        }

        if (mainCollider is BoxCollider box)
        {
            intactColliderSize = box.size;
        }

        if (coverPoints == null || coverPoints.Length == 0)
        {
            coverPoints = GetComponentsInChildren<CoverPoint>(true);
        }

        ApplyStageVisual(DestructionStage.Intact, true);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || Stage == DestructionStage.Destroyed)
        {
            return;
        }

        // Lazy-init: в Edit-mode Awake ещё не выполнялся (currentHealth=0 при живом Intact).
        if (currentHealth <= 0 && Stage == DestructionStage.Intact && currentHealth < maxHealth)
        {
            // Если это свежий объект без урона — стартуем с полного HP.
            // (После реального обнуления Stage уже был бы Destroyed и мы бы вышли выше.)
            currentHealth = maxHealth;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if (currentHealth <= 0)
        {
            SetStage(DestructionStage.Destroyed);
            return;
        }

        // Порог DAMAGED: половина HP. Одна пуля игрока (~25-35) не всегда ломает сразу,
        // две — гарантированно переводят в Damaged/Destroyed. Контролируемо.
        float half = maxHealth * 0.5f;
        if (Stage == DestructionStage.Intact && currentHealth <= half)
        {
            SetStage(DestructionStage.Damaged);
        }
    }

    [ContextMenu("Test Damage 25")]
    private void TestDamage25()
    {
        TakeDamage(25);
    }

    [ContextMenu("Reset Intact")]
    public void ResetIntact()
    {
        currentHealth = maxHealth;
        SetStage(DestructionStage.Intact);
    }

    private void SetStage(DestructionStage next)
    {
        if (Stage == next)
        {
            return;
        }

        Stage = next;
        ApplyStageVisual(next, false);

        if (next == DestructionStage.Damaged)
        {
            onDamaged?.Invoke();
        }
        else if (next == DestructionStage.Destroyed)
        {
            onDestroyed?.Invoke();
        }
    }

    private void ApplyStageVisual(DestructionStage stage, bool force)
    {
        // Группы-контейнеры: видимость конкретных мешей решают массивы ниже.
        // Контейнеры всегда активны как носители (кроме отключения целых групп
        // при необходимости извне); защита от z-fighting — за счёт массивов:
        // overlapping broken/intact из FBX никогда не активны одновременно.
        SetGroupActive(stateIntactRoot, true);
        SetGroupActive(stateDamagedRoot, true);
        SetGroupActive(stateDestroyedRoot, true);

        // Тонкая логика: State_Damaged root держим активным как контейнер,
        // но видимость конкретных broken-объектов решаем массивами.
        if (stage == DestructionStage.Intact)
        {
            SetObjectsActive(hideOnDamaged, true);
            SetObjectsActive(showOnDamaged, false);
            SetObjectsActive(hideOnDestroyed, true);
            SetObjectsActive(showOnDestroyed, false);
            SetCoverActive(true);
            RestoreCollider();
            ParkDebrisBodies();
        }
        else if (stage == DestructionStage.Damaged)
        {
            SetObjectsActive(hideOnDamaged, false);
            SetObjectsActive(showOnDamaged, true);
            // hideOnDestroyed пока живы (оставшееся укрытие продолжает существовать)
            SetObjectsActive(hideOnDestroyed, true);
            SetObjectsActive(showOnDestroyed, false);
            SetCoverActive(keepCoverOnDamaged);
            ApplyDamagedCollider();
        }
        else
        {
            // DESTROYED: повреждённое тоже гаснет частично, debris загорается.
            // hideOnDamaged уже выключены; hideOnDestroyed теперь гаснут.
            SetObjectsActive(hideOnDamaged, false);
            SetObjectsActive(showOnDamaged, true);
            SetObjectsActive(hideOnDestroyed, false);
            SetObjectsActive(showOnDestroyed, true);
            SetCoverActive(!disableCoverOnDestroyed ? true : false);
            ApplyDestroyedCollider();
            EnableDebrisPhysics();
        }
    }

    private void SetGroupActive(GameObject group, bool active)
    {
        if (group != null && group.activeSelf != active)
        {
            group.SetActive(active);
        }
    }

    private void SetObjectsActive(GameObject[] objects, bool active)
    {
        if (objects == null)
        {
            return;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null && objects[i].activeSelf != active)
            {
                objects[i].SetActive(active);
            }
        }
    }

    private void SetCoverActive(bool active)
    {
        if (coverPoints == null)
        {
            return;
        }

        for (int i = 0; i < coverPoints.Length; i++)
        {
            if (coverPoints[i] == null)
            {
                continue;
            }

            if (coverPoints[i].gameObject.activeSelf != active)
            {
                coverPoints[i].gameObject.SetActive(active);
            }

            // Освобождаем занятость, чтобы враги не держали ссылку на мёртвую точку.
            if (!active)
            {
                coverPoints[i].enabled = false;
            }
            else if (!coverPoints[i].enabled)
            {
                coverPoints[i].enabled = true;
            }
        }
    }

    private void RestoreCollider()
    {
        if (mainCollider == null)
        {
            return;
        }

        mainCollider.enabled = true;

        if (shrinkColliderOnDamaged && mainCollider is BoxCollider box && intactColliderSize != Vector3.zero)
        {
            box.size = intactColliderSize;
        }
    }

    private void ApplyDamagedCollider()
    {
        if (mainCollider == null)
        {
            return;
        }

        mainCollider.enabled = true;

        if (shrinkColliderOnDamaged && mainCollider is BoxCollider box)
        {
            box.size = damagedColliderSize;
        }
    }

    private void ApplyDestroyedCollider()
    {
        if (mainCollider == null)
        {
            return;
        }

        // По умолчанию укрытие-остов остаётся (posts + bottom rail) — коллайдер жив,
        // чтобы остатки продолжали блокировать пули/движение. Выключение — только по флагу.
        mainCollider.enabled = !disableColliderOnDestroyed;
    }

    private void ParkDebrisBodies()
    {
        if (debrisBodies == null)
        {
            return;
        }

        for (int i = 0; i < debrisBodies.Length; i++)
        {
            Rigidbody rb = debrisBodies[i];
            if (rb == null)
            {
                continue;
            }

            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            rb.isKinematic = true;
            rb.useGravity = false;

            Collider debrisCollider = rb.GetComponent<Collider>();
            if (debrisCollider != null && debrisCollider.enabled)
            {
                debrisCollider.enabled = false;
            }
        }
    }

    private void EnableDebrisPhysics()
    {
        if (!enableDebrisPhysics || debrisBodies == null)
        {
            return;
        }

        for (int i = 0; i < debrisBodies.Length; i++)
        {
            Rigidbody rb = debrisBodies[i];
            if (rb == null)
            {
                continue;
            }

            if (!rb.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Debris-коллайдеры в prefab выключены (не мешают INTACT/DAMAGED);
            // включаем только в момент разрушения.
            Collider debrisCollider = rb.GetComponent<Collider>();
            if (debrisCollider != null && !debrisCollider.enabled)
            {
                debrisCollider.enabled = true;
            }

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearDamping = 1.2f;
            rb.angularDamping = 1.5f;

            Vector3 impulse = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.5f, 1.2f),
                Random.Range(-1f, 1f)
            ).normalized * debrisImpulse;

            rb.AddForce(impulse, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * debrisTorque, ForceMode.Impulse);
        }
    }
}
