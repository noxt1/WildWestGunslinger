using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyTacticalVision : MonoBehaviour
{
public enum TacticalDecision
{
None,
Clear,
AvoidLeft,
AvoidRight,
CoverAhead
}


public enum VisionState
{
    Idle,
    Scanning,
    PlayerDetected,
    Searching,
    Investigating
}

[Header("Scanning")]
[SerializeField] private float scanInterval = 0.08f;
[SerializeField] private float scanDistance = 18f;
[SerializeField] private float scanRadius = 0.08f;
[SerializeField] private float scanHeight = 1.1f;

[Header("360 Scan")]
[SerializeField] private bool enable360Scan = true;
[SerializeField] private float scanDegreesPerSecond = 65f;
[SerializeField] private float scanSweepAngle = 360f;
[SerializeField] private float scanPauseAtEnds = 0.15f;
[SerializeField] private bool reverseScanDirection = true;

[Header("Player Detection")]
[SerializeField] private float visionFov = 30f;
[SerializeField] private float noiseInvestigationFov = 80f;
[SerializeField] private float playerHeight = 1f;
[SerializeField] private float playerDetectionMemory = 0.25f;

[Header("Noise Investigation")]
[SerializeField] private float noiseLookDuration = 2.5f;
[SerializeField] private float noiseSweepAngle = 55f;
[SerializeField] private float noiseSweepSpeed = 75f;
[SerializeField] private float noiseLookRotationSpeed = 240f;

[Header("Side Route")]
[SerializeField] private float sideProbeDistance = 3.5f;
[SerializeField] private float sideProbeRadius = 0.55f;
[SerializeField] private float routeForwardDistance = 1.5f;
[SerializeField] private float routeSidePadding = 1.2f;

[Header("Obstacle Filtering")]
[SerializeField] private LayerMask obstacleLayers = ~0;
[SerializeField] private bool ignoreTriggers = true;
[SerializeField] private float minimumObstacleHeight = 0.35f;

[Header("Runtime Vision")]
[SerializeField] private bool showRuntimeVision = true;
[SerializeField] private bool showRuntimeObstacleRay = true;
[SerializeField] private bool showRuntimeFov = true;
[SerializeField] private bool showRuntimeNoiseTarget = true;

[SerializeField] private float runtimeRayWidth = 0.035f;
[SerializeField] private float runtimeFovWidth = 0.012f;
[SerializeField] private float runtimeFovDistance = 5f;
[SerializeField] private int runtimeFovSegments = 32;

[Header("Editor Debug")]
[SerializeField] private bool showDebug = true;
[SerializeField] private bool showVisionRay = true;
[SerializeField] private bool showScanArc = true;
[SerializeField] private bool showSideProbes = true;
[SerializeField] private bool showRoute = true;
[SerializeField] private bool showStateLabel = true;

private EnemyController enemy;

private TacticalDecision currentDecision =
    TacticalDecision.None;

private VisionState currentVisionState =
    VisionState.Idle;

private Collider currentObstacle;
private CoverPoint currentCoverPoint;

private Vector3 currentObstaclePoint;
private Vector3 currentObstacleNormal;

private Vector3 leftProbePoint;
private Vector3 rightProbePoint;

private Vector3 selectedRoutePoint;
private Vector3 selectedDirection;

private Vector3 scanDirection;
private Vector3 scanBaseForward;

private Vector3 noiseLookPosition;
private Vector3 noiseBaseDirection;

private float nextScanTime;
private float scanAngle;
private float scanDirectionSign = 1f;
private float scanPauseTimer;

private float noiseLookTimer;

private float lastPlayerSeenTime = -100f;
private float nextVisionDebugTime;

private bool playerDetected;
private bool noiseLookActive;

private Transform player;

private LineRenderer runtimeVisionLine;
private LineRenderer runtimeObstacleLine;
private LineRenderer runtimeFovLine;
private LineRenderer runtimeNoiseLine;

private Material runtimeVisionMaterial;
private Material runtimeObstacleMaterial;
private Material runtimeFovMaterial;
private Material runtimeNoiseMaterial;

public TacticalDecision CurrentDecision =>
    currentDecision;

public VisionState CurrentVisionState =>
    currentVisionState;

public Collider CurrentObstacle =>
    currentObstacle;

public Vector3 CurrentObstaclePoint =>
    currentObstaclePoint;

public CoverPoint CurrentCoverPoint =>
    currentCoverPoint;

public Vector3 SelectedRoutePoint =>
    selectedRoutePoint;

public Vector3 SelectedDirection =>
    selectedDirection;

public Vector3 CurrentScanDirection =>
    scanDirection;

public bool IsScanning =>
    currentVisionState == VisionState.Scanning;

public bool PlayerDetected =>
    playerDetected;

public float LastPlayerSeenTime =>
    lastPlayerSeenTime;

public float VisionFov =>
    visionFov;

public float ScanDistance =>
    scanDistance;

private void Awake()
{
    enemy =
        GetComponent<EnemyController>();

    player =
        FindPlayer();

    InitializeScanDirection();

    CreateRuntimeVision();
}

private void Start()
{
    if (player == null)
    {
        player =
            FindPlayer();
    }

    currentVisionState =
        VisionState.Scanning;

    UpdateRuntimeVision();
}

private void OnDestroy()
{
    DestroyRuntimeMaterials();
}

private void InitializeScanDirection()
{
    scanBaseForward =
        transform.forward;

    scanBaseForward.y = 0f;

    if (scanBaseForward.sqrMagnitude < 0.001f)
    {
        scanBaseForward =
            Vector3.forward;
    }

    scanBaseForward.Normalize();

    scanDirection =
        scanBaseForward;

    selectedDirection =
        scanDirection;
}

private Transform FindPlayer()
{
    GameObject playerObject =
        GameObject.FindGameObjectWithTag(
            "Player"
        );

    if (playerObject == null)
        return null;

    return playerObject.transform;
}

public bool IsPlayerInVision()
{
    return
        playerDetected;
}

public bool HasDirectVisionToPlayer()
{
    return
        CanDetectPlayer();
}
public void SetScanDistance(float distance)
{
    scanDistance = Mathf.Max(1f, distance);
}
public void SetPlayerDetected(
    bool detected
)
{
    playerDetected =
        detected;

    if (detected)
    {
        lastPlayerSeenTime =
            Time.time;

        currentVisionState =
            VisionState.PlayerDetected;

        noiseLookActive =
            false;
    }
    else
    {
        if (
            Time.time -
            lastPlayerSeenTime >
            playerDetectionMemory
        )
        {
            playerDetected =
                false;

            if (!noiseLookActive)
            {
                currentVisionState =
                    VisionState.Scanning;
            }
        }
    }

    UpdateRuntimeVision();
}

public void ScanNow()
{
    if (Time.time < nextScanTime)
        return;

    nextScanTime =
        Time.time +
        Mathf.Max(
            0.02f,
            scanInterval
        );

    if (player == null)
    {
        player =
            FindPlayer();
    }

    UpdateVision();

    PerformEnvironmentScan();

    UpdateRuntimeVision();
}

private void UpdateVision()
{
    if (player == null)
    {
        UpdateScanRotation();
        return;
    }

    bool detected =
        CanDetectPlayer();

    if (detected)
    {
        bool wasDetected =
            playerDetected;

        playerDetected =
            true;

        if (!wasDetected)
        {
            Debug.Log(
                gameObject.name +
                ": VISION -> PLAYER DETECTED"
            );
        }

        lastPlayerSeenTime =
            Time.time;

        noiseLookActive =
            false;

        currentVisionState =
            VisionState.PlayerDetected;

        Vector3 playerDirection =
            player.position -
            transform.position;

        playerDirection.y = 0f;

        if (playerDirection.sqrMagnitude > 0.001f)
        {
            playerDirection.Normalize();

            scanDirection =
                playerDirection;

            RotateTowardDirection(
                playerDirection,
                noiseLookRotationSpeed
            );
        }

        return;
    }

    if (
        playerDetected &&
        Time.time -
        lastPlayerSeenTime >
        playerDetectionMemory
    )
    {
        playerDetected =
            false;
    }

    if (noiseLookActive)
    {
        UpdateNoiseLook();
        return;
    }

    UpdateScanRotation();
}

private bool CanDetectPlayer()
{
    if (player == null)
        return false;

    Vector3 toPlayer =
        player.position -
        transform.position;

    float distance =
        toPlayer.magnitude;

    if (distance <= 0.05f)
        return true;

    if (distance > scanDistance)
        return false;

    Vector3 flatDirection =
        toPlayer;
    flatDirection.y = 0f;

    if (flatDirection.sqrMagnitude < 0.001f)
        return false;

    flatDirection.Normalize();

    Vector3 visionDirection =
        scanDirection;
    visionDirection.y = 0f;

    if (visionDirection.sqrMagnitude < 0.001f)
    {
        visionDirection = transform.forward;
        visionDirection.y = 0f;
    }

    visionDirection.Normalize();

    float angle =
        Vector3.Angle(
            visionDirection,
            flatDirection
        );

    float effectiveFov =
        noiseLookActive
            ? Mathf.Max(visionFov, noiseInvestigationFov)
            : visionFov;

    if (angle > effectiveFov * 0.5f)
    {
        DebugVisionFailure(
            "OUTSIDE FOV angle=" +
            angle.ToString("F1") +
            " fov=" +
            effectiveFov.ToString("F1") +
            " dist=" +
            distance.ToString("F1")
        );
        return false;
    }

    Vector3 eye =
        transform.position +
        Vector3.up *
        scanHeight;

    Vector3[] targets =
    {
        player.position + Vector3.up * 0.35f,
        player.position + Vector3.up * 0.9f,
        player.position + Vector3.up * 1.35f,
        player.position + Vector3.up * 1.7f
    };

    for (int i = 0; i < targets.Length; i++)
    {
        Vector3 target = targets[i];
        Vector3 rayDirection = target - eye;
        float rayDistance = rayDirection.magnitude;

        if (rayDistance <= 0.01f)
            return true;

        rayDirection /= rayDistance;

        RaycastHit[] hits =
            Physics.RaycastAll(
                eye,
                rayDirection,
                rayDistance,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore
            );

        System.Array.Sort(
            hits,
            (a, b) => a.distance.CompareTo(b.distance)
        );

        bool clearToPlayer = false;
        bool blocked = false;
        string blockerName = null;
        float blockerDistance = 0f;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null)
                continue;

            Transform hitTransform = hit.collider.transform;

            if (
                hitTransform == transform ||
                hitTransform.IsChildOf(transform)
            )
            {
                continue;
            }

            EnemyController otherEnemy =
                hitTransform.GetComponentInParent<EnemyController>();

            if (
                otherEnemy != null &&
                otherEnemy != enemy
            )
            {
                blocked = true;
                blockerName = otherEnemy.name;
                blockerDistance = hit.distance;
                break;
            }

            if (
                hitTransform == player ||
                hitTransform.IsChildOf(player)
            )
            {
                clearToPlayer = true;
                break;
            }

            blocked = true;
            blockerName = hitTransform.name;
            blockerDistance = hit.distance;
            break;
        }

        if (clearToPlayer)
            return true;

        if (blocked)
        {
            DebugVisionFailure(
                "BLOCKED BY " +
                blockerName +
                " dist=" +
                blockerDistance.ToString("F1") +
                " point=" +
                i
            );
        }
    }

    DebugVisionFailure(
        "NO LOS POINTS dist=" +
        distance.ToString("F1")
    );

    return false;
}

