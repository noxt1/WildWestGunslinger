using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MobileTouchControls : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GunController gunController;
    [SerializeField] private Camera playerCamera;

    [Header("Movement Zone")]
    [SerializeField, Range(0.2f, 0.8f)]
    private float movementZoneWidth = 0.5f;

    [SerializeField, Range(0.2f, 0.8f)]
    private float movementZoneHeight = 0.55f;

    [Header("Fire Zone")]
    [SerializeField, Range(0.2f, 0.8f)]
    private float fireZoneWidth = 0.5f;

    [SerializeField, Range(0.2f, 0.8f)]
    private float fireZoneHeight = 0.55f;

    [Header("Dynamic Joystick")]
    [SerializeField] private float joystickRadius = 120f;
    [SerializeField] private float joystickHandleRadius = 45f;

    private Canvas canvas;

    private GameObject joystickRoot;
    private RectTransform joystickRootRect;
    private RectTransform joystickBackgroundRect;
    private RectTransform joystickHandleRect;

    private Vector2 joystickCenter;

    private int movementFingerId = -1;
    private int fireFingerId = -1;

    private bool fireHeld;

    private readonly HashSet<int> currentFingerIds =
        new HashSet<int>();

    private readonly HashSet<int> previousFingerIds =
        new HashSet<int>();

    private void Awake()
    {
        if (!Application.isMobilePlatform)
        {
            return;
        }

        if (playerController == null)
        {
            playerController =
                FindFirstObjectByType<PlayerController>();
        }

        if (gunController == null)
        {
            gunController =
                FindFirstObjectByType<GunController>();
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        FindCanvas();
        CreateDynamicJoystick();

        DisableLegacyMobileControls();

        HideJoystick();
    }

    private void Update()
    {
        if (!Application.isMobilePlatform)
        {
            return;
        }

        ProcessTouches();

        /*
         * Пока палец FIRE удерживается,
         * каждый кадр поддерживаем состояние
         * непрерывной стрельбы.
         */
        if (fireHeld)
        {
            KeepFiring();
        }
    }

    private void FindCanvas()
    {
        canvas =
            FindFirstObjectByType<Canvas>();

        if (canvas != null)
        {
            return;
        }

        GameObject canvasObject =
            new GameObject(
                "MobileTouchCanvas"
            );

        canvas =
            canvasObject.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        UnityEngine.UI.CanvasScaler scaler =
            canvasObject.AddComponent<
                UnityEngine.UI.CanvasScaler
            >();

        scaler.uiScaleMode =
            UnityEngine.UI.CanvasScaler.ScaleMode
                .ScaleWithScreenSize;

        canvasObject.AddComponent<
            UnityEngine.UI.GraphicRaycaster
        >();
    }

    private void CreateDynamicJoystick()
    {
        if (canvas == null)
        {
            return;
        }

        joystickRoot =
            new GameObject(
                "DynamicJoystick"
            );

        joystickRoot.transform.SetParent(
            canvas.transform,
            false
        );

        joystickRootRect =
            joystickRoot.AddComponent<
                RectTransform
            >();

        joystickRootRect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickRootRect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickRootRect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickRootRect.sizeDelta =
            new Vector2(
                joystickRadius * 2f,
                joystickRadius * 2f
            );

        CreateJoystickBackground();
        CreateJoystickHandle();
    }

    private void CreateJoystickBackground()
    {
        GameObject background =
            new GameObject(
                "DynamicJoystickBackground"
            );

        background.transform.SetParent(
            joystickRoot.transform,
            false
        );

        joystickBackgroundRect =
            background.AddComponent<
                RectTransform
            >();

        joystickBackgroundRect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickBackgroundRect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickBackgroundRect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickBackgroundRect.anchoredPosition =
            Vector2.zero;

        joystickBackgroundRect.sizeDelta =
            new Vector2(
                joystickRadius * 2f,
                joystickRadius * 2f
            );

        UnityEngine.UI.Image image =
            background.AddComponent<
                UnityEngine.UI.Image
            >();

        image.sprite =
            CreateCircleSprite();

        image.raycastTarget =
            false;

        image.color =
            new Color(
                1f,
                1f,
                1f,
                0.28f
            );
    }

    private void CreateJoystickHandle()
    {
        GameObject handle =
            new GameObject(
                "DynamicJoystickHandle"
            );

        handle.transform.SetParent(
            joystickRoot.transform,
            false
        );

        joystickHandleRect =
            handle.AddComponent<
                RectTransform
            >();

        joystickHandleRect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickHandleRect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickHandleRect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        joystickHandleRect.anchoredPosition =
            Vector2.zero;

        joystickHandleRect.sizeDelta =
            new Vector2(
                joystickHandleRadius * 2f,
                joystickHandleRadius * 2f
            );

        UnityEngine.UI.Image image =
            handle.AddComponent<
                UnityEngine.UI.Image
            >();

        image.sprite =
            CreateCircleSprite();

        image.raycastTarget =
            false;

        image.color =
            new Color(
                1f,
                1f,
                1f,
                0.65f
            );
    }

    private Sprite CreateCircleSprite()
    {
        const int textureSize = 128;

        Texture2D texture =
            new Texture2D(
                textureSize,
                textureSize,
                TextureFormat.RGBA32,
                false
            );

        texture.filterMode =
            FilterMode.Bilinear;

        Vector2 center =
            new Vector2(
                textureSize * 0.5f,
                textureSize * 0.5f
            );

        float radius =
            textureSize * 0.48f;

        Color[] pixels =
            new Color[
                textureSize *
                textureSize
            ];

        for (
            int y = 0;
            y < textureSize;
            y++
        )
        {
            for (
                int x = 0;
                x < textureSize;
                x++
            )
            {
                float distance =
                    Vector2.Distance(
                        new Vector2(
                            x,
                            y
                        ),
                        center
                    );

                if (distance <= radius)
                {
                    pixels[
                        y * textureSize + x
                    ] =
                        Color.white;
                }
                else
                {
                    pixels[
                        y * textureSize + x
                    ] =
                        Color.clear;
                }
            }
        }

        texture.SetPixels(
            pixels
        );

        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(
                0f,
                0f,
                textureSize,
                textureSize
            ),
            new Vector2(
                0.5f,
                0.5f
            ),
            100f
        );
    }

    private void DisableLegacyMobileControls()
    {
        GameObject[] allObjects =
            FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (
            GameObject obj
            in allObjects
        )
        {
            if (obj == null)
            {
                continue;
            }

            if (
                obj.name ==
                    "JoystickBackground" ||
                obj.name ==
                    "FireButton"
            )
            {
                obj.SetActive(
                    false
                );
            }
        }

        MobileJoystick[] oldJoysticks =
            FindObjectsByType<MobileJoystick>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (
            MobileJoystick oldJoystick
            in oldJoysticks
        )
        {
            if (oldJoystick == null)
            {
                continue;
            }

            oldJoystick.gameObject.SetActive(
                false
            );
        }
    }

    private void ProcessTouches()
    {
        Touchscreen touchscreen =
            Touchscreen.current;

        if (touchscreen == null)
        {
            StopAllMobileInput();
            return;
        }

        currentFingerIds.Clear();

        foreach (
            var touch
            in touchscreen.touches
        )
        {
            if (!touch.press.isPressed)
            {
                continue;
            }

            int fingerId =
                touch.touchId.ReadValue();

            if (fingerId < 0)
            {
                continue;
            }

            Vector2 position =
                touch.position.ReadValue();

            currentFingerIds.Add(
                fingerId
            );

            bool isNewFinger =
                !previousFingerIds.Contains(
                    fingerId
                );

            if (isNewFinger)
            {
                HandleNewFinger(
                    fingerId,
                    position
                );
            }

            if (
                fingerId ==
                movementFingerId
            )
            {
                UpdateMovement(
                    position
                );
            }

            if (
                fingerId ==
                fireFingerId
            )
            {
                KeepFiring();
            }
        }

        if (
            movementFingerId != -1 &&
            !currentFingerIds.Contains(
                movementFingerId
            )
        )
        {
            movementFingerId =
                -1;

            if (playerController != null)
            {
                playerController
                    .ClearMobileTouchInput();
            }

            HideJoystick();
        }

        if (
            fireFingerId != -1 &&
            !currentFingerIds.Contains(
                fireFingerId
            )
        )
        {
            fireFingerId =
                -1;

            StopFiring();
        }

        previousFingerIds.Clear();

        foreach (
            int fingerId
            in currentFingerIds
        )
        {
            previousFingerIds.Add(
                fingerId
            );
        }
    }

    private void HandleNewFinger(
        int fingerId,
        Vector2 position
    )
    {
        bool inMovementZone =
            position.x <=
            Screen.width *
            movementZoneWidth &&
            position.y <=
            Screen.height *
            movementZoneHeight;

        bool inFireZone =
            position.x >=
            Screen.width *
            (1f - fireZoneWidth) &&
            position.y <=
            Screen.height *
            fireZoneHeight;

        if (
            movementFingerId == -1 &&
            inMovementZone
        )
        {
            movementFingerId =
                fingerId;

            ShowJoystick(
                position
            );

            UpdateMovement(
                position
            );

            return;
        }

        if (
            fireFingerId == -1 &&
            inFireZone
        )
        {
            fireFingerId =
                fingerId;

            StartFiring();

            return;
        }
    }

    private void ShowJoystick(
        Vector2 screenPosition
    )
    {
        if (joystickRoot == null)
        {
            return;
        }

        joystickCenter =
            screenPosition;

        joystickRootRect.position =
            joystickCenter;

        joystickRoot.SetActive(
            true
        );

        if (joystickHandleRect != null)
        {
            joystickHandleRect.anchoredPosition =
                Vector2.zero;
        }
    }

    private void HideJoystick()
    {
        if (joystickRoot == null)
        {
            return;
        }

        joystickRoot.SetActive(
            false
        );
    }

    private void UpdateMovement(
        Vector2 screenPosition
    )
    {
        if (
            movementFingerId == -1 ||
            playerController == null
        )
        {
            return;
        }

        Vector2 delta =
            screenPosition -
            joystickCenter;

        Vector2 stickInput =
            Vector2.ClampMagnitude(
                delta /
                Mathf.Max(
                    1f,
                    joystickRadius
                ),
                1f
            );

        if (joystickHandleRect != null)
        {
            joystickHandleRect.anchoredPosition =
                stickInput *
                joystickRadius;
        }

        Vector3 movement =
            ConvertInputToCameraMovement(
                stickInput
            );

        playerController
            .SetMobileTouchInput(
                new Vector2(
                    movement.x,
                    movement.z
                )
            );
    }

    private Vector3 ConvertInputToCameraMovement(
        Vector2 input
    )
    {
        if (playerCamera == null)
        {
            playerCamera =
                Camera.main;
        }

        if (playerCamera == null)
        {
            return new Vector3(
                input.x,
                0f,
                input.y
            );
        }

        Vector3 cameraForward =
            playerCamera.transform.forward;

        Vector3 cameraRight =
            playerCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        if (
            cameraForward.sqrMagnitude >
            0.001f
        )
        {
            cameraForward.Normalize();
        }

        if (
            cameraRight.sqrMagnitude >
            0.001f
        )
        {
            cameraRight.Normalize();
        }

        Vector3 movement =
            cameraRight * input.x +
            cameraForward * input.y;

        return Vector3.ClampMagnitude(
            movement,
            1f
        );
    }

    private void StartFiring()
    {
        if (fireHeld)
        {
            return;
        }

        fireHeld =
            true;

        if (gunController == null)
        {
            gunController =
                FindFirstObjectByType<
                    GunController
                >();
        }

        if (gunController == null)
        {
            return;
        }

        gunController.SetFiring(
            true
        );

        gunController.FireButtonDown();
    }

    private void KeepFiring()
    {
        if (!fireHeld)
        {
            return;
        }

        if (gunController == null)
        {
            gunController =
                FindFirstObjectByType<
                    GunController
                >();
        }

        if (gunController == null)
        {
            return;
        }

        gunController.SetFiring(
            true
        );

        gunController.FireButtonDown();
    }

    private void StopFiring()
    {
        fireHeld =
            false;

        if (gunController != null)
        {
            gunController.FireButtonUp();
        }
    }

    private void StopAllMobileInput()
    {
        movementFingerId =
            -1;

        fireFingerId =
            -1;

        previousFingerIds.Clear();
        currentFingerIds.Clear();

        fireHeld =
            false;

        if (playerController != null)
        {
            playerController
                .ClearMobileTouchInput();
        }

        if (gunController != null)
        {
            gunController
                .FireButtonUp();
        }

        HideJoystick();
    }

    private void OnDestroy()
    {
        DestroyJoystickTextures();
    }

    private void DestroyJoystickTextures()
    {
        DestroyJoystickImageTexture(
            joystickBackgroundRect
        );

        DestroyJoystickImageTexture(
            joystickHandleRect
        );
    }

    private void DestroyJoystickImageTexture(
        RectTransform rect
    )
    {
        if (rect == null)
        {
            return;
        }

        UnityEngine.UI.Image image =
            rect.GetComponent<
                UnityEngine.UI.Image
            >();

        if (image == null)
        {
            return;
        }

        Sprite sprite =
            image.sprite;

        if (sprite == null)
        {
            return;
        }

        Texture2D texture =
            sprite.texture;

        Destroy(sprite);

        if (texture != null)
        {
            Destroy(texture);
        }
    }
}