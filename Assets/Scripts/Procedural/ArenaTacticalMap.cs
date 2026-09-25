using System.Collections.Generic;
using UnityEngine;

public class ArenaTacticalMap : MonoBehaviour
{
    public static ArenaTacticalMap Instance { get; private set; }

    [Header("Grid")]
    [SerializeField] private float cellSize = 1.5f;
    [SerializeField] private float agentRadius = 0.65f;
    [SerializeField] private float obstacleCheckHeight = 0.8f;
    [SerializeField] private float obstacleCheckRadius = 0.55f;

    [Header("Pathfinding")]
    [SerializeField] private int maxPathNodes = 2500;
    [SerializeField] private int nearestCellSearchRadius = 5;

    [Header("Tactical")]
    [SerializeField] private float flankDistance = 5.5f;
    [SerializeField] private float pressureDistance = 3.5f;
    [SerializeField] private float supportDistance = 6f;
    [SerializeField] private float exitOffset = 2.2f;

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;
    [SerializeField] private bool drawGrid = false;
    [SerializeField] private bool drawPaths = true;

    private readonly List<RoomMap> rooms =
        new List<RoomMap>();

    private readonly Dictionary<ArenaModule, RoomMap> roomLookup =
        new Dictionary<ArenaModule, RoomMap>();

    private readonly Dictionary<EnemyTacticalPlanner, List<Vector3>> debugPaths =
        new Dictionary<EnemyTacticalPlanner, List<Vector3>>();

    private bool built;

    public bool IsBuilt => built;

    public float CellSize => cellSize;

    public float AgentRadius => agentRadius;

    public IReadOnlyList<RoomMap> Rooms => rooms;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Build()
    {
        rooms.Clear();
        roomLookup.Clear();
        debugPaths.Clear();

        ArenaModule[] modules =
            FindObjectsByType<ArenaModule>(
                FindObjectsSortMode.None
            );

        foreach (ArenaModule module in modules)
        {
            if (module == null)
                continue;

            RoomMap room =
                BuildRoom(module);

            if (room == null)
                continue;

            rooms.Add(room);
            roomLookup[module] = room;
        }

        BuildRoomConnections();

        built = true;

        if (showDebug)
        {
            Debug.Log(
                $"[TACTICAL MAP] BUILT | ROOMS={rooms.Count} | CELL={cellSize:F2}"
            );
        }
    }