private void DebugVisionFailure(
    string reason
)
{
    if (!showDebug)
        return;

    if (Time.time < nextVisionDebugTime)
        return;

    nextVisionDebugTime =
        Time.time + 0.5f;

    Debug.Log(
        gameObject.name +
        ": VISION CHECK FAILED -> " +
        reason +
        " | state=" +
        currentVisionState +
        " noise=" +
        noiseLookActive
    );
}

private void UpdateScanRotation()
{
    if (playerDetected)
        return;

    if (noiseLookActive)
        return;

    if (!enable360Scan)
    {
        scanDirection =
            transform.forward;

        scanDirection.y = 0f;

        if (scanDirection.sqrMagnitude > 0.001f)
        {
            scanDirection.Normalize();
        }

        currentVisionState =
            VisionState.Scanning;

        RotateTowardDirection(
            scanDirection,
            scanDegreesPerSecond
        );

        return;
    }

    if (scanPauseTimer > 0f)
    {
        scanPauseTimer -=
            Time.fixedDeltaTime;

        currentVisionState =
            VisionState.Scanning;

        RotateTowardDirection(
            scanDirection,
            scanDegreesPerSecond
        );

        return;
    }

    float delta =
        scanDegreesPerSecond *
        Time.fixedDeltaTime *
        scanDirectionSign;

    scanAngle +=
        delta;

    float halfSweep =
        Mathf.Clamp(
            scanSweepAngle * 0.5f,
            1f,
            180f
        );

    if (scanAngle >= halfSweep)
    {
        scanAngle =
            halfSweep;

        scanPauseTimer =
            scanPauseAtEnds;

        if (reverseScanDirection)
        {
            scanDirectionSign =
                -1f;
        }
    }

    if (scanAngle <= -halfSweep)
    {
        scanAngle =
            -halfSweep;

        scanPauseTimer =
            scanPauseAtEnds;

        if (reverseScanDirection)
        {
            scanDirectionSign =
                1f;
        }
    }

    scanDirection =
        Quaternion.Euler(
            0f,
            scanAngle,
            0f
        ) *
        scanBaseForward;

    scanDirection.y = 0f;

    if (scanDirection.sqrMagnitude < 0.001f)
    {
        scanDirection =
            scanBaseForward;
    }

    scanDirection.Normalize();

    RotateTowardDirection(
        scanDirection,
        scanDegreesPerSecond
    );

    currentVisionState =
        VisionState.Scanning;
}

