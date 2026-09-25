using System.Collections.Generic;
using UnityEngine;

public class EnemyTacticalEnvironmentScanner : MonoBehaviour
{
    public enum RouteSide
    {
        None,
        Left,
        Right
    }

    [System.Serializable]
    public struct ObstacleInfo
    {
        public Collider collider;
        public Vector3 point;
        public Vector3 normal;
        public Bounds bounds;
        public float distance;
        public RouteSide preferredSide;
        public Vector3 leftCorner;
        public Vector3 rightCorner;
    }

    [System.Serializable]
    public struct RouteCandidate
    {
        public Vector3 position;
        public Vector3 direction;
        public float score;
        public RouteSide side;
        public bool clear;
    }

    [Header("Scan")]
    [SerializeField] private float scanDistance = 5f;
    [SerializeField] private float scanHeight = 1.1f;
    [SerializeField] private float bodyRadius = 0.35f;
    [SerializeField] private float scanInterval = 0.08f;
    [SerializeField] private int rayCount = 15;
    [SerializeField] private float scanAngle = 120f;

    [Header("Obstacle")]
    [SerializeField] private LayerMask obstacleLayers = ~0;
    [SerializeField] private bool ignoreTriggers = true;
    [SerializeField] private float minimumObstacleHeight = 0.35f;
    [SerializeField] private float cornerPadding = 0.8f;
    [SerializeField] private float routeProbeDistance = 2.5f;
    [SerializeField] private float routeProbeRadius = 0.3f;
    [SerializeField] private float sideProbeAngle = 70f;

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private readonly List<ObstacleInfo> visibleObstacles = new List<ObstacleInfo>();
    private readonly List<RouteCandidate> routeCandidates = new List<RouteCandidate>();

    private ObstacleInfo nearestObstacle;
    private bool hasObstacle;
    private Vector3 clearDirection;
    private float nextScanTime;

    public bool HasObstacle => hasObstacle;
    public ObstacleInfo NearestObstacle => nearestObstacle;
    public IReadOnlyList<ObstacleInfo> VisibleObstacles => visibleObstacles;
    public IReadOnlyList<RouteCandidate> RouteCandidates => routeCandidates;
    public Vector3 ClearDirection => clearDirection;

    private void Awake()
    {
        ScanEnvironment(true);
    }

    private void FixedUpdate()
    {
        if (Time.time < nextScanTime)
            return;

        nextScanTime = Time.time + Mathf.Max(0.02f, scanInterval);
        ScanEnvironment(false);
    }

    public void ScanNow()
    {
        ScanEnvironment(true);
    }

    public bool TryGetBestRoute(out RouteCandidate candidate)
    {
        if (routeCandidates.Count == 0)
        {
            candidate = default(RouteCandidate);
            return false;
        }

        float bestScore = float.NegativeInfinity;
        candidate = default(RouteCandidate);

        for (int i = 0; i < routeCandidates.Count; i++)
        {
            RouteCandidate current = routeCandidates[i];

            if (!current.clear)
                continue;

            if (current.score > bestScore)
            {
                bestScore = current.score;
                candidate = current;
            }
        }

        return bestScore > float.NegativeInfinity;
    }

    public bool IsDirectionClear(Vector3 direction, float distance)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return false;

        direction.Normalize();

        Vector3 origin = transform.position + Vector3.up * scanHeight;

