using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float visualSize = 0.18f;
    [SerializeField] private Color bulletColor = Color.yellow;

    private float damage;
    private Vector3 direction;
    private bool initialized;

    private void Awake()
    {
        SetupVisual();

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    public void Initialize(Vector3 shootDirection, float bulletDamage)
    {
        direction = shootDirection.normalized;
        damage = bulletDamage;
        initialized = direction.sqrMagnitude > 0.001f;

        if (!initialized)
            return;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!initialized)
            return;

        transform.position +=
            direction *
            speed *
            Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        // Игнорируем весь объект игрока,
        // включая дочерние Collider'ы вроде Capsule.
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null)
            return;

        EnemyHealth enemy =
            other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(
                Mathf.RoundToInt(damage)
            );

            Destroy(gameObject);
            return;
        }

        DestructibleObject destructible =
            other.GetComponentInParent<DestructibleObject>();

        if (destructible != null)
        {
            destructible.TakeDamage(
                Mathf.RoundToInt(damage)
            );

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null)
            return;

        Collider other =
            collision.collider;

        if (other == null)
            return;

        // Игнорируем весь объект игрока,
        // включая дочерние Collider'ы.
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null)
            return;

        EnemyHealth enemy =
            other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(
                Mathf.RoundToInt(damage)
            );

            Destroy(gameObject);
            return;
        }

        DestructibleObject destructible =
            other.GetComponentInParent<DestructibleObject>();

        if (destructible != null)
        {
            destructible.TakeDamage(
                Mathf.RoundToInt(damage)
            );

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void SetupVisual()
    {
        Renderer renderer =
            GetComponent<Renderer>();

        if (renderer == null)
        {
            Transform existingVisual =
                transform.Find("BulletVisual");

            if (existingVisual != null)
            {
                renderer =
                    existingVisual.GetComponent<Renderer>();
            }
        }

        if (renderer == null)
        {
            GameObject visual =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere
                );

            visual.name =
                "BulletVisual";

            visual.transform.SetParent(transform);

            visual.transform.localPosition =
                Vector3.zero;

            visual.transform.localRotation =
                Quaternion.identity;

            visual.transform.localScale =
                Vector3.one * visualSize;

            Collider visualCollider =
                visual.GetComponent<Collider>();

            if (visualCollider != null)
            {
                Destroy(visualCollider);
            }

            renderer =
                visual.GetComponent<Renderer>();
        }

        if (renderer == null)
            return;

        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Unlit"
            );

        if (shader == null)
        {
            shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );
        }

        if (shader == null)
        {
            shader =
                Shader.Find("Standard");
        }

        if (shader != null)
        {
            Material material =
                new Material(shader);

            material.color =
                bulletColor;

            renderer.material =
                material;
        }
    }
}