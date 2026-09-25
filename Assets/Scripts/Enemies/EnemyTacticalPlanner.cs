using System.Collections.Generic;
using UnityEngine;

public class EnemyTacticalPlanner : MonoBehaviour
{
public enum TacticalRole
{
None,
LeftFlank,
RightFlank,
Support,
Rear,
Pressure
}


public struct TacticalPlan
{
    public TacticalRole role;
    public Vector3 targetPosition;
    public Vector3 approachDirection;
    public float score;
    public bool valid;
}

[Header("References")]
[SerializeField] private EnemyController enemyController;
[SerializeField] private EnemyTacticalEnvironmentScanner environmentScanner;

[Header("Group")]
[SerializeField] private string groupId = "";
[SerializeField] private int groupWave;
[SerializeField] private TacticalRole assignedRole = TacticalRole.None;
[SerializeField] private bool isGroupLeader;

[Header("Group Settings")]
[SerializeField] private float groupRadius = 14f;
[SerializeField] private float minimumMemberDistance = 2.2f;
[SerializeField] private float preferredMemberDistance = 3.5f;
[SerializeField] private float groupCenterWeight = 0.75f;

[Header("Tactical Distances")]
[SerializeField] private float flankDistance = 5f;
[SerializeField] private float supportDistance = 6f;
[SerializeField] private float rearDistance = 4f;
[SerializeField] private float pressureDistance = 3.5f;

[Header("Position Validation")]
[SerializeField] private float positionCheckRadius = 0.65f;
[SerializeField] private float positionSearchRadius = 2.5f;
[SerializeField] private int positionSearchSteps = 12;
[SerializeField] private LayerMask obstacleLayers = ~0;

[Header("Planner")]
[SerializeField] private float minimumPositionChange = 1.25f;
[SerializeField] private float roleRefreshInterval = 1.25f;
[SerializeField] private float threatUpdateThreshold = 0.5f;

[Header("Debug")]
[SerializeField] private bool debugLogs;
[SerializeField] private bool debugGizmos;

private readonly List<EnemyTacticalPlanner> groupMembers =
    new List<EnemyTacticalPlanner>();

private TacticalPlan currentPlan;

private Vector3 threatPosition;
private Vector3 previousThreatPosition;

private bool hasThreatPosition;
private bool groupConfigured;

private float nextRefreshTime;

private bool searchMode;
private Vector3 searchCenter;
private Vector3 searchTarget;
private bool hasSearchTarget;
private float nextRedistributeTime;
private float redistributeInterval = 2.0f;
private bool hasDistributedTargets;
private Vector3 lastDistributedCenter;
private readonly List<ArenaTacticalMap.RoomMap> visitedRooms =
    new List<ArenaTacticalMap.RoomMap>();

public string GroupId => groupId;
public int GroupWave => groupWave;
public TacticalRole AssignedRole => assignedRole;
public bool IsGroupLeader => isGroupLeader;
public bool HasGroup => groupConfigured;
public IReadOnlyList<EnemyTacticalPlanner> GroupMembers => groupMembers;

public Vector3 ThreatPosition => threatPosition;
public bool HasThreatPosition => hasThreatPosition;

public TacticalPlan CurrentPlan => currentPlan;

public bool SearchMode => searchMode;
public Vector3 SearchCenter => searchCenter;
public bool HasSearchTarget => hasSearchTarget;

private void Awake()
{
    if (enemyController == null)
        enemyController = GetComponent<EnemyController>();

    if (environmentScanner == null)
        environmentScanner =
            GetComponent<EnemyTacticalEnvironmentScanner>();

    ResetPlan();
}

private void Start()
{
    nextRefreshTime =
        Time.time +
        Random.Range(
            0f,
            Mathf.Max(0.05f, roleRefreshInterval)
        );
}

private void Update()
{
    if (!groupConfigured)
        return;

    if (Time.time < nextRefreshTime)
        return;

    nextRefreshTime =
        Time.time +
        Mathf.Max(0.05f, roleRefreshInterval);

    RefreshPlanner(false);

    if (isGroupLeader && searchMode &&
        Time.time >= nextRedistributeTime)
    {
        RemoveDeadMembers();

        if (groupMembers.Count > 0)
        {
            DistributeSearchTargets();
        }

        nextRedistributeTime =
            Time.time + redistributeInterval;
    }

    EnsureLeaderExists();
}

public void ConfigureGroup(
    string newGroupId,
    int newWave,
    List<EnemyTacticalPlanner> members,
    TacticalRole role,
    bool leader)
{
    groupId = newGroupId;
    groupWave = newWave;
    assignedRole = role;
    isGroupLeader = leader;

    groupMembers.Clear();

    AddGroupMembers(members);

    groupConfigured = true;

    ResetPlan();

    nextRefreshTime =
        Time.time +
        Random.Range(
            0f,
            Mathf.Max(0.05f, roleRefreshInterval)
        );

    if (debugLogs)
    {
        Debug.Log(
            $"[TACTICAL PLANNER] CONFIGURED | " +
            $"GROUP={groupId} | " +
            $"ROLE={assignedRole} | " +
            $"LEADER={isGroupLeader} | " +
            $"MEMBERS={groupMembers.Count}",
            this
        );
    }
}

public void SetGroupMembers(
    List<EnemyTacticalPlanner> members)
{
    groupMembers.Clear();

    AddGroupMembers(members);

    if (groupConfigured)
        BuildPlan();
}

private void AddGroupMembers(
    List<EnemyTacticalPlanner> members)
{
    if (members == null)
        return;

    for (int i = 0; i < members.Count; i++)
    {
        EnemyTacticalPlanner member =
            members[i];

        if (member == null)
            continue;

        if (!groupMembers.Contains(member))
            groupMembers.Add(member);
    }
}

public void SetThreatPosition(
    Vector3 position)
{
    position.y = transform.position.y;

    if (hasThreatPosition)
    {
        float movement =
            Vector3.Distance(
                previousThreatPosition,
                position
            );

        if (movement < threatUpdateThreshold)
            return;
    }

    previousThreatPosition =
        threatPosition;

    threatPosition =
        position;

    hasThreatPosition = true;

    BuildPlan();
}

public void SetThreatPositionImmediate(
    Vector3 position)
{
    position.y = transform.position.y;

    previousThreatPosition =
        threatPosition;

    threatPosition =
        position;

    hasThreatPosition = true;

    BuildPlan();
}

public bool TryGetPlan(
    out TacticalPlan plan)
{
    plan = currentPlan;

    return
        groupConfigured &&
        currentPlan.valid;
}

public bool TryGetTarget(
    out Vector3 target)
{
    target =
        currentPlan.targetPosition;

    return
        groupConfigured &&
        currentPlan.valid;
}

public bool TryGetApproachDirection(
    out Vector3 direction)
{
    direction =
        currentPlan.approachDirection;

    return
        groupConfigured &&
        currentPlan.valid;
}

public void ClearPlan()
{
    ResetPlan();
    hasThreatPosition = false;
}

public void SetSearchTarget(Vector3 center)
{
    center.y = transform.position.y;
    searchCenter = center;
    searchMode = true;
    hasSearchTarget = false;
    hasDistributedTargets = false;
    nextRedistributeTime =
        Time.time + redistributeInterval;

    if (isGroupLeader)
    {
        DistributeSearchTargets();
    }
}

public void ClearSearchMode()
{
    searchMode = false;
    hasSearchTarget = false;
}

public void RequestNextSearchTargets()
{
    if (!searchMode)
        return;

    searchCenter = transform.position;
    hasDistributedTargets = false;

    Debug.Log(
        $"[SEARCH REQUEST] {gameObject.name} " +
        $"newSearchCenter=({searchCenter.x:F1},{searchCenter.y:F1},{searchCenter.z:F1})"
    );

    ArenaTacticalMap.RoomMap finishedRoom = null;

    if (ArenaTacticalMap.Instance != null)
    {
        finishedRoom =
            ArenaTacticalMap.Instance.FindRoom(searchCenter);
    }

    if (finishedRoom != null &&
        !visitedRooms.Contains(finishedRoom))
    {
        visitedRooms.Add(finishedRoom);
    }

    AssignIndividualSearchTarget(finishedRoom);
}

public bool TryGetSearchTarget(out Vector3 target)
{
    target = searchTarget;

    return searchMode && hasSearchTarget;
}

public void OnMemberDied()
{
    if (!groupConfigured)
        return;

    RemoveDeadMembers();

    if (groupMembers.Count == 0)
        return;

    EnsureLeaderExists();
}

private void ResetPlan()
{
    currentPlan =
        new TacticalPlan
        {
            role = assignedRole,
            targetPosition = transform.position,
            approachDirection = Vector3.zero,
            score = 0f,
            valid = false
        };
}

private void RefreshPlanner(
    bool force)
{
    if (!groupConfigured)
        return;

    if (!hasThreatPosition)
    {
        if (!TryGetEnemyPlayerPosition(
                out Vector3 playerPosition))
        {
            return;
        }

        threatPosition =
            playerPosition;

        previousThreatPosition =
            playerPosition;

        hasThreatPosition = true;
    }

    if (force)
    {
        BuildPlan();
        return;
    }

    BuildPlan();
}

private void BuildPlan()
{
    if (!groupConfigured)
        return;

    if (!hasThreatPosition)
        return;

    Vector3 groupCenter =
        CalculateGroupCenter();

    Vector3 forward =
        threatPosition -
        groupCenter;

    forward.y = 0f;

    if (forward.sqrMagnitude < 0.01f)
    {
        forward =
            transform.forward;

        forward.y = 0f;
    }

    if (forward.sqrMagnitude < 0.01f)
        forward = Vector3.forward;

    forward.Normalize();

    Vector3 right =
        Vector3.Cross(
            Vector3.up,
            forward
        ).normalized;

    Vector3 desiredPosition =
        CalculateDesiredPosition(
            groupCenter,
            forward,
            right
        );

    desiredPosition.y =
        transform.position.y;

    if (!TryFindBestValidPosition(
            desiredPosition,
            out Vector3 validPosition))
    {
        currentPlan =
            new TacticalPlan
            {
                role = assignedRole,
                targetPosition =
                    transform.position,
                approachDirection =
                    Vector3.zero,
                score = -1000f,
                valid = false
            };

        if (debugLogs)
        {
            Debug.LogWarning(
                $"[TACTICAL PLANNER] " +
                $"{gameObject.name} | " +
                $"NO VALID TARGET | " +
                $"GROUP={groupId} | " +
                $"ROLE={assignedRole}",
                this
            );
        }

        return;
    }

    if (
        currentPlan.valid &&
        Vector3.Distance(
            currentPlan.targetPosition,
            validPosition
        ) < minimumPositionChange)
    {
        validPosition =
            currentPlan.targetPosition;
    }

    Vector3 approachDirection =
        validPosition -
        transform.position;

    approachDirection.y = 0f;

    if (approachDirection.sqrMagnitude >
        0.01f)
    {
        approachDirection.Normalize();
    }
    else
    {
        approachDirection =
            Vector3.zero;
    }

    float score =
        CalculatePositionScore(
            validPosition
        );

    currentPlan =
        new TacticalPlan
        {
            role = assignedRole,
            targetPosition = validPosition,
            approachDirection =
                approachDirection,
            score = score,
            valid = true
        };

    if (debugLogs)
    {
        Debug.Log(
            $"[TACTICAL PLANNER] PLAN | " +
            $"GROUP={groupId} | " +
            $"ROLE={assignedRole} | " +
            $"TARGET={validPosition} | " +
            $"SCORE={score:F2}",
            this
        );
    }
}

private Vector3 CalculateDesiredPosition(
    Vector3 groupCenter,
    Vector3 forward,
    Vector3 right)
{
    switch (assignedRole)
    {
        case TacticalRole.LeftFlank:
            return
                threatPosition -
                forward * flankDistance -
                right * flankDistance;

        case TacticalRole.RightFlank:
            return
                threatPosition -
                forward * flankDistance +
                right * flankDistance;

        case TacticalRole.Support:
            return
                threatPosition -
                forward * supportDistance +
                right * 1.5f;

        case TacticalRole.Rear:
            return
                groupCenter -
                forward * rearDistance -
                right * 1.5f;

        case TacticalRole.Pressure:
            return
                threatPosition -
                forward * pressureDistance;

        default:
            return
                groupCenter -
                forward * supportDistance;
    }
}

private bool TryFindBestValidPosition(
    Vector3 desiredPosition,
    out Vector3 validPosition)
{
    desiredPosition.y =
        transform.position.y;

    if (IsPositionValid(desiredPosition))
    {
        validPosition =
            desiredPosition;

        return true;
    }

    Vector3 bestCandidate =
        Vector3.zero;

    float bestScore =
        float.MaxValue;

    bool found =
        false;

    float angleStep =
        360f /
        Mathf.Max(
            1,
            positionSearchSteps
        );

    for (int ring = 1; ring <= 2; ring++)
    {
        float radius =
            positionSearchRadius *
            (ring * 0.5f);

        for (
            int i = 0;
            i < positionSearchSteps;
            i++)
        {
            float angle =
                angleStep * i;

            Vector3 offset =
                new Vector3(
                    Mathf.Cos(
                        angle *
                        Mathf.Deg2Rad
                    ),
                    0f,
                    Mathf.Sin(
                        angle *
                        Mathf.Deg2Rad
                    )
                ) *
                radius;

            Vector3 candidate =
                desiredPosition +
                offset;

            candidate.y =
                transform.position.y;

            if (!IsPositionValid(candidate))
                continue;

            float score =
                CalculatePositionScore(
                    candidate
                );

            if (!found || score > bestScore)
            {
                bestScore = score;
                bestCandidate = candidate;
                found = true;
            }
        }
    }

    if (found)
    {
        validPosition =
            bestCandidate;

        return true;
    }

    validPosition =
        transform.position;

    return false;
}

private bool IsPositionValid(
    Vector3 position)
{
    Collider[] colliders =
        Physics.OverlapSphere(
            position,
            positionCheckRadius,
            obstacleLayers,
            QueryTriggerInteraction.Ignore
        );

    for (
        int i = 0;
        i < colliders.Length;
        i++)
    {
        Collider collider =
            colliders[i];

        if (collider == null)
            continue;

        Transform colliderTransform =
            collider.transform;

        if (
            colliderTransform == transform ||
            colliderTransform.IsChildOf(transform)
        )
        {
            continue;
        }

        EnemyTacticalPlanner otherPlanner =
            collider.GetComponentInParent<
                EnemyTacticalPlanner>();

        if (otherPlanner != null)
            continue;

        return false;
    }

    if (environmentScanner != null)
    {
        Vector3 direction =
            position -
            transform.position;

        direction.y = 0f;

        float distance =
            direction.magnitude;

        if (distance > 0.5f)
        {
            if (
                !environmentScanner
                    .IsDirectionClear(
                        direction,
                        distance
                    )
            )
            {
                return false;
            }
        }
    }

    return true;
}

private float CalculatePositionScore(
    Vector3 position)
{
    float score = 0f;

    float distanceToThreat =
        Vector3.Distance(
            position,
            threatPosition
        );

    score -=
        distanceToThreat * 0.1f;

    Vector3 toPosition =
        position -
        transform.position;

    toPosition.y = 0f;

    if (
        environmentScanner != null &&
        toPosition.sqrMagnitude > 0.25f
    )
    {
        float distance =
            toPosition.magnitude;

        if (
            environmentScanner
                .IsDirectionClear(
                    toPosition,
                    distance
                )
        )
        {
            score += 2f;
        }
        else
        {
            score -= 2f;
        }
    }

    for (
        int i = 0;
        i < groupMembers.Count;
        i++
    )
    {
        EnemyTacticalPlanner member =
            groupMembers[i];

        if (
            member == null ||
            member == this
        )
        {
            continue;
        }

        float distance =
            Vector3.Distance(
                position,
                member.transform.position
            );

        if (
            distance <
            minimumMemberDistance
        )
        {
            score -=
                15f;
        }
        else if (
            distance <
            preferredMemberDistance
        )
        {
            score -=
                2f;
        }
        else if (
            distance <= groupRadius
        )
        {
            score +=
                groupCenterWeight;
        }
    }

    return score;
}

private Vector3 CalculateGroupCenter()
{
    if (groupMembers.Count == 0)
        return transform.position;

    Vector3 center =
        Vector3.zero;

    int count = 0;

    for (
        int i = 0;
        i < groupMembers.Count;
        i++
    )
    {
        EnemyTacticalPlanner member =
            groupMembers[i];

        if (member == null)
            continue;

        center +=
            member.transform.position;

        count++;
    }

    if (count == 0)
        return transform.position;

    center /= count;

    center.y =
        transform.position.y;

    return center;
}

private bool TryGetEnemyPlayerPosition(
    out Vector3 position)
{
    position =
        Vector3.zero;

    if (enemyController == null)
        return false;

    System.Reflection.FieldInfo field =
        typeof(EnemyController).GetField(
            "lastKnownPlayerPosition",
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Public
        );

    if (field == null)
        return false;

    object value =
        field.GetValue(
            enemyController
        );

    if (value is Vector3 vector)
    {
        position =
            vector;

        return true;
    }

    return false;
}

public void DistributeSearchTargets()
{
    if (!isGroupLeader)
        return;

    Debug.Log(
        $"[TACTICAL DEBUG] Instance={(ArenaTacticalMap.Instance != null)} " +
        $"IsBuilt={(ArenaTacticalMap.Instance != null && ArenaTacticalMap.Instance.IsBuilt)} " +
        $"Rooms={(ArenaTacticalMap.Instance != null ? ArenaTacticalMap.Instance.Rooms.Count : -1)} " +
        $"ArenaModules={FindObjectsByType<ArenaModule>(FindObjectsSortMode.None).Length}"
    );

    if (ArenaTacticalMap.Instance == null)
    {
        DistributeFallbackSearchTargets();
        return;
    }

    if (!ArenaTacticalMap.Instance.IsBuilt)
    {
        ArenaTacticalMap.Instance.Build();
    }

    Debug.Log(
        $"[TACTICAL DEBUG AFTER BUILD] IsBuilt={ArenaTacticalMap.Instance.IsBuilt} " +
        $"Rooms={ArenaTacticalMap.Instance.Rooms.Count} " +
        $"ArenaModules={FindObjectsByType<ArenaModule>(FindObjectsSortMode.None).Length}"
    );

    if (hasDistributedTargets &&
        Vector3.Distance(
            searchCenter, lastDistributedCenter
        ) < 0.5f)
    {
        return;
    }

    ArenaTacticalMap.RoomMap searchRoom =
        ArenaTacticalMap.Instance.FindRoom(searchCenter);

    if (searchRoom == null)
    {
        DistributeFallbackSearchTargets();
        return;
    }

    List<ArenaTacticalMap.RoomMap> availableRooms =
        new List<ArenaTacticalMap.RoomMap>();

    availableRooms.Add(searchRoom);

    for (int i = 0; i < searchRoom.Neighbours.Count; i++)
    {
        ArenaTacticalMap.RoomMap neighbour = searchRoom.Neighbours[i];

        if (neighbour != null &&
            !availableRooms.Contains(neighbour))
        {
            availableRooms.Add(neighbour);
        }
    }

    if (availableRooms.Count <= 1)
    {
        for (int i = 0; i < searchRoom.Neighbours.Count; i++)
        {
            ArenaTacticalMap.RoomMap n = searchRoom.Neighbours[i];
            if (n == null) continue;

            for (int j = 0; j < n.Neighbours.Count; j++)
            {
                ArenaTacticalMap.RoomMap nn = n.Neighbours[j];
                if (nn != null && !availableRooms.Contains(nn))
                    availableRooms.Add(nn);
            }
        }
    }

    List<Vector3> assignedTargets =
        new List<Vector3>();

    for (int i = 0; i < groupMembers.Count; i++)
    {
        EnemyTacticalPlanner member = groupMembers[i];

        if (member == null)
            continue;

        ArenaTacticalMap.RoomMap memberRoom = null;

        if (ArenaTacticalMap.Instance != null)
        {
            memberRoom =
                ArenaTacticalMap.Instance.FindRoom(
                    member.transform.position
                );
        }

        ArenaTacticalMap.RoomMap targetRoom =
            SelectRoomForMember(
                member, availableRooms, memberRoom);

        Vector3 target =
            FindWalkablePositionInRoom(
                targetRoom, assignedTargets);

        member.SetDirectSearchTarget(target);
        assignedTargets.Add(target);
    }

    hasDistributedTargets = true;
    lastDistributedCenter = searchCenter;
}

private void AssignIndividualSearchTarget(
    ArenaTacticalMap.RoomMap finishedRoom)
{
    if (ArenaTacticalMap.Instance == null)
    {
        AssignIndividualFallbackTarget();
        return;
    }

    ArenaTacticalMap.RoomMap currentRoom =
        ArenaTacticalMap.Instance.FindRoom(searchCenter);

    if (currentRoom == null)
    {
        AssignIndividualFallbackTarget();
        return;
    }

    List<ArenaTacticalMap.RoomMap> occupiedRooms =
        CollectOccupiedTargetRooms();

    ArenaTacticalMap.RoomMap targetRoom = null;

    List<ArenaTacticalMap.RoomMap> levelRooms =
        new List<ArenaTacticalMap.RoomMap>();

    for (int i = 0; i < currentRoom.Neighbours.Count; i++)
    {
        ArenaTacticalMap.RoomMap neighbour = currentRoom.Neighbours[i];

        if (neighbour != null &&
            !visitedRooms.Contains(neighbour) &&
            !occupiedRooms.Contains(neighbour))
        {
            levelRooms.Add(neighbour);
        }
    }

    if (levelRooms.Count > 0)
    {
        targetRoom = levelRooms[0];
    }
    else
    {
        levelRooms.Clear();

        foreach (ArenaTacticalMap.RoomMap room in ArenaTacticalMap.Instance.Rooms)
        {
            if (room != null &&
                room != currentRoom &&
                !visitedRooms.Contains(room) &&
                !occupiedRooms.Contains(room))
            {
                levelRooms.Add(room);
            }
        }

        if (levelRooms.Count > 0)
        {
            targetRoom = levelRooms[0];
        }
        else
        {
            levelRooms.Clear();

            foreach (ArenaTacticalMap.RoomMap room in ArenaTacticalMap.Instance.Rooms)
            {
                if (room != null &&
                    room != finishedRoom &&
                    !occupiedRooms.Contains(room))
                {
                    levelRooms.Add(room);
                }
            }

            if (levelRooms.Count > 0)
            {
                targetRoom = levelRooms[0];
            }
            else
            {
                foreach (ArenaTacticalMap.RoomMap room in ArenaTacticalMap.Instance.Rooms)
                {
                    if (room != null)
                    {
                        targetRoom = room;
                        break;
                    }
                }

                if (targetRoom == null)
                {
                    AssignIndividualFallbackTarget();
                    return;
                }
            }
        }
    }

    List<Vector3> excludeReached =
        new List<Vector3>();
    excludeReached.Add(searchTarget);

    Vector3 target =
        FindWalkablePositionInRoom(
            targetRoom,
            excludeReached);

    SetDirectSearchTarget(target);

    hasDistributedTargets = true;
    lastDistributedCenter = searchCenter;
}

private List<ArenaTacticalMap.RoomMap> CollectOccupiedTargetRooms()
{
    List<ArenaTacticalMap.RoomMap> occupiedRooms =
        new List<ArenaTacticalMap.RoomMap>();

    if (ArenaTacticalMap.Instance == null)
    {
        return occupiedRooms;
    }

    for (int i = 0; i < groupMembers.Count; i++)
    {
        EnemyTacticalPlanner member = groupMembers[i];

        if (member == null ||
            member == this)
        {
            continue;
        }

        if (!member.searchMode ||
            !member.hasSearchTarget)
        {
            continue;
        }

        ArenaTacticalMap.RoomMap memberRoom =
            ArenaTacticalMap.Instance.FindRoom(
                member.searchTarget
            );

        if (memberRoom != null &&
            !occupiedRooms.Contains(memberRoom))
        {
            occupiedRooms.Add(memberRoom);
        }
    }

    return occupiedRooms;
}

private void AssignIndividualFallbackTarget()
{
    int myIndex = 0;

    for (int i = 0; i < groupMembers.Count; i++)
    {
        if (groupMembers[i] == this)
        {
            myIndex = i;
            break;
        }
    }

    float angle =
        (360f / Mathf.Max(1, groupMembers.Count)) * myIndex;
    float radius = 6f;
    Vector3 offset =
        Quaternion.Euler(0, angle, 0) *
        Vector3.forward * radius;
    Vector3 target = searchCenter + offset;
    target.y = transform.position.y;

    SetDirectSearchTarget(target);

    hasDistributedTargets = true;
    lastDistributedCenter = searchCenter;
}

private ArenaTacticalMap.RoomMap SelectRoomForMember(
    EnemyTacticalPlanner member,
    List<ArenaTacticalMap.RoomMap> availableRooms,
    ArenaTacticalMap.RoomMap memberCurrentRoom)
{
    if (availableRooms.Count == 0)
        return null;

    if (availableRooms.Count == 1)
        return availableRooms[0];

    List<ArenaTacticalMap.RoomMap> otherRooms =
        new List<ArenaTacticalMap.RoomMap>();

    for (int i = 0; i < availableRooms.Count; i++)
    {
        if (availableRooms[i] != memberCurrentRoom)
            otherRooms.Add(availableRooms[i]);
    }

    if (otherRooms.Count == 0)
    {
        int fallbackIndex =
            Mathf.FloorToInt(
                Random.Range(0f, availableRooms.Count)
            );
        return availableRooms[fallbackIndex];
    }

    int memberIndex = 0;

    for (int i = 0; i < groupMembers.Count; i++)
    {
        if (groupMembers[i] == member)
        {
            memberIndex = i;
            break;
        }
    }

    int roomIndex =
        memberIndex % otherRooms.Count;

    return otherRooms[roomIndex];
}

private Vector3 FindWalkablePositionInRoom(
    ArenaTacticalMap.RoomMap room,
    List<Vector3> existingTargets)
{
    if (room == null)
        return searchCenter;

    Vector3 idealCenter = room.Module.transform.position;
    Vector3 bestPosition = idealCenter;
    float bestScore = float.MinValue;

    foreach (ArenaTacticalMap.GridCell cell in room.Cells)
    {
        if (cell == null || !cell.Walkable)
            continue;

        float score = 0f;

        float distToCenter =
            Vector3.Distance(cell.Position, idealCenter);
        score -= distToCenter * 0.1f;

        bool tooClose = false;
        for (int i = 0; i < existingTargets.Count; i++)
        {
            float dist =
                Vector3.Distance(
                    cell.Position, existingTargets[i]);

            if (dist < minimumMemberDistance)
            {
                tooClose = true;
                break;
            }

            if (dist < preferredMemberDistance)
                score -= 2f;
            else
                score += 1f;
        }

        if (tooClose)
            continue;

        score += 10f;

        if (score > bestScore)
        {
            bestScore = score;
            bestPosition = cell.Position;
        }
    }

    return bestPosition;
}

private void DistributeFallbackSearchTargets()
{
    for (int i = 0; i < groupMembers.Count; i++)
    {
        EnemyTacticalPlanner member = groupMembers[i];

        if (member == null)
            continue;

        float angle =
            (360f / Mathf.Max(1, groupMembers.Count)) * i;
        float radius = 6f;
        Vector3 offset =
            Quaternion.Euler(0, angle, 0) *
            Vector3.forward * radius;
        Vector3 target = searchCenter + offset;
        target.y = transform.position.y;

        member.SetDirectSearchTarget(target);
    }

    hasDistributedTargets = true;
    lastDistributedCenter = searchCenter;
}

private void SetDirectSearchTarget(Vector3 target)
{
    searchTarget = target;
    hasSearchTarget = true;

    ArenaTacticalMap.RoomMap currentRoom = null;
    ArenaTacticalMap.RoomMap targetRoom = null;

    if (ArenaTacticalMap.Instance != null)
    {
        currentRoom =
            ArenaTacticalMap.Instance.FindRoom(
                transform.position);
        targetRoom =
            ArenaTacticalMap.Instance.FindRoom(target);
    }

    Debug.Log(
        $"[SEARCH TARGET] {gameObject.name} " +
        $"currentRoom={currentRoom?.Module?.gameObject?.name ?? "null"} " +
        $"targetRoom={targetRoom?.Module?.gameObject?.name ?? "null"} " +
        $"target=({target.x:F1},{target.y:F1},{target.z:F1})"
    );
}

private void RemoveDeadMembers()
{
    for (int i = groupMembers.Count - 1; i >= 0; i--)
    {
        if (groupMembers[i] == null)
            groupMembers.RemoveAt(i);
    }
}

private void EnsureLeaderExists()
{
    bool hasLeader = false;

    for (int i = 0; i < groupMembers.Count; i++)
    {
        if (groupMembers[i] != null &&
            groupMembers[i].isGroupLeader)
        {
            hasLeader = true;
            break;
        }
    }

    if (!hasLeader && groupMembers.Count > 0)
    {
        for (int i = 0; i < groupMembers.Count; i++)
        {
            if (groupMembers[i] != null)
            {
                groupMembers[i].isGroupLeader = true;
                isGroupLeader = false;
                break;
            }
        }
    }
}

private void OnDrawGizmosSelected()
{
    if (!debugGizmos)
        return;

    Gizmos.color =
        Color.white;

    Gizmos.DrawWireSphere(
        transform.position,
        groupRadius
    );

    if (currentPlan.valid)
    {
        Gizmos.color =
            Color.yellow;

        Gizmos.DrawSphere(
            currentPlan.targetPosition,
            0.3f
        );

        Gizmos.DrawLine(
            transform.position,
            currentPlan.targetPosition
        );

        if (hasThreatPosition)
        {
            Gizmos.color =
                Color.red;

            Gizmos.DrawLine(
                transform.position,
                threatPosition
            );
        }
    }
}


}