private void RotateTowardDirection(
    Vector3 direction,
    float speed
)
{
    direction.y = 0f;

    if (direction.sqrMagnitude < 0.001f)
        return;

    direction.Normalize();

    Quaternion targetRotation =
        Quaternion.LookRotation(
            direction
        );

    transform.rotation =
        Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            speed *
            Time.fixedDeltaTime
        );
}

public void LookAtNoise(
    Vector3 noisePosition
)
{
    Vector3 direction =
        noisePosition -
        transform.position;

    direction.y = 0f;

    if (direction.sqrMagnitude < 0.001f)
        return;

    direction.Normalize();

    noiseLookPosition =
        noisePosition;

    noiseBaseDirection =
        direction;

    noiseLookTimer =
        noiseLookDuration;

    scanAngle =
        0f;

    scanDirection =
        direction;

    noiseLookActive =
        true;

    playerDetected =
        false;

    currentVisionState =
        VisionState.Investigating;

    UpdateRuntimeVision();

    Debug.Log(
        gameObject.name +
        ": VISION -> LOOKING AT NOISE " +
        noisePosition
    );
}

private void UpdateNoiseLook()
{
    if (!noiseLookActive)
        return;

    noiseLookTimer -=
        Time.fixedDeltaTime;

    if (noiseLookTimer <= 0f)
    {
        noiseLookActive =
            false;

        currentVisionState =
            VisionState.Scanning;

        scanAngle =
            0f;

        scanDirection =
            transform.forward;

        scanDirection.y = 0f;

        if (scanDirection.sqrMagnitude > 0.001f)
        {
            scanDirection.Normalize();

            scanBaseForward =
                scanDirection;
        }

        UpdateRuntimeVision();

        Debug.Log(
            gameObject.name +
            ": VISION -> NOISE INVESTIGATION FINISHED"
        );

        return;
    }

    float elapsed =
        noiseLookDuration -
        noiseLookTimer;

    float sweepPhase =
        elapsed *
        noiseSweepSpeed;

    float sweep =
        Mathf.Sin(
            sweepPhase *
            Mathf.Deg2Rad
        ) *
        noiseSweepAngle;

    scanDirection =
        Quaternion.Euler(
            0f,
            sweep,
            0f
        ) *
        noiseBaseDirection;

    scanDirection.y = 0f;

    if (scanDirection.sqrMagnitude < 0.001f)
    {
        scanDirection =
            noiseBaseDirection;
    }

    scanDirection.Normalize();

    RotateTowardDirection(
        scanDirection,
        noiseLookRotationSpeed
    );

    currentVisionState =
        VisionState.Investigating;
}