        return !Physics.SphereCast(
            origin,
            routeProbeRadius,
            direction,
            out _,
            Mathf.Max(0.1f, distance),
            obstacleLayers,
            ignoreTriggers
                ? QueryTriggerInteraction.Ignore
                : QueryTriggerInteraction.Collide
        );
    }

    private void ScanEnvironment(bool force)
    {
        if (!force && Time.time < nextScanTime)
            return;

        visibleObstacles.Clear();
        routeCandidates.Clear();
        hasObstacle = false;
        nearestObstacle = default(ObstacleInfo);
        clearDirection = Flatten(transform.forward);

        Vector3 origin = transform.position + Vector3.up * scanHeight;
        Vector3 forward = Flatten(transform.forward);

        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.forward;

        forward.Normalize();

        float nearestDistance = float.MaxValue;

        int count = Mathf.Max(3, rayCount);

        for (int i = 0; i < count; i++)
        {
            float t = count == 1 ? 0.5f : (float)i / (count - 1);
            float angle = Mathf.Lerp(-scanAngle * 0.5f, scanAngle * 0.5f, t);
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * forward;

            if (!Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                scanDistance,
                obstacleLayers,
                ignoreTriggers
                    ? QueryTriggerInteraction.Ignore
                    : QueryTriggerInteraction.Collide
            ))
            {
                continue;
            }

            if (!IsValidObstacle(hit.collider))
                continue;

            ObstacleInfo info = BuildObstacleInfo(hit, forward);
            visibleObstacles.Add(info);

            if (info.distance < nearestDistance)
            {
                nearestDistance = info.distance;
                nearestObstacle = info;
                hasObstacle = true;
            }
        }

        if (hasObstacle)
        {
            BuildRouteCandidates(forward);
        }
        else
        {
            clearDirection = forward;
            AddClearCandidate(forward, 1f, RouteSide.None);
        }

        nextScanTime = Time.time + Mathf.Max(0.02f, scanInterval);
    }

    private bool IsValidObstacle(Collider collider)
    {
        if (collider == null)
            return false;

        if (collider.isTrigger && ignoreTriggers)
            return false;

        if (collider.bounds.size.y < minimumObstacleHeight)
            return false;

        EnemyController enemy = collider.GetComponentInParent<EnemyController>();
        if (enemy != null && enemy.gameObject != gameObject)
            return false;

        return true;
    }

    private ObstacleInfo BuildObstacleInfo(RaycastHit hit, Vector3 forward)
    {
        Bounds bounds = hit.collider.bounds;
        Vector3 center = bounds.center;

        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        if (right.sqrMagnitude < 0.001f)
            right = transform.right;

        Vector3 leftCorner = center - right * bounds.extents.x;
        Vector3 rightCorner = center + right * bounds.extents.x;

        leftCorner.y = transform.position.y;
        rightCorner.y = transform.position.y;

        RouteSide preferredSide = DeterminePreferredSide(hit.point, forward);

        return new ObstacleInfo
        {
            collider = hit.collider,
            point = hit.point,
            normal = hit.normal,
            bounds = bounds,
            distance = hit.distance,
            preferredSide = preferredSide,
            leftCorner = leftCorner,
            rightCorner = rightCorner
        };
    }

    private RouteSide DeterminePreferredSide(Vector3 obstaclePoint, Vector3 forward)
    {
        Vector3 toObstacle = Flatten(obstaclePoint - transform.position);
        if (toObstacle.sqrMagnitude < 0.001f)
            return RouteSide.None;

        toObstacle.Normalize();

        float side = Vector3.Cross(forward, toObstacle).y;

        if (Mathf.Abs(side) < 0.05f)
            return RouteSide.None;

        return side > 0f ? RouteSide.Right : RouteSide.Left;
    }

    private void BuildRouteCandidates(Vector3 forward)
    {
        ObstacleInfo obstacle = nearestObstacle;

        Vector3 leftDirection = Quaternion.Euler(0f, -sideProbeAngle, 0f) * forward;
        Vector3 rightDirection = Quaternion.Euler(0f, sideProbeAngle, 0f) * forward;

        AddRouteCandidate(leftDirection, RouteSide.Left, obstacle.leftCorner, forward);
        AddRouteCandidate(rightDirection, RouteSide.Right, obstacle.rightCorner, forward);

        Vector3 leftCornerDirection = Flatten(obstacle.leftCorner - transform.position);
        Vector3 rightCornerDirection = Flatten(obstacle.rightCorner - transform.position);

        if (leftCornerDirection.sqrMagnitude > 0.001f)
            AddRouteCandidate(leftCornerDirection.normalized, RouteSide.Left, obstacle.leftCorner, forward);

        if (rightCornerDirection.sqrMagnitude > 0.001f)
            AddRouteCandidate(rightCornerDirection.normalized, RouteSide.Right, obstacle.rightCorner, forward);

        Vector3 normal = Flatten(obstacle.normal);

        if (normal.sqrMagnitude > 0.001f)
        {
            normal.Normalize();
            Vector3 slide = Vector3.Cross(Vector3.up, normal).normalized;

            if (Vector3.Dot(slide, rightDirection) < 0f)
                slide = -slide;

            AddRouteCandidate(slide, RouteSide.Right, obstacle.rightCorner, forward);
            AddRouteCandidate(-slide, RouteSide.Left, obstacle.leftCorner, forward);
        }
    }

    private void AddRouteCandidate(
        Vector3 direction,
        RouteSide side,
        Vector3 corner,
        Vector3 forward
    )
    {
        direction = Flatten(direction);

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        Vector3 target = transform.position + direction * routeProbeDistance;

        if (corner != Vector3.zero)
        {
            Vector3 fromCorner = Flatten(corner - transform.position);
            if (fromCorner.sqrMagnitude > 0.001f)
            {
                fromCorner.Normalize();
                target = corner + fromCorner * cornerPadding;
                target.y = transform.position.y;
            }
        }

        bool clear = IsDirectionClear(direction, routeProbeDistance);

        if (clear && !IsTargetPositionClear(target))
            clear = false;

        float alignment = Vector3.Dot(forward, direction);
        float sideBonus = side == nearestObstacle.preferredSide ? 0.35f : 0f;
        float distancePenalty = Vector3.Distance(transform.position, target) * 0.08f;

        float score = alignment + sideBonus - distancePenalty;

        routeCandidates.Add(new RouteCandidate
        {
            position = target,
            direction = direction,
            score = score,
            side = side,
            clear = clear
        });
    }

    private void AddClearCandidate(Vector3 direction, float score, RouteSide side)
    {
        direction = Flatten(direction);

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        routeCandidates.Add(new RouteCandidate
        {
            position = transform.position + direction * routeProbeDistance,
            direction = direction,
            score = score,
            side = side,
            clear = true
        });
    }

    private bool IsTargetPositionClear(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        float distance = direction.magnitude;

        if (distance < 0.1f)
            return true;

        direction /= distance;

        Vector3 origin = transform.position + Vector3.up * 0.1f;

        return !Physics.CapsuleCast(
            origin,
            origin + Vector3.up * Mathf.Max(0.5f, scanHeight),
            Mathf.Max(0.1f, bodyRadius),
            direction,
            out _,
            distance,
            obstacleLayers,
            ignoreTriggers
                ? QueryTriggerInteraction.Ignore
                : QueryTriggerInteraction.Collide
        );
    }

    private Vector3 Flatten(Vector3 value)
    {
        value.y = 0f;
        return value;
    }

    private void OnDrawGizmos()
    {
        if (!showDebug)
            return;

        Vector3 origin = transform.position + Vector3.up * scanHeight;
        Vector3 forward = Flatten(transform.forward);
        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.forward;
        forward.Normalize();

        Gizmos.DrawRay(origin, forward * scanDistance);

        if (hasObstacle)
        {
            Gizmos.DrawWireCube(nearestObstacle.bounds.center, nearestObstacle.bounds.size);
            Gizmos.DrawSphere(nearestObstacle.leftCorner, 0.12f);
            Gizmos.DrawSphere(nearestObstacle.rightCorner, 0.12f);
        }

        for (int i = 0; i < routeCandidates.Count; i++)
        {
            RouteCandidate candidate = routeCandidates[i];
            Gizmos.DrawRay(origin, candidate.direction * routeProbeDistance);
            Gizmos.DrawWireSphere(candidate.position, 0.15f);
        }
    }
}
