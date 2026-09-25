using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [Header("Joystick")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleRange = 0.7f;
    [SerializeField] private float deadZone = 0.1f;

    private Vector2 inputVector;
    private Canvas canvas;

    private bool externalControl;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    public Vector2 Input => inputVector;

    public Vector2 InputVector => inputVector;
    public Vector2 Direction => inputVector;
    public Vector2 InputDirection => inputVector;

    private void Awake()
    {
        if (background == null)
        {
            background =
                GetComponent<RectTransform>();
        }

        canvas =
            GetComponentInParent<Canvas>();

        ResetJoystick();
    }

    public void OnPointerDown(
        PointerEventData eventData)
    {
        if (externalControl)
            return;

        OnDrag(eventData);
    }

    public void OnDrag(
        PointerEventData eventData)
    {
        if (externalControl)
            return;

        ProcessScreenPosition(
            eventData.position
        );
    }

    public void OnPointerUp(
        PointerEventData eventData)
    {
        if (externalControl)
            return;

        ResetJoystick();
    }

    public void BeginExternalControl(
        Vector2 screenPosition)
    {
        externalControl = true;

        SetDynamicPosition(
            screenPosition
        );

        inputVector =
            Vector2.zero;

        if (handle != null)
        {
            handle.anchoredPosition =
                Vector2.zero;
        }
    }

    public void SetExternalInput(
        Vector2 value)
    {
        if (!externalControl)
            return;

        inputVector =
            Vector2.ClampMagnitude(
                value,
                1f
            );

        UpdateHandleVisual();
    }

    public void EndExternalControl()
    {
        externalControl = false;

        ResetJoystick();
    }

    public void SetDynamicPosition(
        Vector2 screenPosition)
    {
        RectTransform root =
            GetComponent<RectTransform>();

        if (root == null)
            return;

        RectTransform parent =
            root.parent as RectTransform;

        if (parent == null)
        {
            root.position =
                screenPosition;

            return;
        }

        Camera eventCamera = null;

        if (
            canvas != null &&
            canvas.renderMode !=
            RenderMode.ScreenSpaceOverlay
        )
        {
            eventCamera =
                canvas.worldCamera;
        }

        if (
            RectTransformUtility
                .ScreenPointToLocalPointInRectangle(
                    parent,
                    screenPosition,
                    eventCamera,
                    out Vector2 localPoint
                )
        )
        {
            root.anchoredPosition =
                localPoint;
        }
    }

    private void ProcessScreenPosition(
        Vector2 screenPosition)
    {
        if (
            background == null ||
            handle == null
        )
        {
            return;
        }

        Camera eventCamera = null;

        if (
            canvas != null &&
            canvas.renderMode !=
            RenderMode.ScreenSpaceOverlay
        )
        {
            eventCamera =
                canvas.worldCamera;
        }

        if (
            !RectTransformUtility
                .ScreenPointToLocalPointInRectangle(
                    background,
                    screenPosition,
                    eventCamera,
                    out Vector2 localPoint
                )
        )
        {
            return;
        }

        Vector2 halfSize =
            background.rect.size * 0.5f;

        if (
            halfSize.x <= 0f ||
            halfSize.y <= 0f
        )
        {
            return;
        }

        Vector2 normalized =
            new Vector2(
                localPoint.x / halfSize.x,
                localPoint.y / halfSize.y
            );

        normalized =
            Vector2.ClampMagnitude(
                normalized,
                1f
            );

        if (
            normalized.magnitude <
            deadZone
        )
        {
            inputVector =
                Vector2.zero;

            handle.anchoredPosition =
                Vector2.zero;

            return;
        }

        float correctedMagnitude =
            Mathf.InverseLerp(
                deadZone,
                1f,
                normalized.magnitude
            );

        inputVector =
            normalized.normalized *
            correctedMagnitude;

        UpdateHandleVisual();
    }

    private void UpdateHandleVisual()
    {
        if (
            background == null ||
            handle == null
        )
        {
            return;
        }

        Vector2 halfSize =
            background.rect.size * 0.5f;

        Vector2 handleLimit =
            new Vector2(
                halfSize.x * handleRange,
                halfSize.y * handleRange
            );

        handle.anchoredPosition =
            new Vector2(
                inputVector.x *
                handleLimit.x,

                inputVector.y *
                handleLimit.y
            );
    }

    private void ResetJoystick()
    {
        inputVector =
            Vector2.zero;

        if (handle != null)
        {
            handle.anchoredPosition =
                Vector2.zero;
        }
    }
}