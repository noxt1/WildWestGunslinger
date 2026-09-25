using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    [Header("Cover Settings")]
    public float safeDistance = 2f;
    public float maxUseDistance = 20f;

    [Header("Peek Settings")]
    public float peekDistance = 1.2f;
    public float peekHeight = 1f;

    [Header("Search")]
    public float searchDistance = 3f;
    public float searchHeight = 1f;

    [Header("Occupancy")]
    public bool allowOnlyOneEnemy = true;

    private GameObject visualMarker;
    private EnemyController occupyingEnemy;
    private Collider sourceCollider;

    public void SetSourceCollider(Collider collider)
    {
        sourceCollider = collider;
    }

    public Collider GetSourceCollider()
    {
        return sourceCollider;
    }

    public Transform GetCoverPosition()
    {
        return transform;
    }

    public bool IsOccupied()
    {
        return occupyingEnemy != null;
    }

    public bool IsOccupiedByOther(
        EnemyController enemy
    )
    {
        if (!allowOnlyOneEnemy)
            return false;

        return occupyingEnemy != null &&
               occupyingEnemy != enemy;
    }

    public bool TryReserve(
        EnemyController enemy
    )
    {
        if (!allowOnlyOneEnemy)
            return true;

        if (enemy == null)
            return false;

        if (occupyingEnemy == null)
        {
            occupyingEnemy = enemy;
            return true;
        }

        return occupyingEnemy == enemy;
    }

    public void Release(
        EnemyController enemy
    )
    {
        if (occupyingEnemy == enemy)
        {
            occupyingEnemy = null;
        }
    }

    public Vector3 GetPeekPosition(
        Vector3 playerPosition,
        bool leftSide
    )
    {
        Vector3 toPlayer =
            playerPosition -
            transform.position;

        toPlayer.y = 0f;

        if (
            toPlayer.sqrMagnitude <
            0.01f
        )
        {
            toPlayer =
                Vector3.forward;
        }

        toPlayer.Normalize();

        Vector3 side =
            Vector3.Cross(
                Vector3.up,
                toPlayer
            ).normalized;

        if (!leftSide)
        {
            side = -side;
        }

        Vector3 peekPosition =
            transform.position +
            side *
            peekDistance;

        peekPosition.y +=
            peekHeight;

        return peekPosition;
    }

    public Vector3 GetSearchPosition(
        Vector3 investigationPosition,
        int variation
    )
    {
        Vector3 direction =
            transform.position -
            investigationPosition;

        direction.y = 0f;

        if (
            direction.sqrMagnitude <
            0.01f
        )
        {
            direction =
                transform.forward;
        }

        direction.Normalize();

        Vector3 side =
            Vector3.Cross(
                Vector3.up,
                direction
            ).normalized;

        Vector3 position;

        if ((variation % 2) == 0)
        {
            position =
                transform.position +
                direction *
                searchDistance;
        }
        else
        {
            position =
                transform.position +
                side *
                searchDistance;
        }

        position.y +=
            searchHeight;

        return position;
    }

    public bool IsValidSearchPoint(
        Vector3 enemyPosition,
        Vector3 investigationPosition,
        float maxDistance
    )
    {
        Vector3 offset =
            transform.position -
            enemyPosition;

        offset.y = 0f;

        if (
            offset.magnitude >
            maxDistance
        )
        {
            return false;
        }

        Vector3 fromInvestigation =
            transform.position -
            investigationPosition;

        fromInvestigation.y = 0f;

        if (
            fromInvestigation.sqrMagnitude <
            0.25f
        )
        {
            return false;
        }

        return true;
    }

    public bool IsValidCover(
        Vector3 enemyPosition,
        Vector3 playerPosition
    )
    {
        Vector3 directionToPlayer =
            playerPosition -
            transform.position;

        float distanceToPlayer =
            directionToPlayer.magnitude;

        if (
            distanceToPlayer <=
            0.1f
        )
        {
            return false;
        }

        if (
            distanceToPlayer >
            maxUseDistance
        )
        {
            return false;
        }

        Vector3 rayStart =
            transform.position +
            Vector3.up *
            0.8f;

        Vector3 target =
            playerPosition +
            Vector3.up *
            0.8f;

        Vector3 rayDirection =
            target -
            rayStart;

        float rayDistance =
            rayDirection.magnitude;

        if (
            rayDistance <=
            0.1f
        )
        {
            return false;
        }

        rayDirection.Normalize();

        RaycastHit[] hits =
            Physics.RaycastAll(
                rayStart,
                rayDirection,
                rayDistance
            );

        System.Array.Sort(
            hits,
            (a, b) =>
                a.distance.CompareTo(
                    b.distance
                )
        );

        foreach (
            RaycastHit hit
            in hits
        )
        {
            if (hit.collider == null)
                continue;

            Transform hitTransform =
                hit.collider.transform;

            if (
                hitTransform == transform
            )
            {
                continue;
            }

            if (
                hitTransform.GetComponentInParent<EnemyController>() != null
            )
            {
                continue;
            }

            if (
                hitTransform.CompareTag(
                    "Player"
                )
            )
            {
                continue;
            }

            if (
                hitTransform.GetComponentInParent<PlayerHealth>() != null
            )
            {
                continue;
            }

            return true;
        }

        return false;
    }

    private void Start()
    {
        CreateVisualMarker();
    }

    private void CreateVisualMarker()
    {
        if (visualMarker != null)
            return;

        visualMarker =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere
            );

        visualMarker.name =
            "CoverPoint_Marker";

        visualMarker.transform.SetParent(
            transform
        );

        visualMarker.transform.localPosition =
            Vector3.zero;

        visualMarker.transform.localRotation =
            Quaternion.identity;

        visualMarker.transform.localScale =
            Vector3.one *
            0.45f;

        Collider markerCollider =
            visualMarker.GetComponent<Collider>();

        if (markerCollider != null)
        {
            Destroy(
                markerCollider
            );
        }

        Renderer renderer =
            visualMarker.GetComponent<Renderer>();

        if (renderer != null)
        {
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
                    Color.green;

                renderer.material =
                    material;
            }
        }
    }

    private void OnDisable()
    {
        occupyingEnemy = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color =
            IsOccupied()
                ? Color.red
                : Color.green;

        Gizmos.DrawSphere(
            transform.position,
            0.25f
        );

        Gizmos.color =
            Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            safeDistance
        );

        Gizmos.color =
            Color.blue;

        Gizmos.DrawWireSphere(
            transform.position,
            maxUseDistance
        );

        Gizmos.color =
            Color.white;

        Gizmos.DrawWireSphere(
            transform.position,
            searchDistance
        );

        Vector3 leftPeek =
            GetPeekPosition(
                transform.position +
                Vector3.forward * 5f,
                true
            );

        Vector3 rightPeek =
            GetPeekPosition(
                transform.position +
                Vector3.forward * 5f,
                false
            );

        Gizmos.color =
            Color.cyan;

        Gizmos.DrawSphere(
            leftPeek,
            0.12f
        );

        Gizmos.color =
            Color.magenta;

        Gizmos.DrawSphere(
            rightPeek,
            0.12f
        );
    }
}