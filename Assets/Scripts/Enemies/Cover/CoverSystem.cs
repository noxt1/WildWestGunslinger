using System.Collections.Generic;
using UnityEngine;

public class CoverSystem : MonoBehaviour
{
    public static CoverSystem Instance;

    [Header("Cover Search")]
    [SerializeField] private float searchRadius = 25f;
    [SerializeField] private float maxCoverDistanceFromPlayer = 20f;

    [Header("Enemy Spacing")]
    [SerializeField] private float minimumEnemySpacing = 2f;

    [Header("Automatic Cover")]
    [SerializeField] private bool automaticCover = false;
    [SerializeField] private float coverPadding = 1.2f;
    [SerializeField] private float minimumObjectHeight = 0.7f;

    [Header("Search Points")]
    [SerializeField] private float searchPointRadius = 12f;
    [SerializeField] private float minimumSearchPointDistance = 2f;

    private readonly List<CoverPoint> registeredCoverPoints = new List<CoverPoint>();
    private readonly List<CoverPoint> autoCoverPoints = new List<CoverPoint>();

    private CoverPoint[] manualCoverPoints = new CoverPoint[0];
    private CoverPoint[] allCoverPoints = new CoverPoint[0];

    private bool autoCoverCreated;

    private void Awake()
    {
        Instance = this;

        RefreshExistingCoverPoints();

        Debug.Log("CoverSystem: Awake. Existing cover points found: " + registeredCoverPoints.Count);
    }

    private void RefreshExistingCoverPoints()
    {
        CoverPoint[] existing = FindObjectsByType<CoverPoint>(FindObjectsSortMode.None);

        foreach (CoverPoint point in existing)
        {
            RegisterCoverPointInternal(point);
        }

        RebuildCoverList();
    }

    private void RegisterCoverPointInternal(CoverPoint point)
    {
        if (point == null) return;

        if (registeredCoverPoints.Contains(point)) return;
        if (autoCoverPoints.Contains(point)) return;

        registeredCoverPoints.Add(point);
    }

    public void RegisterCoverPoint(CoverPoint point)
    {
        if (point == null) return;

        RegisterCoverPointInternal(point);
        RebuildCoverList();
    }

    public void RegisterCoverPoints(IEnumerable<CoverPoint> points)
    {
        if (points == null) return;

        foreach (CoverPoint point in points)
        {
            RegisterCoverPointInternal(point);
        }

        RebuildCoverList();
    }

    public void FinalizeGeneratedCoverPoints()
    {
        CleanupNullPoints();
        RebuildCoverList();

        Debug.Log("CoverSystem: Generated cover registration finished. Total cover points: " + allCoverPoints.Length);
    }

    private void BuildAutomaticCoverPoints(Vector3 center)
    {
        if (!automaticCover || autoCoverCreated) return;

        autoCoverCreated = true;

        Collider[] colliders = Physics.OverlapSphere(center, searchRadius);

        int created = 0;

        foreach (Collider collider in colliders)
        {
            if (!IsValidCoverObject(collider)) continue;

            Bounds bounds = collider.bounds;

            CreateAutoCoverPoint(
                collider,
                new Vector3(bounds.max.x + coverPadding, bounds.center.y, bounds.center.z)
            );

            CreateAutoCoverPoint(
                collider,
                new Vector3(bounds.min.x - coverPadding, bounds.center.y, bounds.center.z)
            );

            CreateAutoCoverPoint(
                collider,
                new Vector3(bounds.center.x, bounds.center.y, bounds.max.z + coverPadding)
            );

            CreateAutoCoverPoint(
                collider,
                new Vector3(bounds.center.x, bounds.center.y, bounds.min.z - coverPadding)
            );

            created += 4;
        }

        RebuildCoverList();

        Debug.Log("CoverSystem: Automatic cover points created = " + created);
    }

