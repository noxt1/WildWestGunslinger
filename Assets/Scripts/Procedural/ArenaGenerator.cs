using UnityEngine;
using System.Collections.Generic;

public class ArenaGenerator : MonoBehaviour
{
    [System.Serializable]
    private class RoomRuntime
    {
        public ArenaModule module;
        public int index;
        public float width;
        public float depth;
        public ArenaModule.RoomType roomType;

        public bool[] open = new bool[4];
        public float[] openingWidth = new float[4];

        public int parentIndex = -1;
        public ArenaModule.RoomDirection parentDirection;
        public float corridorLength;
    }

    private class RoomConnectionRuntime
    {
        public int roomA;
        public int roomB;
        public ArenaModule.RoomDirection directionFromA;
        public float width;
    }

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Room Count")]
    [SerializeField] private int minimumRooms = 9;
    [SerializeField] private int maximumRooms = 9;

    [Header("Honeycomb Layout")]
    [SerializeField] private bool useHoneycombLayout = true;
    [SerializeField] private float honeycombGridSpacing = 28f;

    [Header("Room Size")]
    [SerializeField] private float minimumRoomWidth = 16f;
    [SerializeField] private float maximumRoomWidth = 22f;
    [SerializeField] private float minimumRoomDepth = 16f;
    [SerializeField] private float maximumRoomDepth = 22f;

    [Header("Walls")]
    [SerializeField] private float minimumWallHeight = 2.4f;
    [SerializeField] private float maximumWallHeight = 3.2f;
    [SerializeField] private float minimumWallThickness = 0.4f;
    [SerializeField] private float maximumWallThickness = 0.8f;

    [Header("Passages")]
    [SerializeField] private float minimumPassageWidth = 3.5f;
    [SerializeField] private float maximumPassageWidth = 5f;
    [SerializeField] private float minimumCorridorLength = 4f;
    [SerializeField] private float maximumCorridorLength = 6f;

    [Header("Corridor Walls")]
    [SerializeField] private float corridorWallInset = 0.05f;

    [Header("Cover")]
    [SerializeField] private int minimumCoverCount = 3;
    [SerializeField] private int maximumCoverCount = 5;
    [SerializeField] private float coverHeightPadding = 0.08f;
    [SerializeField] private float minimumCoverWidth = 1.2f;
    [SerializeField] private float maximumCoverWidth = 3.0f;
    [SerializeField] private float minimumCoverDepth = 1.0f;
    [SerializeField] private float maximumCoverDepth = 2.2f;
    [SerializeField] private float minimumDistanceBetweenCovers = 2.2f;

    [Header("Enemy Spawn Zones")]
    [SerializeField] private int minimumSpawnZones = 3;
    [SerializeField] private int maximumSpawnZones = 5;
    [SerializeField] private float minimumSpawnZoneRadius = 1.5f;
    [SerializeField] private float maximumSpawnZoneRadius = 3f;
    [SerializeField] private float minimumDistanceBetweenSpawnZones = 4f;

    [Header("Validation")]
    [SerializeField] private int placementAttempts = 500;
    [SerializeField] private float roomOverlapPadding = 2f;

    [Header("Generation")]
    [SerializeField] private bool generateOnStart = true;
    [SerializeField] private bool useRandomSeed = true;
    [SerializeField] private int seed = 12345;

    [Header("Procedural Material")]
    [SerializeField] private Material generatedMaterial;
    [SerializeField] private Color generatedObjectColor = Color.white;
    [SerializeField] private float generatedMetallic = 0f;
    [SerializeField] private float generatedSmoothness = 0.15f;

    [Header("Floor Material (optional)")]
    [Tooltip("Если назначен — полы/коридоры используют его вместо generatedMaterial. Если null — старое поведение без изменений. Hook для будущего Western_Floor_Chunk (BLOCKED: FBX отсутствует).")]
    [SerializeField] private Material floorMaterial;

    private Transform generatedRoot;
    private int generatedRoomCount;

    private readonly List<RoomRuntime> rooms =
        new List<RoomRuntime>();

    private readonly List<RoomConnectionRuntime> roomConnections =
        new List<RoomConnectionRuntime>();

    private readonly List<Vector3> generatedCoverPositions =
        new List<Vector3>();

    private readonly List<Vector3> generatedSpawnZonePositions =
        new List<Vector3>();

    [Header("Cover Clusters (western compositions from real prefabs)")]
    [Tooltip("Если выключено или prefab'ы не назначены — старый primitive-путь без изменений (рабочая сцена не затрагивается).")]
    [SerializeField] private bool useCoverClusters = false;
    [SerializeField] private GameObject fencePrefab;
    [SerializeField] private GameObject coverPrefab;
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private GameObject wagonPrefab;
    [SerializeField] private GameObject lanternPrefab;
    [SerializeField] private GameObject logWallPrefab;
    [SerializeField] private int minimumClustersPerRoom = 2;
    [SerializeField] private int maximumClustersPerRoom = 3;
    [SerializeField] private float clusterSpacing = 5.5f;
    [SerializeField] private float clusterMargin = 3.5f;
    [SerializeField] private float clusterCenterClearance = 3.5f;

    [System.Serializable]
    private struct ClusterFootprint
    {
        public int roomIndex;
        public Vector3 localPosition;
        public float radius;
    }

    private readonly List<ClusterFootprint> clusterFootprints =
        new List<ClusterFootprint>();

    private void Start()
    {
        if (generateOnStart)
            GenerateArena();
    }

    private void Awake()
    {
        CreateGeneratedMaterial();
    }

    private void CreateGeneratedMaterial()
    {
        // Если материал назначен в Inspector,
        // используем именно его и ничего не создаём.
        if (generatedMaterial != null)
            return;

        Shader urpShader =
            Shader.Find(
                "Universal Render Pipeline/Lit"
            );

        if (urpShader == null)
        {
            Debug.LogError(
                "ArenaGenerator: не найден URP Lit shader. " +
                "Проверь, что проект использует Universal Render Pipeline."
            );

            return;
        }

        generatedMaterial =
            new Material(urpShader);

        generatedMaterial.name =
            "GeneratedArena_URP_Material";

        if (
            generatedMaterial.HasProperty(
                "_BaseColor"
            )
        )
        {
            generatedMaterial.SetColor(
                "_BaseColor",
                generatedObjectColor
            );
        }

        if (
            generatedMaterial.HasProperty(
                "_Metallic"
            )
        )
        {
            generatedMaterial.SetFloat(
                "_Metallic",
                generatedMetallic
            );
        }

        if (
            generatedMaterial.HasProperty(
                "_Smoothness"
            )
        )
        {
            generatedMaterial.SetFloat(
                "_Smoothness",
                generatedSmoothness
            );
        }
    }

    private void ApplyFloorMaterial(
        GameObject target
    )
    {
        if (
            target == null
        )
        {
            return;
        }

        if (floorMaterial != null)
        {
            Renderer renderer =
                target.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    floorMaterial;
            }

            return;
        }

