using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [Header("Zone")]
    [SerializeField] private float radius = 2f;

    [Header("Room")]
    [SerializeField] private ArenaModule room;

    public float Radius => radius;

    public ArenaModule Room => room;

    public void Initialize(
        ArenaModule module,
        float zoneRadius
    )
    {
        room =
            module;

        radius =
            Mathf.Max(
                0.5f,
                zoneRadius
            );
    }

    public Vector3 GetRandomPosition()
    {
        Vector2 offset =
            Random.insideUnitCircle *
            radius;

        return
            transform.position +
            new Vector3(
                offset.x,
                0f,
                offset.y
            );
    }

    public bool IsInsideRoom(
        Vector3 position,
        float margin = 0.5f
    )
    {
        if (room == null)
            return true;

        Bounds bounds =
            room.GetWorldBounds();

        return
            position.x >
                bounds.min.x + margin &&
            position.x <
                bounds.max.x - margin &&
            position.z >
                bounds.min.z + margin &&
            position.z <
                bounds.max.z - margin;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            radius
        );
    }

}