    public void BuildAutomaticCoverPointsNow(Vector3 center)
    {
        BuildAutomaticCoverPoints(center);
    }

    private bool IsValidCoverObject(Collider collider)
    {
        if (collider == null) return false;
        if (collider.isTrigger) return false;

        GameObject obj = collider.gameObject;

        if (obj.CompareTag("Player")) return false;
        if (obj.CompareTag("Enemy")) return false;

        if (obj.GetComponentInParent<EnemyController>() != null) return false;
        if (obj.GetComponent<EnemySpawner>() != null) return false;
        if (obj.GetComponent<WaveManager>() != null) return false;

        if (collider.bounds.size.y < minimumObjectHeight) return false;

        return true;
    }

    private void CreateAutoCoverPoint(Collider sourceCollider, Vector3 position)
    {
        if (sourceCollider == null) return;

        GameObject pointObject = new GameObject("AutoCoverPoint_" + autoCoverPoints.Count);

        pointObject.transform.SetParent(transform);
        pointObject.transform.position = position;

        CoverPoint point = pointObject.AddComponent<CoverPoint>();
        point.SetSourceCollider(sourceCollider);

        autoCoverPoints.Add(point);
    }

    private void RebuildCoverList()
    {
        CleanupNullPoints();

        List<CoverPoint> points = new List<CoverPoint>();

        CoverPoint[] scenePoints = FindObjectsByType<CoverPoint>(FindObjectsSortMode.None);

        foreach (CoverPoint point in scenePoints)
        {
            if (point == null) continue;
            if (points.Contains(point)) continue;

            points.Add(point);
        }

        foreach (CoverPoint point in registeredCoverPoints)
        {
            if (point == null) continue;
            if (points.Contains(point)) continue;

            points.Add(point);
        }

        foreach (CoverPoint point in autoCoverPoints)
        {
            if (point == null) continue;
            if (points.Contains(point)) continue;

            points.Add(point);
        }

        allCoverPoints = points.ToArray();

        Debug.Log("CoverSystem: Total cover points = " + allCoverPoints.Length);
    }

    private void CleanupNullPoints()
    {
        registeredCoverPoints.RemoveAll(point => point == null);
        autoCoverPoints.RemoveAll(point => point == null);
    }

    public CoverPoint GetBestCover(
        Vector3 enemyPosition,
        Vector3 playerPosition,
        EnemyController enemy
    )
    {
        return GetBestCover(
            enemyPosition,
            playerPosition,
            enemy,
            null
        );
    }

    public CoverPoint GetBestCover(
        Vector3 enemyPosition,
        Vector3 playerPosition,
        EnemyController enemy,
        CoverPoint ignoredCover
    )
    {
        CleanupNullPoints();

        if (allCoverPoints == null || allCoverPoints.Length == 0)
        {
            RebuildCoverList();
        }

        if (allCoverPoints == null || allCoverPoints.Length == 0)
        {
            Debug.Log("CoverSystem: No cover points available for " + enemy.name);
            return null;
        }

        CoverPoint bestCover = null;
        float bestScore = Mathf.Infinity;

        foreach (CoverPoint cover in allCoverPoints)
        {
            if (cover == null) continue;
            if (!cover.isActiveAndEnabled) continue;
            if (cover == ignoredCover) continue;
            if (cover.IsOccupiedByOther(enemy)) continue;

            float enemyDistance =
                Vector3.Distance(
                    enemyPosition,
                    cover.transform.position
                );

            if (enemyDistance > searchRadius)
                continue;

            float playerDistance =
                Vector3.Distance(
                    cover.transform.position,
                    playerPosition
                );

            if (playerDistance > maxCoverDistanceFromPlayer)
                continue;

            if (IsTooCloseToOtherEnemies(
                cover.transform.position,
                enemy))
            {
                continue;
            }

            if (!cover.IsValidCover(
                enemyPosition,
                playerPosition))
            {
                continue;
            }
            if (ArenaTacticalMap.Instance != null)
{
    if (!ArenaTacticalMap.Instance.IsPathAvailable(
        enemyPosition,
        cover.transform.position
    ))
    {
        continue;
    }
}

            float score = enemyDistance;

            Vector3 coverToPlayer =
                playerPosition - cover.transform.position;

            float directionScore =
                Vector3.Angle(
                    cover.transform.forward,
                    coverToPlayer
                );

            score += directionScore * 0.02f;

            if (cover.IsOccupied())
                score += 1000f;

            if (score < bestScore)
            {
                bestScore = score;
                bestCover = cover;
            }
        }

        if (bestCover != null)
        {
            Debug.Log(
                "CoverSystem: Best cover = " +
                bestCover.name +
                " for " +
                enemy.name
            );
        }
        else
        {
            Debug.Log(
                "CoverSystem: No suitable cover for " +
                enemy.name
            );
        }

        return bestCover;
    }

