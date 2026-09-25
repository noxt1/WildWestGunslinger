using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float silentSpeed = 1.35f;
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float silentInputThreshold = 0.18f;
    [SerializeField] private float walkInputThreshold = 0.58f;
    [SerializeField] private float runInputThreshold = 0.88f;

    [Header("Mobile")]
    [SerializeField] private MobileJoystick joystick;

    [Header("Footstep Noise")]
    [SerializeField] private float walkStepInterval = 0.48f;
    [SerializeField] private float sprintStepInterval = 0.32f;
    [SerializeField] private float sprintSpeedThreshold = 4f;

    private Rigidbody rb;

    private Vector2 input;
    private float movementIntensity;
    private bool movementIsSilent;
    private float currentMoveSpeed;
    private float footstepTimer;

    private float fixedYRotation;

    private bool externalMobileInputActive;
    private Vector2 externalMobileInput;

    private bool externalAimActive;
    private Vector3 externalAimDirection;

    private void Awake()
    {
        rb =
            GetComponent<Rigidbody>();

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        Vector3 euler =
            rb.rotation.eulerAngles;

        fixedYRotation =
            euler.y;

        rb.angularVelocity =
            Vector3.zero;
    }

    private void Update()
    {
        ReadInput();
        UpdateFootstepNoise();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        KeepPlayerUpright();
    }

    public void SetMobileTouchInput(
        Vector2 value)
    {
        externalMobileInputActive =
            true;

        externalMobileInput =
            Vector2.ClampMagnitude(
                value,
                1f
            );
    }

    public void ClearMobileTouchInput()
    {
        externalMobileInputActive =
            false;

        externalMobileInput =
            Vector2.zero;

        input =
            Vector2.zero;

        movementIntensity =
            0f;

        movementIsSilent =
            false;
    }

    public void SetAimDirection(
        Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude <
            0.0001f)
        {
            return;
        }

        externalAimDirection =
            direction.normalized;

        externalAimActive =
            true;

        fixedYRotation =
            Quaternion.LookRotation(
                externalAimDirection,
                Vector3.up
            ).eulerAngles.y;
    }

    public void ClearAimDirection()
    {
        externalAimActive =
            false;

        externalAimDirection =
            Vector3.zero;
    }

    private void ReadInput()
    {
        input =
            Vector2.zero;

        if (externalMobileInputActive)
        {
            input =
                Vector2.ClampMagnitude(
                    externalMobileInput,
                    1f
                );

            movementIntensity =
                input.magnitude;

            movementIsSilent =
                movementIntensity >
                0.01f &&
                movementIntensity <=
                silentInputThreshold;

            return;
        }

        if (
            joystick != null &&
            joystick.Input.sqrMagnitude >
            0.01f
        )
        {
            input =
                Vector2.ClampMagnitude(
                    joystick.Input,
                    1f
                );

            movementIntensity =
                input.magnitude;

            movementIsSilent =
                movementIntensity <
                silentInputThreshold;

            return;
        }

        if (Keyboard.current == null)
        {
            movementIntensity =
                0f;

            movementIsSilent =
                false;

            return;
        }

        if (Keyboard.current.wKey.isPressed)
            input.y += 1f;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1f;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1f;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1f;

        movementIntensity =
            Mathf.Clamp01(
                input.magnitude
            );

        movementIsSilent =
            false;

        if (movementIntensity > 0.001f)
        {
            input /=
                movementIntensity;
        }
    }

    private void MovePlayer()
    {
        Vector3 movement =
            new Vector3(
                input.x,
                0f,
                input.y
            );

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        movementIntensity =
            Mathf.Clamp01(
                input.magnitude
            );

        if (movementIntensity <=
            0.01f)
        {
            currentMoveSpeed =
                0f;

            movementIsSilent =
                false;
        }
        else if (
            externalMobileInputActive ||
            (
                joystick != null &&
                joystick.Input.sqrMagnitude >
                0.01f
            )
        )
        {
            float normalizedSpeed =
                Mathf.InverseLerp(
                    silentInputThreshold,
                    1f,
                    movementIntensity
                );

            if (
                movementIntensity <=
                silentInputThreshold
            )
            {
                float silentT =
                    movementIntensity /
                    Mathf.Max(
                        0.001f,
                        silentInputThreshold
                    );

                currentMoveSpeed =
                    Mathf.Lerp(
                        0f,
                        silentSpeed,
                        silentT
                    );

                movementIsSilent =
                    true;
            }
            else
            {
                currentMoveSpeed =
                    Mathf.Lerp(
                        silentSpeed,
                        runSpeed,
                        normalizedSpeed
                    );

                movementIsSilent =
                    false;
            }
        }
        else
        {
            bool sprinting =
                Keyboard.current != null &&
                Keyboard.current.leftShiftKey
                    .isPressed;

            currentMoveSpeed =
                sprinting
                    ? runSpeed
                    : walkSpeed;

            movementIsSilent =
                false;

            movementIntensity =
                sprinting
                    ? 1f
                    : 0.65f;
        }

        Vector3 velocity =
            rb.linearVelocity;

        velocity.x =
            movement.x *
            currentMoveSpeed;

        velocity.z =
            movement.z *
            currentMoveSpeed;

        rb.linearVelocity =
            velocity;

        if (
            !externalAimActive &&
            movement.sqrMagnitude >
            0.01f
        )
        {
            fixedYRotation =
                Quaternion.LookRotation(
                    movement.normalized,
                    Vector3.up
                ).eulerAngles.y;
        }
    }

    private void KeepPlayerUpright()
    {
        if (
            externalAimActive &&
            externalAimDirection.sqrMagnitude >
            0.0001f
        )
        {
            fixedYRotation =
                Quaternion.LookRotation(
                    externalAimDirection,
                    Vector3.up
                ).eulerAngles.y;
        }

        Vector3 velocity =
            rb.linearVelocity;

        velocity.y =
            rb.linearVelocity.y;

        rb.linearVelocity =
            velocity;

        rb.angularVelocity =
            new Vector3(
                0f,
                rb.angularVelocity.y,
                0f
            );

        Quaternion targetRotation =
            Quaternion.Euler(
                0f,
                fixedYRotation,
                0f
            );

        rb.MoveRotation(
            targetRotation
        );
    }

    private void UpdateFootstepNoise()
    {
        if (
            NoiseSystem.Instance == null
        )
        {
            footstepTimer =
                0f;

            return;
        }

        Vector3 horizontalVelocity =
            rb.linearVelocity;

        horizontalVelocity.y =
            0f;

        float currentSpeed =
            horizontalVelocity.magnitude;

        if (currentSpeed < 0.15f)
        {
            footstepTimer =
                0f;

            return;
        }

        if (movementIsSilent)
        {
            footstepTimer =
                0f;

            return;
        }

        float interval;

        if (
            currentSpeed >=
            sprintSpeedThreshold
        )
        {
            interval =
                sprintStepInterval;
        }
        else
        {
            float t =
                Mathf.InverseLerp(
                    Mathf.Max(
                        0.1f,
                        silentSpeed
                    ),
                    Mathf.Max(
                        walkSpeed,
                        0.1f
                    ),
                    currentSpeed
                );

            interval =
                Mathf.Lerp(
                    0.95f,
                    walkStepInterval,
                    t
                );
        }

        footstepTimer +=
            Time.deltaTime;

        if (
            footstepTimer <
            interval
        )
        {
            return;
        }

        footstepTimer =
            0f;

        if (
            currentSpeed >=
            sprintSpeedThreshold
        )
        {
            NoiseSystem.EmitSprint(
                transform.position
            );
        }
        else
        {
            NoiseSystem.EmitFootstep(
                transform.position
            );
        }
    }

    public float MovementIntensity =>
        movementIntensity;

    public float CurrentMoveSpeed =>
        currentMoveSpeed;

    public bool IsMovingSilently =>
        movementIsSilent;
}