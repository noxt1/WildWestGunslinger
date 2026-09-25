using System.Collections.Generic;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    public static NoiseSystem Instance { get; private set; }

    public enum NoiseType
    {
        Gunshot,
        Footstep,
        Sprint,
        Explosion,
        Impact,
        Reload,
        Interaction
    }

    [System.Serializable]
    public struct NoiseEvent
    {
        public NoiseType type;
        public Vector3 position;
        public float radius;
        public float intensity;
        public float time;

        public NoiseEvent(
            NoiseType type,
            Vector3 position,
            float radius,
            float intensity)
        {
            this.type = type;
            this.position = position;
            this.radius = radius;
            this.intensity = intensity;
            this.time = Time.time;
        }
    }

    [Header("Gunshot")]
    [SerializeField] private float gunshotRadius = 35f;
    [SerializeField] private float gunshotIntensity = 1f;

    [Header("Footstep")]
    [SerializeField] private float footstepRadius = 9f;
    [SerializeField] private float footstepIntensity = 0.3f;

    [Header("Sprint")]
    [SerializeField] private float sprintRadius = 16f;
    [SerializeField] private float sprintIntensity = 0.55f;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 45f;
    [SerializeField] private float explosionIntensity = 1.5f;

    [Header("Impact")]
    [SerializeField] private float impactRadius = 12f;
    [SerializeField] private float impactIntensity = 0.4f;

    [Header("Reload")]
    [SerializeField] private float reloadRadius = 8f;
    [SerializeField] private float reloadIntensity = 0.2f;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 6f;
    [SerializeField] private float interactionIntensity = 0.15f;

    [Header("Propagation")]
    [SerializeField] private int maxWallBlocks = 4;
    [SerializeField] private float wallStrengthMultiplier = 0.55f;

    [Header("Detection")]
    [SerializeField] private LayerMask wallMask = ~0;
    [SerializeField] private bool useWallBlocking = true;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    private readonly List<EnemyHearing> listeners =
        new List<EnemyHearing>();

    private readonly List<EnemyHearing> listenersBuffer =
        new List<EnemyHearing>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log("NoiseSystem: READY");
    }

    private void Start()
    {
        RefreshListeners();
    }

    private void Update()
    {
        RefreshListeners();
    }

    private void RefreshListeners()
    {
        EnemyHearing[] found =
            FindObjectsByType<EnemyHearing>(
                FindObjectsSortMode.None
            );

        for (int i = 0; i < found.Length; i++)
        {
            EnemyHearing hearing = found[i];

            if (hearing == null)
                continue;

            if (!listeners.Contains(hearing))
            {
                listeners.Add(hearing);
            }
        }

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            EnemyHearing hearing = listeners[i];

            if (hearing == null ||
                !hearing.isActiveAndEnabled)
            {
                listeners.RemoveAt(i);
            }
        }

        for (int i = 0; i < listeners.Count; i++)
        {
            EnemyHearing hearing = listeners[i];

            if (hearing != null)
            {
                hearing.RegisterWithNoiseSystem();
            }
        }
    }

    public void RegisterListener(EnemyHearing listener)
    {
        if (listener == null)
            return;

        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(EnemyHearing listener)
    {
        if (listener == null)
            return;

        listeners.Remove(listener);
    }

    public void Emit(
        NoiseType type,
        Vector3 position)
    {
        GetPreset(
            type,
            out float radius,
            out float intensity
        );

        Emit(
            type,
            position,
            radius,
            intensity
        );
    }

    public void Emit(
        NoiseType type,
        Vector3 position,
        float radius,
        float intensity)
    {
        if (radius <= 0f)
            return;

        if (intensity <= 0f)
            return;

        NoiseEvent noise =
            new NoiseEvent(
                type,
                position,
                radius,
                intensity
            );

        listenersBuffer.Clear();

        for (int i = 0; i < listeners.Count; i++)
        {
            EnemyHearing listener = listeners[i];

            if (listener != null &&
                listener.isActiveAndEnabled)
            {
                listenersBuffer.Add(listener);
            }
        }

        int heardCount = 0;

        for (int i = 0; i < listenersBuffer.Count; i++)
        {
            EnemyHearing listener =
                listenersBuffer[i];

            if (listener == null)
                continue;

            float distance =
                Vector3.Distance(
                    position,
                    listener.transform.position
                );

            if (distance > radius)
                continue;

            float distanceFactor =
                1f -
                Mathf.Clamp01(
                    distance / radius
                );

            float strength =
                intensity *
                distanceFactor;

            if (strength <= 0f)
                continue;

            if (useWallBlocking)
            {
                int blockedWalls =
                    CountBlockingWalls(
                        position,
                        listener.transform.position,
                        listener
                    );

                if (blockedWalls > maxWallBlocks)
                    continue;

                if (blockedWalls > 0)
                {
                    strength *= Mathf.Pow(
                        wallStrengthMultiplier,
                        blockedWalls
                    );
                }
            }

            if (strength <= 0.001f)
                continue;

            listener.ReceiveNoise(
                noise,
                strength
            );

            heardCount++;

            if (debugLogs)
            {
                Debug.Log(
                    $"NoiseSystem: {type} HEARD BY " +
                    $"{listener.gameObject.name} " +
                    $"strength={strength:F2} " +
                    $"distance={distance:F1}"
                );
            }
        }

        if (debugLogs)
        {
            Debug.Log(
                $"NoiseSystem: SOUND {type} " +
                $"Position={position} " +
                $"Radius={radius:F1} " +
                $"Intensity={intensity:F2} " +
                $"HEARD BY {heardCount} ENEMIES"
            );
        }
    }

    public void EmitNoise(
        NoiseType type,
        Vector3 position)
    {
        Emit(
            type,
            position
        );
    }

    public void EmitNoise(
        NoiseType type,
        Vector3 position,
        float radius,
        float intensity)
    {
        Emit(
            type,
            position,
            radius,
            intensity
        );
    }

    public static void EmitGunshot(
        Vector3 position)
    {
        if (Instance == null)
            return;

        Instance.Emit(
            NoiseType.Gunshot,
            position
        );
    }

    public static void EmitFootstep(
        Vector3 position)
    {
        if (Instance == null)
            return;

        Instance.Emit(
            NoiseType.Footstep,
            position
        );
    }

    public static void EmitSprint(
        Vector3 position)
    {
        if (Instance == null)
            return;

        Instance.Emit(
            NoiseType.Sprint,
            position
        );
    }

    public static void EmitExplosion(
        Vector3 position)
    {
        if (Instance == null)
            return;

        Instance.Emit(
            NoiseType.Explosion,
            position
        );
    }

    public static void EmitImpact(
        Vector3 position)
    {
        if (Instance == null)
            return;

        Instance.Emit(
            NoiseType.Impact,
            position
        );
    }

    public static void EmitReload(
        Vector3 position)
    {
        if (Instance == null)
            return;

        Instance.Emit(
            NoiseType.Reload,
            position
        );
    }

    public static void EmitInteraction(
        Vector3 position)
    {
        if (Instance == null)
            return;

        Instance.Emit(
            NoiseType.Interaction,
            position
        );
    }

    private void GetPreset(
        NoiseType type,
        out float radius,
        out float intensity)
    {
        switch (type)
        {
            case NoiseType.Gunshot:
                radius = gunshotRadius;
                intensity = gunshotIntensity;
                break;

            case NoiseType.Footstep:
                radius = footstepRadius;
                intensity = footstepIntensity;
                break;

            case NoiseType.Sprint:
                radius = sprintRadius;
                intensity = sprintIntensity;
                break;

            case NoiseType.Explosion:
                radius = explosionRadius;
                intensity = explosionIntensity;
                break;

            case NoiseType.Impact:
                radius = impactRadius;
                intensity = impactIntensity;
                break;

            case NoiseType.Reload:
                radius = reloadRadius;
                intensity = reloadIntensity;
                break;

            case NoiseType.Interaction:
                radius = interactionRadius;
                intensity = interactionIntensity;
                break;

            default:
                radius = 10f;
                intensity = 0.3f;
                break;
        }
    }

    private int CountBlockingWalls(
        Vector3 source,
        Vector3 target,
        EnemyHearing listener)
    {
        Vector3 direction =
            target - source;

        float distance =
            direction.magnitude;

        if (distance <= 0.01f)
            return 0;

        direction /= distance;

        RaycastHit[] hits =
            Physics.RaycastAll(
                source,
                direction,
                distance,
                wallMask,
                QueryTriggerInteraction.Ignore
            );

        if (hits == null ||
            hits.Length == 0)
        {
            return 0;
        }

        System.Array.Sort(
            hits,
            (a, b) =>
                a.distance.CompareTo(
                    b.distance
                )
        );

        HashSet<Collider> counted =
            new HashSet<Collider>();

        int blockingWalls = 0;

        for (int i = 0; i < hits.Length; i++)
        {
            Collider collider =
                hits[i].collider;

            if (collider == null)
                continue;

            if (counted.Contains(collider))
                continue;

            counted.Add(collider);

            if (listener != null &&
                collider.transform.IsChildOf(
                    listener.transform
                ))
            {
                continue;
            }

            if (collider.CompareTag("Player"))
                continue;

            EnemyController enemy =
                collider.GetComponentInParent<EnemyController>();

            if (enemy != null)
                continue;

            blockingWalls++;
        }

        return blockingWalls;
    }

    public int GetListenerCount()
    {
        return listeners.Count;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}