private void PerformEnvironmentScan()
{
    ResetEnvironmentResult();

    Vector3 origin =
        transform.position +
        Vector3.up *
        scanHeight;

    Vector3 direction =
        scanDirection;

    direction.y = 0f;

    if (direction.sqrMagnitude < 0.001f)
    {
        direction =
            transform.forward;

        direction.y = 0f;
    }

    if (direction.sqrMagnitude < 0.001f)
        return;

    direction.Normalize();

    RaycastHit obstacleHit;

    if (
        !TryFindObstacle(
            origin,
            direction,
            scanDistance,
            out obstacleHit
        )
    )
    {
        currentDecision =
            TacticalDecision.Clear;

        selectedDirection =
            direction;

        return;
    }

    currentObstacle =
        obstacleHit.collider;

    currentObstaclePoint =
        obstacleHit.point;

    currentObstacleNormal =
        obstacleHit.normal;

    currentCoverPoint =
        obstacleHit.collider.GetComponentInParent<CoverPoint>();

    if (currentCoverPoint != null)
    {
        currentDecision =
            TacticalDecision.CoverAhead;
    }

    BuildSideProbePoints(
        origin,
        direction,
        obstacleHit
    );

    bool leftValid =
        IsRouteValid(
            leftProbePoint,
            direction
        );

    bool rightValid =
        IsRouteValid(
            rightProbePoint,
            direction
        );

    if (!leftValid && !rightValid)
    {
        selectedDirection =
            Vector3.zero;

        return;
    }

    float leftScore =
        leftValid
            ? ScoreRoute(
                leftProbePoint,
                direction
            )
            : float.MaxValue;

    float rightScore =
        rightValid
            ? ScoreRoute(
                rightProbePoint,
                direction
            )
            : float.MaxValue;

    if (leftScore <= rightScore)
    {
        currentDecision =
            TacticalDecision.AvoidLeft;

        selectedRoutePoint =
            leftProbePoint;

        selectedDirection =
            BuildRouteDirection(
                leftProbePoint,
                direction
            );
    }
    else
    {
        currentDecision =
            TacticalDecision.AvoidRight;

        selectedRoutePoint =
            rightProbePoint;

        selectedDirection =
            BuildRouteDirection(
                rightProbePoint,
                direction
            );
    }
}

private void ResetEnvironmentResult()
{
    currentDecision =
        TacticalDecision.None;

    currentObstacle =
        null;

    currentCoverPoint =
        null;

    currentObstaclePoint =
        Vector3.zero;

    currentObstacleNormal =
        Vector3.zero;

    leftProbePoint =
        Vector3.zero;

    rightProbePoint =
        Vector3.zero;

    selectedRoutePoint =
        Vector3.zero;

    selectedDirection =
        scanDirection;
}