    private RoomMap BuildRoom(
        ArenaModule module
    )
    {
        Bounds bounds =
            module.GetWorldBounds();

        if (bounds.size.x <= 1f ||
            bounds.size.z <= 1f)
        {
            return null;
        }

        int width =
            Mathf.Clamp(
                Mathf.CeilToInt(
                    bounds.size.x / cellSize
                ),
                3,
                80
            );

        int height =
            Mathf.Clamp(
                Mathf.CeilToInt(
                    bounds.size.z / cellSize
                ),
                3,
                80
            );

        RoomMap room =
            new RoomMap(
                module,
                width,
                height,
                cellSize
            );

        Vector3 origin =
            new Vector3(
                bounds.min.x + cellSize * 0.5f,
                bounds.center.y,
                bounds.min.z + cellSize * 0.5f
            );

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position =
                    origin +
                    new Vector3(
                        x * cellSize,
                        0f,
                        z * cellSize
                    );

                bool walkable =
                    IsWorldPositionWalkable(
                        position,
                        module
                    );

                room.SetCell(
                    x,
                    z,
                    position,
                    walkable
                );
            }
        }

        BuildRoomSpecialPoints(room);

        return room;
    }

    private bool IsWorldPositionWalkable(
        Vector3 position,
        ArenaModule room
    )
    {
        Bounds bounds =
            room.GetWorldBounds();

        float margin =
            Mathf.Max(
                agentRadius,
                cellSize * 0.45f
            );

        if (position.x <= bounds.min.x + margin ||
            position.x >= bounds.max.x - margin ||
            position.z <= bounds.min.z + margin ||
            position.z >= bounds.max.z - margin)
        {
            return false;
        }

        Vector3 origin =
            position +
            Vector3.up *
            obstacleCheckHeight;

        Collider[] hits =
            Physics.OverlapSphere(
                origin,
                obstacleCheckRadius,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore
            );

        foreach (Collider hit in hits)
        {
            if (hit == null)
                continue;

            EnemyController enemy =
                hit.GetComponentInParent<EnemyController>();

            if (enemy != null)
                continue;

            if (hit.GetComponentInParent<CoverPoint>() != null)
                return false;

            if (hit.CompareTag("Player"))
                continue;

            return false;
        }

        return true;
    }

    private void BuildRoomSpecialPoints(
        RoomMap room
    )
    {
        ArenaModule module =
            room.Module;

        if (module == null)
            return;

        Transform[] connections =
            module.Connections;

        if (connections != null)
        {
            foreach (Transform connection in connections)
            {
                if (connection == null)
                    continue;

                Vector3 point =
                    connection.position;

                point.y =
                    module.transform.position.y;

                room.Exits.Add(point);
            }
        }

        Transform[] covers =
            module.CoverPoints;

        if (covers != null)
        {
            foreach (Transform cover in covers)
            {
                if (cover == null)
                    continue;

                CoverPoint coverPoint =
                    cover.GetComponent<CoverPoint>();

                if (coverPoint == null)
                    coverPoint =
                        cover.GetComponentInChildren<CoverPoint>();

                if (coverPoint == null)
                    continue;

                if (!IsCoverReachable(room, coverPoint.transform.position))
                    continue;

                room.ValidCovers.Add(coverPoint);
            }
        }
    }

    private bool IsCoverReachable(
        RoomMap room,
        Vector3 position
    )
    {
        GridCell cell;

        if (!TryGetNearestCell(
            room,
            position,
            out cell
        ))
        {
            return false;
        }

        return cell != null &&
               cell.Walkable;
    }

    private void BuildRoomConnections()
    {
        foreach (RoomMap room in rooms)
        {
            room.Neighbours.Clear();

            if (room.Module == null)
                continue;

            if (room.Module.gameObject.name == "Room_2_Combat")
            {
                Transform[] dbgConnections =
                    room.Module.Connections;

                string dbgText =
                    "[TACTICAL CONNECTION DEBUG] " +
                    "room=Room_2_Combat " +
                    "moduleNull=False " +
                    "connectionsNull=" + (dbgConnections == null) + " " +
                    "connectionsLength=" +
                    (dbgConnections != null ? dbgConnections.Length : -1);

                if (dbgConnections != null)
                {
                    for (
                        int dbgI = 0;
                        dbgI < dbgConnections.Length;
                        dbgI++)
                    {
                        Transform dbgC =
                            dbgConnections[dbgI];

                        if (dbgC == null)
                        {
                            dbgText +=
                                " connection[" + dbgI + "]=NULL";
                        }
                        else
                        {
                            dbgText +=
                                " connection[" + dbgI + "]=" +
                                dbgC.name +
                                "@(" +
                                dbgC.position.x.ToString("F2") + "," +
                                dbgC.position.y.ToString("F2") + "," +
                                dbgC.position.z.ToString("F2") + ")";
                        }
                    }
                }

                Debug.Log(dbgText);
            }

            Transform[] connections =
                room.Module.Connections;

            if (connections == null)
                continue;

            foreach (Transform connection in connections)
            {
                if (connection == null)
                    continue;

                RoomMap best =
                    FindRoomNearConnection(
                        room,
                        connection.position
                    );

                if (room.Module.gameObject.name == "Room_2_Combat")
                {
                    string dbgNear =
                        "[TACTICAL NEAR DEBUG] " +
                        "source=Room_2_Combat " +
                        "connection=" + connection.name +
                        "@(" +
                        connection.position.x.ToString("F2") + "," +
                        connection.position.y.ToString("F2") + "," +
                        connection.position.z.ToString("F2") + ")";

                    string dbgBestName = "NULL";
                    float dbgBestDist = float.MaxValue;

                    foreach (RoomMap dbgRoom in rooms)
                    {
                        if (dbgRoom == null ||
                            dbgRoom == room)
                        {
                            continue;
                        }

                        string dbgRoomName =
                            (dbgRoom.Module != null) ?
                            dbgRoom.Module.gameObject.name :
                            "NULL-MODULE";

                        float dbgDist = float.MaxValue;

                        if (dbgRoom.Module != null)
                        {
                            dbgDist =
                                DistanceToBounds(
                                    dbgRoom.Module.GetWorldBounds(),
                                    connection.position);
                        }

                        dbgNear +=
                            " cand[" + dbgRoomName + "]=" +
                            dbgDist.ToString("F2");

                        if (dbgDist < dbgBestDist)
                        {
                            dbgBestDist = dbgDist;
                            dbgBestName = dbgRoomName;
                        }
                    }

                    float dbgThreshold =
                        Mathf.Max(12f, cellSize * 8f);

                    dbgNear +=
                        " best=" + dbgBestName +
                        " bestDist=" + dbgBestDist.ToString("F2") +
                        " threshold=" + dbgThreshold.ToString("F2") +
                        " result=" +
                        ((best != null && best.Module != null) ?
                            best.Module.gameObject.name : "NULL");

                    Debug.Log(dbgNear);
                }

                if (best == null)
                    continue;

                if (!room.Neighbours.Contains(best))
                    room.Neighbours.Add(best);
            }
        }
    }

    private RoomMap FindRoomNearConnection(
        RoomMap source,
        Vector3 connection
    )
    {
        if (source == null ||
            source.Module == null)
        {
            return null;
        }

        Vector3 sourceCenter =
            source.Module.GetWorldBounds().center;

        Vector3 outward =
            connection - sourceCenter;

        outward.y = 0f;

        if (outward.sqrMagnitude <= 0f)
        {
            return null;
        }

        outward.Normalize();

        RoomMap best = null;
        float bestDistance = float.MaxValue;

        foreach (RoomMap room in rooms)
        {
            if (room == null ||
                room == source ||
                room.Module == null)
            {
                continue;
            }

            Bounds bounds =
                room.Module.GetWorldBounds();

            Vector3 closest =
                bounds.ClosestPoint(connection);

            Vector3 toCandidate =
                closest - connection;

            toCandidate.y = 0f;

            if (Vector3.Dot(toCandidate, outward) < 0f)
            {
                continue;
            }

            float distance =
                DistanceToBounds(
                    bounds,
                    connection
                );

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = room;
            }
        }

        return best;
    }

    private float DistanceToBounds(
        Bounds bounds,
        Vector3 point
    )
    {
        Vector3 closest =
            bounds.ClosestPoint(point);

        return Vector3.Distance(
            closest,
            point
        );
    }

    public RoomMap GetRoom(
        ArenaModule module
    )
    {
        if (module == null)
            return null;

        RoomMap room;

        if (roomLookup.TryGetValue(
            module,
            out room
        ))
        {
            return room;
        }

        return null;
    }

    public RoomMap FindRoom(
        Vector3 position
    )
    {
        foreach (RoomMap room in rooms)
        {
            if (room == null ||
                room.Module == null)
            {
                continue;
            }

            Bounds bounds =
                room.Module.GetWorldBounds();

            if (bounds.Contains(position))
                return room;
        }

        RoomMap nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (RoomMap room in rooms)
        {
            if (room == null ||
                room.Module == null)
            {
                continue;
            }

            float distance =
                DistanceToBounds(
                    room.Module.GetWorldBounds(),
                    position
                );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = room;
            }
        }

        return nearest;
    }

    public bool TryFindPath(
        Vector3 start,
        Vector3 target,
        List<Vector3> result
    )
    {
        result.Clear();

        if (!built)
            Build();

        RoomMap startRoom =
            FindRoom(start);

        RoomMap targetRoom =
            FindRoom(target);

        if (startRoom == null ||
            targetRoom == null)
        {
            return false;
        }

        if (startRoom == targetRoom)
        {
            return TryFindPathInsideRoom(
                startRoom,
                start,
                target,
                result
            );
        }

        List<RoomMap> roomRoute =
            FindRoomRoute(
                startRoom,
                targetRoom
            );

        if (roomRoute.Count == 0)
            return false;

        Vector3 current =
            start;

        for (int i = 0; i < roomRoute.Count; i++)
        {
            RoomMap room =
                roomRoute[i];

            Vector3 roomTarget;

            if (i == roomRoute.Count - 1)
            {
                roomTarget = target;
            }
            else
            {
                RoomMap next =
                    roomRoute[i + 1];

                roomTarget =
                    FindExitPointBetweenRooms(
                        room,
                        next
                    );
            }

            List<Vector3> segment =
                new List<Vector3>();

            if (!TryFindPathInsideRoom(
                room,
                current,
                roomTarget,
                segment
            ))
            {
                return false;
            }

            AppendPath(
                result,
                segment
            );

            current =
                roomTarget;

            if (i < roomRoute.Count - 1)
            {
                Vector3 entry =
                    FindEntryPointBetweenRooms(
                        room,
                        roomRoute[i + 1]
                    );

                if (Vector3.Distance(
                    current,
                    entry
                ) > 0.25f)
                {
                    result.Add(entry);
                }

                current = entry;
            }
        }

        SimplifyPath(result);

        return result.Count > 0;
    }

    private bool TryFindPathInsideRoom(
        RoomMap room,
        Vector3 start,
        Vector3 target,
        List<Vector3> result
    )
    {
        result.Clear();

        GridCell startCell;
        GridCell targetCell;

        if (!TryGetNearestWalkableCell(
            room,
            start,
            out startCell
        ))
        {
            return false;
        }

        if (!TryGetNearestWalkableCell(
            room,
            target,
            out targetCell
        ))
        {
            return false;
        }

        if (startCell == targetCell)
        {
            result.Add(target);
            return true;
        }

        List<GridCell> open =
            new List<GridCell>();

        HashSet<GridCell> closed =
            new HashSet<GridCell>();

        Dictionary<GridCell, GridCell> cameFrom =
            new Dictionary<GridCell, GridCell>();

        Dictionary<GridCell, float> gScore =
            new Dictionary<GridCell, float>();

        Dictionary<GridCell, float> fScore =
            new Dictionary<GridCell, float>();

        open.Add(startCell);
        gScore[startCell] = 0f;
        fScore[startCell] =
            Heuristic(
                startCell,
                targetCell
            );

        int processed = 0;

        while (
            open.Count > 0 &&
            processed < maxPathNodes
        )
        {
            processed++;

            GridCell current =
                GetLowestScoreCell(
                    open,
                    fScore
                );

            if (current == targetCell)
            {
                ReconstructPath(
                    cameFrom,
                    current,
                    result
                );

                if (result.Count > 0)
                {
                    result[result.Count - 1] =
                        target;
                }

                return true;
            }

            open.Remove(current);
            closed.Add(current);

            List<GridCell> neighbours =
                room.GetNeighbours(current);

            foreach (GridCell neighbour in neighbours)
            {
                if (neighbour == null ||
                    !neighbour.Walkable ||
                    closed.Contains(neighbour))
                {
                    continue;
                }

                float tentative =
                    GetScore(
                        gScore,
                        current
                    ) +
                    Vector3.Distance(
                        current.Position,
                        neighbour.Position
                    );

                float old =
                    GetScore(
                        gScore,
                        neighbour
                    );

                if (!open.Contains(neighbour))
                    open.Add(neighbour);
                else if (tentative >= old)
                    continue;

                cameFrom[neighbour] =
                    current;

                gScore[neighbour] =
                    tentative;

                fScore[neighbour] =
                    tentative +
                    Heuristic(
                        neighbour,
                        targetCell
                    );
            }
        }

        return false;
    }

    private float GetScore(
        Dictionary<GridCell, float> scores,
        GridCell cell
    )
    {
        float value;

        if (scores.TryGetValue(
            cell,
            out value
        ))
        {
            return value;
        }

        return float.PositiveInfinity;
    }

    private float Heuristic(
        GridCell a,
        GridCell b
    )
    {
        return Vector3.Distance(
            a.Position,
            b.Position
        );
    }

    private GridCell GetLowestScoreCell(
        List<GridCell> cells,
        Dictionary<GridCell, float> scores
    )
    {
        GridCell best =
            cells[0];

        float bestScore =
            GetScore(
                scores,
                best
            );

        for (int i = 1; i < cells.Count; i++)
        {
            GridCell candidate =
                cells[i];

            float score =
                GetScore(
                    scores,
                    candidate
                );

            if (score < bestScore)
            {
                best = candidate;
                bestScore = score;
            }
        }

        return best;
    }

    private void ReconstructPath(
        Dictionary<GridCell, GridCell> cameFrom,
        GridCell current,
        List<Vector3> result
    )
    {
        List<Vector3> reverse =
            new List<Vector3>();

        reverse.Add(current.Position);

        while (
            cameFrom.ContainsKey(current)
        )
        {
            current =
                cameFrom[current];

            reverse.Add(
                current.Position
            );
        }

        reverse.Reverse();

        result.AddRange(reverse);
    }

    private bool TryGetNearestWalkableCell(
        RoomMap room,
        Vector3 position,
        out GridCell result
    )
    {
        if (TryGetNearestCell(
            room,
            position,
            out result
        ))
        {
            if (result.Walkable)
                return true;
        }

        for (int radius = 1;
             radius <= nearestCellSearchRadius;
             radius++)
        {
            GridCell best = null;
            float bestDistance = float.MaxValue;

            foreach (GridCell cell in room.Cells)
            {
                if (cell == null ||
                    !cell.Walkable)
                {
                    continue;
                }

                int dx =
                    Mathf.Abs(
                        cell.X -
                        room.GetApproximateX(position)
                    );

                int dz =
                    Mathf.Abs(
                        cell.Z -
                        room.GetApproximateZ(position)
                    );

                if (Mathf.Max(dx, dz) > radius)
                    continue;

                float distance =
                    Vector3.Distance(
                        cell.Position,
                        position
                    );

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = cell;
                }
            }

            if (best != null)
            {
                result = best;
                return true;
            }
        }

        result = null;
        return false;
    }

    private bool TryGetNearestCell(
        RoomMap room,
        Vector3 position,
        out GridCell result
    )
    {
        result = null;

        if (room == null)
            return false;

        float bestDistance =
            float.MaxValue;

        foreach (GridCell cell in room.Cells)
        {
            if (cell == null)
                continue;

            float distance =
                Vector3.Distance(
                    cell.Position,
                    position
                );

            if (distance < bestDistance)
            {
                bestDistance = distance;
                result = cell;
            }
        }

        return result != null;
    }

    private List<RoomMap> FindRoomRoute(
        RoomMap start,
        RoomMap target
    )
    {
        List<RoomMap> result =
            new List<RoomMap>();

        if (start == null ||
            target == null)
        {
            return result;
        }

        Queue<RoomMap> queue =
            new Queue<RoomMap>();

        Dictionary<RoomMap, RoomMap> previous =
            new Dictionary<RoomMap, RoomMap>();

        queue.Enqueue(start);
        previous[start] = null;

        while (queue.Count > 0)
        {
            RoomMap current =
                queue.Dequeue();

            if (current == target)
                break;

            foreach (RoomMap next in current.Neighbours)
            {
                if (next == null)
                    continue;

                if (previous.ContainsKey(next))
                    continue;

                previous[next] = current;
                queue.Enqueue(next);
            }
        }

        if (!previous.ContainsKey(target))
            return result;

        RoomMap room =
            target;

        while (room != null)
        {
            result.Add(room);
            room = previous[room];
        }

        result.Reverse();

        return result;
    }

    private Vector3 FindExitPointBetweenRooms(
        RoomMap from,
        RoomMap to
    )
    {
        Vector3 best =
            from.Module.transform.position;

        float bestDistance =
            float.MaxValue;

        foreach (Vector3 exit in from.Exits)
        {
            float distance =
                DistanceToBounds(
                    to.Module.GetWorldBounds(),
                    exit
                );

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = exit;
            }
        }

        Vector3 direction =
            to.Module.transform.position -
            from.Module.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
            direction.Normalize();

        best += direction * exitOffset;

        return best;
    }

    private Vector3 FindEntryPointBetweenRooms(
        RoomMap from,
        RoomMap to
    )
    {
        Vector3 best =
            to.Module.transform.position;

        float bestDistance =
            float.MaxValue;

        foreach (Vector3 exit in to.Exits)
        {
            float distance =
                DistanceToBounds(
                    from.Module.GetWorldBounds(),
                    exit
                );

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = exit;
            }
        }

        Vector3 direction =
            from.Module.transform.position -
            to.Module.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
            direction.Normalize();

        best += direction * exitOffset;

        return best;
    }

    private void AppendPath(
        List<Vector3> destination,
        List<Vector3> source
    )
    {
        for (int i = 0; i < source.Count; i++)
        {
            if (destination.Count == 0 ||
                Vector3.Distance(
                    destination[destination.Count - 1],
                    source[i]
                ) > 0.25f)
            {
                destination.Add(source[i]);
            }
        }
    }

    private void SimplifyPath(
        List<Vector3> path
    )
    {
        if (path.Count < 3)
            return;

        int index = 0;

        while (index < path.Count - 2)
        {
            Vector3 a =
                path[index];

            Vector3 b =
                path[index + 1];

            Vector3 c =
                path[index + 2];

            Vector3 ab =
                b - a;

            Vector3 bc =
                c - b;

            ab.y = 0f;
            bc.y = 0f;

            if (ab.sqrMagnitude < 0.01f ||
                bc.sqrMagnitude < 0.01f)
            {
                path.RemoveAt(index + 1);
                continue;
            }

            float angle =
                Vector3.Angle(
                    ab,
                    bc
                );

            if (angle < 10f &&
                HasClearPath(
                    a,
                    c
                ))
            {
                path.RemoveAt(index + 1);
                continue;
            }

            index++;
        }
    }

    private bool HasClearPath(
        Vector3 a,
        Vector3 b
    )
    {
        Vector3 start =
            a +
            Vector3.up *
            obstacleCheckHeight;

        Vector3 end =
            b +
            Vector3.up *
            obstacleCheckHeight;

        Vector3 direction =
            end - start;

        float distance =
            direction.magnitude;

        if (distance <= 0.05f)
            return true;

        direction.Normalize();

        RaycastHit[] hits =
            Physics.SphereCastAll(
                start,
                obstacleCheckRadius,
                direction,
                distance,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore
            );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null)
                continue;

            EnemyController enemy =
                hit.collider.GetComponentInParent<EnemyController>();

            if (enemy != null)
                continue;

            return false;
        }

        return true;
    }

    public Vector3 GetTacticalPosition(
        EnemyTacticalPlanner.TacticalRole role,
        Vector3 threat,
        Vector3 enemyPosition,
        EnemyTacticalPlanner planner
    )
    {
        RoomMap room =
            FindRoom(threat);

        if (room == null)
            return threat;

        Vector3 preferred;

        Vector3 toThreat =
            threat -
            room.Module.transform.position;

        toThreat.y = 0f;

        if (toThreat.sqrMagnitude < 0.01f)
            toThreat = Vector3.forward;

        toThreat.Normalize();

        Vector3 right =
            Vector3.Cross(
                Vector3.up,
                toThreat
            ).normalized;

        switch (role)
        {
            case EnemyTacticalPlanner.TacticalRole.LeftFlank:
                preferred =
                    threat -
                    toThreat * flankDistance +
                    right * flankDistance;
                break;

            case EnemyTacticalPlanner.TacticalRole.RightFlank:
                preferred =
                    threat -
                    toThreat * flankDistance -
                    right * flankDistance;
                break;

            case EnemyTacticalPlanner.TacticalRole.Support:
                preferred =
                    threat -
                    toThreat * supportDistance;
                break;

            case EnemyTacticalPlanner.TacticalRole.Rear:
                preferred =
                    threat -
                    toThreat * supportDistance -
                    right * 2f;
                break;

            case EnemyTacticalPlanner.TacticalRole.Pressure:
            default:
                preferred =
                    threat -
                    toThreat * pressureDistance;
                break;
        }

        preferred.y =
            enemyPosition.y;

        GridCell cell;

        if (TryGetNearestWalkableCell(
            room,
            preferred,
            out cell
        ))
        {
            return cell.Position;
        }

        return GetNearestWalkablePoint(
            room,
            preferred
        );
    }

    private Vector3 GetNearestWalkablePoint(
        RoomMap room,
        Vector3 position
    )
    {
        GridCell best = null;
        float bestDistance = float.MaxValue;

        foreach (GridCell cell in room.Cells)
        {
            if (cell == null ||
                !cell.Walkable)
            {
                continue;
            }

            float distance =
                Vector3.Distance(
                    cell.Position,
                    position
                );

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = cell;
            }
        }

        if (best != null)
            return best.Position;

        return room.Module.transform.position;
    }

    public bool IsPositionWalkable(
        Vector3 position
    )
    {
        RoomMap room =
            FindRoom(position);

        if (room == null)
            return false;

        GridCell cell;

        if (!TryGetNearestCell(
            room,
            position,
            out cell
        ))
        {
            return false;
        }

        return cell.Walkable;
    }

    public bool IsPathAvailable(
        Vector3 start,
        Vector3 target
    )
    {
        List<Vector3> path =
            new List<Vector3>();

        return TryFindPath(
            start,
            target,
            path
        );
    }

    public List<CoverPoint> GetValidCovers(
        Vector3 threatPosition,
        Vector3 enemyPosition
    )
    {
        List<CoverPoint> result =
            new List<CoverPoint>();

        RoomMap room =
            FindRoom(enemyPosition);

        if (room == null)
            return result;

        foreach (CoverPoint cover in room.ValidCovers)
        {
            if (cover == null)
                continue;

            if (!IsPositionWalkable(
                cover.transform.position
            ))
            {
                continue;
            }

            if (!IsPathAvailable(
                enemyPosition,
                cover.transform.position
            ))
            {
                continue;
            }

            result.Add(cover);
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        if (!drawGrid ||
            !Application.isPlaying)
        {
            return;
        }

        foreach (RoomMap room in rooms)
        {
            foreach (GridCell cell in room.Cells)
            {
                if (cell == null)
                    continue;

                Gizmos.color =
                    cell.Walkable
                        ? Color.white
                        : Color.red;

                Gizmos.DrawWireCube(
                    cell.Position,
                    new Vector3(
                        cellSize * 0.85f,
                        0.05f,
                        cellSize * 0.85f
                    )
                );
            }
        }
    }

    public class RoomMap
    {
        public ArenaModule Module { get; private set; }

        public List<GridCell> Cells { get; private set; }

        public List<Vector3> Exits { get; private set; }

        public List<CoverPoint> ValidCovers { get; private set; }

        public List<RoomMap> Neighbours { get; private set; }

        private readonly int width;
        private readonly int height;
        private readonly float size;

        private readonly GridCell[,] grid;

        public RoomMap(
            ArenaModule module,
            int width,
            int height,
            float size
        )
        {
            Module = module;
            this.width = width;
            this.height = height;
            this.size = size;

            grid =
                new GridCell[
                    width,
                    height
                ];

            Cells =
                new List<GridCell>();

            Exits =
                new List<Vector3>();

            ValidCovers =
                new List<CoverPoint>();

            Neighbours =
                new List<RoomMap>();
        }

        public void SetCell(
            int x,
            int z,
            Vector3 position,
            bool walkable
        )
        {
            GridCell cell =
                new GridCell(
                    x,
                    z,
                    position,
                    walkable
                );

            grid[x, z] =
                cell;

            Cells.Add(cell);
        }

        public List<GridCell> GetNeighbours(
            GridCell cell
        )
        {
            List<GridCell> result =
                new List<GridCell>();

            AddNeighbour(
                result,
                cell.X + 1,
                cell.Z
            );

            AddNeighbour(
                result,
                cell.X - 1,
                cell.Z
            );

            AddNeighbour(
                result,
                cell.X,
                cell.Z + 1
            );

            AddNeighbour(
                result,
                cell.X,
                cell.Z - 1
            );

            return result;
        }

        private void AddNeighbour(
            List<GridCell> result,
            int x,
            int z
        )
        {
            if (x < 0 ||
                z < 0 ||
                x >= width ||
                z >= height)
            {
                return;
            }

            GridCell cell =
                grid[x, z];

            if (cell != null)
                result.Add(cell);
        }

        public int GetApproximateX(
            Vector3 position
        )
        {
            GridCell best = null;
            float bestDistance = float.MaxValue;

            foreach (GridCell cell in Cells)
            {
                float distance =
                    Vector3.Distance(
                        cell.Position,
                        position
                    );

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = cell;
                }
            }

            return best != null
                ? best.X
                : 0;
        }

        public int GetApproximateZ(
            Vector3 position
        )
        {
            GridCell best = null;
            float bestDistance = float.MaxValue;

            foreach (GridCell cell in Cells)
            {
                float distance =
                    Vector3.Distance(
                        cell.Position,
                        position
                    );

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = cell;
                }
            }

            return best != null
                ? best.Z
                : 0;
        }
    }

    public class GridCell
    {
        public int X { get; private set; }

        public int Z { get; private set; }

        public Vector3 Position { get; private set; }

        public bool Walkable { get; private set; }

        public GridCell(
            int x,
            int z,
            Vector3 position,
            bool walkable
        )
        {
            X = x;
            Z = z;
            Position = position;
            Walkable = walkable;
        }
    }
}