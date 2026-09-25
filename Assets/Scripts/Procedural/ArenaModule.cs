using UnityEngine;

public class ArenaModule : MonoBehaviour
{
    public enum RoomType
    {
        Start,
        Combat,
        Tactical,
        Elite,
        Final
    }

    public enum RoomDirection
    {
        North,
        East,
        South,
        West
    }

    [Header("Module")]
    [SerializeField] private string moduleId = "Room";
    [SerializeField] private RoomType roomType = RoomType.Combat;

    [Header("Connections")]
    [SerializeField] private Transform[] connections;

    [Header("Player")]
    [SerializeField] private Transform playerSpawn;

    [Header("Enemy Spawns")]
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private EnemySpawnZone[] enemySpawnZones;

    [Header("Cover")]
    [SerializeField] private Transform[] coverPoints;

    [Header("Bounds")]
    [SerializeField] private Vector2 roomSize = new Vector2(20f, 20f);

    private readonly Transform[] directionalConnections =
        new Transform[4];

    public string ModuleId => moduleId;

    public RoomType Type => roomType;

    public Transform[] Connections => connections;

    public Transform PlayerSpawn => playerSpawn;

    public Transform[] EnemySpawnPoints => enemySpawnPoints;

    public EnemySpawnZone[] EnemySpawnZones => enemySpawnZones;

    public Transform[] CoverPoints => coverPoints;

    public Vector2 RoomSize => roomSize;

    public void SetModuleId(string id)
    {
        moduleId = id;
    }

    public void SetRoomType(RoomType type)
    {
        roomType = type;
    }

    public void SetRoomSize(
        float width,
        float depth
    )
    {
        roomSize =
            new Vector2(
                width,
                depth
            );
    }

    public void SetPlayerSpawn(
        Transform spawn
    )
    {
        playerSpawn =
            spawn;
    }

    public void SetDirectionalConnection(
        RoomDirection direction,
        Transform connection
    )
    {
        int index =
            DirectionToIndex(
                direction
            );

        directionalConnections[index] =
            connection;
    }

    public Transform GetDirectionalConnection(
        RoomDirection direction
    )
    {
        int index =
            DirectionToIndex(
                direction
            );

        return directionalConnections[index];
    }

    public void SetActiveConnections(
        RoomDirection[] directions
    )
    {
        if (directions == null)
        {
            connections =
                new Transform[0];

            return;
        }

        connections =
            new Transform[directions.Length];

        for (
            int i = 0;
            i < directions.Length;
            i++
        )
        {
            connections[i] =
                GetDirectionalConnection(
                    directions[i]
                );
        }
    }

    public void SetConnectionsCount(
        int count
    )
    {
        if (count < 0)
            count = 0;

        connections =
            new Transform[count];
    }

    public void SetConnection(
        int index,
        Transform connection
    )
    {
        if (connections == null)
            return;

        if (
            index < 0 ||
            index >= connections.Length
        )
        {
            return;
        }

        connections[index] =
            connection;
    }

    public Transform GetConnection(
        int index
    )
    {
        if (connections == null)
            return null;

        if (
            index < 0 ||
            index >= connections.Length
        )
        {
            return null;
        }

        return connections[index];
    }

    public void SetEnemySpawnCount(
        int count
    )
    {
        if (count < 0)
            count = 0;

        enemySpawnPoints =
            new Transform[count];

        enemySpawnZones =
            new EnemySpawnZone[count];
    }

    public void SetEnemySpawn(
        int index,
        Transform spawn
    )
    {
        if (enemySpawnPoints == null)
            return;

        if (
            index < 0 ||
            index >= enemySpawnPoints.Length
        )
        {
            return;
        }

        enemySpawnPoints[index] =
            spawn;

        if (
            enemySpawnZones != null &&
            index < enemySpawnZones.Length &&
            spawn != null
        )
        {
            enemySpawnZones[index] =
                spawn.GetComponent<EnemySpawnZone>();
        }
    }

    public void SetEnemySpawnZone(
        int index,
        EnemySpawnZone zone
    )
    {
        if (enemySpawnZones == null)
            return;

        if (
            index < 0 ||
            index >= enemySpawnZones.Length
        )
        {
            return;
        }

        enemySpawnZones[index] =
            zone;

        if (
            enemySpawnPoints != null &&
            index < enemySpawnPoints.Length &&
            zone != null
        )
        {
            enemySpawnPoints[index] =
                zone.transform;
        }
    }

    public void SetCoverCount(
        int count
    )
    {
        if (count < 0)
            count = 0;

        coverPoints =
            new Transform[count];
    }

    public void SetCoverPoint(
        int index,
        Transform cover
    )
    {
        if (coverPoints == null)
            return;

        if (
            index < 0 ||
            index >= coverPoints.Length
        )
        {
            return;
        }

        coverPoints[index] =
            cover;
    }

    public Transform GetEnemySpawnPoint(
        int index
    )
    {
        if (enemySpawnPoints == null)
            return null;

        if (
            index < 0 ||
            index >= enemySpawnPoints.Length
        )
        {
            return null;
        }

        return enemySpawnPoints[index];
    }

    public EnemySpawnZone GetEnemySpawnZone(
        int index
    )
    {
        if (enemySpawnZones == null)
            return null;

        if (
            index < 0 ||
            index >= enemySpawnZones.Length
        )
        {
            return null;
        }

        return enemySpawnZones[index];
    }

    public Bounds GetWorldBounds()
    {
        return new Bounds(
            transform.position,
            new Vector3(
                roomSize.x,
                10f,
                roomSize.y
            )
        );
    }

    private int DirectionToIndex(
        RoomDirection direction
    )
    {
        switch (direction)
        {
            case RoomDirection.North:
                return 0;

            case RoomDirection.East:
                return 1;

            case RoomDirection.South:
                return 2;

            default:
                return 3;
        }
    }

}