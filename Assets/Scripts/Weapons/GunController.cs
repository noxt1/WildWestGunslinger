using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Transform weaponPoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 0.4f;
    [SerializeField] private float attackRange = 15f;

    [Header("Damage")]
    [SerializeField] private float damage = 200f;

    [Header("Target")]
    [SerializeField] private float targetHeight = 1.1f;
    [SerializeField] private float targetRadius = 0.8f;
    [SerializeField] private float targetLineWidth = 0.06f;

    [Header("Aim")]
    [SerializeField] private bool rotateWeaponToTarget = true;

    private float nextFireTime;
    private bool firing;

    private EnemyHealth currentTarget;

    private PlayerController playerController;

    private GameObject targetIndicator;
    private LineRenderer targetLine;
    private Material targetIndicatorMaterial;

    private void Awake()
    {
        damage =
            Mathf.Max(
                damage,
                200f
            );

        playerController =
            GetComponentInParent<
                PlayerController
            >();

        if (playerController == null)
        {
            playerController =
                FindFirstObjectByType<
                    PlayerController
                >();
        }
    }

    private void Update()
    {
        currentTarget =
            FindNearestEnemy();

        UpdatePlayerAim();

        UpdateTargetIndicator();

        if (!firing)
            return;

        if (weaponPoint == null)
            return;

        if (
            Time.time <
            nextFireTime
        )
        {
            return;
        }

        Vector3 direction =
            GetFireDirection();

        if (
            direction.sqrMagnitude <
            0.0001f
        )
        {
            return;
        }

        Shoot(
            direction.normalized
        );

        nextFireTime =
            Time.time +
            fireRate;
    }

    private void LateUpdate()
    {
        if (
            !rotateWeaponToTarget ||
            weaponPoint == null ||
            currentTarget == null
        )
        {
            return;
        }

        Vector3 targetPoint =
            GetTargetPoint(
                currentTarget
            );

        Vector3 direction =
            targetPoint -
            weaponPoint.position;

        direction.y = 
            targetPoint.y -
            weaponPoint.position.y;

        if (
            direction.sqrMagnitude <
            0.0001f
        )
        {
            return;
        }

        /*
         * Оружие получает фактическое мировое
         * направление на цель уже после
         * поворота PlayerController.
         */
        weaponPoint.rotation =
            Quaternion.LookRotation(
                direction.normalized,
                Vector3.up
            );
    }

    private void UpdatePlayerAim()
    {
        if (playerController == null)
            return;

        if (currentTarget == null)
        {
            playerController
                .ClearAimDirection();

            return;
        }

        Vector3 targetPoint =
            GetTargetPoint(
                currentTarget
            );

        Vector3 direction =
            targetPoint -
            transform.position;

        direction.y = 0f;

        if (
            direction.sqrMagnitude <
            0.0001f
        )
        {
            return;
        }

        /*
         * Пока враг захвачен,
         * игрок смотрит именно на него.
         */
        playerController
            .SetAimDirection(
                direction.normalized
            );
    }

    private Vector3 GetFireDirection()
    {
        /*
         * ПЕРВЫЙ ПРИОРИТЕТ:
         * текущая захваченная цель.
         */
        if (
            currentTarget != null &&
            weaponPoint != null
        )
        {
            Vector3 targetPoint =
                GetTargetPoint(
                    currentTarget
                );

            Vector3 direction =
                targetPoint -
                weaponPoint.position;

            if (
                direction.sqrMagnitude >
                0.0001f
            )
            {
                return direction.normalized;
            }
        }

        /*
         * Если цели нет,
         * стреляем в направлении оружия.
         */
        if (weaponPoint != null)
        {
            Vector3 forward =
                weaponPoint.forward;

            if (
                forward.sqrMagnitude >
                0.0001f
            )
            {
                return forward.normalized;
            }
        }

        return Vector3.zero;
    }

    private EnemyHealth FindNearestEnemy()
    {
        EnemyHealth[] enemies =
            FindObjectsByType<
                EnemyHealth
            >(
                FindObjectsSortMode.None
            );

        EnemyHealth nearest =
            null;

        float nearestDistanceSqr =
            attackRange *
            attackRange;

        foreach (
            EnemyHealth enemy
            in enemies
        )
        {
            if (enemy == null)
                continue;

            Vector3 offset =
                enemy.transform.position -
                transform.position;

            offset.y = 0f;

            float distanceSqr =
                offset.sqrMagnitude;

            if (
                distanceSqr <=
                nearestDistanceSqr
            )
            {
                nearestDistanceSqr =
                    distanceSqr;

                nearest =
                    enemy;
            }
        }

        return nearest;
    }

    private Vector3 GetTargetPoint(
        EnemyHealth target)
    {
        if (target == null)
        {
            return transform.position;
        }

        Collider targetCollider =
            target.GetComponentInChildren<
                Collider
            >();

        if (targetCollider != null)
        {
            return targetCollider.bounds.center;
        }

        return target.transform.position +
               Vector3.up *
               targetHeight;
    }

    private void Shoot(
        Vector3 direction)
    {
        if (bulletPrefab == null)
            return;

        if (weaponPoint == null)
            return;

        Vector3 spawnPosition =
            weaponPoint.position +
            direction *
            0.6f;

        GameObject bulletObject =
            Instantiate(
                bulletPrefab,
                spawnPosition,
                Quaternion.LookRotation(
                    direction
                )
            );

        if (bulletObject == null)
            return;

        Bullet bullet =
            bulletObject.GetComponent<
                Bullet
            >();

        if (bullet == null)
            return;

        bullet.Initialize(
            direction,
            damage
        );

        if (
            NoiseSystem.Instance != null
        )
        {
            NoiseSystem.EmitGunshot(
                weaponPoint.position
            );
        }
    }

    public void FireButtonDown()
    {
        firing =
            true;

        if (
            Time.time <
            nextFireTime
        )
        {
            return;
        }

        if (weaponPoint == null)
            return;

        Vector3 direction =
            GetFireDirection();

        if (
            direction.sqrMagnitude <
            0.0001f
        )
        {
            return;
        }

        Shoot(
            direction.normalized
        );

        nextFireTime =
            Time.time +
            fireRate;
    }

    public void FireButtonUp()
    {
        firing =
            false;
    }

    public void SetFiring(
        bool value)
    {
        firing =
            value;
    }

    public void IncreaseDamage(
        float multiplier)
    {
        if (multiplier <= 0f)
            return;

        damage *=
            multiplier;
    }

    public void IncreaseFireRate(
        float multiplier)
    {
        if (multiplier <= 0f)
            return;

        fireRate /=
            multiplier;
    }

    public void IncreaseAttackRange(
        float amount)
    {
        if (amount <= 0f)
            return;

        attackRange +=
            amount;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    private void UpdateTargetIndicator()
    {
        if (currentTarget == null)
        {
            HideTargetIndicator();
            return;
        }

        if (targetIndicator == null)
        {
            CreateTargetIndicator();
        }

        if (
            targetIndicator == null ||
            targetLine == null
        )
        {
            return;
        }

        if (!targetIndicator.activeSelf)
        {
            targetIndicator.SetActive(
                true
            );
        }

        Vector3 center =
            GetTargetPoint(
                currentTarget
            );

        center.y += 0.04f;

        const int pointCount = 32;

        for (
            int i = 0;
            i < pointCount;
            i++
        )
        {
            float angle =
                (float)i /
                pointCount *
                Mathf.PI *
                2f;

            Vector3 point =
                center +
                new Vector3(
                    Mathf.Cos(angle) *
                    targetRadius,

                    0f,

                    Mathf.Sin(angle) *
                    targetRadius
                );

            targetLine.SetPosition(
                i,
                point
            );
        }
    }

    private void CreateTargetIndicator()
    {
        targetIndicator =
            new GameObject(
                "TargetIndicator"
            );

        targetLine =
            targetIndicator.AddComponent<
                LineRenderer
            >();

        targetLine.useWorldSpace =
            true;

        targetLine.loop =
            true;

        targetLine.positionCount =
            32;

        targetLine.startWidth =
            targetLineWidth;

        targetLine.endWidth =
            targetLineWidth;

        targetLine.shadowCastingMode =
            UnityEngine.Rendering
                .ShadowCastingMode.Off;

        targetLine.receiveShadows =
            false;

        targetLine.numCapVertices =
            4;

        targetLine.numCornerVertices =
            4;

        Shader shader =
            Shader.Find(
                "Sprites/Default"
            );

        if (shader == null)
        {
            shader =
                Shader.Find(
                    "Universal Render Pipeline/Unlit"
                );
        }

        if (shader != null)
        {
            targetIndicatorMaterial =
                new Material(
                    shader
                );

            targetIndicatorMaterial.color =
                new Color(
                    1f,
                    0.15f,
                    0.05f,
                    1f
                );

            targetLine.material =
                targetIndicatorMaterial;
        }
    }

    private void HideTargetIndicator()
    {
        if (targetIndicator != null)
        {
            targetIndicator.SetActive(
                false
            );
        }
    }

    private void OnDestroy()
    {
        if (targetIndicator != null)
        {
            Destroy(
                targetIndicator
            );
        }

        if (
            targetIndicatorMaterial !=
            null
        )
        {
            Destroy(
                targetIndicatorMaterial
            );
        }
    }
}