        ApplyGeneratedMaterial(
            target
        );
    }

    private void ApplyGeneratedMaterial(
        GameObject target
    )
    {
        if (
            target == null
        )
        {
            return;
        }

        if (
            generatedMaterial == null
        )
        {
            CreateGeneratedMaterial();
        }

        if (
            generatedMaterial == null
        )
        {
            return;
        }

        Renderer renderer =
            target.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sharedMaterial =
                generatedMaterial;
        }
    }

    public void GenerateArena()
    {
        ClearArena();

        if (player == null)
        {
            Debug.LogError(
                "ArenaGenerator: Player не назначен."
            );

            return;
        }

        CreateGeneratedMaterial();

        if (useRandomSeed)
        {
            Random.InitState(
                System.Environment.TickCount
            );
        }
        else
        {
            Random.InitState(seed);
        }

        generatedRoot =
            new GameObject(
                "GeneratedArena"
            ).transform;

        rooms.Clear();

        int roomCount =
            9;

        if (!useHoneycombLayout)
        {
            roomCount =
                Mathf.Clamp(
                    Random.Range(
                        Mathf.Max(2, minimumRooms),
                        Mathf.Max(
                            Mathf.Max(2, minimumRooms),
                            maximumRooms
                        ) + 1
                    ),
                    2,
                    100
                );
        }

        BuildHoneycombLayout(roomCount);

        generatedRoomCount =
            rooms.Count;

        foreach (RoomRuntime room in rooms)
        {
            Vector3 position =
                room.module.transform.position;

            Debug.Log(
                $"ROOM {room.index + 1} | " +
                $"TYPE={room.roomType} | " +
                $"POS=({position.x:F2}, {position.z:F2}) | " +
                $"SIZE=({room.width:F2}, {room.depth:F2}) | " +
                $"PARENT={room.parentIndex + 1}"
            );
        }

        FinalizeRoomConnections();

        BuildRoomGeometry();
        BuildCorridors();
        BuildPlayerSpawns();

        BuildTimberFraming();

        BuildCovers();

        if (CoverSystem.Instance != null)
        {
            CoverSystem.Instance.FinalizeGeneratedCoverPoints();
        }

        BuildEnemySpawnZones();

        ArenaModule start =
            FindStartRoom();

        if (start != null)
            SpawnPlayer(start);

        Debug.Log(
            $"ArenaGenerator: генерация завершена. " +
            $"Комнат = {rooms.Count}."
        );
    }

    private void BuildHoneycombLayout(int roomCount)
    {
        rooms.Clear();
        roomConnections.Clear();

        if (roomCount <= 0)
            return;

        if (!useHoneycombLayout)
        {
            RoomRuntime startRoom =
                CreateRoomRuntime(
                    0,
                    ArenaModule.RoomType.Start
                );

            startRoom.module.transform.position =
                Vector3.zero;

            rooms.Add(startRoom);

            RoomRuntime currentParent =
                startRoom;

            for (
                int i = 1;
                i < roomCount;
                i++
            )
            {
                RoomRuntime newRoom;

                ArenaModule.RoomType roomType =
                    DetermineRoomType(
                        i,
                        roomCount
                    );

                if (!TryCreateConnectedRoom(
                        i,
                        roomType,
                        currentParent,
                        out newRoom))
                {
                    break;
                }

                currentParent =
                    newRoom;
            }

            return;
        }

        roomCount =
            9;

        Vector2Int[] grid =
        {
            new Vector2Int(0, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0)
        };

        float calculatedSpacing =
            Mathf.Max(
                maximumRoomWidth,
                maximumRoomDepth
            ) +
            Mathf.Max(
                minimumCorridorLength,
                4f
            ) +
            2f;

        float spacing =
            Mathf.Max(
                honeycombGridSpacing,
                calculatedSpacing
            );

        for (
            int i = 0;
            i < roomCount;
            i++
        )
        {
            ArenaModule.RoomType roomType =
                DetermineRoomType(
                    i,
                    roomCount
                );

            RoomRuntime room =
                CreateRoomRuntime(
                    i,
                    roomType
                );

            Vector2Int cell =
                grid[i];

            room.module.transform.position =
                new Vector3(
                    cell.x * spacing,
                    0f,
                    cell.y * spacing
                );

            rooms.Add(
                room
            );
        }

        ConnectHoneycombRooms(
            0,
            2,
            ArenaModule.RoomDirection.North
        );

        ConnectHoneycombRooms(
            0,
            4,
            ArenaModule.RoomDirection.East
        );

        ConnectHoneycombRooms(
            0,
            6,
            ArenaModule.RoomDirection.South
        );

        ConnectHoneycombRooms(
            0,
            8,
            ArenaModule.RoomDirection.West
        );

        ConnectHoneycombRooms(
            1,
            2,
            ArenaModule.RoomDirection.East
        );

        ConnectHoneycombRooms(
            2,
            3,
            ArenaModule.RoomDirection.East
        );

        ConnectHoneycombRooms(
            3,
            4,
            ArenaModule.RoomDirection.South
        );

        ConnectHoneycombRooms(
            4,
            5,
            ArenaModule.RoomDirection.South
        );

        ConnectHoneycombRooms(
            5,
            6,
            ArenaModule.RoomDirection.West
        );

        ConnectHoneycombRooms(
            6,
            7,
            ArenaModule.RoomDirection.West
        );

        ConnectHoneycombRooms(
            7,
            8,
            ArenaModule.RoomDirection.North
        );

        ConnectHoneycombRooms(
            8,
            1,
            ArenaModule.RoomDirection.North
        );

        for (
            int i = 0;
            i < rooms.Count;
            i++
        )
        {
            RoomRuntime room =
                rooms[i];

            if (room.parentIndex >= 0)
                continue;

            foreach (
                RoomConnectionRuntime connection
                in roomConnections
            )
            {
                if (connection == null)
                    continue;

                if (connection.roomB == i)
                {
                    room.parentIndex =
                        connection.roomA;

                    room.parentDirection =
                        GetOppositeDirection(
                            connection.directionFromA
                        );

                    room.corridorLength =
                        GetConnectionDistance(
                            connection.roomA,
                            connection.roomB
                        );

                    break;
                }

                if (connection.roomA == i)
                {
                    room.parentIndex =
                        connection.roomB;

                    room.parentDirection =
                        GetOppositeDirection(
                            connection.directionFromA
                        );

                    room.corridorLength =
                        GetConnectionDistance(
                            connection.roomA,
                            connection.roomB
                        );

                    break;
                }
            }
        }

        Debug.Log(
            "[ARENA] Honeycomb layout generated | " +
            "ROOMS=" +
            rooms.Count +
            " | CONNECTIONS=" +
            roomConnections.Count
        );
    }

    private void ConnectHoneycombRooms(
        int roomAIndex,
        int roomBIndex,
        ArenaModule.RoomDirection direction)
    {
        if (
            roomAIndex < 0 ||
            roomBIndex < 0 ||
            roomAIndex >= rooms.Count ||
            roomBIndex >= rooms.Count
        )
        {
            return;
        }

        RoomRuntime roomA =
            rooms[roomAIndex];

        RoomRuntime roomB =
            rooms[roomBIndex];

        if (
            roomA == null ||
            roomB == null
        )
        {
            return;
        }

        ArenaModule.RoomDirection opposite =
            GetOppositeDirection(
                direction
            );

        int directionIndex =
            DirectionToIndex(
                direction
            );

        int oppositeIndex =
            DirectionToIndex(
                opposite
            );

        float passageWidth =
            Mathf.Min(
                Random.Range(
                    minimumPassageWidth,
                    maximumPassageWidth
                ),
                GetMaximumPassageWidth(
                    roomA,
                    direction
                ),
                GetMaximumPassageWidth(
                    roomB,
                    opposite
                )
            );

        roomA.open[
            directionIndex
        ] = true;

        roomB.open[
            oppositeIndex
        ] = true;

        roomA.openingWidth[
            directionIndex
        ] = passageWidth;

        roomB.openingWidth[
            oppositeIndex
        ] = passageWidth;

        if (roomB.parentIndex < 0)
        {
            roomB.parentIndex =
                roomA.index;

            roomB.parentDirection =
                direction;

            roomB.corridorLength =
                GetConnectionDistance(
                    roomAIndex,
                    roomBIndex
                );
        }

        RoomConnectionRuntime connection =
            new RoomConnectionRuntime();

        connection.roomA =
            roomAIndex;

        connection.roomB =
            roomBIndex;

        connection.directionFromA =
            direction;

        connection.width =
            passageWidth;

        roomConnections.Add(
            connection
        );
    }

    private float GetConnectionDistance(
        int roomAIndex,
        int roomBIndex)
    {
        if (
            roomAIndex < 0 ||
            roomBIndex < 0 ||
            roomAIndex >= rooms.Count ||
            roomBIndex >= rooms.Count
        )
        {
            return 0f;
        }

        return Vector3.Distance(
            rooms[roomAIndex].module.transform.position,
            rooms[roomBIndex].module.transform.position
        );
    }

    private RoomRuntime CreateRoomRuntime(
        int index,
        ArenaModule.RoomType roomType
    )
    {
        float width =
            Random.Range(
                minimumRoomWidth,
                maximumRoomWidth
            );

        float depth =
            Random.Range(
                minimumRoomDepth,
                maximumRoomDepth
            );

        GameObject roomObject =
            new GameObject(
                $"Room_{index + 1}_{roomType}"
            );

        roomObject.transform.SetParent(
            generatedRoot
        );

        ArenaModule module =
            roomObject.AddComponent<ArenaModule>();

        module.SetModuleId(
            $"Room_{index + 1}"
        );

        module.SetRoomType(
            roomType
        );

        module.SetRoomSize(
            width,
            depth
        );

        CreateDirectionalConnections(
            module.transform,
            module,
            width,
            depth
        );

        RoomRuntime runtime =
            new RoomRuntime();

        runtime.module =
            module;

        runtime.index =
            index;

        runtime.width =
            width;

        runtime.depth =
            depth;

        runtime.roomType =
            roomType;

        return runtime;
    }

    private void CreateDirectionalConnections(
        Transform parent,
        ArenaModule module,
        float width,
        float depth
    )
    {
        float halfWidth =
            width * 0.5f;

        float halfDepth =
            depth * 0.5f;

        CreateDirectionalConnection(
            parent,
            module,
            ArenaModule.RoomDirection.North,
            new Vector3(
                0f,
                0f,
                halfDepth
            )
        );

        CreateDirectionalConnection(
            parent,
            module,
            ArenaModule.RoomDirection.East,
            new Vector3(
                halfWidth,
                0f,
                0f
            )
        );

        CreateDirectionalConnection(
            parent,
            module,
            ArenaModule.RoomDirection.South,
            new Vector3(
                0f,
                0f,
                -halfDepth
            )
        );

        CreateDirectionalConnection(
            parent,
            module,
            ArenaModule.RoomDirection.West,
            new Vector3(
                -halfWidth,
                0f,
                0f
            )
        );
    }

    private void CreateDirectionalConnection(
        Transform parent,
        ArenaModule module,
        ArenaModule.RoomDirection direction,
        Vector3 localPosition
    )
    {
        GameObject connection =
            new GameObject(
                $"Connection_{direction}"
            );

        connection.transform.SetParent(
            parent
        );

        connection.transform.localPosition =
            localPosition;

        connection.transform.localRotation =
            Quaternion.identity;

        module.SetDirectionalConnection(
            direction,
            connection.transform
        );
    }

    private bool TryCreateConnectedRoom(
        int index,
        ArenaModule.RoomType roomType,
        RoomRuntime parent,
        out RoomRuntime createdRoom
    )
    {
        createdRoom = null;

        if (parent == null)
            return false;

        List<int> directions =
            GetAvailableDirections(parent);

        if (directions.Count == 0)
            return false;

        ShuffleList(directions);

        for (
            int attempt = 0;
            attempt < placementAttempts;
            attempt++
        )
        {
            if (directions.Count == 0)
                break;

            if (
                attempt > 0 &&
                attempt % directions.Count == 0
            )
            {
                ShuffleList(directions);
            }

            int directionIndex =
                directions[
                    attempt % directions.Count
                ];

            ArenaModule.RoomDirection direction =
                IndexToDirection(
                    directionIndex
                );

            ArenaModule.RoomDirection opposite =
                GetOppositeDirection(
                    direction
                );

            RoomRuntime child =
                CreateRoomRuntime(
                    index,
                    roomType
                );

            Transform parentConnection =
                parent.module.GetDirectionalConnection(
                    direction
                );

            Transform childConnection =
                child.module.GetDirectionalConnection(
                    opposite
                );

            if (
                parentConnection == null ||
                childConnection == null
            )
            {
                Destroy(
                    child.module.gameObject
                );

                continue;
            }

            float corridorLength =
                Random.Range(
                    minimumCorridorLength,
                    maximumCorridorLength
                );

            Vector3 outward =
                DirectionToVector(
                    direction
                );

            Vector3 targetConnection =
                parentConnection.position +
                outward * corridorLength;

            Vector3 childPosition =
                targetConnection -
                childConnection.localPosition;

            child.module.transform.position =
                new Vector3(
                    childPosition.x,
                    0f,
                    childPosition.z
                );

            float passageWidth =
                Mathf.Min(
                    Random.Range(
                        minimumPassageWidth,
                        maximumPassageWidth
                    ),
                    GetMaximumPassageWidth(
                        parent,
                        direction
                    ),
                    GetMaximumPassageWidth(
                        child,
                        opposite
                    )
                );

            if (
                !IsRoomPlacementValid(
                    child,
                    parent,
                    direction
                )
            )
            {
                Destroy(
                    child.module.gameObject
                );

                continue;
            }

            int parentDirectionIndex =
                DirectionToIndex(
                    direction
                );

            int childDirectionIndex =
                DirectionToIndex(
                    opposite
                );

            parent.open[
                parentDirectionIndex
            ] = true;

            child.open[
                childDirectionIndex
            ] = true;

            parent.openingWidth[
                parentDirectionIndex
            ] = passageWidth;

            child.openingWidth[
                childDirectionIndex
            ] = passageWidth;

            child.parentIndex =
                parent.index;

            child.parentDirection =
                direction;

            child.corridorLength =
                corridorLength;

            rooms.Add(child);

            createdRoom =
                child;

            return true;
        }

        return false;
    }

    private bool IsRoomPlacementValid(
        RoomRuntime child,
        RoomRuntime parent,
        ArenaModule.RoomDirection direction
    )
    {
        if (
            child == null ||
            child.module == null ||
            parent == null ||
            parent.module == null
        )
        {
            return false;
        }

        Vector3 childCenter =
            child.module.transform.position;

        float childHalfWidth =
            child.width * 0.5f;

        float childHalfDepth =
            child.depth * 0.5f;

        float padding =
            Mathf.Max(
                0.1f,
                roomOverlapPadding
            );

        foreach (
            RoomRuntime existing in rooms
        )
        {
            if (
                existing == null ||
                existing.module == null
            )
            {
                continue;
            }

            Vector3 existingCenter =
                existing.module.transform.position;

            float existingHalfWidth =
                existing.width * 0.5f;

            float existingHalfDepth =
                existing.depth * 0.5f;

            float distanceX =
                Mathf.Abs(
                    childCenter.x -
                    existingCenter.x
                );

            float distanceZ =
                Mathf.Abs(
                    childCenter.z -
                    existingCenter.z
                );

            float requiredX =
                childHalfWidth +
                existingHalfWidth +
                padding;

            float requiredZ =
                childHalfDepth +
                existingHalfDepth +
                padding;

            if (
                distanceX < requiredX &&
                distanceZ < requiredZ
            )
            {
                return false;
            }
        }

        if (
            !IsRoomOutsideParent(
                child,
                parent,
                direction
            )
        )
        {
            return false;
        }

        if (
            !IsConnectionAlignmentValid(
                child,
                parent,
                direction
            )
        )
        {
            return false;
        }

        return true;
    }

    private bool IsRoomOutsideParent(
        RoomRuntime child,
        RoomRuntime parent,
        ArenaModule.RoomDirection direction
    )
    {
        Vector3 childCenter =
            child.module.transform.position;

        Vector3 parentCenter =
            parent.module.transform.position;

        float childHalfWidth =
            child.width * 0.5f;

        float childHalfDepth =
            child.depth * 0.5f;

        float parentHalfWidth =
            parent.width * 0.5f;

        float parentHalfDepth =
            parent.depth * 0.5f;

        float minimumGap =
            0.05f;

        switch (direction)
        {
            case ArenaModule.RoomDirection.North:
                return
                    childCenter.z - childHalfDepth >=
                    parentCenter.z + parentHalfDepth + minimumGap;

            case ArenaModule.RoomDirection.East:
                return
                    childCenter.x - childHalfWidth >=
                    parentCenter.x + parentHalfWidth + minimumGap;

            case ArenaModule.RoomDirection.South:
                return
                    childCenter.z + childHalfDepth <=
                    parentCenter.z - parentHalfDepth - minimumGap;

            default:
                return
                    childCenter.x + childHalfWidth <=
                    parentCenter.x - parentHalfWidth - minimumGap;
        }
    }

    private bool IsConnectionAlignmentValid(
        RoomRuntime child,
        RoomRuntime parent,
        ArenaModule.RoomDirection direction
    )
    {
        ArenaModule.RoomDirection opposite =
            GetOppositeDirection(direction);

        Transform parentConnection =
            parent.module.GetDirectionalConnection(
                direction
            );

        Transform childConnection =
            child.module.GetDirectionalConnection(
                opposite
            );

        if (
            parentConnection == null ||
            childConnection == null
        )
        {
            return false;
        }

        Vector3 difference =
            childConnection.position -
            parentConnection.position;

        difference.y = 0f;

        Vector3 expectedDirection =
            DirectionToVector(
                direction
            );

        float forwardDistance =
            Vector3.Dot(
                difference,
                expectedDirection
            );

        Vector3 lateralDifference =
            difference -
            expectedDirection *
            forwardDistance;

        float lateralDistance =
            lateralDifference.magnitude;

        if (lateralDistance > 0.05f)
            return false;

        if (forwardDistance < 0f)
            return false;

        return true;
    }

    private List<int> GetAvailableDirections(
        RoomRuntime room
    )
    {
        List<int> result =
            new List<int>();

        for (int i = 0; i < 4; i++)
        {
            if (!room.open[i])
                result.Add(i);
        }

        return result;
    }

    private void FinalizeRoomConnections()
    {
        foreach (
            RoomRuntime room in rooms
        )
        {
            List<ArenaModule.RoomDirection> active =
                new List<ArenaModule.RoomDirection>();

            for (
                int i = 0;
                i < 4;
                i++
            )
            {
                if (room.open[i])
                {
                    active.Add(
                        IndexToDirection(i)
                    );
                }
            }

            room.module.SetActiveConnections(
                active.ToArray()
            );
        }
    }

    private void BuildRoomGeometry()
    {
        foreach (
            RoomRuntime room in rooms
        )
        {
            float wallHeight =
                Random.Range(
                    minimumWallHeight,
                    maximumWallHeight
                );

            float wallThickness =
                Random.Range(
                    minimumWallThickness,
                    maximumWallThickness
                );

            CreateFloor(
                room.module.transform,
                room.width,
                room.depth
            );

            CreateWalls(
                room.module.transform,
                room,
                wallHeight,
                wallThickness
            );
        }
    }

    private void CreateFloor(
        Transform parent,
        float width,
        float depth
    )
    {
        GameObject floor =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        floor.name =
            "Floor";

        floor.transform.SetParent(
            parent
        );

        floor.transform.localPosition =
            new Vector3(
                0f,
                -0.1f,
                0f
            );

        floor.transform.localRotation =
            Quaternion.identity;

        floor.transform.localScale =
            new Vector3(
                width,
                0.2f,
                depth
            );

        BoxCollider collider =
            floor.GetComponent<BoxCollider>();

        if (collider != null)
            collider.isTrigger = false;

        ApplyFloorMaterial(
            floor
        );
    }

    private void CreateWalls(
        Transform parent,
        RoomRuntime room,
        float wallHeight,
        float wallThickness
    )
    {
        CreateHorizontalSide(
            parent,
            room,
            ArenaModule.RoomDirection.North,
            wallHeight,
            wallThickness
        );

        CreateHorizontalSide(
            parent,
            room,
            ArenaModule.RoomDirection.South,
            wallHeight,
            wallThickness
        );

        CreateVerticalSide(
            parent,
            room,
            ArenaModule.RoomDirection.East,
            wallHeight,
            wallThickness
        );

        CreateVerticalSide(
            parent,
            room,
            ArenaModule.RoomDirection.West,
            wallHeight,
            wallThickness
        );
    }

    private void CreateHorizontalSide(
        Transform parent,
        RoomRuntime room,
        ArenaModule.RoomDirection direction,
        float wallHeight,
        float wallThickness
    )
    {
        float z =
            direction ==
            ArenaModule.RoomDirection.North
                ? room.depth * 0.5f
                : -room.depth * 0.5f;

        int index =
            DirectionToIndex(direction);

        float opening =
            room.open[index]
                ? room.openingWidth[index]
                : 0f;

        float halfWidth =
            room.width * 0.5f;

        float wallY =
            wallHeight * 0.5f;

        if (opening <= 0.01f)
        {
            CreateWall(
                parent,
                $"Wall_{direction}",
                new Vector3(
                    0f,
                    wallY,
                    z
                ),
                new Vector3(
                    room.width,
                    wallHeight,
                    wallThickness
                )
            );

            return;
        }

        float halfOpening =
            Mathf.Clamp(
                opening * 0.5f,
                0.5f,
                halfWidth - 0.5f
            );

        float leftLength =
            halfWidth - halfOpening;

        if (leftLength > 0.05f)
        {
            CreateWall(
                parent,
                $"Wall_{direction}_Left",
                new Vector3(
                    -(halfOpening + leftLength * 0.5f),
                    wallY,
                    z
                ),
                new Vector3(
                    leftLength,
                    wallHeight,
                    wallThickness
                )
            );

            CreateWall(
                parent,
                $"Wall_{direction}_Right",
                new Vector3(
                    halfOpening + leftLength * 0.5f,
                    wallY,
                    z
                ),
                new Vector3(
                    leftLength,
                    wallHeight,
                    wallThickness
                )
            );
        }
    }

    private void CreateVerticalSide(
        Transform parent,
        RoomRuntime room,
        ArenaModule.RoomDirection direction,
        float wallHeight,
        float wallThickness
    )
    {
        float x =
            direction ==
            ArenaModule.RoomDirection.East
                ? room.width * 0.5f
                : -room.width * 0.5f;

        int index =
            DirectionToIndex(direction);

        float opening =
            room.open[index]
                ? room.openingWidth[index]
                : 0f;

        float halfDepth =
            room.depth * 0.5f;

        float wallY =
            wallHeight * 0.5f;

        if (opening <= 0.01f)
        {
            CreateWall(
                parent,
                $"Wall_{direction}",
                new Vector3(
                    x,
                    wallY,
                    0f
                ),
                new Vector3(
                    wallThickness,
                    wallHeight,
                    room.depth
                )
            );

            return;
        }

        float halfOpening =
            Mathf.Clamp(
                opening * 0.5f,
                0.5f,
                halfDepth - 0.5f
            );

        float frontLength =
            halfDepth - halfOpening;

        if (frontLength > 0.05f)
        {
            CreateWall(
                parent,
                $"Wall_{direction}_Front",
                new Vector3(
                    x,
                    wallY,
                    halfOpening + frontLength * 0.5f
                ),
                new Vector3(
                    wallThickness,
                    wallHeight,
                    frontLength
                )
            );

            CreateWall(
                parent,
                $"Wall_{direction}_Back",
                new Vector3(
                    x,
                    wallY,
                    -(halfOpening + frontLength * 0.5f)
                ),
                new Vector3(
                    wallThickness,
                    wallHeight,
                    frontLength
                )
            );
        }
    }

    private void CreateWall(
        Transform parent,
        string wallName,
        Vector3 localPosition,
        Vector3 localScale
    )
    {
        GameObject wall =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        wall.name =
            wallName;

        wall.transform.SetParent(
            parent
        );

        wall.transform.localPosition =
            localPosition;

        wall.transform.localRotation =
            Quaternion.identity;

        wall.transform.localScale =
            localScale;

        BoxCollider collider =
            wall.GetComponent<BoxCollider>();

        if (collider != null)
            collider.isTrigger = false;

        ApplyGeneratedMaterial(
            wall
        );
    }

    private void BuildCorridors()
    {
        for (
            int i = 0;
            i < roomConnections.Count;
            i++
        )
        {
            RoomConnectionRuntime connection =
                roomConnections[i];

            if (connection == null)
                continue;

            if (
                connection.roomA < 0 ||
                connection.roomB < 0 ||
                connection.roomA >= rooms.Count ||
                connection.roomB >= rooms.Count
            )
            {
                continue;
            }

            RoomRuntime roomA =
                rooms[connection.roomA];

            RoomRuntime roomB =
                rooms[connection.roomB];

            if (
                roomA == null ||
                roomB == null
            )
            {
                continue;
            }

            Transform connectionA =
                roomA.module.GetDirectionalConnection(
                    connection.directionFromA
                );

            Transform connectionB =
                roomB.module.GetDirectionalConnection(
                    GetOppositeDirection(
                        connection.directionFromA
                    )
                );

            if (
                connectionA == null ||
                connectionB == null
            )
            {
                continue;
            }

            CreateCorridor(
                roomA,
                roomB,
                connection.directionFromA,
                connectionA.position,
                connectionB.position,
                connection.width
            );
        }
    }

    private void CreateCorridor(
        RoomRuntime parent,
        RoomRuntime child,
        ArenaModule.RoomDirection direction,
        Vector3 start,
        Vector3 end,
        float width
    )
    {
        Vector3 forward =
            DirectionToVector(
                direction
            );

        float wallThickness =
            Random.Range(
                minimumWallThickness,
                maximumWallThickness
            );

        float wallHeight =
            Random.Range(
                minimumWallHeight,
                maximumWallHeight
            );

        float overlap =
            Mathf.Max(
                0.5f,
                wallThickness
            );

        Vector3 corridorStart =
            start -
            forward *
            overlap;

        Vector3 corridorEnd =
            end +
            forward *
            overlap;

        Vector3 difference =
            corridorEnd -
            corridorStart;

        difference.y = 0f;

        float length =
            difference.magnitude;

        if (length <= 0.1f)
            return;

        Vector3 midpoint =
            (corridorStart + corridorEnd) *
            0.5f;

        CreateCorridorFloor(
            midpoint,
            forward,
            width,
            length
        );

        CreateCorridorWalls(
            start,
            end,
            forward,
            width,
            wallThickness,
            wallHeight
        );
    }

    private void CreateCorridorFloor(
        Vector3 midpoint,
        Vector3 direction,
        float width,
        float length
    )
    {
        GameObject floor =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        floor.name =
            "CorridorFloor";

        floor.transform.SetParent(
            generatedRoot
        );

        floor.transform.position =
            new Vector3(
                midpoint.x,
                -0.1f,
                midpoint.z
            );

        floor.transform.rotation =
            Quaternion.LookRotation(
                direction
            );

        float floorWidth =
            width + 0.05f;

        floor.transform.localScale =
            new Vector3(
                floorWidth,
                0.2f,
                length
            );

        BoxCollider collider =
            floor.GetComponent<BoxCollider>();

        if (collider != null)
            collider.isTrigger = false;

        ApplyFloorMaterial(
            floor
        );
    }

    private void CreateCorridorWalls(
        Vector3 start,
        Vector3 end,
        Vector3 direction,
        float width,
        float wallThickness,
        float wallHeight
    )
    {
        Vector3 side =
            Vector3.Cross(
                Vector3.up,
                direction
            ).normalized;

        float halfWidth =
            width * 0.5f;

        float offset =
            halfWidth +
            wallThickness * 0.5f;

        float length =
            Vector3.Distance(
                start,
                end
            );

        if (length <= 0.1f)
            return;

        Vector3 midpoint =
            (start + end) *
            0.5f;

        float y =
            wallHeight * 0.5f;

        Vector3 leftPosition =
            midpoint +
            side *
            offset;

        Vector3 rightPosition =
            midpoint -
            side *
            offset;

        CreateSingleCorridorWall(
            "CorridorWall_Left",
            leftPosition,
            direction,
            wallThickness,
            wallHeight,
            length,
            y
        );

        CreateSingleCorridorWall(
            "CorridorWall_Right",
            rightPosition,
            direction,
            wallThickness,
            wallHeight,
            length,
            y
        );
    }

    private void CreateSingleCorridorWall(
        string wallName,
        Vector3 position,
        Vector3 direction,
        float thickness,
        float height,
        float length,
        float y
    )
    {
        GameObject wall =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        wall.name =
            wallName;

        wall.transform.SetParent(
            generatedRoot
        );

        wall.transform.position =
            new Vector3(
                position.x,
                y,
                position.z
            );

        wall.transform.rotation =
            Quaternion.LookRotation(
                direction
            );

        wall.transform.localScale =
            new Vector3(
                thickness,
                height,
                length
            );

        BoxCollider collider =
            wall.GetComponent<BoxCollider>();

        if (collider != null)
            collider.isTrigger = false;

        ApplyGeneratedMaterial(
            wall
        );
    }

    private void BuildPlayerSpawns()
    {
        foreach (
            RoomRuntime room in rooms
        )
        {
            if (
                room.roomType !=
                ArenaModule.RoomType.Start
            )
            {
                continue;
            }

            GameObject spawn =
                new GameObject(
                    "PlayerSpawn"
                );

            spawn.transform.SetParent(
                room.module.transform
            );

            spawn.transform.localPosition =
                new Vector3(
                    0f,
                    1.2f,
                    0f
                );

            spawn.transform.localRotation =
                Quaternion.identity;

            room.module.SetPlayerSpawn(
                spawn.transform
            );

            break;
        }
    }

    private void BuildCovers()
    {
        float playerCoverHeight =
            GetPlayerCoverHeight();

        Debug.Log(
            $"ArenaGenerator: Cover height = {playerCoverHeight:F2}m " +
            $"(based on Player collider)."
        );

        foreach (
            RoomRuntime room in rooms
        )
        {
            generatedCoverPositions.Clear();

            if (
                useCoverClusters &&
                HaveClusterPrefabs()
            )
            {
                BuildCoverClusters(room);
                continue;
            }

            BuildLegacyCovers(
                room,
                playerCoverHeight
            );
        }
    }

    private bool HaveClusterPrefabs()
    {
        return
            fencePrefab != null &&
            coverPrefab != null &&
            cratePrefab != null &&
            barrelPrefab != null;
    }

    private void BuildLegacyCovers(
        RoomRuntime room,
        float playerCoverHeight
    )
    {
            int count =
                Random.Range(
                    minimumCoverCount,
                    maximumCoverCount + 1
                );

            switch (room.roomType)
            {
                case ArenaModule.RoomType.Tactical:
                    count += 2;
                    break;

                case ArenaModule.RoomType.Elite:
                    count += 1;
                    break;

                case ArenaModule.RoomType.Final:
                    count += 3;
                    break;
            }

            count =
                Mathf.Clamp(
                    count,
                    minimumCoverCount,
                    maximumCoverCount + 3
                );

            room.module.SetCoverCount(
                count
            );

            int created = 0;

            for (
                int attempt = 0;
                attempt < count * 150;
                attempt++
            )
            {
                if (created >= count)
                    break;

                float margin = 2.5f;

                float x =
                    Random.Range(
                        -room.width * 0.5f + margin,
                        room.width * 0.5f - margin
                    );

                float z =
                    Random.Range(
                        -room.depth * 0.5f + margin,
                        room.depth * 0.5f - margin
                    );

                Vector3 localPosition =
                    new Vector3(
                        x,
                        0f,
                        z
                    );

                if (
                    new Vector2(
                        x,
                        z
                    ).magnitude < 3f
                )
                {
                    continue;
                }

                if (
                    IsNearAnyOpenConnection(
                        room,
                        localPosition
                    )
                )
                {
                    continue;
                }

                if (
                    IsTooClose(
                        localPosition,
                        generatedCoverPositions,
                        minimumDistanceBetweenCovers
                    )
                )
                {
                    continue;
                }

                float coverWidth =
                    Random.Range(
                        minimumCoverWidth,
                        maximumCoverWidth
                    );

                float coverDepth =
                    Random.Range(
                        minimumCoverDepth,
                        maximumCoverDepth
                    );

                GameObject cover =
                    GameObject.CreatePrimitive(
                        PrimitiveType.Cube
                    );

                cover.name =
                    $"Cover_{created + 1}";

                cover.transform.SetParent(
                    room.module.transform
                );

                cover.transform.localPosition =
                    new Vector3(
                        x,
                        playerCoverHeight * 0.5f,
                        z
                    );

                cover.transform.localRotation =
                    Quaternion.Euler(
                        0f,
                        Random.Range(
                            0f,
                            360f
                        ),
                        0f
                    );

                cover.transform.localScale =
                    new Vector3(
                        coverWidth,
                        playerCoverHeight,
                        coverDepth
                    );

                BoxCollider collider =
                    cover.GetComponent<BoxCollider>();

                if (collider != null)
                {
                    collider.isTrigger = false;
                    collider.center = Vector3.zero;
                    collider.size = Vector3.one;
                }

                ApplyGeneratedMaterial(
                    cover
                );

                CoverPoint coverPoint =
                    cover.AddComponent<CoverPoint>();

                coverPoint.SetSourceCollider(
                    collider
                );

                if (CoverSystem.Instance != null)
                {
                    CoverSystem.Instance.RegisterCoverPoint(
                        coverPoint
                    );
                }

                generatedCoverPositions.Add(
                    localPosition
                );

                room.module.SetCoverPoint(
                    created,
                    cover.transform
                );

                created++;
            }
    }

    private enum ClusterTemplate
    {
        FenceBarrel,
        FenceCrate,
        CoverBarrel,
        CoverCrates,
        FenceCover,
        WagonCamp
    }

    private void BuildCoverClusters(
        RoomRuntime room
    )
    {
        int clusterCount =
            Random.Range(
                minimumClustersPerRoom,
                maximumClustersPerRoom + 1
            );

        bool isStart =
            room.roomType ==
            ArenaModule.RoomType.Start;

        if (
            room.roomType ==
            ArenaModule.RoomType.Final
        )
        {
            clusterCount += 1;
        }

        if (isStart)
        {
            clusterCount =
                Mathf.Min(
                    clusterCount,
                    2
                );
        }

        List<Transform> anchors =
            new List<Transform>();

        int created = 0;
        int clusterIndex = 0;

        for (
            int attempt = 0;
            attempt < clusterCount * 60 &&
            created < clusterCount;
            attempt++
        )
        {
            float x =
                Random.Range(
                    -room.width * 0.5f + clusterMargin,
                    room.width * 0.5f - clusterMargin
                );

            float z =
                Random.Range(
                    -room.depth * 0.5f + clusterMargin,
                    room.depth * 0.5f - clusterMargin
                );

            Vector3 localPosition =
                new Vector3(
                    x,
                    0f,
                    z
                );

            if (
                new Vector2(
                    x,
                    z
                ).magnitude < clusterCenterClearance
            )
            {
                continue;
            }

            if (
                IsNearAnyOpenConnection(
                    room,
                    localPosition
                )
            )
            {
                continue;
            }

            if (
                IsTooClose(
                    localPosition,
                    generatedCoverPositions,
                    clusterSpacing
                )
            )
            {
                continue;
            }

            ClusterTemplate template =
                PickClusterTemplate(
                    room,
                    isStart
                );

            float radius =
                GetClusterRadius(
                    template
                );

            if (
                Mathf.Abs(x) + radius >
                room.width * 0.5f - 0.6f
            )
            {
                continue;
            }

            if (
                Mathf.Abs(z) + radius >
                room.depth * 0.5f - 0.6f
            )
            {
                continue;
            }

            float yaw =
                Random.Range(
                    0f,
                    360f
                );

            GameObject cluster =
                SpawnCluster(
                    room,
                    template,
                    localPosition,
                    yaw,
                    clusterIndex,
                    anchors
                );

            if (cluster == null)
            {
                continue;
            }

            generatedCoverPositions.Add(
                localPosition
            );

            ClusterFootprint footprint;
            footprint.roomIndex = room.index;
            footprint.localPosition = localPosition;
            footprint.radius = radius;
            clusterFootprints.Add(footprint);

            if (
                lanternPrefab != null &&
                Random.value < 0.4f
            )
            {
                TryPlaceLantern(
                    room,
                    localPosition,
                    yaw
                );
            }

            created++;
            clusterIndex++;
        }

        room.module.SetCoverCount(
            anchors.Count
        );

        for (
            int i = 0;
            i < anchors.Count;
            i++
        )
        {
            room.module.SetCoverPoint(
                i,
                anchors[i]
            );
        }

        Debug.Log(
            $"ArenaGenerator: Room {room.index + 1} " +
            $"clusters={created} anchors={anchors.Count}"
        );
    }

    private ClusterTemplate PickClusterTemplate(
        RoomRuntime room,
        bool isStart
    )
    {
        bool bigRoom =
            Mathf.Min(
                room.width,
                room.depth
            ) >= 17f;

        float roll =
            Random.value;

        if (
            !isStart &&
            bigRoom &&
            wagonPrefab != null &&
            roll < 0.08f
        )
        {
            return ClusterTemplate.WagonCamp;
        }

        if (roll < 0.30f)
            return ClusterTemplate.FenceBarrel;

        if (roll < 0.50f)
            return ClusterTemplate.FenceCrate;

        if (roll < 0.68f)
            return ClusterTemplate.CoverBarrel;

        if (roll < 0.85f)
            return ClusterTemplate.CoverCrates;

        return ClusterTemplate.FenceCover;
    }

    private float GetClusterRadius(
        ClusterTemplate template
    )
    {
        switch (template)
        {
            case ClusterTemplate.WagonCamp:
                return 4.4f;

            case ClusterTemplate.FenceCover:
                return 2.9f;

            case ClusterTemplate.CoverBarrel:
                return 2.3f;

            default:
                return 2.7f;
        }
    }

    private GameObject SpawnCluster(
        RoomRuntime room,
        ClusterTemplate template,
        Vector3 localPosition,
        float yaw,
        int clusterIndex,
        List<Transform> anchors
    )
    {
        GameObject cluster =
            new GameObject(
                $"CoverCluster_{clusterIndex + 1}_{template}"
            );

        cluster.transform.SetParent(
            room.module.transform
        );

        cluster.transform.localPosition =
            localPosition;

        cluster.transform.localRotation =
            Quaternion.Euler(
                0f,
                yaw,
                0f
            );

        switch (template)
        {
            case ClusterTemplate.FenceBarrel:
                AddClusterMember(
                    cluster.transform,
                    fencePrefab,
                    "Anchor_Fence",
                    Vector3.zero,
                    0f,
                    anchors
                );
                AddClusterMember(
                    cluster.transform,
                    barrelPrefab,
                    "Prop_Barrel",
                    new Vector3(2.2f, 0f, 0.4f),
                    30f,
                    anchors
                );
                break;

            case ClusterTemplate.FenceCrate:
                AddClusterMember(
                    cluster.transform,
                    fencePrefab,
                    "Anchor_Fence",
                    Vector3.zero,
                    0f,
                    anchors
                );
                AddClusterMember(
                    cluster.transform,
                    cratePrefab,
                    "Prop_Crate",
                    new Vector3(2.1f, 0f, -0.5f),
                    15f,
                    anchors
                );
                break;

            case ClusterTemplate.CoverBarrel:
                AddClusterMember(
                    cluster.transform,
                    coverPrefab,
                    "Anchor_Cover",
                    Vector3.zero,
                    0f,
                    anchors
                );
                AddClusterMember(
                    cluster.transform,
                    barrelPrefab,
                    "Prop_Barrel",
                    new Vector3(-1.7f, 0f, 0.9f),
                    0f,
                    anchors
                );
                break;

            case ClusterTemplate.CoverCrates:
                AddClusterMember(
                    cluster.transform,
                    coverPrefab,
                    "Anchor_Cover",
                    Vector3.zero,
                    0f,
                    anchors
                );
                AddClusterMember(
                    cluster.transform,
                    cratePrefab,
                    "Prop_CrateA",
                    new Vector3(1.8f, 0f, 0.7f),
                    10f,
                    anchors
                );
                AddClusterMember(
                    cluster.transform,
                    cratePrefab,
                    "Prop_CrateB",
                    new Vector3(-0.3f, 0f, 1.6f),
                    -20f,
                    anchors
                );
                break;

            case ClusterTemplate.FenceCover:
                AddClusterMember(
                    cluster.transform,
                    fencePrefab,
                    "Anchor_Fence",
                    Vector3.zero,
                    0f,
                    anchors
                );
                AddClusterMember(
                    cluster.transform,
                    coverPrefab,
                    "Anchor_Cover",
                    new Vector3(0.5f, 0f, 2.0f),
                    90f,
                    anchors
                );
                break;

            case ClusterTemplate.WagonCamp:
                AddClusterMember(
                    cluster.transform,
                    wagonPrefab,
                    "Anchor_Wagon",
                    Vector3.zero,
                    0f,
                    anchors
                );
                AddClusterMember(
                    cluster.transform,
                    cratePrefab,
                    "Prop_CrateA",
                    new Vector3(2.8f, 0f, 1.3f),
                    20f,
                    anchors
                );
                AddClusterMember(
                    cluster.transform,
                    cratePrefab,
                    "Prop_CrateB",
                    new Vector3(-2.6f, 0f, 1.5f),
                    -15f,
                    anchors
                );
                break;
        }

        return cluster;
    }

    private void AddClusterMember(
        Transform clusterRoot,
        GameObject prefab,
        string memberName,
        Vector3 localOffset,
        float localYaw,
        List<Transform> anchors
    )
    {
        if (
            clusterRoot == null ||
            prefab == null
        )
        {
            return;
        }

        GameObject member =
            Instantiate(
                prefab,
                clusterRoot
            );

        member.name =
            memberName;

        member.transform.localPosition =
            localOffset;

        member.transform.localRotation =
            Quaternion.Euler(
                0f,
                localYaw,
                0f
            );

        member.transform.localScale =
            Vector3.one;

        if (CoverSystem.Instance != null)
        {
            CoverPoint[] points =
                member.GetComponentsInChildren<CoverPoint>(
                    true
                );

            foreach (
                CoverPoint point
                in points
            )
            {
                if (
                    point == null ||
                    !point.gameObject.activeSelf
                )
                {
                    continue;
                }

                CoverSystem.Instance.RegisterCoverPoint(
                    point
                );
            }
        }

        if (
            anchors != null &&
            anchors.Count < 10 &&
            memberName.StartsWith("Anchor_")
        )
        {
            anchors.Add(
                member.transform
            );
        }
    }

    private void TryPlaceLantern(
        RoomRuntime room,
        Vector3 clusterCenter,
        float clusterYaw
    )
    {
        float angle =
            clusterYaw + 140f;

        float distance = 2.6f;

        Vector3 offset =
            new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
                0f,
                Mathf.Sin(angle * Mathf.Deg2Rad) * distance
            );

        Vector3 localPosition =
            clusterCenter + offset;

        if (
            Mathf.Abs(localPosition.x) >
            room.width * 0.5f - 1f
        )
        {
            return;
        }

        if (
            Mathf.Abs(localPosition.z) >
            room.depth * 0.5f - 1f
        )
        {
            return;
        }

        if (
            new Vector2(
                localPosition.x,
                localPosition.z
            ).magnitude < clusterCenterClearance
        )
        {
            return;
        }

        if (
            IsNearAnyOpenConnection(
                room,
                localPosition
            )
        )
        {
            return;
        }

        GameObject lantern =
            Instantiate(
                lanternPrefab,
                room.module.transform
            );

        lantern.name =
            "Accent_Lantern";

        lantern.transform.localPosition =
            localPosition;

        lantern.transform.localRotation =
            Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

        lantern.transform.localScale =
            Vector3.one;
    }

    private bool IsInsideClusterFootprint(
        int roomIndex,
        Vector3 localPosition,
        float extraRadius
    )
    {
        for (
            int i = 0;
            i < clusterFootprints.Count;
            i++
        )
        {
            ClusterFootprint footprint =
                clusterFootprints[i];

            if (
                footprint.roomIndex !=
                roomIndex
            )
            {
                continue;
            }

            Vector3 difference =
                localPosition -
                footprint.localPosition;

            difference.y = 0f;

            if (
                difference.magnitude <
                footprint.radius + extraRadius
            )
            {
                return true;
            }
        }

        return false;
    }

    private float GetPlayerCoverHeight()
    {
        if (player == null)
            return 2.0f;

        CapsuleCollider capsule =
            player.GetComponent<CapsuleCollider>();

        if (capsule != null)
        {
            float worldHeight =
                GetWorldCapsuleHeight(
                    capsule
                );

            return Mathf.Max(
                0.5f,
                worldHeight + coverHeightPadding
            );
        }

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
        {
            float worldHeight =
                Mathf.Abs(
                    controller.height *
                    player.lossyScale.y
                );

            return Mathf.Max(
                0.5f,
                worldHeight + coverHeightPadding
            );
        }

        Debug.LogWarning(
            "ArenaGenerator: CapsuleCollider или CharacterController у Player не найден. " +
            "Используется резервная высота укрытия 2.0 м."
        );

        return 2.0f;
    }

    private float GetWorldCapsuleHeight(
        CapsuleCollider capsule
    )
    {
        if (capsule == null)
            return 2.0f;

        float scaleX =
            Mathf.Abs(
                capsule.transform.lossyScale.x
            );

        float scaleY =
            Mathf.Abs(
                capsule.transform.lossyScale.y
            );

        float scaleZ =
            Mathf.Abs(
                capsule.transform.lossyScale.z
            );

        float scaleAlongAxis;

        switch (capsule.direction)
        {
            case 0:
                scaleAlongAxis =
                    scaleX;
                break;

            case 2:
                scaleAlongAxis =
                    scaleZ;
                break;

            default:
                scaleAlongAxis =
                    scaleY;
                break;
        }

        return Mathf.Abs(
            capsule.height *
            scaleAlongAxis
        );
    }

    private void BuildEnemySpawnZones()
    {
        foreach (
            RoomRuntime room in rooms
        )
        {
            generatedSpawnZonePositions.Clear();

            int count =
                Random.Range(
                    minimumSpawnZones,
                    maximumSpawnZones + 1
                );

            if (
                room.roomType ==
                ArenaModule.RoomType.Start
            )
            {
                count =
                    Mathf.Max(
                        minimumSpawnZones - 1,
                        count - 1
                    );
            }

            if (
                room.roomType ==
                ArenaModule.RoomType.Final
            )
            {
                count =
                    Mathf.Min(
                        maximumSpawnZones + 1,
                        count + 1
                    );
            }

            room.module.SetEnemySpawnCount(
                count
            );

            int created = 0;

            for (
                int attempt = 0;
                attempt < count * 150;
                attempt++
            )
            {
                if (created >= count)
                    break;

                float margin = 2.5f;

                float x =
                    Random.Range(
                        -room.width * 0.5f + margin,
                        room.width * 0.5f - margin
                    );

                float z =
                    Random.Range(
                        -room.depth * 0.5f + margin,
                        room.depth * 0.5f - margin
                    );

                Vector3 localPosition =
                    new Vector3(
                        x,
                        0.05f,
                        z
                    );

                if (
                    new Vector2(
                        x,
                        z
                    ).magnitude < 3f
                )
                {
                    continue;
                }

                if (
                    IsNearAnyOpenConnection(
                        room,
                        localPosition
                    )
                )
                {
                    continue;
                }

                if (
                    IsTooClose(
                        localPosition,
                        generatedSpawnZonePositions,
                        minimumDistanceBetweenSpawnZones
                    )
                )
                {
                    continue;
                }

                float radius =
                    Random.Range(
                        minimumSpawnZoneRadius,
                        maximumSpawnZoneRadius
                    );

                if (
                    IsInsideClusterFootprint(
                        room.index,
                        localPosition,
                        radius + 0.5f
                    )
                )
                {
                    continue;
                }

                GameObject zoneObject =
                    new GameObject(
                        $"EnemySpawnZone_{created + 1}"
                    );

                zoneObject.transform.SetParent(
                    room.module.transform
                );

                zoneObject.transform.localPosition =
                    localPosition;

                zoneObject.transform.localRotation =
                    Quaternion.identity;

                EnemySpawnZone zone =
                    zoneObject.AddComponent<
                        EnemySpawnZone
                    >();

                zone.Initialize(
                    room.module,
                    radius
                );

                generatedSpawnZonePositions.Add(
                    localPosition
                );

                room.module.SetEnemySpawnZone(
                    created,
                    zone
                );

                created++;
            }
        }
    }

    private bool IsNearAnyOpenConnection(
        RoomRuntime room,
        Vector3 localPosition
    )
    {
        for (
            int i = 0;
            i < 4;
            i++
        )
        {
            if (!room.open[i])
                continue;

            ArenaModule.RoomDirection direction =
                IndexToDirection(i);

            Transform connection =
                room.module.GetDirectionalConnection(
                    direction
                );

            if (connection == null)
                continue;

            if (
                Vector3.Distance(
                    localPosition,
                    connection.localPosition
                ) < 4f
            )
            {
                return true;
            }
        }

        return false;
    }

    private bool IsTooClose(
        Vector3 position,
        List<Vector3> positions,
        float minimumDistance
    )
    {
        float minSqr =
            minimumDistance *
            minimumDistance;

        foreach (
            Vector3 other in positions
        )
        {
            if (
                (position - other).sqrMagnitude <
                minSqr
            )
            {
                return true;
            }
        }

        return false;
    }

    private void SpawnPlayer(
        ArenaModule startRoom
    )
    {
        if (
            startRoom == null ||
            startRoom.PlayerSpawn == null
        )
        {
            Debug.LogError(
                "ArenaGenerator: PlayerSpawn не найден."
            );

            return;
        }

        player.position =
            startRoom.PlayerSpawn.position;

        player.rotation =
            startRoom.PlayerSpawn.rotation;

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;

            rb.WakeUp();
        }
    }

    private ArenaModule FindStartRoom()
    {
        foreach (
            RoomRuntime room in rooms
        )
        {
            if (
                room.roomType ==
                ArenaModule.RoomType.Start
            )
            {
                return room.module;
            }
        }

        return null;
    }

    private RoomRuntime FindRoomRuntime(
        int index
    )
    {
        foreach (
            RoomRuntime room in rooms
        )
        {
            if (room.index == index)
                return room;
        }

        return null;
    }

    private ArenaModule.RoomType DetermineRoomType(
        int index,
        int totalRooms
    )
    {
        if (index == 0)
            return ArenaModule.RoomType.Start;

        if (index == totalRooms - 1)
            return ArenaModule.RoomType.Final;

        float progress =
            (float)index /
            Mathf.Max(
                1,
                totalRooms - 1
            );

        if (progress < 0.3f)
        {
            return Random.value < 0.7f
                ? ArenaModule.RoomType.Combat
                : ArenaModule.RoomType.Tactical;
        }

        if (progress < 0.7f)
        {
            float roll =
                Random.value;

            if (roll < 0.45f)
                return ArenaModule.RoomType.Combat;

            if (roll < 0.8f)
                return ArenaModule.RoomType.Tactical;

            return ArenaModule.RoomType.Elite;
        }

        float lateRoll =
            Random.value;

        if (lateRoll < 0.25f)
            return ArenaModule.RoomType.Combat;

        if (lateRoll < 0.65f)
            return ArenaModule.RoomType.Tactical;

        return ArenaModule.RoomType.Elite;
    }

    private void ShuffleList<T>(
        List<T> list
    )
    {
        for (
            int i = list.Count - 1;
            i > 0;
            i--
        )
        {
            int randomIndex =
                Random.Range(
                    0,
                    i + 1
                );

            T temp =
                list[i];

            list[i] =
                list[randomIndex];

            list[randomIndex] =
                temp;
        }
    }

    private int DirectionToIndex(
        ArenaModule.RoomDirection direction
    )
    {
        switch (direction)
        {
            case ArenaModule.RoomDirection.North:
                return 0;

            case ArenaModule.RoomDirection.East:
                return 1;

            case ArenaModule.RoomDirection.South:
                return 2;

            default:
                return 3;
        }
    }

    private ArenaModule.RoomDirection IndexToDirection(
        int index
    )
    {
        switch (index)
        {
            case 0:
                return ArenaModule.RoomDirection.North;

            case 1:
                return ArenaModule.RoomDirection.East;

            case 2:
                return ArenaModule.RoomDirection.South;

            default:
                return ArenaModule.RoomDirection.West;
        }
    }

    private ArenaModule.RoomDirection GetOppositeDirection(
        ArenaModule.RoomDirection direction
    )
    {
        switch (direction)
        {
            case ArenaModule.RoomDirection.North:
                return ArenaModule.RoomDirection.South;

            case ArenaModule.RoomDirection.East:
                return ArenaModule.RoomDirection.West;

            case ArenaModule.RoomDirection.South:
                return ArenaModule.RoomDirection.North;

            default:
                return ArenaModule.RoomDirection.East;
        }
    }

    private Vector3 DirectionToVector(
        ArenaModule.RoomDirection direction
    )
    {
        switch (direction)
        {
            case ArenaModule.RoomDirection.North:
                return Vector3.forward;

            case ArenaModule.RoomDirection.East:
                return Vector3.right;

            case ArenaModule.RoomDirection.South:
                return Vector3.back;

            default:
                return Vector3.left;
        }
    }

    private float GetMaximumPassageWidth(
        RoomRuntime room,
        ArenaModule.RoomDirection direction
    )
    {
        if (
            direction ==
            ArenaModule.RoomDirection.North ||
            direction ==
            ArenaModule.RoomDirection.South
        )
        {
            return room.width * 0.35f;
        }

        return room.depth * 0.35f;
    }

    private void ClearArena()
    {
        if (generatedRoot != null)
        {
            Destroy(
                generatedRoot.gameObject
            );

            generatedRoot = null;
        }

        rooms.Clear();

        roomConnections.Clear();

        generatedCoverPositions.Clear();

        generatedSpawnZonePositions.Clear();

        clusterFootprints.Clear();

        timberPostTemplate = null;

        generatedRoomCount = 0;
    }

    private Transform timberPostTemplate;

    private void BuildTimberFraming()
    {
        if (logWallPrefab == null)
        {
            return;
        }

        Transform model =
            logWallPrefab.transform.Find("Model");

        if (model != null)
        {
            foreach (
                Transform child
                in model
            )
            {
                if (
                    child != null &&
                    child.name.Contains("Post")
                )
                {
                    timberPostTemplate = child;
                    break;
                }
            }
        }

        if (timberPostTemplate == null)
        {
            return;
        }

        int posts = 0;

        foreach (
            RoomRuntime room
            in rooms
        )
        {
            if (
                room == null ||
                room.module == null
            )
            {
                continue;
            }

            float hx =
                room.width * 0.5f - 0.35f;

            float hz =
                room.depth * 0.5f - 0.35f;

            SpawnTimberPost(
                room.module.transform,
                new Vector3(hx, 0f, hz),
                0f
            );
            SpawnTimberPost(
                room.module.transform,
                new Vector3(-hx, 0f, hz),
                90f
            );
            SpawnTimberPost(
                room.module.transform,
                new Vector3(hx, 0f, -hz),
                90f
            );
            SpawnTimberPost(
                room.module.transform,
                new Vector3(-hx, 0f, -hz),
                0f
            );
            posts += 4;

            for (
                int i = 0;
                i < 4;
                i++
            )
            {
                if (!room.open[i])
                    continue;

                ArenaModule.RoomDirection direction =
                    IndexToDirection(i);

                Transform connection =
                    room.module.GetDirectionalConnection(
                        direction
                    );

                if (connection == null)
                    continue;

                Vector3 center =
                    connection.localPosition;

                Vector3 lateral =
                    (
                        direction ==
                        ArenaModule.RoomDirection.North ||
                        direction ==
                        ArenaModule.RoomDirection.South
                    )
                        ? new Vector3(1f, 0f, 0f)
                        : new Vector3(0f, 0f, 1f);

                float halfOpening =
                    Mathf.Max(
                        1.5f,
                        room.openingWidth[i] * 0.5f
                    );

                SpawnTimberPost(
                    room.module.transform,
                    center + lateral * (halfOpening + 0.35f),
                    0f
                );
                SpawnTimberPost(
                    room.module.transform,
                    center - lateral * (halfOpening + 0.35f),
                    0f
                );
                posts += 2;
            }
        }

        Debug.Log(
            $"ArenaGenerator: Timber framing posts={posts}"
        );
    }

    private void SpawnTimberPost(
        Transform parent,
        Vector3 localPosition,
        float yaw
    )
    {
        if (
            parent == null ||
            timberPostTemplate == null
        )
        {
            return;
        }

        GameObject holder =
            new GameObject(
                "TimberPost"
            );

        holder.transform.SetParent(
            parent,
            false
        );

        holder.transform.localPosition =
            localPosition;

        holder.transform.localRotation =
            Quaternion.Euler(
                0f,
                yaw,
                0f
            );

        GameObject frame =
            new GameObject(
                "Frame"
            );

        frame.transform.SetParent(
            holder.transform,
            false
        );

        frame.transform.localRotation =
            Quaternion.Euler(
                -90f,
                0f,
                0f
            );

        GameObject post =
            Instantiate(
                timberPostTemplate.gameObject
            );

        post.name =
            "Post";

        post.transform.SetParent(
            frame.transform,
            false
        );

        post.transform.localPosition =
            timberPostTemplate.localPosition;

        post.transform.localRotation =
            timberPostTemplate.localRotation;

        post.transform.localScale =
            timberPostTemplate.localScale;

        Collider collider =
            post.GetComponent<Collider>();

        if (collider != null)
        {
            Destroy(collider);
        }
    }
}