private bool TryFindObstacle(
    Vector3 origin,
    Vector3 direction,
    float distance,
    out RaycastHit nearestHit
)
{
    nearestHit =
        default;

    RaycastHit[] hits =
        Physics.SphereCastAll(
            origin,
            scanRadius,
            direction,
            distance,
            obstacleLayers,
            ignoreTriggers
                ? QueryTriggerInteraction.Ignore
                : QueryTriggerInteraction.Collide
        );

    bool found =
        false;

    float nearestDistance =
        float.MaxValue;

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
            hitTransform == transform ||
            hitTransform.IsChildOf(transform)
        )
        {
            continue;
        }

        EnemyController otherEnemy =
            hitTransform.GetComponentInParent<EnemyController>();

        if (
            otherEnemy != null &&
            otherEnemy != enemy
        )
        {
            continue;
        }

        if (
            hit.collider.bounds.size.y <
            minimumObstacleHeight
        )
        {
            continue;
        }

        if (
            hit.distance <
            nearestDistance
        )
        {
            nearestDistance =
                hit.distance;

            nearestHit =
                hit;

            found =
                true;
        }
    }

    return found;
}

private void BuildSideProbePoints(
    Vector3 origin,
    Vector3 forward,
    RaycastHit obstacleHit
)
{
    Vector3 side =
        Vector3.Cross(
            Vector3.up,
            forward
        ).normalized;

    Bounds bounds =
        obstacleHit.collider.bounds;

    float sideOffset =
        Mathf.Max(
            bounds.extents.x,
            bounds.extents.z
        ) +
        routeSidePadding;

    Vector3 center =
        bounds.center;

    center.y =
        transform.position.y;

    Vector3 forwardOffset =
        forward *
        routeForwardDistance;

    leftProbePoint =
        center -
        side *
        sideOffset +
        forwardOffset;

    rightProbePoint =
        center +
        side *
        sideOffset +
        forwardOffset;

    leftProbePoint.y =
        transform.position.y;

    rightProbePoint.y =
        transform.position.y;
}

private bool IsRouteValid(
    Vector3 probePoint,
    Vector3 desiredForward
)
{
    Vector3 origin =
        transform.position +
        Vector3.up *
        scanHeight;

    Vector3 toProbe =
        probePoint -
        origin;

    toProbe.y = 0f;

    float distance =
        toProbe.magnitude;

    if (distance < 0.1f)
        return false;

    toProbe.Normalize();

    RaycastHit[] hits =
        Physics.SphereCastAll(
            origin,
            sideProbeRadius,
            toProbe,
            distance,
            obstacleLayers,
            ignoreTriggers
                ? QueryTriggerInteraction.Ignore
                : QueryTriggerInteraction.Collide
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
            hitTransform == transform ||
            hitTransform.IsChildOf(transform)
        )
        {
            continue;
        }

        if (
            currentObstacle != null &&
            hit.collider == currentObstacle
        )
        {
            continue;
        }

        EnemyController otherEnemy =
            hitTransform.GetComponentInParent<EnemyController>();

        if (
            otherEnemy != null &&
            otherEnemy != enemy
        )
        {
            continue;
        }

        return false;
    }

    Vector3 afterOrigin =
        probePoint +
        Vector3.up *
        0.75f;

    RaycastHit afterHit;

    if (
        Physics.SphereCast(
            afterOrigin,
            sideProbeRadius,
            desiredForward,
            out afterHit,
            routeForwardDistance + 1f,
            obstacleLayers,
            ignoreTriggers
                ? QueryTriggerInteraction.Ignore
                : QueryTriggerInteraction.Collide
        )
    )
    {
        if (afterHit.collider != null)
        {
            Transform hitTransform =
                afterHit.collider.transform;

            if (
                hitTransform != transform &&
                !hitTransform.IsChildOf(transform)
            )
            {
                EnemyController otherEnemy =
                    hitTransform.GetComponentInParent<EnemyController>();

                if (
                    otherEnemy == null ||
                    otherEnemy == enemy
                )
                {
                    return false;
                }
            }
        }
    }

    Collider[] overlaps =
        Physics.OverlapSphere(
            probePoint +
            Vector3.up * 0.7f,
            sideProbeRadius,
            obstacleLayers,
            ignoreTriggers
                ? QueryTriggerInteraction.Ignore
                : QueryTriggerInteraction.Collide
        );

    foreach (
        Collider collider
        in overlaps
    )
    {
        if (collider == null)
            continue;

        Transform hitTransform =
            collider.transform;

        if (
            hitTransform == transform ||
            hitTransform.IsChildOf(transform)
        )
        {
            continue;
        }

        if (
            currentObstacle != null &&
            collider == currentObstacle
        )
        {
            continue;
        }

        EnemyController otherEnemy =
            hitTransform.GetComponentInParent<EnemyController>();

        if (
            otherEnemy != null &&
            otherEnemy != enemy
        )
        {
            continue;
        }

        return false;
    }

    return true;
}

