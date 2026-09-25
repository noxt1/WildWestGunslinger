using UnityEngine;
using UnityEngine.UI;

public class XPBarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private XPManager xpManager;
    [SerializeField] private Image xpFill;

    private void Start()
    {
        if (xpManager == null)
        {
            Debug.LogError(
                "XPManager не назначен в XPBarController."
            );

            return;
        }

        UpdateXPBar();
    }

    private void Update()
    {
        if (xpManager == null || xpFill == null)
            return;

        UpdateXPBar();
    }

    private void UpdateXPBar()
    {
        float progress =
            (float)xpManager.CurrentXP /
            xpManager.RequiredXP;

        xpFill.fillAmount =
            Mathf.Clamp01(progress);
    }
}