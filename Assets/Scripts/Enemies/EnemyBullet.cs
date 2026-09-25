using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
[Header("Bullet")]
[SerializeField] private float speed = 18f;
[SerializeField] private float lifetime = 3f;
[SerializeField] private int damage = 5;
[SerializeField] private float visualSize = 0.22f;
[SerializeField] private Color bulletColor = Color.red;


[Header("Collision")]
[SerializeField] private float minimumTraceRadius = 0.08f;
[SerializeField] private int maximumSubSteps = 4;
[SerializeField] private float minimumStepDistance = 0.05f;

private Vector3 direction;
private bool initialized;

private void Awake()
{
    SetupVisual();
}

public void Initialize(
    Vector3 shootDirection,
    int bulletDamage
)
{
    direction =
        shootDirection.normalized;

    damage =
        bulletDamage;

    initialized =
        direction.sqrMagnitude >
        0.001f;

    if (initialized)
    {
        transform.rotation =
            Quaternion.LookRotation(
                direction
            );
    }
}

private void Start()
{
    Destroy(
        gameObject,
        lifetime
    );
}

private void Update()
{
    if (!initialized)
        return;

    float frameDistance =
        speed *
        Time.deltaTime;

    if (frameDistance <= 0.0001f)
        return;

    int subSteps =
        Mathf.CeilToInt(
            frameDistance /
            minimumStepDistance
        );

    subSteps =
        Mathf.Clamp(
            subSteps,
            1,
            maximumSubSteps
        );

    float stepDistance =
        frameDistance /
        subSteps;

    float radius =
        Mathf.Max(
            minimumTraceRadius,
            visualSize * 0.5f
        );

    for (
        int i = 0;
        i < subSteps;
        i++
    )
    {
        if (
            TraceStep(
                stepDistance,
                radius
            )
        )
        {
            return;
        }

        transform.position +=
            direction *
            stepDistance;
    }
}

private bool TraceStep(
    float distance,
    float radius
)
{
    RaycastHit[] hits =
        CombatPhysics.SphereCastAll(
            transform.position,
            direction,
            distance,
            radius
        );

    foreach (RaycastHit hit in hits)
    {
        Collider hitCollider =
            hit.collider;

        if (hitCollider == null)
            continue;

        PlayerHealth playerHealth =
            hitCollider.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(
                damage
            );

            Destroy(
                gameObject
            );

            return true;
        }

        EnemyController enemyController =
            hitCollider.GetComponentInParent<EnemyController>();

        if (enemyController != null)
            continue;

        EnemyHealth enemyHealth =
            hitCollider.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
            continue;

        DestructibleObject destructible =
            hitCollider.GetComponentInParent<DestructibleObject>();

        if (destructible != null)
        {
            destructible.TakeDamage(damage);
        }

        transform.position =
            hit.point;

        Destroy(
            gameObject
        );

        return true;
    }

    return false;
}

private void OnTriggerEnter(
    Collider other
)
{
    PlayerHealth playerHealth =
        other.GetComponentInParent<PlayerHealth>();

    if (playerHealth != null)
    {
        playerHealth.TakeDamage(
            damage
        );

        Destroy(
            gameObject
        );

        return;
    }

    if (
        other.GetComponentInParent<EnemyController>() != null
    )
    {
        return;
    }

    if (
        other.GetComponentInParent<EnemyHealth>() != null
    )
    {
        return;
    }

    DestructibleObject destructible =
        other.GetComponentInParent<DestructibleObject>();

    if (destructible != null)
    {
        destructible.TakeDamage(damage);
    }

    Destroy(
        gameObject
    );
}

private void OnCollisionEnter(
    Collision collision
)
{
    PlayerHealth playerHealth =
        collision.collider.GetComponentInParent<PlayerHealth>();

    if (playerHealth != null)
    {
        playerHealth.TakeDamage(
            damage
        );

        Destroy(
            gameObject
        );

        return;
    }

    if (
        collision.collider.GetComponentInParent<EnemyController>() != null
    )
    {
        return;
    }

    if (
        collision.collider.GetComponentInParent<EnemyHealth>() != null
    )
    {
        return;
    }

    DestructibleObject destructible =
        collision.collider.GetComponentInParent<DestructibleObject>();

    if (destructible != null)
    {
        destructible.TakeDamage(damage);
    }

    Destroy(
        gameObject
    );
}

private void SetupVisual()
{
    Renderer renderer =
        GetComponent<Renderer>();

    if (renderer == null)
    {
        Transform existingVisual =
            transform.Find(
                "EnemyBulletVisual"
            );

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
            "EnemyBulletVisual";

        visual.transform.SetParent(
            transform
        );

        visual.transform.localPosition =
            Vector3.zero;

        visual.transform.localRotation =
            Quaternion.identity;

        visual.transform.localScale =
            Vector3.one *
            visualSize;

        Collider visualCollider =
            visual.GetComponent<Collider>();

        if (visualCollider != null)
        {
            Destroy(
                visualCollider
            );
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
            Shader.Find(
                "Standard"
            );
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