private float ScoreRoute(
    Vector3 routePoint,
    Vector3 desiredForward
)
{
    Vector3 fromEnemy =
        routePoint -
        transform.position;

    fromEnemy.y = 0f;

    if (fromEnemy.sqrMagnitude < 0.001f)
        return float.MaxValue;

    float distance =
        fromEnemy.magnitude;

    fromEnemy.Normalize();

    float directionPenalty =
        1f -
        Mathf.Clamp01(
            Vector3.Dot(
                fromEnemy,
                desiredForward
            )
        );

    float coverBonus =
        0f;

    Collider[] colliders =
        Physics.OverlapSphere(
            routePoint,
            1.25f,
            obstacleLayers,
            ignoreTriggers
                ? QueryTriggerInteraction.Ignore
                : QueryTriggerInteraction.Collide
        );

    foreach (
        Collider collider
        in colliders
    )
    {
        if (collider == null)
            continue;

        if (
            collider.GetComponentInParent<CoverPoint>() !=
            null
        )
        {
            coverBonus =
                -0.75f;

            break;
        }
    }

    return
        distance +
        directionPenalty * 2f +
        coverBonus;
}

private Vector3 BuildRouteDirection(
    Vector3 routePoint,
    Vector3 desiredForward
)
{
    Vector3 toRoute =
        routePoint -
        transform.position;

    toRoute.y = 0f;

    if (toRoute.sqrMagnitude < 0.001f)
        return desiredForward;

    toRoute.Normalize();

    Vector3 result =
        Vector3.Slerp(
            desiredForward,
            toRoute,
            0.8f
        );

    result.y = 0f;

    if (result.sqrMagnitude < 0.001f)
        return desiredForward;

    return result.normalized;
}

public Vector3 GetTacticalDirection(
    Vector3 desiredDirection
)
{
    desiredDirection.y = 0f;

    if (desiredDirection.sqrMagnitude < 0.001f)
        return Vector3.zero;

    desiredDirection.Normalize();

    if (
        currentDecision !=
            TacticalDecision.AvoidLeft &&
        currentDecision !=
            TacticalDecision.AvoidRight
    )
    {
        return desiredDirection;
    }

    if (
        selectedDirection.sqrMagnitude <
        0.001f
    )
    {
        return desiredDirection;
    }

    Vector3 result =
        Vector3.Slerp(
            desiredDirection,
            selectedDirection,
            0.9f
        );

    result.y = 0f;

    if (result.sqrMagnitude < 0.001f)
        return selectedDirection;

    return result.normalized;
}

public void SetSearchingState(
    bool searching
)
{
    if (playerDetected)
    {
        currentVisionState =
            VisionState.PlayerDetected;

        UpdateRuntimeVision();

        return;
    }

    currentVisionState =
        searching
            ? VisionState.Searching
            : VisionState.Scanning;

    UpdateRuntimeVision();
}

public void SetInvestigatingState(
    bool investigating
)
{
    if (playerDetected)
    {
        currentVisionState =
            VisionState.PlayerDetected;

        UpdateRuntimeVision();

        return;
    }

    currentVisionState =
        investigating
            ? VisionState.Investigating
            : VisionState.Scanning;

    UpdateRuntimeVision();
}

private void CreateRuntimeVision()
{
    if (!showRuntimeVision)
        return;

    runtimeVisionLine =
        CreateLineRenderer(
            "RuntimeVisionLine",
            runtimeRayWidth
        );

    runtimeObstacleLine =
        CreateLineRenderer(
            "RuntimeObstacleLine",
            runtimeRayWidth
        );

    runtimeFovLine =
        CreateLineRenderer(
            "RuntimeFOVLine",
            runtimeFovWidth
        );

    runtimeNoiseLine =
        CreateLineRenderer(
            "RuntimeNoiseLine",
            runtimeRayWidth
        );

    runtimeVisionMaterial =
        CreateLineMaterial(
            "VisionMaterial"
        );

    runtimeObstacleMaterial =
        CreateLineMaterial(
            "ObstacleMaterial"
        );

    runtimeFovMaterial =
        CreateLineMaterial(
            "FOVMaterial"
        );

    runtimeNoiseMaterial =
        CreateLineMaterial(
            "NoiseMaterial"
        );

    runtimeVisionLine.material =
        runtimeVisionMaterial;

    runtimeObstacleLine.material =
        runtimeObstacleMaterial;

    runtimeFovLine.material =
        runtimeFovMaterial;

    runtimeNoiseLine.material =
        runtimeNoiseMaterial;
}

