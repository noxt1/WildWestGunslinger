using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Bandit,
        Shooter,
        Rusher,
        Tactical
    }

    private enum AIState
    {
        Combat,
        Searching,
        Investigating,
        RoomSearching,
        MovingToCover,
        InCover,
        Flanking
    }

    private enum PeekState
    {
        None,
        Waiting,
        MovingOut,
        Shooting,
        MovingBack
    }

    [Header("Enemy Type")]
    [SerializeField] private EnemyType enemyType = EnemyType.Bandit;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Melee Attack")]
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 10;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private float rangedAttackDistance = 12f;
    [SerializeField] private float rangedAttackCooldown = 0.75f;
    [SerializeField] private int rangedAttackDamage = 5;
    [SerializeField] private float bulletSpawnOffset = 0.2f;

    [Header("Detection")]
    [SerializeField] private float detectionDistance = 18f;
    [SerializeField] private float fieldOfView = 120f;
    [SerializeField] private float eyeHeight = 1f;
    [SerializeField] private float playerHeight = 1f;

    [Header("Memory")]
    [SerializeField] private float memoryDuration = 3.5f;
    [SerializeField] private float investigationDuration = 2.5f;
    [SerializeField] private float investigationLookTime = 1.2f;
    [SerializeField] private float searchPointDistance = 4f;
    [SerializeField] private float searchPointReachDistance = 1.2f;
    [SerializeField] private int searchPointsCount = 5;
    [SerializeField] private float searchMoveSpeedMultiplier = 0.7f;

    [Header("Room Search")]
    [SerializeField] private float roomSearchPause = 0.8f;
    [SerializeField] private float roomSearchSpeedMultiplier = 0.65f;
    [SerializeField] private float roomConnectionReachDistance = 1.4f;

    [Header("Cover AI")]
    [SerializeField] private bool useCover = true;
    [SerializeField] private float coverCheckInterval = 0.8f;
    [SerializeField] private float dangerousCoverCheckInterval = 0.2f;
    [SerializeField] private float coverDetectionDistance = 18f;
    [SerializeField] private float coverStayTime = 4f;
    [SerializeField] private float coverReachDistance = 1.5f;
    [SerializeField] private float playerTooCloseDistance = 4f;
    [SerializeField] private float coverDangerDistance = 7f;

    [Header("Peek AI")]
    [SerializeField] private float peekWaitTime = 1.2f;
    [SerializeField] private float peekMoveSpeed = 3f;
    [SerializeField] private float peekTime = 0.8f;
    [SerializeField] private float returnToCoverDistance = 0.2f;

    [Header("Shooter Settings")]
    [SerializeField] private float shooterPreferredDistance = 8f;
    [SerializeField] private float shooterRetreatDistance = 5f;

    [Header("Rusher Settings")]
    [SerializeField] private float rusherSpeedMultiplier = 1.35f;
    [SerializeField] private bool rusherUseCover = false;

    [Header("Tactical Settings")]
    [SerializeField] private float tacticalCoverTimeMin = 2f;
    [SerializeField] private float tacticalCoverTimeMax = 4f;
    [SerializeField] private float tacticalFlankDistance = 6f;
    [SerializeField] private float tacticalFlankStartDistance = 10f;
    [SerializeField] private float tacticalFlankReachDistance = 1.2f;
    [SerializeField] private float tacticalFlankChance = 0.65f;
    [SerializeField] private float tacticalFlankDuration = 5f;

    [Header("Wave Scaling")]
    [SerializeField] private float damageGrowthPerWave = 0.05f;
    [SerializeField] private float maxDamageMultiplier = 2f;

    [Header("Elite")]
    [SerializeField] private float eliteDamageMultiplier = 1.5f;
    [SerializeField] private float eliteSpeedMultiplier = 1.15f;

    [Header("Combat Movement") ]
    [SerializeField] private float combatSpreadRadius = 3.5f;
    [SerializeField] private float combatSpreadMinDistance = 2.2f;
    [SerializeField] private float enemySeparationRadius = 1.8f;
    [SerializeField] private float enemySeparationStrength = 2.2f;
    [SerializeField] private float obstacleProbeDistance = 1.1f;
    [SerializeField] private float obstacleProbeRadius = 0.35f;
    [SerializeField] private float obstacleSideProbeDistance = 1.6f;
    [SerializeField] private float obstacleEscapeDistance = 1.8f;
    [SerializeField] private float obstacleStuckTime = 0.45f;
    [SerializeField] private float obstacleRouteCommitTime = 0.9f;
    [SerializeField] private float obstacleRouteProbeDistance = 2.2f;
    [SerializeField] private float obstacleRouteProbeAngle = 58f;
    [SerializeField] private float searchExpansionDistance = 3f;
    [SerializeField] private int maxSearchCyclesBeforeMovingOn = 2;
    [SerializeField] private float initialAmbushHoldTime = 4f;
    [SerializeField] private float combatRoleRefreshInterval = 1.5f;
    [SerializeField] private float combatPositionRepathDistance = 1.25f;
    [SerializeField] private float banditCoverChance = 0.18f;
    [SerializeField] private float groupRoleRadius = 12f;

    [Header("Spawn Search") ]
    [SerializeField] private float initialSearchDelay = 0.35f;
    [SerializeField] private float initialAmbushChance = 0f;
    [SerializeField] private float initialSearchPause = 0.4f;
    [SerializeField] private float searchScanDuration = 1.15f;
    [SerializeField] private float searchScanSpeed = 300f;

    [Header("Debug Vision")]
    [SerializeField] private bool showVision = true;
    [SerializeField] private bool showVisionOnlySelected = false;
    [SerializeField] private bool showDetectionRadius = true;
    [SerializeField] private bool showLastKnownPosition = true;
    [SerializeField] private bool showSearchPoints = true;
    [SerializeField] private int visionRayCount = 18;

    [Header("Runtime State Display")]
    [SerializeField] private bool showRuntimeState = true;
    [SerializeField] private float runtimeStateHeight = 2.2f;
    [SerializeField] private Vector2 runtimeStateOffset = Vector2.zero;

    private Rigidbody rb;
    private Transform player;
    private EnemyTacticalVision tacticalVision;
    private EnemyHearing enemyHearing;
    private EnemyTacticalPlanner tacticalPlanner;

    private readonly List<Vector3> currentPathWaypoints =
        new List<Vector3>();
    private int currentPathIndex;
    private Vector3 pathTargetPosition;
    private float nextPathRefreshTime;
    private float nextEmergencyRebuildTime;

    private const float pathRebuildDistance = 3.0f;
    private const float waypointReachDistance = 1.2f;
    private const float pathRefreshInterval = 1.5f;
    private const float emergencyRebuildCooldown = 0.5f;
    private const float aStarActivationDistance = 5.0f;

    private float baseAttackDamage;
    private float baseRangedAttackDamage;

    private float nextAttackTime;
    private float nextRangedAttackTime;
    private float nextCoverCheckTime;
    private float coverLeaveTime;
    private float nextCombatRoleRefreshTime;
    private int combatRoleIndex = -1;
    private int combatRoleCount = 1;
    private Vector3 combatRoleTargetPosition;

    private float peekTimer;
    private float peekReturnTimer;
    private float flankTimer;

    private float lostPlayerTimer;
    private float initialSearchTimer;
    private float initialSearchPauseTimer;
    private bool initialSearchStarted;
    private bool initialAmbushHolding;
    private Vector3 initialAmbushPosition;
    private float initialAmbushHoldTimer;
    private float obstacleStuckTimer;
    private Vector3 lastMovementPosition;
    private int searchCycle;
    private bool initialAmbush;
    private float investigationTimer;
    private float investigationLookTimer;

    private bool obstacleRouteActive;
    private Vector3 obstacleRouteDirection;
    private float obstacleRouteTimer;
    private float searchScanAngle;
    private bool roomSearchIsTargeted;
    private bool plannerRearmRequested;

    private CoverPoint currentCover;
    private CoverPoint lastTacticalCover;

    private bool movingToCover;
    private bool inCover;
    private bool isElite;
    private bool flanking;

    private bool playerCurrentlyVisible;
    private bool hadPlayerContact;

    private Vector3 lastKnownPlayerPosition;
    private Vector3 investigationPosition;
    private Vector3 flankTargetPosition;
    private Vector3 peekTargetPosition;
    private Vector3 coverPosition;

    private bool investigatingNoise;
    private Vector3 noiseInvestigationPosition;
    private float noiseInvestigationTimer;

    private AIState currentState =
        AIState.Combat;

    private PeekState peekState =
        PeekState.None;

    private bool peekLeft;
    private bool flankLeft;

    private readonly System.Collections.Generic.List<Vector3> searchPoints =
        new System.Collections.Generic.List<Vector3>();

    private readonly System.Collections.Generic.List<ArenaModule> roomSearchRoute =
        new System.Collections.Generic.List<ArenaModule>();

    private readonly System.Collections.Generic.List<Vector3> roomSearchWaypoints =
        new System.Collections.Generic.List<Vector3>();

    private int currentSearchPoint;
    private float searchPointTimer;
    private float nextSearchProgressLogTime;
    private Vector3 dbgLastSteerTarget;
    private Vector3 dbgLastMidDirection;
    private Vector3 dbgLastFinalDirection;
    private float dbgLastMoveSpeed;

    private int currentRoomSearchWaypoint;
    private float roomSearchPauseTimer;

    private void Awake()
    {
        rb =
            GetComponent<Rigidbody>();

        baseAttackDamage =
            attackDamage;

        baseRangedAttackDamage =
            rangedAttackDamage;

        tacticalVision =
            GetComponent<EnemyTacticalVision>();

        if (tacticalVision == null)
        {
            tacticalVision =
                gameObject.AddComponent<EnemyTacticalVision>();
        }

        enemyHearing =
            GetComponent<EnemyHearing>();

        if (enemyHearing == null)
        {
            enemyHearing =
                gameObject.AddComponent<EnemyHearing>();
        }

        tacticalPlanner =
            GetComponent<EnemyTacticalPlanner>();

        if (FindFirstObjectByType<NoiseSystem>() == null)
        {
            GameObject noiseSystemObject =
                new GameObject("NoiseSystem");

            noiseSystemObject.AddComponent<NoiseSystem>();
        }
    }

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        if (playerObject != null)
        {
            player =
                playerObject.transform;
        }

        if (firePoint == null)
        {
            Transform foundFirePoint =
                transform.Find(
                    "FirePoint"
                );

            if (foundFirePoint != null)
            {
                firePoint =
                    foundFirePoint;
            }
        }

        ApplyEnemyTypeSettings();

        if (firePoint == null)
        {
            Transform[] children =
                GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                if (child == transform)
                    continue;

                if (child.name.ToLowerInvariant().Contains("firepoint") ||
                    child.name.ToLowerInvariant().Contains("muzzle") ||
                    child.name.ToLowerInvariant().Contains("weaponpoint"))
                {
                    firePoint = child;
                    break;
                }
            }
        }

        if (firePoint == null)
            firePoint = transform;

        lastKnownPlayerPosition =
            transform.position;

        investigationPosition =
            transform.position;

        initialSearchTimer = initialSearchDelay + Random.Range(0f, 0.35f);
        initialAmbush = Random.value < initialAmbushChance;
        lastMovementPosition = transform.position;
    }

    private void ApplyEnemyTypeSettings()
    {
        switch (enemyType)
        {
            case EnemyType.Bandit:
                rangedAttackDistance = 16f;
                rangedAttackCooldown = 0.75f;
                rangedAttackDamage = 6;
                detectionDistance = 16f;
                break;

            case EnemyType.Shooter:
                useCover = true;
                moveSpeed = 2.2f;
                rangedAttackDistance = 17f;
                rangedAttackCooldown = 0.8f;
                rangedAttackDamage = 7;
                coverStayTime = 5f;
                playerTooCloseDistance = 5f;
                shooterPreferredDistance = 8f;
                detectionDistance = 20f;
                break;

            case EnemyType.Rusher:
                moveSpeed *=
                    rusherSpeedMultiplier;

                useCover =
                    rusherUseCover;

                attackDistance = 1.7f;
                attackDamage = 15;
                detectionDistance = 20f;
                break;

            case EnemyType.Tactical:
                useCover = true;
                moveSpeed = 2.7f;
                rangedAttackDistance = 16f;
                rangedAttackCooldown = 0.8f;
                rangedAttackDamage = 6;

                coverStayTime =
                    Random.Range(
                        tacticalCoverTimeMin,
                        tacticalCoverTimeMax
                    );

                playerTooCloseDistance = 4.5f;
                detectionDistance = 22f;
                break;
        }

        baseAttackDamage =
            attackDamage;

        baseRangedAttackDamage =
            rangedAttackDamage;
    }

    public void SetEnemyType(
        EnemyType type
    )
    {
        enemyType =
            type;
    }

    public void SetElite(
        bool elite
    )
    {
        isElite =
            elite;

        if (!isElite)
        {
            attackDamage =
                Mathf.RoundToInt(
                    baseAttackDamage
                );

            rangedAttackDamage =
                Mathf.RoundToInt(
                    baseRangedAttackDamage
                );

            return;
        }

        moveSpeed *=
            eliteSpeedMultiplier;

        attackDamage =
            Mathf.RoundToInt(
                baseAttackDamage *
                eliteDamageMultiplier
            );

        rangedAttackDamage =
            Mathf.RoundToInt(
                baseRangedAttackDamage *
                eliteDamageMultiplier
            );

        Debug.Log(
            $"{gameObject.name}: ELITE. " +
            $"Melee={attackDamage}, " +
            $"Ranged={rangedAttackDamage}"
        );
    }

    public void ApplyWaveScaling(
        int wave
    )
    {
        if (wave < 1)
            wave = 1;

        float multiplier =
            1f +
            (wave - 1) *
            damageGrowthPerWave;

        multiplier =
            Mathf.Min(
                multiplier,
                maxDamageMultiplier
            );

        attackDamage =
            Mathf.RoundToInt(
                baseAttackDamage *
                multiplier
            );

        rangedAttackDamage =
            Mathf.RoundToInt(
                baseRangedAttackDamage *
                multiplier
            );

        if (isElite)
        {
            attackDamage =
                Mathf.RoundToInt(
                    attackDamage *
                    eliteDamageMultiplier
                );

            rangedAttackDamage =
                Mathf.RoundToInt(
                    rangedAttackDamage *
                    eliteDamageMultiplier
                );
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        if (tacticalVision != null)
        {
            tacticalVision.ScanNow();
        }

        UpdatePerception();
        UpdateInitialBehavior();

        switch (currentState)
        {
            case AIState.Combat:
                UpdateCombatState();
                break;

            case AIState.Searching:
                UpdateSearchingState();
                break;

            case AIState.Investigating:
                UpdateInvestigatingState();
                break;

            case AIState.RoomSearching:
                UpdateRoomSearchingState();
                break;

            case AIState.MovingToCover:
                UpdateMovingToCoverState();
                break;

            case AIState.InCover:
                UpdateInCoverState();
                break;

            case AIState.Flanking:
                UpdateFlankingState();
                break;
        }
    }

    private void UpdatePerception()
    {
        playerCurrentlyVisible =
            CanSeePlayer();

        if (tacticalVision != null &&
            tacticalVision.PlayerDetected)
        {
            playerCurrentlyVisible = true;
        }

        if (playerCurrentlyVisible)
        {
            lastKnownPlayerPosition =
                player.position;

            tacticalPlanner?.SetThreatPosition(
                player.position
            );

            lostPlayerTimer =
                0f;

            hadPlayerContact =
                true;

            if (
                currentState ==
                AIState.Searching ||
                currentState ==
                AIState.Investigating ||
                currentState ==
                AIState.RoomSearching
            )
            {
                ClearNavigationPath();

                searchPoints.Clear();

                roomSearchRoute.Clear();

                roomSearchWaypoints.Clear();

                currentSearchPoint = 0;

                currentRoomSearchWaypoint = 0;

                currentState =
                    AIState.Combat;

                tacticalPlanner?.ClearSearchMode();
                plannerRearmRequested = true;

                Debug.Log(
                    $"{gameObject.name}: PLAYER FOUND"
                );
            }

            return;
        }

        if (!hadPlayerContact)
            return;

        lostPlayerTimer +=
            Time.fixedDeltaTime;

        if (
            lostPlayerTimer >
            memoryDuration &&
            currentState ==
            AIState.Combat
        )
        {
            StartInvestigation();
        }
    }

    private void UpdateCombatState()
    {
        if (!playerCurrentlyVisible)
        {
            if (hadPlayerContact)
            {
                StartInvestigation();
            }

            return;
        }

        lostPlayerTimer =
            0f;

        FacePlayer();

        // Always give a visible enemy an immediate chance to fire before
        // tactical movement can move the AI into a cover state.
        TryRangedAttack();

        if (useCover && ShouldUseCombatCover())
        {
            if (Time.time >= nextRangedAttackTime)
                TryRangedAttack();

            EvaluateCoverQuickly();
        }

        if (
            currentState !=
            AIState.Combat
        )
        {
            return;
        }

        switch (enemyType)
        {
            case EnemyType.Bandit:
                UpdateBanditCombat();
                break;

            case EnemyType.Shooter:
                UpdateShooterCombat();
                break;

            case EnemyType.Rusher:
                UpdateRusherCombat();
                break;

            case EnemyType.Tactical:
                UpdateTacticalCombat();
                break;
        }
    }

    private void UpdateBanditCombat()
    {
        Vector3 target = GetCombatTargetPosition();
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= rangedAttackDistance)
            TryRangedAttack();

        if (distance <= attackDistance)
        {
            TryAttack();
            return;
        }

        if (distance > rangedAttackDistance * 0.82f)
            MoveTowardsKnownTarget(target, moveSpeed);
        else
            MoveInDirection(GetCombatStrafeDirection(), moveSpeed * 0.55f);
    }

    private void UpdateShooterCombat()
    {
        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        TryRangedAttack();

        if (distance < shooterRetreatDistance)
        {
            MoveAwayFromKnownTarget(player.position, moveSpeed);
            MoveInDirection(GetCombatStrafeDirection(), moveSpeed * 0.25f);
            return;
        }

        if (distance > shooterPreferredDistance + 1.5f)
        {
            MoveTowardsKnownTarget(GetCombatTargetPosition(), moveSpeed * 0.9f);
            return;
        }

        MoveInDirection(GetCombatStrafeDirection(), moveSpeed * 0.45f);
        FacePlayer();
    }

    private void UpdateRusherCombat()
    {
        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        TryRangedAttack();

        if (distance <= attackDistance)
        {
            TryAttack();
            return;
        }

        MoveTowardsKnownTarget(GetCombatTargetPosition(), moveSpeed);
    }

    private void UpdateTacticalCombat()
    {
        if (flanking)
        {
            currentState = AIState.Flanking;
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        TryRangedAttack();

        if (distance <= attackDistance)
        {
            TryAttack();
            return;
        }

        if (
            distance >= tacticalFlankStartDistance &&
            distance <= coverDetectionDistance &&
            Random.value < tacticalFlankChance * Time.fixedDeltaTime
        )
        {
            StartFlanking();
            return;
        }

        if (useCover && distance <= coverDetectionDistance)
        {
            EvaluateCoverQuickly();
            if (currentState != AIState.Combat)
                return;
        }

        if (distance > rangedAttackDistance * 0.8f)
            MoveTowardsKnownTarget(GetCombatTargetPosition(), moveSpeed);
        else
            MoveInDirection(GetCombatStrafeDirection(), moveSpeed * 0.5f);
    }

    private Vector3 GetCombatTargetPosition()
    {
        if (player == null)
            return transform.position;

        if (tacticalPlanner != null &&
            tacticalPlanner.TryGetTarget(
                out Vector3 plannerTarget
            ))
        {
            return plannerTarget;
        }

        UpdateCombatRole();

        return combatRoleTargetPosition;
    }

    private void UpdateCombatRole()
    {
        if (player == null)
            return;

        if (Time.time < nextCombatRoleRefreshTime &&
            combatRoleTargetPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, combatRoleTargetPosition) > combatPositionRepathDistance)
                return;
        }

        nextCombatRoleRefreshTime = Time.time + combatRoleRefreshInterval + Random.Range(0f, 0.25f);

        EnemyController[] enemies =
            FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        List<EnemyController> group = new List<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            if (enemy == null || enemy.player == null)
                continue;

            if (Vector3.Distance(enemy.transform.position, transform.position) <= groupRoleRadius)
                group.Add(enemy);
        }

        if (!group.Contains(this))
            group.Add(this);

        group.Sort((a, b) => a.GetInstanceID().CompareTo(b.GetInstanceID()));

        combatRoleIndex = Mathf.Max(0, group.IndexOf(this));
        combatRoleCount = Mathf.Max(1, group.Count);

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        if (toPlayer.sqrMagnitude < 0.01f)
            toPlayer = transform.forward;

        toPlayer.Normalize();

        Vector3 right = Vector3.Cross(Vector3.up, toPlayer).normalized;
        Vector3 back = -toPlayer;

        float radius = combatSpreadRadius;
        Vector3 offset;

        if (combatRoleCount == 1)
        {
            offset = Vector3.zero;
        }
        else
        {
            int slot = combatRoleIndex % 6;

            switch (slot)
            {
                case 0:
                    offset = right * radius;
                    break;

                case 1:
                    offset = -right * radius;
                    break;

                case 2:
                    offset = back * radius;
                    break;

                case 3:
                    offset = (right + back).normalized * radius;
                    break;

                case 4:
                    offset = (-right + back).normalized * radius;
                    break;

                default:
                    offset = back * (radius * 0.65f);
                    break;
            }
        }

        if (enemyType == EnemyType.Rusher)
            offset *= 0.45f;
        else if (enemyType == EnemyType.Shooter)
            offset *= 1.15f;
        else if (enemyType == EnemyType.Tactical)
            offset *= 1.2f;

        combatRoleTargetPosition = player.position + offset;
        combatRoleTargetPosition.y = transform.position.y;
    }

    private Vector3 GetCombatStrafeDirection()
    {
        if (player == null)
            return transform.forward;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        if (toPlayer.sqrMagnitude < 0.01f)
            return transform.forward;

        toPlayer.Normalize();
        Vector3 right = Vector3.Cross(Vector3.up, toPlayer).normalized;

        if ((GetInstanceID() & 1) == 0)
            return right;

        return -right;
    }

    private void UpdateInitialBehavior()
    {
        if (playerCurrentlyVisible || hadPlayerContact || initialSearchStarted)
            return;

        initialSearchTimer -= Time.fixedDeltaTime;
        if (initialSearchTimer > 0f)
            return;

        initialSearchStarted = true;
        initialAmbush = false;
        initialAmbushHolding = false;

        lastKnownPlayerPosition = transform.position;
        investigationPosition = transform.position;
        roomSearchRoute.Clear();
        roomSearchWaypoints.Clear();
        roomSearchIsTargeted = false;
        searchCycle = 0;

        CreateSearchPattern();
        currentState = AIState.Searching;
        initialSearchPauseTimer = initialSearchPause;

        tacticalPlanner?.SetSearchTarget(transform.position);
    }

    private void CreateInitialAmbushPoint()
    {
        initialAmbushPosition = Vector3.zero;

        if (CoverSystem.Instance != null)
        {
            Vector3 virtualThreat = transform.position + transform.forward * 6f;
            List<Vector3> candidates = CoverSystem.Instance.GetSearchPositions(
                virtualThreat,
                transform.position,
                5
            );

            float bestDistance = float.MaxValue;
            foreach (Vector3 rawCandidate in candidates)
            {
                Vector3 candidate = rawCandidate;
                candidate.y = transform.position.y;
                float distance = Vector3.Distance(transform.position, candidate);
                if (distance > 1f && distance < bestDistance)
                {
                    bestDistance = distance;
                    initialAmbushPosition = candidate;
                }
            }
        }

        if (initialAmbushPosition == Vector3.zero)
            initialAmbushPosition = transform.position;
    }

    private bool ShouldUseCombatCover()
    {
        switch (enemyType)
        {
            case EnemyType.Rusher:
                return false;

            case EnemyType.Bandit:
                if (currentCover != null)
                    return true;

                return Random.value < banditCoverChance * Time.fixedDeltaTime * 8f;

            case EnemyType.Shooter:
            case EnemyType.Tactical:
                return true;

            default:
                return useCover;
        }
    }

    private void EvaluateCoverQuickly()
    {
        if (
            !useCover ||
            CoverSystem.Instance == null
        )
        {
            return;
        }

        if (
            Time.time <
            nextCoverCheckTime
        )
        {
            return;
        }

        float interval =
            IsInImmediateDanger()
                ? dangerousCoverCheckInterval
                : coverCheckInterval;

        nextCoverCheckTime =
            Time.time +
            interval;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (
            distance >
            coverDetectionDistance
        )
        {
            return;
        }

        if (!playerCurrentlyVisible)
            return;

        CoverPoint bestCover =
            CoverSystem.Instance.GetBestCover(
                transform.position,
                player.position,
                this
            );

        if (bestCover == null)
            return;

        if (
            bestCover ==
            currentCover
        )
        {
            return;
        }

        if (
            !bestCover.TryReserve(
                this
            )
        )
        {
            return;
        }

        if (currentCover != null)
        {
            currentCover.Release(
                this
            );
        }

        currentCover =
            bestCover;

        float coverDistance =
            Vector3.Distance(
                transform.position,
                bestCover.transform.position
            );

        if (
            coverDistance <=
            coverReachDistance
        )
        {
            EnterCover();

            return;
        }

        movingToCover =
            true;

        inCover =
            false;

        ClearNavigationPath();

        currentState =
            AIState.MovingToCover;
    }

    private bool IsInImmediateDanger()
    {
        if (player == null)
            return false;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        return
            distance <=
            coverDangerDistance &&
            playerCurrentlyVisible;
    }

    public void OnNoiseHeard(
        Vector3 noisePosition,
        float strength,
        NoiseSystem.NoiseType noiseType
    )
    {
        if (player == null)
            return;

        if (playerCurrentlyVisible)
            return;

        noiseInvestigationPosition =
            noisePosition;

        noiseInvestigationPosition.y =
            transform.position.y;

        investigatingNoise = true;

        noiseInvestigationTimer =
            investigationDuration;

        investigationPosition =
            noiseInvestigationPosition;

        lastKnownPlayerPosition =
            noiseInvestigationPosition;

        investigationLookTimer =
            investigationLookTime;

        movingToCover = false;
        inCover = false;
        flanking = false;

        ReleaseCurrentCover();

        ClearNavigationPath();

        searchPoints.Clear();
        roomSearchRoute.Clear();
        roomSearchWaypoints.Clear();
        roomSearchIsTargeted = false;

        currentSearchPoint = 0;
        currentRoomSearchWaypoint = 0;
        searchPointTimer = 0f;
        roomSearchPauseTimer = 0f;

        currentState =
            AIState.Investigating;

        if (tacticalVision != null)
        {
            tacticalVision.LookAtNoise(
                noiseInvestigationPosition
            );
        }

        Debug.Log(
            $"{gameObject.name}: AI RECEIVED SOUND -> " +
            $"{noiseType} | Strength={strength:F2} | " +
            $"Position={noiseInvestigationPosition}"
        );
    }

    private void StartInvestigation()
    {
        if (player == null)
            return;

        if (
            currentState ==
            AIState.Searching ||
            currentState ==
            AIState.Investigating ||
            currentState ==
            AIState.RoomSearching
        )
        {
            return;
        }

        ReleaseCurrentCover();

        obstacleRouteActive = false;
        obstacleRouteTimer = 0f;

        movingToCover =
            false;

        inCover =
            false;

        flanking =
            false;

        ClearNavigationPath();

        searchPoints.Clear();

        roomSearchRoute.Clear();

        roomSearchWaypoints.Clear();

        currentSearchPoint =
            0;

        currentRoomSearchWaypoint =
            0;

        searchPointTimer =
            0f;

        roomSearchPauseTimer =
            0f;

        investigationPosition =
            lastKnownPlayerPosition;

        investigationTimer =
            investigationDuration;

        investigationLookTimer =
            investigationLookTime;

        ArenaModule currentRoom =
            FindRoomContainingPosition(
                transform.position
            );

        ArenaModule targetRoom =
            FindRoomContainingPosition(
                lastKnownPlayerPosition
            );

        if (
            currentRoom != null &&
            targetRoom != null &&
            currentRoom != targetRoom
        )
        {
            if (
                BuildRoomSearchRoute(
                    currentRoom,
                    targetRoom
                )
            )
            {
                currentState =
                    AIState.RoomSearching;

                if (tacticalPlanner == null ||
                    !tacticalPlanner.SearchMode)
                {
                    tacticalPlanner?.SetSearchTarget(lastKnownPlayerPosition);
                }

                Debug.Log(
                    $"{gameObject.name}: " +
                    $"PLAYER LOST. " +
                    $"Searching through {roomSearchRoute.Count} rooms."
                );

                return;
            }
        }

        currentState =
            AIState.Investigating;

        if (tacticalPlanner == null ||
            !tacticalPlanner.SearchMode)
        {
            tacticalPlanner?.SetSearchTarget(lastKnownPlayerPosition);
        }

        Debug.Log(
            $"{gameObject.name}: " +
            $"PLAYER LOST. " +
            $"Investigating last known position."
        );
    }

    private ArenaModule FindRoomContainingPosition(
        Vector3 position
    )
    {
        ArenaModule[] rooms =
            FindObjectsByType<ArenaModule>(
                FindObjectsSortMode.None
            );

        ArenaModule bestRoom =
            null;

        float bestDistance =
            float.MaxValue;

        foreach (
            ArenaModule room
            in rooms
        )
        {
            if (room == null)
                continue;

            Bounds bounds =
                room.GetWorldBounds();

            if (
                bounds.Contains(
                    position
                )
            )
            {
                return room;
            }

            Vector3 closest =
                bounds.ClosestPoint(
                    position
                );

            float distance =
                Vector3.Distance(
                    closest,
                    position
                );

            if (
                distance <
                bestDistance
            )
            {
                bestDistance =
                    distance;

                bestRoom =
                    room;
            }
        }

        return bestRoom;
    }

    private bool BuildRoomSearchRoute(
        ArenaModule startRoom,
        ArenaModule targetRoom
    )
    {
        roomSearchRoute.Clear();

        roomSearchWaypoints.Clear();

        if (
            startRoom == null ||
            targetRoom == null
        )
        {
            return false;
        }

        if (
            startRoom ==
            targetRoom
        )
        {
            return false;
        }

        System.Collections.Generic.Queue<ArenaModule> queue =
            new System.Collections.Generic.Queue<ArenaModule>();

        System.Collections.Generic.Dictionary<ArenaModule, ArenaModule> previous =
            new System.Collections.Generic.Dictionary<ArenaModule, ArenaModule>();

        queue.Enqueue(
            startRoom
        );

        previous[startRoom] =
            null;

        while (
            queue.Count > 0
        )
        {
            ArenaModule current =
                queue.Dequeue();

            if (
                current ==
                targetRoom
            )
            {
                break;
            }

            List<ArenaModule> neighbours =
                GetConnectedRooms(
                    current
                );

            foreach (
                ArenaModule neighbour
                in neighbours
            )
            {
                if (neighbour == null)
                    continue;

                if (
                    previous.ContainsKey(
                        neighbour
                    )
                )
                {
                    continue;
                }

                previous[neighbour] =
                    current;

                queue.Enqueue(
                    neighbour
                );
            }
        }

        if (
            !previous.ContainsKey(
                targetRoom
            )
        )
        {
            return false;
        }

        List<ArenaModule> reverseRoute =
            new List<ArenaModule>();

        ArenaModule room =
            targetRoom;

        while (room != null)
        {
            reverseRoute.Add(
                room
            );

            room =
                previous[room];
        }

        reverseRoute.Reverse();

        roomSearchRoute.AddRange(
            reverseRoute
        );

        for (
            int i = 0;
            i <
                roomSearchRoute.Count - 1;
            i++
        )
        {
            ArenaModule fromRoom =
                roomSearchRoute[i];

            ArenaModule toRoom =
                roomSearchRoute[i + 1];

            Transform fromConnection =
                FindConnectionToRoom(
                    fromRoom,
                    toRoom
                );

            Transform toConnection =
                FindConnectionToRoom(
                    toRoom,
                    fromRoom
                );

            if (
                fromConnection == null ||
                toConnection == null
            )
            {
                roomSearchRoute.Clear();

                roomSearchWaypoints.Clear();

                return false;
            }

            roomSearchWaypoints.Add(
                fromConnection.position
            );

            roomSearchWaypoints.Add(
                toConnection.position
            );
        }

        roomSearchWaypoints.Add(
            lastKnownPlayerPosition
        );

        roomSearchIsTargeted = roomSearchWaypoints.Count > 0;
        return roomSearchWaypoints.Count > 0;
    }

    private System.Collections.Generic.List<ArenaModule> GetConnectedRooms(
        ArenaModule room
    )
    {
        System.Collections.Generic.List<ArenaModule> result =
            new System.Collections.Generic.List<ArenaModule>();

        if (room == null)
            return result;

        ArenaModule[] rooms =
            FindObjectsByType<ArenaModule>(
                FindObjectsSortMode.None
            );

        foreach (
            ArenaModule otherRoom
            in rooms
        )
        {
            if (
                otherRoom == null ||
                otherRoom == room
            )
            {
                continue;
            }

            if (
                AreRoomsConnected(
                    room,
                    otherRoom
                )
            )
            {
                result.Add(
                    otherRoom
                );
            }
        }

        return result;
    }

    private bool AreRoomsConnected(
        ArenaModule first,
        ArenaModule second
    )
    {
        if (
            first.Connections == null ||
            second.Connections == null
        )
        {
            return false;
        }

        foreach (
            Transform firstConnection
            in first.Connections
        )
        {
            if (firstConnection == null)
                continue;

            foreach (
                Transform secondConnection
                in second.Connections
            )
            {
                if (secondConnection == null)
                    continue;

                float distance =
                    Vector3.Distance(
                        firstConnection.position,
                        secondConnection.position
                    );

                if (
                    distance <=
                    roomConnectionReachDistance
                )
                {
                    return true;
                }
            }
        }

        return false;
    }

    private Transform FindConnectionToRoom(
        ArenaModule room,
        ArenaModule targetRoom
    )
    {
        if (
            room == null ||
            targetRoom == null ||
            room.Connections == null ||
            targetRoom.Connections == null
        )
        {
            return null;
        }

        Transform bestConnection =
            null;

        float bestDistance =
            float.MaxValue;

        foreach (
            Transform connection
            in room.Connections
        )
        {
            if (connection == null)
                continue;

            foreach (
                Transform targetConnection
                in targetRoom.Connections
            )
            {
                if (targetConnection == null)
                    continue;

                float distance =
                    Vector3.Distance(
                        connection.position,
                        targetConnection.position
                    );

                if (
                    distance <
                    bestDistance
                )
                {
                    bestDistance =
                        distance;

                    bestConnection =
                        connection;
                }
            }
        }

        return bestConnection;
    }

    private void UpdateNoiseInvestigation()
    {
        if (!investigatingNoise)
            return;

        if (playerCurrentlyVisible)
        {
            investigatingNoise = false;
            noiseInvestigationTimer = 0f;
            currentState = AIState.Combat;
            return;
        }

        noiseInvestigationTimer -=
            Time.fixedDeltaTime;

        if (investigationLookTimer > 0f)
        {
            investigationLookTimer -=
                Time.fixedDeltaTime;

            FaceDirectionToward(
                noiseInvestigationPosition
            );
        }

        Vector3 direction =
            noiseInvestigationPosition -
            transform.position;

        direction.y = 0f;

        float distance =
            direction.magnitude;

        if (distance > searchPointReachDistance)
        {
            direction.Normalize();

            MoveTowardsKnownTarget(
                noiseInvestigationPosition,
                moveSpeed * searchMoveSpeedMultiplier
            );

            return;
        }

        FaceDirectionToward(
            noiseInvestigationPosition
        );

        if (noiseInvestigationTimer <= 0f)
        {
            investigatingNoise = false;

            investigationPosition =
                noiseInvestigationPosition;

            lastKnownPlayerPosition =
                noiseInvestigationPosition;

            investigationTimer =
                investigationDuration;

            investigationLookTimer =
                investigationLookTime;

            CreateSearchPattern();

            currentState =
                AIState.Searching;

            Debug.Log(
                $"{gameObject.name}: " +
                "NOISE INVESTIGATION COMPLETE -> SEARCHING"
            );
        }
    }

    private void UpdateInvestigatingState()
    {
        if (investigatingNoise)
        {
            UpdateNoiseInvestigation();

            if (currentState != AIState.Investigating)
                return;
        }

        if (playerCurrentlyVisible)
        {
            currentState =
                AIState.Combat;

            return;
        }

        investigationTimer -=
            Time.fixedDeltaTime;

        Vector3 direction =
            investigationPosition -
            transform.position;

        direction.y = 0f;

        float distance =
            direction.magnitude;

        if (
            distance >
            searchPointReachDistance
        )
        {
            MoveTowardsKnownTarget(
                investigationPosition,
                moveSpeed * searchMoveSpeedMultiplier
            );

            return;
        }

        investigationLookTimer -=
            Time.fixedDeltaTime;

        FaceDirectionToward(
            lastKnownPlayerPosition
        );

        if (
            investigationLookTimer <= 0f ||
            investigationTimer <= 0f
        )
        {
            CreateSearchPattern();

            currentState =
                AIState.Searching;
        }
    }

    private void CreateSearchPattern()
    {
        searchPoints.Clear();

        if (
            CoverSystem.Instance != null
        )
        {
            List<Vector3> coverSearchPoints =
                CoverSystem.Instance.GetSearchPositions(
                    lastKnownPlayerPosition,
                    transform.position,
                    searchPointsCount
                );

            foreach (
                Vector3 coverPoint
                in coverSearchPoints
            )
            {
                Vector3 searchPoint =
                    coverPoint;

                searchPoint.y =
                    transform.position.y;

                searchPoints.Add(
                    searchPoint
                );
            }
        }

        if (
            searchPoints.Count <
            searchPointsCount
        )
        {
            CreateFallbackSearchPoints();
        }

        currentSearchPoint =
            0;

        searchPointTimer =
            0f;

        searchScanAngle =
            transform.eulerAngles.y;

        Debug.Log(
            $"{gameObject.name}: " +
            $"SEARCH STARTED. " +
            $"Points={searchPoints.Count}"
        );
    }

    private void CreateFallbackSearchPoints()
    {
        Vector3 center = lastKnownPlayerPosition;
        ArenaModule currentRoom = FindRoomContainingPosition(transform.position);
        Bounds roomBounds = currentRoom != null ? currentRoom.GetWorldBounds() : new Bounds(transform.position, new Vector3(18f, 10f, 18f));
        int missing = Mathf.Max(0, searchPointsCount - searchPoints.Count);
        float margin = 1.35f;
        float minX = roomBounds.min.x + margin;
        float maxX = roomBounds.max.x - margin;
        float minZ = roomBounds.min.z + margin;
        float maxZ = roomBounds.max.z - margin;
        for (int i = 0; i < missing; i++)
        {
            float angle = (360f / Mathf.Max(1, missing)) * i + Random.Range(-25f, 25f);
            float radius = Random.Range(searchPointDistance * 0.55f, searchPointDistance * 1.15f);
            Vector3 offset = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
            Vector3 point = center + offset;
            point.x = Mathf.Clamp(point.x, minX, maxX);
            point.z = Mathf.Clamp(point.z, minZ, maxZ);
            point.y = transform.position.y;
            bool tooClose = false;
            foreach (Vector3 existing in searchPoints)
            {
                if (Vector3.Distance(existing, point) < searchPointReachDistance) { tooClose = true; break; }
            }
            if (!tooClose && IsSearchPointClear(point)) searchPoints.Add(point);
        }
        if (searchPoints.Count < searchPointsCount)
        {
            Vector3 fallback = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, Mathf.Clamp(transform.position.z, minZ, maxZ));
            if (IsSearchPointClear(fallback)) searchPoints.Add(fallback);
        }
    }

    private bool IsSearchPointClear(Vector3 point)
    {
        Vector3 direction = point - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;
        if (distance < 0.1f) return true;
        direction.Normalize();
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        if (!Physics.SphereCast(origin, obstacleProbeRadius, direction, out RaycastHit hit, distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) return true;
        return hit.collider.GetComponentInParent<EnemyController>() != null;
    }

    private void UpdateSearchingState()
    {
        if (playerCurrentlyVisible)
        {
            initialAmbushHolding = false;
            currentState = AIState.Combat;
            return;
        }

        if (plannerRearmRequested)
        {
            plannerRearmRequested = false;

            if (tacticalPlanner != null &&
                !tacticalPlanner.SearchMode &&
                !playerCurrentlyVisible)
            {
                tacticalPlanner.SetSearchTarget(transform.position);
            }
        }

        if (searchPoints.Count == 0)
            CreateSearchPattern();

        if (currentSearchPoint >= searchPoints.Count)
        {
            if (initialAmbushHolding)
                return;

            if (initialSearchStarted && !hadPlayerContact)
            {
                initialSearchPauseTimer -= Time.fixedDeltaTime;
                if (initialSearchPauseTimer > 0f)
                    return;

                if (TryMoveToAnotherRoom())
                    return;

                CreateSearchPattern();
                return;
            }

            if (searchCycle < maxSearchCyclesBeforeMovingOn)
            {
                searchCycle++;
                CreateExpandedSearchPattern();
                return;
            }

            if (TryMoveToAnotherRoom())
                return;

            searchCycle = 0;
            CreateExpandedSearchPattern();
            return;
        }

        Vector3 target;

        if (tacticalPlanner != null &&
            tacticalPlanner.SearchMode &&
            tacticalPlanner.TryGetSearchTarget(out Vector3 searchTgt))
        {
            target = searchTgt;

            if (Time.time >= nextSearchProgressLogTime)
            {
                nextSearchProgressLogTime = Time.time + 2f;

                Vector3 dbgToTarget = searchTgt - transform.position;
                dbgToTarget.y = 0f;

                Vector3 dbgToWp = dbgLastSteerTarget - transform.position;
                dbgToWp.y = 0f;

                Debug.Log(
                    "[SEARCH PROGRESS] " +
                    "name=" + gameObject.name + " " +
                    "id=" + GetInstanceID() + " " +
                    "position=(" +
                    transform.position.x.ToString("F1") + "," +
                    transform.position.y.ToString("F1") + "," +
                    transform.position.z.ToString("F1") + ") " +
                    "searchTarget=(" +
                    searchTgt.x.ToString("F1") + "," +
                    searchTgt.y.ToString("F1") + "," +
                    searchTgt.z.ToString("F1") + ") " +
                    "distanceXZ=" + dbgToTarget.magnitude.ToString("F2") + " " +
                    "state=" + currentState + " " +
                    "velocity=(" +
                    rb.linearVelocity.x.ToString("F2") + "," +
                    rb.linearVelocity.z.ToString("F2") + ") " +
                    "currentWaypointIndex=" + currentPathIndex + " " +
                    "waypointCount=" + currentPathWaypoints.Count + " " +
                    "waypointPosition=(" +
                    dbgLastSteerTarget.x.ToString("F1") + "," +
                    dbgLastSteerTarget.y.ToString("F1") + "," +
                    dbgLastSteerTarget.z.ToString("F1") + ") " +
                    "distanceToWaypointXZ=" + dbgToWp.magnitude.ToString("F2") + " " +
                    "movementDirection=(" +
                    dbgLastMidDirection.x.ToString("F2") + "," +
                    dbgLastMidDirection.z.ToString("F2") + ") " +
                    "movementSpeed=" + dbgLastMoveSpeed.ToString("F2") + " " +
                    "finalMoveDirection=(" +
                    dbgLastFinalDirection.x.ToString("F2") + "," +
                    dbgLastFinalDirection.z.ToString("F2") + ")"
                );
            }
        }
        else
        {
            target = searchPoints[currentSearchPoint];
        }

        Vector3 direction = target - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;

        if (distance > searchPointReachDistance)
        {
            MoveTowardsKnownTarget(target, moveSpeed * searchMoveSpeedMultiplier);
            return;
        }

        if (tacticalPlanner != null &&
            tacticalPlanner.SearchMode &&
            tacticalPlanner.HasSearchTarget)
        {
            Debug.Log(
                $"[SEARCH REACHED] {gameObject.name} " +
                $"position=({transform.position.x:F1},{transform.position.y:F1},{transform.position.z:F1}) " +
                $"target=({target.x:F1},{target.y:F1},{target.z:F1})"
            );
        }

        if (initialAmbushHolding)
        {
            FaceDirectionToward(transform.position + transform.forward * 3f);
            initialAmbushHoldTimer -= Time.fixedDeltaTime;
            if (initialAmbushHoldTimer <= 0f)
            {
                initialAmbushHolding = false;
                currentSearchPoint = searchPoints.Count;
            }
            return;
        }

        bool usingPlannerTarget =
            tacticalPlanner != null &&
            tacticalPlanner.SearchMode &&
            tacticalPlanner.HasSearchTarget;

        bool routineQuickScan =
            usingPlannerTarget &&
            !hadPlayerContact &&
            !investigatingNoise;

        searchPointTimer += Time.fixedDeltaTime;
        float scanDuration = routineQuickScan
            ? 0.5f
            : Mathf.Max(0.35f, searchScanDuration);
        if (searchPointTimer < scanDuration)
        {
            searchScanAngle += searchScanSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.Euler(0f, searchScanAngle, 0f);
            return;
        }
        searchPointTimer = 0f;

        if (usingPlannerTarget)
        {
            tacticalPlanner?.RequestNextSearchTargets();
            currentSearchPoint = 0;
            searchPointTimer = 0f;
        }
        else
        {
            currentSearchPoint++;
        }

        searchScanAngle = transform.eulerAngles.y;
    }

    private void CreateExpandedSearchPattern()
    {
        searchPoints.Clear();

        Vector3 center = lastKnownPlayerPosition;
        center += Random.insideUnitSphere * searchExpansionDistance;
        center.y = transform.position.y;

        for (int i = 0; i < searchPointsCount; i++)
        {
            float angle = (360f / Mathf.Max(1, searchPointsCount)) * i + Random.Range(-25f, 25f);
            float radius = Random.Range(searchPointDistance, searchPointDistance * 1.5f);
            Vector3 point = center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
            point.y = transform.position.y;
            searchPoints.Add(point);
        }

        currentSearchPoint = 0;
        searchPointTimer = 0f;
    }

    private bool TryMoveToAnotherRoom()
    {
        ArenaModule currentRoom = FindRoomContainingPosition(transform.position);
        if (currentRoom == null)
            return false;

        List<ArenaModule> neighbours = GetConnectedRooms(currentRoom);
        if (neighbours.Count == 0)
            return false;

        ArenaModule nextRoom = neighbours[Random.Range(0, neighbours.Count)];
        Transform connection = FindConnectionToRoom(currentRoom, nextRoom);
        Transform targetConnection = FindConnectionToRoom(nextRoom, currentRoom);

        if (connection == null || targetConnection == null)
            return false;

        roomSearchRoute.Clear();
        roomSearchWaypoints.Clear();
        roomSearchWaypoints.Add(connection.position);
        roomSearchWaypoints.Add(targetConnection.position);
        currentRoomSearchWaypoint = 0;
        roomSearchPauseTimer = 0f;
        roomSearchIsTargeted = false;
        currentState = AIState.RoomSearching;
        return true;
    }

    private void UpdateRoomSearchingState()
    {
        if (playerCurrentlyVisible)
        {
            ClearNavigationPath();

            roomSearchRoute.Clear();

            roomSearchWaypoints.Clear();

            currentRoomSearchWaypoint =
                0;

            currentState =
                AIState.Combat;

            return;
        }

        if (
            roomSearchWaypoints.Count == 0
        )
        {
            StartInvestigation();

            return;
        }

        if (
            currentRoomSearchWaypoint >=
            roomSearchWaypoints.Count
        )
        {
            roomSearchWaypoints.Clear();
            currentRoomSearchWaypoint = 0;
            roomSearchPauseTimer = 0f;

            if (!roomSearchIsTargeted)
            {
                lastKnownPlayerPosition = transform.position;
                investigationPosition = transform.position;
                searchCycle = 0;
                CreateSearchPattern();
                currentState = AIState.Searching;

                if (tacticalPlanner == null ||
                    !tacticalPlanner.SearchMode)
                {
                    tacticalPlanner?.SetSearchTarget(transform.position);
                }

                return;
            }

            investigationPosition = lastKnownPlayerPosition;
            investigationTimer = investigationDuration;
            investigationLookTimer = investigationLookTime;
            currentState = AIState.Investigating;

            if (tacticalPlanner == null ||
                !tacticalPlanner.SearchMode)
            {
                tacticalPlanner?.SetSearchTarget(lastKnownPlayerPosition);
            }

            roomSearchIsTargeted = false;
            return;
        }

        Vector3 target =
            roomSearchWaypoints[
                currentRoomSearchWaypoint
            ];

        target.y =
            transform.position.y;

        Vector3 direction =
            target -
            transform.position;

        direction.y = 0f;

        float distance =
            direction.magnitude;

        if (
            distance >
            roomConnectionReachDistance
        )
        {
            MoveTowardsKnownTarget(
                target,
                moveSpeed * roomSearchSpeedMultiplier
            );

            return;
        }

        rb.linearVelocity =
            Vector3.zero;

        FaceDirectionToward(
            target
        );

        roomSearchPauseTimer +=
            Time.fixedDeltaTime;

        float roomPause = roomSearchIsTargeted
            ? roomSearchPause
            : 0.3f;

        if (
            roomSearchPauseTimer <
            roomPause
        )
        {
            return;
        }

        roomSearchPauseTimer =
            0f;

        currentRoomSearchWaypoint++;

        if (
            currentRoomSearchWaypoint >=
            roomSearchWaypoints.Count
        )
        {
            investigationPosition =
                lastKnownPlayerPosition;

            investigationTimer =
                investigationDuration;

            investigationLookTimer =
                investigationLookTime;

            currentState =
                AIState.Investigating;

            Debug.Log(
                $"{gameObject.name}: " +
                "Reached player's last known room."
            );
        }
    }

    private void StartFlanking()
    {
        if (player == null)
            return;

        flankLeft =
            Random.value >
            0.5f;

        Vector3 toPlayer =
            player.position -
            transform.position;

        toPlayer.y = 0f;

        if (
            toPlayer.sqrMagnitude <
            0.01f
        )
        {
            return;
        }

        toPlayer.Normalize();

        Vector3 side =
            Vector3.Cross(
                Vector3.up,
                toPlayer
            ).normalized;

        if (!flankLeft)
        {
            side = -side;
        }

        Vector3 target =
            player.position +
            side *
            tacticalFlankDistance;

        target.y =
            transform.position.y;

        flankTargetPosition =
            target;

        flankTimer =
            tacticalFlankDuration;

        flanking =
            true;

        ClearNavigationPath();

        currentState =
            AIState.Flanking;

        Debug.Log(
            $"{gameObject.name}: TACTICAL FLANK STARTED"
        );
    }

    private void UpdateFlankingState()
    {
        if (!playerCurrentlyVisible)
        {
            flanking =
                false;

            StartInvestigation();

            return;
        }

        UpdateFlanking();
    }

    private void UpdateFlanking()
    {
        if (player == null)
            return;

        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (
            distanceToPlayer <=
            playerTooCloseDistance
        )
        {
            flanking =
                false;

            currentState =
                AIState.Combat;

            return;
        }

        flankTimer -=
            Time.fixedDeltaTime;

        if (
            flankTimer <=
            0f
        )
        {
            flanking =
                false;

            currentState =
                AIState.Combat;

            return;
        }

        Vector3 toSide =
            flankTargetPosition -
            transform.position;

        toSide.y = 0f;

        float sideDistance =
            toSide.magnitude;

        if (
            sideDistance >
            tacticalFlankReachDistance
        )
        {
            TryRangedAttack();
            MoveTowardsKnownTarget(
                flankTargetPosition,
                moveSpeed
            );

            return;
        }

        FacePlayer();

        TryRangedAttack();

        if (
            distanceToPlayer <=
            shooterPreferredDistance
        )
        {
            flanking =
                false;

            currentState =
                AIState.Combat;
        }
    }

    private void UpdateMovingToCoverState()
    {
        if (currentCover == null)
        {
            movingToCover =
                false;

            inCover =
                false;

            currentState =
                AIState.Combat;

            return;
        }

        if (!playerCurrentlyVisible)
        {
            ReleaseCurrentCover();

            movingToCover =
                false;

            inCover =
                false;

            StartInvestigation();

            return;
        }

        // Keep fighting while moving to cover. Cover selection must never
        // turn a visible enemy into a non-shooting target.
        TryRangedAttack();
        MoveToCover();

        if (!movingToCover)
        {
            currentState =
                AIState.InCover;
        }
    }

    private void UpdateInCoverState()
    {
        if (currentCover == null)
        {
            inCover =
                false;

            currentState =
                AIState.Combat;

            return;
        }

        if (!playerCurrentlyVisible)
        {
            ExitCoverForSearch();

            StartInvestigation();

            return;
        }

        StayInCover();
    }

    private void MoveToCover()
    {
        if (currentCover == null)
        {
            movingToCover = false;
            return;
        }

        Vector3 direction = currentCover.transform.position - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;

        if (distance <= coverReachDistance)
        {
            EnterCover();
            return;
        }

        MoveTowardsKnownTarget(currentCover.transform.position, moveSpeed);
    }

    private void EnterCover()
    {
        movingToCover =
            false;

        inCover =
            true;

        peekState =
            PeekState.Waiting;

        peekTimer =
            peekWaitTime;

        coverLeaveTime =
            Time.time +
            coverStayTime;

        rb.linearVelocity =
            Vector3.zero;

        rb.angularVelocity =
            Vector3.zero;

        coverPosition =
            transform.position;

        FacePlayer();

        ClearNavigationPath();

        currentState =
            AIState.InCover;
    }

    private void StayInCover()
    {
        if (currentCover == null)
        {
            ExitCover();

            return;
        }

        if (!playerCurrentlyVisible)
        {
            ExitCoverForSearch();

            StartInvestigation();

            return;
        }

        float playerDistance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (
            playerDistance <=
            playerTooCloseDistance
        )
        {
            ExitCover();

            return;
        }

        // A visible enemy in cover should still engage; do not wait for the
        // peek timer before allowing the first shot.
        TryRangedAttack();

        if (
            Time.time >=
            coverLeaveTime
        )
        {
            ExitCover();

            return;
        }

        switch (peekState)
        {
            case PeekState.Waiting:
                UpdatePeekWaiting();
                break;

            case PeekState.MovingOut:
                MoveOutToPeek();
                break;

            case PeekState.Shooting:
                UpdatePeekShooting();
                break;

            case PeekState.MovingBack:
                MoveBackToCover();
                break;
        }
    }

    private void UpdatePeekWaiting()
    {
        FacePlayer();

        peekTimer -=
            Time.fixedDeltaTime;

        if (
            peekTimer <=
            0f
        )
        {
            StartPeek();
        }
    }

    private void StartPeek()
    {
        if (currentCover == null)
            return;

        peekLeft =
            Random.value >
            0.5f;

        peekTargetPosition =
            currentCover.GetPeekPosition(
                player.position,
                peekLeft
            );

        peekTargetPosition.y =
            transform.position.y;

        peekState =
            PeekState.MovingOut;
    }

    private void MoveOutToPeek()
    {
        if (!playerCurrentlyVisible)
        {
            peekState =
                PeekState.MovingBack;

            return;
        }

        Vector3 direction =
            peekTargetPosition -
            transform.position;

        direction.y = 0f;

        float distance =
            direction.magnitude;

        if (
            distance <=
            returnToCoverDistance
        )
        {
            rb.linearVelocity =
                Vector3.zero;

            peekState =
                PeekState.Shooting;

            peekReturnTimer =
                peekTime;

            FacePlayer();

            return;
        }

        direction.Normalize();

        Vector3 newPosition =
            rb.position +
            direction *
            peekMoveSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(
            newPosition
        );

        if (
            direction.sqrMagnitude >
            0.01f
        )
        {
            transform.forward =
                direction;
        }

        FacePlayer();

        TryRangedAttack();
    }

    private void UpdatePeekShooting()
    {
        if (!playerCurrentlyVisible)
        {
            peekState =
                PeekState.MovingBack;

            return;
        }

        FacePlayer();

        TryRangedAttack();

        peekReturnTimer -=
            Time.fixedDeltaTime;

        if (
            peekReturnTimer <=
            0f
        )
        {
            peekState =
                PeekState.MovingBack;
        }
    }

    private void MoveBackToCover()
    {
        if (currentCover == null)
        {
            ExitCover();
            return;
        }

        Vector3 direction = currentCover.transform.position - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;

        if (distance <= returnToCoverDistance)
        {
            rb.linearVelocity = Vector3.zero;
            peekState = PeekState.Waiting;
            peekTimer = peekWaitTime;
            return;
        }

        MoveTowardsKnownTarget(currentCover.transform.position, peekMoveSpeed);
    }

    private void MoveAwayFromKnownTarget(
        Vector3 target,
        float speed
    )
    {
        Vector3 direction =
            transform.position - target;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            direction = -transform.forward;
            direction.y = 0f;
        }

        if (direction.sqrMagnitude < 0.01f)
            return;

        direction.Normalize();

        MoveInDirection(direction, speed);
    }

    private void MoveTowardsKnownTarget(
        Vector3 target,
        float speed
    )
    {
        float directDistance =
            Vector3.Distance(
                transform.position,
                target
            );

        if (directDistance < aStarActivationDistance)
        {
            dbgLastSteerTarget = target;
            MoveDirectlyToTarget(target, speed);
            return;
        }

        if (
            TryGetPathToTarget(
                target,
                out Vector3 nextWaypoint
            ))
        {
            dbgLastSteerTarget = nextWaypoint;
            MoveDirectlyToTarget(nextWaypoint, speed);
        }
        else
        {
            dbgLastSteerTarget = target;
            MoveDirectlyToTarget(target, speed);
        }
    }

    private void MoveInDirection(
        Vector3 desiredDirection,
        float speed
    )
    {
        desiredDirection.y = 0f;
        if (desiredDirection.sqrMagnitude < 0.001f) { dbgLastMidDirection = Vector3.zero; dbgLastFinalDirection = Vector3.zero; dbgLastMoveSpeed = 0f; rb.linearVelocity = Vector3.zero; return; }
        desiredDirection.Normalize();
        Vector3 movementDirection = desiredDirection;
        if (tacticalVision != null)
        {
            Vector3 tacticalDirection = tacticalVision.GetTacticalDirection(desiredDirection);
            if (tacticalDirection.sqrMagnitude >= 0.001f) movementDirection = Vector3.Lerp(desiredDirection, tacticalDirection.normalized, 0.55f).normalized;
        }
        movementDirection = ApplyEnemySeparation(movementDirection);
        movementDirection = ApplyObstacleAvoidance(movementDirection);
        dbgLastMidDirection = movementDirection;
        if (movementDirection.sqrMagnitude < 0.001f)
        {
            Vector3 fallback = Vector3.Cross(Vector3.up, desiredDirection).normalized;
            if ((GetInstanceID() & 1) != 0) fallback = -fallback;
            movementDirection = ApplyObstacleAvoidance(fallback);
            if (movementDirection.sqrMagnitude < 0.001f) movementDirection = fallback;
        }
        float actualSpeed = Mathf.Max(0f, speed);
        dbgLastFinalDirection = movementDirection;
        dbgLastMoveSpeed = actualSpeed;
        Vector3 newPosition = rb.position + movementDirection * actualSpeed * Time.fixedDeltaTime;
        if (Vector3.Distance(rb.position, lastMovementPosition) < 0.01f) obstacleStuckTimer += Time.fixedDeltaTime; else obstacleStuckTimer = 0f;
        lastMovementPosition = rb.position;
        rb.MovePosition(newPosition);
        if (movementDirection.sqrMagnitude > 0.001f) transform.forward = movementDirection;
    }

    private void ClearNavigationPath()
    {
        currentPathWaypoints.Clear();
        currentPathIndex = 0;
        pathTargetPosition = Vector3.zero;
        nextPathRefreshTime = 0f;
        nextEmergencyRebuildTime = 0f;
    }

    private bool TryGetPathToTarget(
        Vector3 target,
        out Vector3 nextWaypoint)
    {
        nextWaypoint = transform.position;

        bool pathEmpty =
            currentPathWaypoints.Count == 0;

        bool pathFinished =
            currentPathIndex >=
            currentPathWaypoints.Count;

        bool pathExpired =
            Time.time >= nextPathRefreshTime;

        bool targetChanged =
            Vector3.Distance(
                target,
                pathTargetPosition
            ) > pathRebuildDistance;

        bool emergencyAllowed =
            Time.time >= nextEmergencyRebuildTime;

        bool hardRebuild =
            pathEmpty ||
            pathFinished ||
            (targetChanged && emergencyAllowed);

        if (hardRebuild)
        {
            BuildPathToTarget(target);

            if (targetChanged && emergencyAllowed)
            {
                nextEmergencyRebuildTime =
                    Time.time +
                    emergencyRebuildCooldown;
            }
        }
        else if (pathExpired)
        {
            RefreshPathIfBetter(target);
        }

        if (
            currentPathIndex <
            currentPathWaypoints.Count)
        {
            nextWaypoint =
                currentPathWaypoints[
                    currentPathIndex
                ];

            float distance =
                Vector3.Distance(
                    transform.position,
                    nextWaypoint
                );

            if (distance <= waypointReachDistance)
            {
                currentPathIndex++;
            }
        }
        else
        {
            nextWaypoint = target;
        }

        return currentPathWaypoints.Count > 0;
    }

    private bool BuildPathToTarget(Vector3 target)
    {
        currentPathWaypoints.Clear();
        currentPathIndex = 0;

        if (ArenaTacticalMap.Instance == null)
        {
            pathTargetPosition = target;
            nextPathRefreshTime =
                Time.time + pathRefreshInterval;
            return false;
        }

        bool found =
            ArenaTacticalMap.Instance.TryFindPath(
                transform.position,
                target,
                currentPathWaypoints
            );

        pathTargetPosition = target;
        nextPathRefreshTime =
            Time.time + pathRefreshInterval;

        bool success = found && currentPathWaypoints.Count > 0;

        if (success)
        {
            Debug.Log(
                $"[A* PATH] {gameObject.name} " +
                $"start=({transform.position.x:F1},{transform.position.y:F1},{transform.position.z:F1}) " +
                $"target=({target.x:F1},{target.y:F1},{target.z:F1}) " +
                $"waypoints={currentPathWaypoints.Count}"
            );
        }

        return success;
    }

    private void RefreshPathIfBetter(Vector3 target)
    {
        List<Vector3> candidatePath =
            new List<Vector3>();

        bool found = false;

        if (ArenaTacticalMap.Instance != null)
        {
            found =
                ArenaTacticalMap.Instance.TryFindPath(
                    transform.position,
                    target,
                    candidatePath
                );
        }

        nextPathRefreshTime =
            Time.time + pathRefreshInterval;

        if (!found || candidatePath.Count == 0)
        {
            return;
        }

        float remainingCurrent =
            MeasurePathLength(
                currentPathWaypoints,
                currentPathIndex
            );

        float candidateLength =
            MeasurePathLength(
                candidatePath,
                0
            );

        if (candidateLength < remainingCurrent * 0.85f)
        {
            currentPathWaypoints.Clear();
            currentPathWaypoints.AddRange(candidatePath);
            currentPathIndex = 0;
            pathTargetPosition = target;
        }
    }

    private float MeasurePathLength(
        System.Collections.Generic.List<Vector3> path,
        int startIndex)
    {
        if (path == null ||
            path.Count == 0 ||
            startIndex < 0 ||
            startIndex >= path.Count)
        {
            return float.MaxValue;
        }

        float length =
            Vector3.Distance(
                transform.position,
                path[startIndex]
            );

        for (
            int i = startIndex;
            i < path.Count - 1;
            i++)
        {
            length +=
                Vector3.Distance(
                    path[i],
                    path[i + 1]
                );
        }

        return length;
    }

    private void MoveDirectlyToTarget(
        Vector3 target,
        float speed)
    {
        Vector3 direction =
            target - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        direction.Normalize();

        MoveInDirection(direction, speed);
    }

    private Vector3 ApplyEnemySeparation(Vector3 direction)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, enemySeparationRadius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        Vector3 push = Vector3.zero;
        foreach (Collider hit in hits)
        {
            if (hit == null) continue;
            EnemyController other = hit.GetComponentInParent<EnemyController>();
            if (other == null || other == this) continue;
            Vector3 away = transform.position - other.transform.position;
            away.y = 0f;
            float distance = away.magnitude;
            if (distance > 0.01f) push += away.normalized * ((enemySeparationRadius - distance) / enemySeparationRadius);
        }
        if (push.sqrMagnitude > 0.001f) direction = (direction + push.normalized * enemySeparationStrength).normalized;
        return direction;
    }

    private Vector3 ApplyObstacleAvoidance(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return Vector3.zero;
        direction.Normalize();
        Vector3 origin = transform.position + Vector3.up * 0.65f;
        int layers = Physics.DefaultRaycastLayers;

        if (obstacleRouteActive)
        {
            obstacleRouteTimer -= Time.fixedDeltaTime;
            if (obstacleRouteTimer > 0f)
            {
                if (!Physics.SphereCast(origin, obstacleProbeRadius, obstacleRouteDirection, out RaycastHit routeHit, obstacleRouteProbeDistance, layers, QueryTriggerInteraction.Ignore) || routeHit.collider.GetComponentInParent<EnemyController>() != null) return obstacleRouteDirection;
            }
            obstacleRouteActive = false;
        }

        if (!Physics.SphereCast(origin, obstacleProbeRadius, direction, out RaycastHit hit, obstacleRouteProbeDistance, layers, QueryTriggerInteraction.Ignore)) { obstacleStuckTimer = 0f; return direction; }
        if (hit.collider.GetComponentInParent<EnemyController>() != null) { obstacleStuckTimer = 0f; return direction; }

        Vector3[] candidates = {
            Quaternion.Euler(0f, -obstacleRouteProbeAngle, 0f) * direction,
            Quaternion.Euler(0f, obstacleRouteProbeAngle, 0f) * direction,
            Quaternion.Euler(0f, -90f, 0f) * direction,
            Quaternion.Euler(0f, 90f, 0f) * direction,
            Quaternion.Euler(0f, -120f, 0f) * direction,
            Quaternion.Euler(0f, 120f, 0f) * direction
        };
        float bestScore = float.NegativeInfinity;
        Vector3 bestDirection = Vector3.zero;
        Vector3 targetBias = Flatten(GetMovementGoalPosition() - transform.position);
        if (targetBias.sqrMagnitude > 0.001f) targetBias.Normalize();
        for (int i = 0; i < candidates.Length; i++)
        {
            Vector3 candidate = candidates[i]; candidate.y = 0f;
            if (candidate.sqrMagnitude < 0.001f) continue;
            candidate.Normalize();
            bool blocked = Physics.SphereCast(origin, obstacleProbeRadius, candidate, out RaycastHit candidateHit, obstacleRouteProbeDistance, layers, QueryTriggerInteraction.Ignore);
            if (blocked && candidateHit.collider.GetComponentInParent<EnemyController>() != null) blocked = false;
            if (blocked) continue;
            float alignment = Vector3.Dot(direction, candidate);
            float goalAlignment = targetBias.sqrMagnitude > 0.001f ? Vector3.Dot(targetBias, candidate) : 0f;
            float score = alignment * 1.5f + goalAlignment * 1.8f;
            if (score > bestScore) { bestScore = score; bestDirection = candidate; }
        }
        if (bestDirection.sqrMagnitude > 0.001f)
        {
            obstacleRouteActive = true; obstacleRouteDirection = bestDirection; obstacleRouteTimer = Mathf.Max(0.35f, obstacleRouteCommitTime); obstacleStuckTimer = 0f; return bestDirection;
        }
        Vector3 wallNormal = hit.normal; wallNormal.y = 0f;
        if (wallNormal.sqrMagnitude > 0.01f)
        {
            Vector3 slide = Vector3.ProjectOnPlane(direction, wallNormal).normalized;
            if (slide.sqrMagnitude > 0.001f) { obstacleRouteActive = true; obstacleRouteDirection = slide; obstacleRouteTimer = Mathf.Max(0.3f, obstacleRouteCommitTime * 0.75f); return slide; }
        }
        obstacleStuckTimer += Time.fixedDeltaTime;
        if (obstacleStuckTimer < obstacleStuckTime) return Vector3.zero;
        obstacleStuckTimer = 0f;
        return Vector3.Cross(Vector3.up, direction).normalized;
    }

    private Vector3 GetMovementGoalPosition()
    {
        if (currentState == AIState.Investigating) return investigationPosition;
        if (currentState == AIState.RoomSearching && currentRoomSearchWaypoint < roomSearchWaypoints.Count) return roomSearchWaypoints[currentRoomSearchWaypoint];
        if (currentState == AIState.Searching && currentSearchPoint < searchPoints.Count) return searchPoints[currentSearchPoint];
        if (currentState == AIState.MovingToCover && currentCover != null) return currentCover.transform.position;
        if (currentState == AIState.Flanking) return flankTargetPosition;
        if (player != null) return player.position;
        return transform.position;
    }

    private Vector3 Flatten(Vector3 value) { value.y = 0f; return value; }

    private void FacePlayer()
    {
        if (player == null || !playerCurrentlyVisible)
            return;

        FaceDirectionToward(
            player.position
        );
    }

    private void FaceDirectionToward(
        Vector3 target
    )
    {
        Vector3 direction =
            target -
            transform.position;

        direction.y = 0f;

        if (
            direction.sqrMagnitude >
            0.01f
        )
        {
            transform.forward =
                direction.normalized;
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        if (tacticalVision == null)
            return false;

        if (tacticalVision.PlayerDetected)
            return true;

        bool directVision =
            tacticalVision.HasDirectVisionToPlayer();

        if (directVision)
        {
            tacticalVision.SetPlayerDetected(true);
            return true;
        }

        return false;
    }

    private void TryRangedAttack()
    {
        if (
            Time.time <
            nextRangedAttackTime
        )
        {
            return;
        }

        if (
            enemyBulletPrefab == null ||
            firePoint == null ||
            player == null
        )
        {
            return;
        }

        if (!playerCurrentlyVisible)
            return;

        if (tacticalVision == null)
            return;

        if (!playerCurrentlyVisible)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (
            distance >
            rangedAttackDistance
        )
        {
            return;
        }

        Vector3 target =
            player.position +
            Vector3.up;

        Vector3 shootDirection =
            target -
            firePoint.position;

        if (
            shootDirection.sqrMagnitude <=
            0.001f
        )
        {
            return;
        }

        shootDirection.Normalize();

        Vector3 spawnPosition =
            firePoint.position +
            shootDirection *
            bulletSpawnOffset;

        GameObject bullet =
            Instantiate(
                enemyBulletPrefab,
                spawnPosition,
                Quaternion.LookRotation(
                    shootDirection
                )
            );

        EnemyBullet enemyBullet =
            bullet.GetComponent<EnemyBullet>();

        if (enemyBullet != null)
        {
            enemyBullet.Initialize(
                shootDirection,
                rangedAttackDamage
            );
        }

        nextRangedAttackTime =
            Time.time +
            rangedAttackCooldown;
    }

    private void TryAttack()
    {
        if (player == null)
            return;

        Vector3 offset =
            player.position -
            transform.position;

        offset.y = 0f;

        if (
            offset.magnitude >
            attackDistance
        )
        {
            return;
        }

        if (
            Time.time <
            nextAttackTime
        )
        {
            return;
        }

        if (!playerCurrentlyVisible)
            return;

        if (tacticalVision == null ||
            !tacticalVision.PlayerDetected)
            return;

        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(
                attackDamage
            );

            nextAttackTime =
                Time.time +
                attackCooldown;
        }
    }

    private bool distanceToPlayerForRanged()
    {
        if (player == null)
            return false;

        return
            Vector3.Distance(
                transform.position,
                player.position
            ) <=
            rangedAttackDistance;
    }

    private void ExitCover()
    {
        ReleaseCurrentCover();

        inCover =
            false;

        movingToCover =
            false;

        peekState =
            PeekState.None;

        currentState =
            AIState.Combat;

        nextCoverCheckTime =
            Time.time +
            0.5f;
    }

    private void ExitCoverForSearch()
    {
        ReleaseCurrentCover();

        inCover =
            false;

        movingToCover =
            false;

        peekState =
            PeekState.None;
    }

    private void ReleaseCurrentCover()
    {
        if (currentCover != null)
        {
            if (
                enemyType ==
                EnemyType.Tactical
            )
            {
                lastTacticalCover =
                    currentCover;
            }

            currentCover.Release(
                this
            );
        }

        currentCover =
            null;
    }

    private void OnDestroy()
    {
        if (currentCover != null)
        {
            currentCover.Release(
                this
            );
        }

        if (tacticalPlanner != null)
        {
            tacticalPlanner.OnMemberDied();
        }
    }

    private void OnGUI()
    {
        if (!showRuntimeState)
            return;

        Camera camera = Camera.main;
        if (camera == null)
            return;

        Vector3 worldPosition =
            transform.position +
            Vector3.up * runtimeStateHeight;

        Vector3 screenPosition =
            camera.WorldToScreenPoint(worldPosition);

        if (screenPosition.z <= 0f)
            return;

        screenPosition.y =
            Screen.height - screenPosition.y;

        Rect rect =
            new Rect(
                screenPosition.x - 70f + runtimeStateOffset.x,
                screenPosition.y - 12f + runtimeStateOffset.y,
                140f,
                24f
            );

        GUI.Label(
            rect,
            currentState.ToString()
        );
    }

    private void OnDrawGizmos()
    {
        if (!showVision)
            return;

#if UNITY_EDITOR
        if (
            showVisionOnlySelected &&
            !UnityEditor.Selection.Contains(
                gameObject
            )
        )
        {
            return;
        }
#endif

        Vector3 eye =
            transform.position +
            Vector3.up *
            eyeHeight;

        if (showDetectionRadius)
        {
            Gizmos.DrawWireSphere(
                eye,
                detectionDistance
            );
        }

        int rays =
            Mathf.Max(
                4,
                visionRayCount
            );

        float halfFov =
            fieldOfView *
            0.5f;

        Vector3 previous =
            Vector3.zero;

        for (
            int i = 0;
            i <= rays;
            i++
        )
        {
            float t =
                (float)i /
                rays;

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
                transform.forward;

            Vector3 end =
                eye +
                direction *
                detectionDistance;

            Gizmos.DrawLine(
                eye,
                end
            );

            if (i > 0)
            {
                Gizmos.DrawLine(
                    previous,
                    end
                );
            }

            previous =
                end;
        }

        if (
            player != null &&
            playerCurrentlyVisible
        )
        {
            Gizmos.DrawLine(
                eye,
                player.position +
                Vector3.up *
                playerHeight
            );
        }

        if (showLastKnownPosition)
        {
            Gizmos.DrawWireSphere(
                lastKnownPlayerPosition,
                0.35f
            );

            Gizmos.DrawLine(
                transform.position,
                lastKnownPlayerPosition
            );
        }

        if (
            showSearchPoints &&
            searchPoints != null
        )
        {
            foreach (
                Vector3 point
                in searchPoints
            )
            {
                Gizmos.DrawWireSphere(
                    point,
                    0.3f
                );
            }
        }

        if (currentCover != null)
        {
            Gizmos.DrawLine(
                transform.position,
                currentCover.transform.position
            );
        }

        if (
            roomSearchWaypoints != null &&
            roomSearchWaypoints.Count > 0
        )
        {
            for (
                int i = 0;
                i < roomSearchWaypoints.Count;
                i++
            )
            {
                Gizmos.DrawWireSphere(
                    roomSearchWaypoints[i],
                    0.18f
                );

                if (i > 0)
                {
                    Gizmos.DrawLine(
                        roomSearchWaypoints[i - 1],
                        roomSearchWaypoints[i]
                    );
                }
            }
        }
    }
}