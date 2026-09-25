using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    private class TacticalGroupData
    {
        public string groupId;
        public int wave;
        public List<EnemyTacticalPlanner> members =
            new List<EnemyTacticalPlanner>();
    }

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject shooterPrefab;
    [SerializeField] private GameObject rusherPrefab;
    [SerializeField] private GameObject tacticalPrefab;

    [Header("Spawn")]
    [SerializeField] private int currentWave = 1;
    [SerializeField] private int startingGroupSize = 4;
    [SerializeField] private int startingGroups = 1;
    [SerializeField] private int extraGroupEveryWaves = 3;

    [SerializeField] private float minimumSpawnDistance = 6f;
    [SerializeField] private float minimumDistanceBetweenSpawns = 4f;
    [SerializeField] private int positionAttempts = 50;

    [Header("Group Spawn")]
    [SerializeField] private float groupSpawnRadius = 2.5f;
    [SerializeField] private float minimumGroupCenterDistance = 4f;
    [SerializeField] private int groupPositionAttempts = 80;

    [Header("Member Placement")]
    [SerializeField] private float memberSpacing = 2f;
    [SerializeField] private int memberPositionAttempts = 30;

    [Header("Camera Visibility")]
    [SerializeField] private bool avoidCameraVisibility = true;
    [SerializeField]
    [Range(0f, 0.25f)]
    private float screenMargin = 0.05f;

    [Header("Ground Detection")]
    [SerializeField] private float groundRayStartHeight = 8f;
    [SerializeField] private float groundRayDistance = 20f;
    [SerializeField] private LayerMask groundMask = ~0;

    [Header("Enemy Types")]
    [SerializeField] private int shooterStartWave = 3;
    [SerializeField] private int rusherStartWave = 4;
    [SerializeField] private int tacticalStartWave = 5;

    [SerializeField]
    [Range(0f, 1f)]
    private float shooterChance = 0.25f;

    [SerializeField]
    [Range(0f, 1f)]
    private float rusherChance = 0.15f;

    [SerializeField]
    [Range(0f, 1f)]
    private float tacticalChance = 0.10f;

    [Header("Elite")]
    [SerializeField] private int eliteStartWave = 5;

    [SerializeField]
    [Range(0f, 1f)]
    private float eliteChance = 0.15f;

    private readonly List<Vector3> usedSpawnPositions =
        new List<Vector3>();

    private readonly List<EnemySpawnZone> spawnZones =
        new List<EnemySpawnZone>();

    private readonly List<TacticalGroupData> activeGroups =
        new List<TacticalGroupData>();

    private readonly HashSet<EnemyController> liveEnemies =
        new HashSet<EnemyController>();

    private Camera mainCamera;

    public int GetLiveEnemyCount()
    {
        CleanupLiveEnemies();

        return liveEnemies.Count;
    }

    public bool IsWaveCleared()
    {
        CleanupLiveEnemies();

        return liveEnemies.Count == 0;
    }

    private void CleanupLiveEnemies()
    {
        liveEnemies.RemoveWhere(
            enemy =>
                enemy == null ||
                !enemy.gameObject.activeInHierarchy
        );
    }

    private void Awake()
    {
        mainCamera = Camera.main;

        RefreshSpawnZones();
    }

    public void SetCurrentWave(int wave)
    {
        currentWave =
            Mathf.Max(1, wave);

        RefreshSpawnZones();

        Debug.Log(
            "[SPAWNER] Current wave set to " +
            currentWave
        );
    }

    public void SpawnEnemies(int amount)
    {
        SpawnWave(
            currentWave,
            amount
        );
    }

    public void SpawnWave(
        int wave,
        int amount)
    {
        currentWave =
            Mathf.Max(1, wave);

        amount =
            Mathf.Max(0, amount);

        CleanupLiveEnemies();

        liveEnemies.Clear();

        usedSpawnPositions.Clear();

        CleanupGroups();

        RefreshSpawnZones();

        if (amount <= 0)
        {
            Debug.LogWarning(
                "WAVE " +
                currentWave +
                " | No enemies requested."
            );

            return;
        }

        if (enemyPrefab == null)
        {
            Debug.LogError(
                "EnemySpawner: enemyPrefab is not assigned."
            );

            return;
        }

        if (spawnZones.Count == 0)
        {
            Debug.LogError(
                "EnemySpawner: EnemySpawnZones not found."
            );

            return;
        }

        int groupCount =
            CalculateGroupCount(amount);

        Debug.Log(
            "WAVE " +
            currentWave +
            " GROUP SPAWN: GROUPS=" +
            groupCount +
            ", TOTAL=" +
            amount
        );

        int remaining =
            amount;

        for (
            int groupIndex = 0;
            groupIndex < groupCount;
            groupIndex++)
        {
            int groupsLeft =
                groupCount -
                groupIndex;

            int groupSize =
                Mathf.CeilToInt(
                    (float)remaining /
                    groupsLeft
                );

            groupSize =
                Mathf.Max(
                    1,
                    groupSize
                );

            string groupId =
                "G" +
                currentWave +
                "_" +
                (groupIndex + 1);

            TacticalGroupData group =
                new TacticalGroupData
                {
                    groupId = groupId,
                    wave = currentWave
                };

            Vector3 groupCenter;

            if (!TryGetGroupSpawnPosition(
                    out groupCenter,
                    groupIndex))
            {
                Debug.LogWarning(
                    "[SPAWNER] " +
                    groupId +
                    " | Не удалось найти позицию группы. Используется fallback."
                );

                if (!TryGetFallbackGroupPosition(
                        out groupCenter))
                {
                    Debug.LogError(
                        "[SPAWNER] " +
                        groupId +
                        " | FALLBACK FAILED. Группа не создана."
                    );

                    remaining -= groupSize;

                    continue;
                }
            }

            usedSpawnPositions.Add(
                groupCenter
            );

            SpawnGroup(
                group,
                groupCenter,
                groupSize
            );

            if (group.members.Count > 0)
            {
                activeGroups.Add(
                    group
                );

                ConfigureTacticalGroup(
                    group
                );

                Debug.Log(
                    "[SPAWNER] " +
                    group.groupId +
                    " READY | MEMBERS = " +
                    group.members.Count +
                    " | LEADER = " +
                    group.members[0].gameObject.name
                );
            }

            remaining -= groupSize;
        }

        Debug.Log(
            "WAVE " +
            currentWave +
            " SPAWN COMPLETE | GROUPS CREATED = " +
            activeGroups.Count +
            " | ENEMIES CREATED = " +
            CountActiveGroupMembers() +
            " | LIVE = " +
            GetLiveEnemyCount()
        );
    }

    private int CalculateGroupCount(
        int amount)
    {
        if (amount <= 0)
            return 0;

        int desiredGroupSize =
            Mathf.Max(
                1,
                startingGroupSize
            );

        int groupsByAmount =
            Mathf.CeilToInt(
                (float)amount /
                desiredGroupSize
            );

        int groups =
            Mathf.Min(
                Mathf.Max(
                    1,
                    startingGroups
                ),
                groupsByAmount
            );

        if (extraGroupEveryWaves > 0)
        {
            int extraGroups =
                Mathf.Max(
                    0,
                    (currentWave - 1) /
                    extraGroupEveryWaves
                );

            groups =
                Mathf.Min(
                    groupsByAmount,
                    groups + extraGroups
                );
        }

        return Mathf.Max(
            1,
            groups
        );
    }

    private void SpawnGroup(
        TacticalGroupData group,
        Vector3 center,
        int groupSize)
    {
        for (
            int memberIndex = 0;
            memberIndex < groupSize;
            memberIndex++)
        {
            Vector3 spawnPosition;

            if (!TryGetMemberSpawnPosition(
                    center,
                    memberIndex,
                    out spawnPosition))
            {
                if (!TryGetFallbackMemberPosition(
                        center,
                        memberIndex,
                        out spawnPosition))
                {
                    Debug.LogWarning(
                        "[SPAWNER] " +
                        group.groupId +
                        " | MEMBER " +
                        memberIndex +
                        " | VALID SPAWN POSITION NOT FOUND."
                    );

                    continue;
                }
            }

            GameObject prefab =
                SelectEnemyPrefab(
                    currentWave
                );

            if (prefab == null)
                prefab = enemyPrefab;

            if (prefab == null)
            {
                Debug.LogError(
                    "[SPAWNER] No enemy prefab available."
                );

                continue;
            }

            GameObject instance =
                Instantiate(
                    prefab,
                    spawnPosition,
                    Quaternion.identity
                );

            if (instance == null)
                continue;

            EnemyController enemy =
                instance.GetComponent<EnemyController>();

            if (enemy == null)
            {
                Debug.LogError(
                    "[SPAWNER] " +
                    instance.name +
                    " has no EnemyController. Destroying."
                );

                Destroy(
                    instance
                );

                continue;
            }

            liveEnemies.Add(
                enemy
            );

            EnemyController.EnemyType enemyType =
                SelectEnemyType(
                    currentWave
                );

            enemy.SetEnemyType(
                enemyType
            );

            bool elite =
                ShouldSpawnElite(
                    currentWave
                );

            enemy.SetElite(
                elite
            );

            enemy.ApplyWaveScaling(
                currentWave
            );

            EnemyTacticalPlanner planner =
                instance.GetComponent<EnemyTacticalPlanner>();

            if (planner != null)
            {
                planner.enabled = true;

                group.members.Add(
                    planner
                );
            }

            usedSpawnPositions.Add(
                spawnPosition
            );
        }
    }

    private void ConfigureTacticalGroup(
        TacticalGroupData group)
    {
        if (group.members.Count == 0)
            return;

        for (
            int i = 0;
            i < group.members.Count;
            i++)
        {
            EnemyTacticalPlanner planner =
                group.members[i];

            if (planner == null)
                continue;

            EnemyTacticalPlanner.TacticalRole role =
                GetInitialRole(
                    i,
                    group.members.Count
                );

            bool isLeader =
                i == 0;

            planner.ConfigureGroup(
                group.groupId,
                group.wave,
                group.members,
                role,
                isLeader
            );
        }

        Debug.Log(
            "[SPAWNER] " +
            group.groupId +
            " TACTICAL PLANNER LINKED | MEMBERS = " +
            group.members.Count
        );
    }

    private EnemyTacticalPlanner.TacticalRole GetInitialRole(
        int index,
        int groupSize)
    {
        if (index == 0)
        {
            return EnemyTacticalPlanner.TacticalRole.Pressure;
        }

        if (groupSize == 2)
        {
            return EnemyTacticalPlanner.TacticalRole.Support;
        }

        if (index == 1)
        {
            return EnemyTacticalPlanner.TacticalRole.LeftFlank;
        }

        if (index == 2)
        {
            return EnemyTacticalPlanner.TacticalRole.RightFlank;
        }

        if (index == 3)
        {
            return EnemyTacticalPlanner.TacticalRole.Support;
        }

        return EnemyTacticalPlanner.TacticalRole.Rear;
    }

    private bool TryGetGroupSpawnPosition(
        out Vector3 position,
        int groupIndex)
    {
        position =
            Vector3.zero;

        int strictFailures = 0;
        int relaxedFailures = 0;

        int sNull = 0;
        int sInside = 0;
        int sGround = 0;
        int sValid = 0;
        int sPlayer = 0;
        int sGroups = 0;
        int sCamera = 0;
        int sArea = 0;

        for (
            int attempt = 0;
            attempt < groupPositionAttempts;
            attempt++)
        {
            EnemySpawnZone zone =
                GetRandomSpawnZone();

            if (zone == null)
            {
                strictFailures++;
                sNull++;

                continue;
            }

            Vector3 candidate =
                zone.GetRandomPosition();

            if (!zone.IsInsideRoom(
                    candidate,
                    0.5f))
            {
                strictFailures++;
                sInside++;

                continue;
            }

            Vector3 grounded;

            if (!TryGetGroundPosition(
                    candidate,
                    out grounded))
            {
                strictFailures++;
                sGround++;

                continue;
            }

            candidate =
                grounded;

            if (!IsValidEnemySpawnPosition(
                    candidate))
            {
                strictFailures++;
                sValid++;

                continue;
            }

            if (!IsFarEnoughFromPlayer(
                    candidate))
            {
                strictFailures++;
                sPlayer++;

                continue;
            }

            if (!IsFarEnoughFromOtherGroups(
                    candidate))
            {
                strictFailures++;
                sGroups++;

                continue;
            }

            if (avoidCameraVisibility &&
                IsVisibleToCamera(
                    candidate))
            {
                strictFailures++;
                sCamera++;

                continue;
            }

            if (!IsGroupAreaFree(
                    candidate,
                    groupSpawnRadius))
            {
                strictFailures++;
                sArea++;

                continue;
            }

            position =
                candidate;

            return true;
        }

        int rNull = 0;
        int rInside = 0;
        int rGround = 0;
        int rValid = 0;
        int rMembers = 0;
        int rGroups = 0;

        for (
            int attempt = 0;
            attempt < groupPositionAttempts;
            attempt++)
        {
            EnemySpawnZone zone =
                GetRandomSpawnZone();

            if (zone == null)
            {
                relaxedFailures++;
                rNull++;

                continue;
            }

            Vector3 candidate =
                zone.GetRandomPosition();

            if (!zone.IsInsideRoom(
                    candidate,
                    0.25f))
            {
                relaxedFailures++;
                rInside++;

                continue;
            }

            Vector3 grounded;

            if (!TryGetGroundPosition(
                    candidate,
                    out grounded))
            {
                relaxedFailures++;
                rGround++;

                continue;
            }

            candidate =
                grounded;

            if (!IsValidEnemySpawnPosition(
                    candidate))
            {
                relaxedFailures++;
                rValid++;

                continue;
            }

            if (!IsFarEnoughFromOtherMembers(
                    candidate,
                    1.0f))
            {
                relaxedFailures++;
                rMembers++;

                continue;
            }

            if (!IsFarEnoughFromOtherGroups(
                    candidate,
                    true))
            {
                relaxedFailures++;
                rGroups++;

                continue;
            }

            position =
                candidate;

            Debug.Log(
                "[SPAWNER] GROUP POSITION RELAXED | " +
                "attempt=" +
                attempt
            );

            return true;
        }

        Debug.LogWarning(
            "[SPAWNER] GROUP POSITION FAILED | " +
            "strict=" +
            strictFailures +
            " | relaxed=" +
            relaxedFailures
        );

        Debug.LogWarning(
            "[SPAWNER DIAG STRICT] null=" + sNull +
            " | inside=" + sInside +
            " | ground=" + sGround +
            " | valid=" + sValid +
            " | player=" + sPlayer +
            " | groups=" + sGroups +
            " | camera=" + sCamera +
            " | area=" + sArea
        );

        Debug.LogWarning(
            "[SPAWNER DIAG RELAXED] null=" + rNull +
            " | inside=" + rInside +
            " | ground=" + rGround +
            " | valid=" + rValid +
            " | members=" + rMembers +
            " | groups=" + rGroups
        );

        return false;
    }

    private bool TryGetFallbackGroupPosition(
        out Vector3 position)
    {
        position =
            Vector3.zero;

        if (spawnZones.Count == 0)
            return false;

        int f1Null = 0;
        int f1Inside = 0;
        int f1Ground = 0;
        int f1Valid = 0;
        int f1Player = 0;

        for (
            int i = 0;
            i < spawnZones.Count;
            i++)
        {
            EnemySpawnZone zone =
                spawnZones[i];

            if (zone == null)
            {
                f1Null++;
                continue;
            }

            Vector3 candidate =
                zone.transform.position;

            if (!zone.IsInsideRoom(
                    candidate,
                    0.1f))
            {
                f1Inside++;
                continue;
            }

            Vector3 grounded;

            if (!TryGetGroundPosition(
                    candidate,
                    out grounded))
            {
                f1Ground++;
                continue;
            }

            candidate =
                grounded;

            if (!IsValidEnemySpawnPosition(
                    candidate))
            {
                f1Valid++;
                continue;
            }

            if (!IsFarEnoughFromPlayer(
                    candidate,
                    true))
            {
                f1Player++;
                continue;
            }

            position =
                candidate;

            Debug.Log(
                "[SPAWNER] FALLBACK GROUP POSITION FOUND | ZONE=" +
                zone.name
            );

            return true;
        }

        int f2Null = 0;
        int f2Inside = 0;
        int f2Valid = 0;
        bool f2Dumped = false;

        for (
            int i = 0;
            i < spawnZones.Count;
            i++)
        {
            EnemySpawnZone zone =
                spawnZones[i];

            if (zone == null)
            {
                f2Null++;
                continue;
            }

            Vector3 candidate =
                zone.transform.position;

            if (!f2Dumped)
            {
                f2Dumped = true;

                ArenaModule zoneRoom =
                    zone.Room;

                if (zoneRoom == null)
                {
                    Debug.LogWarning(
                        "[SPAWNER DIAG ZONE] zone=" + zone.name +
                        " | zonePos=" + candidate +
                        " | room=NULL"
                    );
                }
                else
                {
                    Bounds zb =
                        zoneRoom.GetWorldBounds();

                    EnemySpawnZone found =
                        FindZoneContaining(
                            candidate
                        );

                    Debug.LogWarning(
                        "[SPAWNER DIAG ZONE] zone=" + zone.name +
                        " | zonePos=" + candidate +
                        " | roomPos=" + zoneRoom.transform.position +
                        " | boundsMin=" + zb.min +
                        " | boundsMax=" + zb.max +
                        " | roomSize=" + zoneRoom.RoomSize +
                        " | inside01=" + zone.IsInsideRoom(candidate, 0.1f) +
                        " | foundZone=" + (found != null ? found.name : "NULL")
                    );
                }
            }

            if (!zone.IsInsideRoom(
                    candidate,
                    0.1f))
            {
                f2Inside++;
                continue;
            }

            if (!IsValidEnemySpawnPosition(
                    candidate))
            {
                f2Valid++;
                continue;
            }

            position =
                candidate;

            Debug.Log(
                "[SPAWNER] ABSOLUTE FALLBACK GROUP POSITION | ZONE=" +
                zone.name
            );

            return true;
        }

        Debug.LogWarning(
            "[SPAWNER DIAG FALLBACK1] null=" + f1Null +
            " | inside=" + f1Inside +
            " | ground=" + f1Ground +
            " | valid=" + f1Valid +
            " | player=" + f1Player
        );

        Debug.LogWarning(
            "[SPAWNER DIAG FALLBACK2] null=" + f2Null +
            " | inside=" + f2Inside +
            " | valid=" + f2Valid
        );

        return false;
    }

    private bool TryGetMemberSpawnPosition(
        Vector3 center,
        int memberIndex,
        out Vector3 position)
    {
        position =
            center;

        if (memberIndex == 0)
        {
            if (!IsValidEnemySpawnPosition(
                    center))
            {
                return false;
            }

            return true;
        }

        float angleStep =
            360f /
            Mathf.Max(
                1,
                startingGroupSize
            );

        for (
            int attempt = 0;
            attempt < memberPositionAttempts;
            attempt++)
        {
            float angle =
                (memberIndex * angleStep) +
                Random.Range(
                    -30f,
                    30f
                );

            float distance =
                Random.Range(
                    memberSpacing * 0.75f,
                    memberSpacing * 1.5f
                );

            Vector3 offset =
                Quaternion.Euler(
                    0f,
                    angle,
                    0f
                ) *
                Vector3.forward *
                distance;

            Vector3 candidate =
                center +
                offset;

            EnemySpawnZone zone =
                FindZoneContaining(
                    candidate
                );

            if (zone == null)
                continue;

            if (!zone.IsInsideRoom(
                    candidate,
                    0.4f))
            {
                continue;
            }

            Vector3 grounded;

            if (!TryGetGroundPosition(
                    candidate,
                    out grounded))
            {
                continue;
            }

            candidate =
                grounded;

            if (!IsValidEnemySpawnPosition(
                    candidate))
            {
                continue;
            }

            if (!IsFarEnoughFromOtherMembers(
                    candidate,
                    1.0f))
            {
                continue;
            }

            position =
                candidate;

            return true;
        }

        return false;
    }

    private bool TryGetFallbackMemberPosition(
        Vector3 center,
        int memberIndex,
        out Vector3 position)
    {
        position =
            center;

        if (memberIndex == 0)
        {
            if (IsValidEnemySpawnPosition(
                    center))
            {
                return true;
            }

            return false;
        }

        const int fallbackAttempts = 24;

        for (
            int attempt = 0;
            attempt < fallbackAttempts;
            attempt++)
        {
            float angle =
                (memberIndex * 90f) +
                Random.Range(
                    -45f,
                    45f
                );

            float distance =
                memberSpacing *
                Random.Range(
                    1f,
                    1.5f
                );

            Vector3 candidate =
                center +
                Quaternion.Euler(
                    0f,
                    angle,
                    0f
                ) *
                Vector3.forward *
                distance;

            EnemySpawnZone zone =
                FindZoneContaining(
                    candidate
                );

            if (zone == null)
                continue;

            if (!zone.IsInsideRoom(
                    candidate,
                    0.25f))
            {
                continue;
            }

            Vector3 grounded;

            if (!TryGetGroundPosition(
                    candidate,
                    out grounded))
            {
                continue;
            }

            candidate =
                grounded;

            if (!IsValidEnemySpawnPosition(
                    candidate))
            {
                continue;
            }

            if (!IsFarEnoughFromOtherMembers(
                    candidate,
                    0.75f))
            {
                continue;
            }

            position =
                candidate;

            return true;
        }

        return false;
    }

    private bool IsGroupAreaFree(
        Vector3 position,
        float radius)
    {
        for (
            int i = 0;
            i < usedSpawnPositions.Count;
            i++)
        {
            Vector3 used =
                usedSpawnPositions[i];

            Vector2 a =
                new Vector2(
                    position.x,
                    position.z
                );

            Vector2 b =
                new Vector2(
                    used.x,
                    used.z
                );

            if (Vector2.Distance(
                    a,
                    b) <
                minimumGroupCenterDistance +
                radius)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsFarEnoughFromOtherGroups(
        Vector3 position,
        bool relaxed = false)
    {
        float requiredDistance =
            relaxed
                ? minimumDistanceBetweenSpawns * 0.5f
                : minimumDistanceBetweenSpawns;

        for (
            int i = 0;
            i < usedSpawnPositions.Count;
            i++)
        {
            Vector3 other =
                usedSpawnPositions[i];

            Vector2 a =
                new Vector2(
                    position.x,
                    position.z
                );

            Vector2 b =
                new Vector2(
                    other.x,
                    other.z
                );

            if (Vector2.Distance(
                    a,
                    b) <
                requiredDistance)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsFarEnoughFromPlayer(
        Vector3 position,
        bool relaxed = false)
    {
        if (!PlayerControllerExists())
            return true;

        GameObject player =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        if (player == null)
            return true;

        float requiredDistance =
            relaxed
                ? minimumSpawnDistance * 0.65f
                : minimumSpawnDistance;

        Vector2 a =
            new Vector2(
                position.x,
                position.z
            );

        Vector2 b =
            new Vector2(
                player.transform.position.x,
                player.transform.position.z
            );

        return Vector2.Distance(
            a,
            b
        ) >= requiredDistance;
    }

    private bool PlayerControllerExists()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        return player != null;
    }

    private bool IsFarEnoughFromOtherMembers(
        Vector3 position,
        float minimumDistance)
    {
        for (
            int i = 0;
            i < usedSpawnPositions.Count;
            i++)
        {
            Vector3 other =
                usedSpawnPositions[i];

            Vector2 a =
                new Vector2(
                    position.x,
                    position.z
                );

            Vector2 b =
                new Vector2(
                    other.x,
                    other.z
                );

            if (Vector2.Distance(
                    a,
                    b) <
                minimumDistance)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsValidEnemySpawnPosition(
        Vector3 position)
    {
        EnemySpawnZone zone =
            FindZoneContaining(
                position
            );

        if (zone == null)
            return false;

        if (!zone.IsInsideRoom(
                position,
                0.25f))
        {
            return false;
        }

        if (ArenaTacticalMap.Instance != null)
        {
            if (!ArenaTacticalMap.Instance.IsPositionWalkable(
                    position))
            {
                return false;
            }
        }

        return true;
    }

    private bool TryGetGroundPosition(
        Vector3 position,
        out Vector3 grounded)
    {
        Vector3 rayStart =
            position +
            Vector3.up *
            groundRayStartHeight;

        Ray ray =
            new Ray(
                rayStart,
                Vector3.down
            );

        RaycastHit hit;

        if (Physics.Raycast(
                ray,
                out hit,
                groundRayDistance,
                groundMask,
                QueryTriggerInteraction.Ignore))
        {
            grounded =
                hit.point;

            return true;
        }

        grounded =
            position;

        return false;
    }

    private bool IsVisibleToCamera(
        Vector3 position)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return false;

        Vector3 screen =
            mainCamera.WorldToViewportPoint(
                position
            );

        if (screen.z <= 0f)
            return false;

        return screen.x >= screenMargin &&
               screen.x <= 1f - screenMargin &&
               screen.y >= screenMargin &&
               screen.y <= 1f - screenMargin;
    }

    private EnemySpawnZone GetRandomSpawnZone()
    {
        if (spawnZones.Count == 0)
            return null;

        return spawnZones[
            Random.Range(
                0,
                spawnZones.Count
            )
        ];
    }

    private EnemySpawnZone FindZoneContaining(
        Vector3 position)
    {
        for (
            int i = 0;
            i < spawnZones.Count;
            i++)
        {
            EnemySpawnZone zone =
                spawnZones[i];

            if (zone == null)
                continue;

            if (zone.IsInsideRoom(
                    position,
                    0.5f))
            {
                return zone;
            }
        }

        return null;
    }

    private GameObject SelectEnemyPrefab(
        int wave)
    {
        float roll =
            Random.value;

        if (wave >= tacticalStartWave &&
            tacticalPrefab != null &&
            roll < tacticalChance)
        {
            return tacticalPrefab;
        }

        roll -=
            tacticalChance;

        if (wave >= rusherStartWave &&
            rusherPrefab != null &&
            roll < rusherChance)
        {
            return rusherPrefab;
        }

        roll -=
            rusherChance;

        if (wave >= shooterStartWave &&
            shooterPrefab != null &&
            roll < shooterChance)
        {
            return shooterPrefab;
        }

        return enemyPrefab;
    }

    private EnemyController.EnemyType SelectEnemyType(
        int wave)
    {
        float roll =
            Random.value;

        if (wave >= tacticalStartWave &&
            roll < tacticalChance)
        {
            return EnemyController.EnemyType.Tactical;
        }

        roll -=
            tacticalChance;

        if (wave >= rusherStartWave &&
            roll < rusherChance)
        {
            return EnemyController.EnemyType.Rusher;
        }

        roll -=
            rusherChance;

        if (wave >= shooterStartWave &&
            roll < shooterChance)
        {
            return EnemyController.EnemyType.Shooter;
        }

        return EnemyController.EnemyType.Bandit;
    }

    private bool ShouldSpawnElite(
        int wave)
    {
        if (wave < eliteStartWave)
            return false;

        return Random.value <=
               eliteChance;
    }

    private void RefreshSpawnZones()
    {
        spawnZones.Clear();

        EnemySpawnZone[] zones =
            FindObjectsByType<EnemySpawnZone>(
                FindObjectsSortMode.None
            );

        if (zones == null)
            return;

        for (
            int i = 0;
            i < zones.Length;
            i++)
        {
            if (zones[i] != null)
            {
                spawnZones.Add(
                    zones[i]
                );
            }
        }

        Debug.Log(
            "EnemySpawner: найдено EnemySpawnZones = " +
            spawnZones.Count
        );
    }

    private void CleanupGroups()
    {
        activeGroups.Clear();
    }

    private int CountActiveGroupMembers()
    {
        int count = 0;

        for (
            int i = 0;
            i < activeGroups.Count;
            i++)
        {
            if (activeGroups[i] == null)
                continue;

            count +=
                activeGroups[i]
                    .members.Count;
        }

        return count;
    }

    private void OnDrawGizmosSelected()
    {
        if (usedSpawnPositions == null)
            return;

        for (
            int i = 0;
            i < usedSpawnPositions.Count;
            i++)
        {
            Gizmos.DrawWireSphere(
                usedSpawnPositions[i],
                groupSpawnRadius
            );
        }
    }
}