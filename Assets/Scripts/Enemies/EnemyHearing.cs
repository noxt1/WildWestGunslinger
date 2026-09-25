using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyHearing : MonoBehaviour
{
    [Header("Hearing")]
    [SerializeField] private bool enabledHearing = true;
    [SerializeField] private float minimumNoiseStrength = 0.03f;

    [Header("Reaction")]
    [SerializeField] private float investigateDuration = 5f;
    [SerializeField] private float directLookTime = 0.8f;
    [SerializeField] private float lookAtNoiseSpeed = 260f;

    [Header("Priority")]
    [SerializeField] private float gunshotPriority = 10f;
    [SerializeField] private float explosionPriority = 12f;
    [SerializeField] private float sprintPriority = 6f;
    [SerializeField] private float footstepPriority = 3f;
    [SerializeField] private float impactPriority = 4f;
    [SerializeField] private float reloadPriority = 2f;
    [SerializeField] private float interactionPriority = 1f;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    private EnemyController enemyController;
    private EnemyTacticalVision tacticalVision;

    private NoiseSystem.NoiseEvent currentNoise;

    private bool investigating;
    private bool lookingAtNoise;
    private bool registered;

    private float investigationTimer;
    private float lookTimer;

    private Vector3 noisePosition;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();

        tacticalVision = GetComponent<EnemyTacticalVision>();

        if (tacticalVision == null)
        {
            tacticalVision = gameObject.AddComponent<EnemyTacticalVision>();
        }
    }

    private void Start()
    {
        RegisterWithNoiseSystem();
    }

    private void OnEnable()
    {
        registered = false;
    }

    private void OnDisable()
    {
        UnregisterFromNoiseSystem();
    }

    public void RegisterWithNoiseSystem()
    {
        if (registered)
            return;

        if (NoiseSystem.Instance == null)
            return;

        NoiseSystem.Instance.RegisterListener(this);

        registered = true;

        if (debugLogs)
        {
            Debug.Log(
                $"{gameObject.name}: Hearing registered."
            );
        }
    }

    private void UnregisterFromNoiseSystem()
    {
        if (!registered)
            return;

        if (NoiseSystem.Instance != null)
        {
            NoiseSystem.Instance.UnregisterListener(this);
        }

        registered = false;
    }

    public void ReceiveNoise(
        NoiseSystem.NoiseEvent noise,
        float strength
    )
    {
        if (!enabledHearing)
            return;

        if (strength < minimumNoiseStrength)
            return;

        if (enemyController == null)
        {
            enemyController = GetComponent<EnemyController>();
        }

        if (enemyController == null)
            return;

        float newPriority =
            strength * GetNoisePriority(noise.type);

        if (investigating)
        {
            float currentPriority =
                currentNoise.intensity *
                GetNoisePriority(currentNoise.type);

            if (newPriority < currentPriority)
                return;
        }

        currentNoise = noise;

        noisePosition = noise.position;
        noisePosition.y = transform.position.y;

        investigating = true;
        lookingAtNoise = true;

        investigationTimer = investigateDuration;
        lookTimer = directLookTime;

        if (tacticalVision != null)
        {
            tacticalVision.LookAtNoise(noisePosition);
        }

        FaceNoise();

        enemyController.OnNoiseHeard(
            noisePosition,
            strength,
            noise.type
        );

        if (debugLogs)
        {
            float distance =
                Vector3.Distance(
                    transform.position,
                    noisePosition
                );

            Debug.Log(
                $"{gameObject.name}: HEARD {noise.type}! " +
                $"NoisePosition={noisePosition} " +
                $"Strength={strength:F2} " +
                $"Distance={distance:F1}"
            );
        }
    }

    private void FixedUpdate()
    {
        if (!registered)
        {
            RegisterWithNoiseSystem();
        }

        if (!investigating)
            return;

        if (enemyController == null)
        {
            enemyController = GetComponent<EnemyController>();
        }

        if (enemyController == null)
            return;

        if (tacticalVision != null &&
            tacticalVision.PlayerDetected)
        {
            StopInvestigation();
            return;
        }

        investigationTimer -= Time.fixedDeltaTime;

        if (lookingAtNoise)
        {
            UpdateLookingAtNoise();
            return;
        }

        if (investigationTimer <= 0f)
        {
            StopInvestigation();
        }
    }

    private void UpdateLookingAtNoise()
    {
        Vector3 direction =
            noisePosition - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            lookingAtNoise = false;
            return;
        }

        direction.Normalize();

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction,
                Vector3.up
            );

        transform.rotation =
            Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                lookAtNoiseSpeed *
                Time.fixedDeltaTime
            );

        lookTimer -= Time.fixedDeltaTime;

        if (lookTimer <= 0f)
        {
            lookingAtNoise = false;
        }
    }

    private void FaceNoise()
    {
        Vector3 direction =
            noisePosition - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        transform.forward =
            direction.normalized;
    }

    private float GetNoisePriority(
        NoiseSystem.NoiseType type
    )
    {
        switch (type)
        {
            case NoiseSystem.NoiseType.Gunshot:
                return gunshotPriority;

            case NoiseSystem.NoiseType.Explosion:
                return explosionPriority;

            case NoiseSystem.NoiseType.Sprint:
                return sprintPriority;

            case NoiseSystem.NoiseType.Footstep:
                return footstepPriority;

            case NoiseSystem.NoiseType.Impact:
                return impactPriority;

            case NoiseSystem.NoiseType.Reload:
                return reloadPriority;

            case NoiseSystem.NoiseType.Interaction:
                return interactionPriority;

            default:
                return 1f;
        }
    }

    public bool IsInvestigating()
    {
        return investigating;
    }

    public bool IsLookingAtNoise()
    {
        return lookingAtNoise;
    }

    public Vector3 GetCurrentNoisePosition()
    {
        return noisePosition;
    }

    public NoiseSystem.NoiseEvent GetCurrentNoise()
    {
        return currentNoise;
    }

    public float GetRemainingInvestigationTime()
    {
        return investigationTimer;
    }

    public void StopInvestigation()
    {
        investigating = false;
        lookingAtNoise = false;

        investigationTimer = 0f;
        lookTimer = 0f;

        if (tacticalVision != null)
        {
            tacticalVision.SetInvestigatingState(false);
        }

        if (debugLogs)
        {
            Debug.Log(
                $"{gameObject.name}: " +
                "HEARING -> INVESTIGATION STOPPED"
            );
        }
    }
}