    public List<Vector3> GetSearchPositions(
        Vector3 center,
        Vector3 enemyPosition,
        int maxPoints
    )
    {
        List<Vector3> result = new List<Vector3>();

        CleanupNullPoints();

        if (allCoverPoints == null || allCoverPoints.Length == 0)
        {
            RebuildCoverList();
        }

        if (allCoverPoints == null || allCoverPoints.Length == 0)
            return result;

        List<CoverPoint> candidates =
            new List<CoverPoint>();

        foreach (CoverPoint cover in allCoverPoints)
        {
            if (cover == null)
                continue;

            if (!cover.isActiveAndEnabled)
                continue;

            if (!cover.IsValidSearchPoint(
                enemyPosition,
                center,
                searchPointRadius))
            {
                continue;
            }

            candidates.Add(cover);
        }

        candidates.Sort(
            (a, b) =>
                Vector3.Distance(
                    enemyPosition,
                    a.transform.position
                ).CompareTo(
                    Vector3.Distance(
                        enemyPosition,
                        b.transform.position
                    )
                )
        );

        foreach (CoverPoint cover in candidates)
        {
            if (result.Count >= maxPoints)
                break;

            Vector3 candidate =
                cover.GetSearchPosition(
                    center,
                    result.Count
                );

            if (Vector3.Distance(
                candidate,
                enemyPosition) > searchPointRadius)
            {
                continue;
            }

            bool tooClose = false;

            foreach (Vector3 existing in result)
            {
                if (Vector3.Distance(
                    existing,
                    candidate
                ) < minimumSearchPointDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;

            result.Add(candidate);
        }

        return result;
    }

    public CoverPoint[] GetAllCoverPoints()
    {
        CleanupNullPoints();

        if (allCoverPoints == null)
        {
            RebuildCoverList();
        }

        return allCoverPoints ?? new CoverPoint[0];
    }

    private bool IsTooCloseToOtherEnemies(
        Vector3 coverPosition,
        EnemyController requestingEnemy
    )
    {
        EnemyController[] enemies =
            FindObjectsByType<EnemyController>(
                FindObjectsSortMode.None
            );

        foreach (EnemyController otherEnemy in enemies)
        {
            if (otherEnemy == null)
                continue;

            if (otherEnemy == requestingEnemy)
                continue;

            if (Vector3.Distance(
                coverPosition,
                otherEnemy.transform.position
            ) < minimumEnemySpacing)
            {
                return true;
            }
        }

        return false;
    }

    public void RebuildAutomaticCover()
    {
        foreach (CoverPoint point in autoCoverPoints)
        {
            if (point != null)
                Destroy(point.gameObject);
        }

        autoCoverPoints.Clear();
        autoCoverCreated = false;

        RebuildCoverList();

        Debug.Log(
            "CoverSystem: Automatic cover cleared. Generated cover points remain."
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position,
            searchRadius
        );

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(
            transform.position,
            searchPointRadius
        );
    }
}