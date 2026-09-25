using System;
using UnityEngine;

public static class CombatPhysics
{
public static bool TryGetClearPlayerTarget(
Transform attacker,
Transform player,
Vector3 origin,
float playerHeight,
float sphereRadius,
out Vector3 clearTarget
)
{
clearTarget = Vector3.zero;


    if (player == null)
        return false;

    Vector3 basePosition =
        player.position;

    Vector3[] targets =
    {
        basePosition + Vector3.up * 0.1f,
        basePosition + Vector3.up * 0.35f,
        basePosition + Vector3.up * 0.8f,
        basePosition + Vector3.up * playerHeight,
        basePosition + Vector3.up * 1.5f,
        basePosition + Vector3.up * 1.8f
    };

    foreach (Vector3 target in targets)
    {
        if (TryClearTrace(
            attacker,
            player,
            origin,
            target,
            sphereRadius
        ))
        {
            clearTarget = target;
            return true;
        }
    }

    return false;
}

public static bool TryClearTrace(
    Transform attacker,
    Transform player,
    Vector3 origin,
    Vector3 target,
    float sphereRadius
)
{
    Vector3 direction =
        target - origin;

    float distance =
        direction.magnitude;

    if (distance <= 0.001f)
        return false;

    direction.Normalize();

    RaycastHit[] hits =
        Physics.SphereCastAll(
            origin,
            Mathf.Max(0.01f, sphereRadius),
            direction,
            distance,
            ~0,
            QueryTriggerInteraction.Ignore
        );

    if (hits == null || hits.Length == 0)
        return false;

    Array.Sort(
        hits,
        (a, b) =>
            a.distance.CompareTo(
                b.distance
            )
    );

    foreach (RaycastHit hit in hits)
    {
        if (hit.collider == null)
            continue;

        Transform hitTransform =
            hit.collider.transform;

        if (attacker != null)
        {
            if (
                hitTransform == attacker ||
                hitTransform.IsChildOf(attacker)
            )
            {
                continue;
            }
        }

        EnemyController enemy =
            hitTransform.GetComponentInParent<EnemyController>();

        if (enemy != null)
            continue;

        PlayerHealth playerHealth =
            hitTransform.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            if (
                player != null &&
                (
                    hitTransform == player ||
                    hitTransform.IsChildOf(player)
                )
            )
            {
                return true;
            }

            return false;
        }

        if (
            player != null &&
            (
                hitTransform == player ||
                hitTransform.IsChildOf(player)
            )
        )
        {
            return true;
        }

        return false;
    }

    return false;
}

public static RaycastHit[] SphereCastAll(
    Vector3 origin,
    Vector3 direction,
    float distance,
    float radius
)
{
    if (direction.sqrMagnitude < 0.000001f)
        return Array.Empty<RaycastHit>();

    direction.Normalize();

    RaycastHit[] hits =
        Physics.SphereCastAll(
            origin,
            Mathf.Max(0.01f, radius),
            direction,
            distance,
            ~0,
            QueryTriggerInteraction.Ignore
        );

    if (hits == null || hits.Length == 0)
        return Array.Empty<RaycastHit>();

    Array.Sort(
        hits,
        (a, b) =>
            a.distance.CompareTo(
                b.distance
            )
    );

    return hits;
}


}