private LineRenderer CreateLineRenderer(
    string objectName,
    float width
)
{
    GameObject lineObject =
        new GameObject(
            objectName
        );

    lineObject.transform.SetParent(
        transform,
        false
    );

    LineRenderer line =
        lineObject.AddComponent<LineRenderer>();

    line.useWorldSpace =
        true;

    line.positionCount =
        0;

    line.startWidth =
        width;

    line.endWidth =
        width;

    line.numCapVertices =
        4;

    line.numCornerVertices =
        4;

    line.shadowCastingMode =
        UnityEngine.Rendering.ShadowCastingMode.Off;

    line.receiveShadows =
        false;

    line.sortingOrder =
        100;

    line.enabled =
        false;

    return line;
}

private Material CreateLineMaterial(
    string materialName
)
{
    Shader shader =
        Shader.Find(
            "Sprites/Default"
        );

    if (shader == null)
    {
        shader =
            Shader.Find(
                "Unlit/Color"
            );
    }

    Material material =
        new Material(shader);

    material.name =
        materialName;

    return material;
}

private void UpdateRuntimeVision()
{
    if (!showRuntimeVision)
    {
        DisableRuntimeLines();
        return;
    }

    if (runtimeVisionLine == null)
        return;

    Vector3 origin =
        transform.position +
        Vector3.up *
        scanHeight;

    Vector3 direction =
        scanDirection;

    direction.y = 0f;

    if (direction.sqrMagnitude < 0.001f)
    {
        direction =
            transform.forward;

        direction.y = 0f;
    }

    if (direction.sqrMagnitude < 0.001f)
        return;

    direction.Normalize();

    Color mainColor =
        Color.cyan;

    switch (currentVisionState)
    {
        case VisionState.PlayerDetected:
            mainColor =
                Color.green;
            break;

        case VisionState.Searching:
            mainColor =
                Color.red;
            break;

        case VisionState.Investigating:
            mainColor =
                Color.yellow;
            break;

        case VisionState.Scanning:
            mainColor =
                Color.cyan;
            break;

        default:
            mainColor =
                Color.white;
            break;
    }

    UpdateLine(
        runtimeVisionLine,
        origin,
        origin +
        direction *
        scanDistance,
        mainColor
    );

    if (
        showRuntimeObstacleRay &&
        currentObstacle != null
    )
    {
        Color obstacleColor =
            currentCoverPoint != null
                ? Color.blue
                : Color.red;

        UpdateLine(
            runtimeObstacleLine,
            origin,
            currentObstaclePoint,
            obstacleColor
        );
    }
    else
    {
        DisableLine(
            runtimeObstacleLine
        );
    }

    if (
        showRuntimeFov &&
        currentVisionState !=
        VisionState.Idle
    )
    {
        UpdateRuntimeFov(
            origin
        );
    }
    else
    {
        DisableLine(
            runtimeFovLine
        );
    }

    if (
        showRuntimeNoiseTarget &&
        noiseLookActive
    )
    {
        UpdateLine(
            runtimeNoiseLine,
            origin,
            noiseLookPosition +
            Vector3.up * 0.5f,
            Color.yellow
        );
    }
    else
    {
        DisableLine(
            runtimeNoiseLine
        );
    }
}

private void UpdateRuntimeFov(
    Vector3 origin
)
{
    if (runtimeFovLine == null)
        return;

    int segments =
        Mathf.Max(
            8,
            runtimeFovSegments
        );

    float effectiveFov =
        noiseLookActive
            ? Mathf.Max(visionFov, noiseInvestigationFov)
            : visionFov;

    float halfFov =
        effectiveFov * 0.5f;

    float radius =
        Mathf.Max(
            0.5f,
            runtimeFovDistance
        );

    runtimeFovLine.positionCount =
        segments + 1;

    for (
        int i = 0;
        i <= segments;
        i++
    )
    {
        float t =
            (float)i /
            segments;

        float angle =
            Mathf.Lerp(
                -halfFov,
                halfFov,
                t
            );

        Vector3 direction =
            Quaternion.Euler(
                0f,
                angle,
                0f
            ) *
            scanDirection;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            direction =
                scanDirection;

        direction.Normalize();

        Vector3 point =
            origin +
            direction *
            radius;

        runtimeFovLine.SetPosition(
            i,
            point
        );
    }

    runtimeFovLine.startColor =
        new Color(
            0f,
            1f,
            1f,
            0.22f
        );

    runtimeFovLine.endColor =
        new Color(
            0f,
            1f,
            1f,
            0.22f
        );

    runtimeFovLine.enabled =
        true;
}

private void UpdateLine(
    LineRenderer line,
    Vector3 start,
    Vector3 end,
    Color color
)
{
    if (line == null)
        return;

    line.positionCount =
        2;

    line.SetPosition(
        0,
        start
    );

    line.SetPosition(
        1,
        end
    );

    line.startColor =
        color;

    line.endColor =
        color;

    line.enabled =
        true;
}

private void DisableLine(
    LineRenderer line
)
{
    if (line == null)
        return;

    line.enabled =
        false;
}

private void DisableRuntimeLines()
{
    DisableLine(
        runtimeVisionLine
    );

    DisableLine(
        runtimeObstacleLine
    );

    DisableLine(
        runtimeFovLine
    );

    DisableLine(
        runtimeNoiseLine
    );
}

private void DestroyRuntimeMaterials()
{
    if (runtimeVisionMaterial != null)
    {
        Destroy(
            runtimeVisionMaterial
        );
    }

    if (runtimeObstacleMaterial != null)
    {
        Destroy(
            runtimeObstacleMaterial
        );
    }

    if (runtimeFovMaterial != null)
    {
        Destroy(
            runtimeFovMaterial
        );
    }

    if (runtimeNoiseMaterial != null)
    {
        Destroy(
            runtimeNoiseMaterial
        );
    }
}

private void LateUpdate()
{
    if (!showRuntimeVision)
        return;

    UpdateRuntimeVision();
}

private void OnDrawGizmos()
{
    if (!showDebug)
        return;

    Vector3 origin =
        transform.position +
        Vector3.up *
        scanHeight;

    if (showVisionRay)
    {
        switch (currentVisionState)
        {
            case VisionState.PlayerDetected:
                Gizmos.color =
                    Color.green;
                break;

            case VisionState.Searching:
                Gizmos.color =
                    Color.red;
                break;

            case VisionState.Investigating:
                Gizmos.color =
                    Color.yellow;
                break;

            default:
                Gizmos.color =
                    Color.cyan;
                break;
        }

        Gizmos.DrawLine(
            origin,
            origin +
            scanDirection *
            scanDistance
        );

        Gizmos.DrawSphere(
            origin +
            scanDirection *
            scanDistance,
            0.08f
        );
    }

    if (showScanArc)
    {
        Gizmos.color =
            new Color(
                0f,
                1f,
                1f,
                0.25f
            );

        float arcRadius =
            Mathf.Min(
                scanDistance,
                4f
            );

        int segments =
            48;

        Vector3 previous =
            origin +
            Quaternion.Euler(
                0f,
                -180f,
                0f
            ) *
            scanBaseForward *
            arcRadius;

        for (
            int i = 1;
            i <= segments;
            i++
        )
        {
            float t =
                (float)i /
                segments;

            float angle =
                Mathf.Lerp(
                    -180f,
                    180f,
                    t
                );

            Vector3 next =
                origin +
                Quaternion.Euler(
                    0f,
                    angle,
                    0f
                ) *
                scanBaseForward *
                arcRadius;

            Gizmos.DrawLine(
                previous,
                next
            );

            previous =
                next;
        }
    }

    if (playerDetected)
    {
        Gizmos.color =
            Color.green;

        if (player != null)
        {
            Gizmos.DrawLine(
                origin,
                player.position +
                Vector3.up *
                playerHeight
            );
        }
    }

    if (noiseLookActive)
    {
        Gizmos.color =
            Color.yellow;

        Gizmos.DrawWireSphere(
            noiseLookPosition,
            0.45f
        );

        Gizmos.DrawLine(
            origin,
            noiseLookPosition +
            Vector3.up * 0.5f
        );
    }

    if (currentObstacle != null)
    {
        Gizmos.color =
            currentCoverPoint != null
                ? Color.blue
                : Color.red;

        Gizmos.DrawSphere(
            currentObstaclePoint,
            0.12f
        );

        Gizmos.DrawLine(
            origin,
            currentObstaclePoint
        );
    }

    if (showSideProbes)
    {
        Gizmos.color =
            Color.yellow;

        if (leftProbePoint != Vector3.zero)
        {
            Gizmos.DrawWireSphere(
                leftProbePoint,
                sideProbeRadius
            );
        }

        if (rightProbePoint != Vector3.zero)
        {
            Gizmos.DrawWireSphere(
                rightProbePoint,
                sideProbeRadius
            );
        }

        if (currentObstacle != null)
        {
            Gizmos.DrawLine(
                origin,
                leftProbePoint
            );

            Gizmos.DrawLine(
                origin,
                rightProbePoint
            );
        }
    }

    if (
        showRoute &&
        selectedDirection.sqrMagnitude >
        0.001f
    )
    {
        Gizmos.color =
            Color.yellow;

        Gizmos.DrawLine(
            origin,
            origin +
            selectedDirection *
            sideProbeDistance
        );

        Gizmos.DrawSphere(
            selectedRoutePoint,
            0.18f
        );
    }


#if UNITY_EDITOR
if (showStateLabel)
{
string text =
"VISION: " +
currentVisionState +
"\nDECISION: " +
currentDecision;


        if (playerDetected)
        {
            text +=
                "\nPLAYER DETECTED";
        }

        if (noiseLookActive)
        {
            text +=
                "\nNOISE INVESTIGATION";
        }

        Handles.Label(
            transform.position +
            Vector3.up *
            2.8f,
            text
        );
    }


#endif
}
}
