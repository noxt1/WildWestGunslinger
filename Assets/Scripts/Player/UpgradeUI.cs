using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject upgradePanel;

    [Header("Buttons")]
    [SerializeField] private Button damageButton;
    [SerializeField] private Button fireRateButton;
    [SerializeField] private Button maxHealthButton;

    [Header("HUD")]
    [SerializeField] private GameObject xpBar;

    private UpgradeManager upgradeManager;

    private void Awake()
    {
        Debug.Log("UpgradeUI: Awake");

        Debug.Log("Upgrade Panel: " + (upgradePanel != null));
        Debug.Log("Damage Button: " + (damageButton != null));
        Debug.Log("Fire Rate Button: " + (fireRateButton != null));
        Debug.Log("Max Health Button: " + (maxHealthButton != null));
        Debug.Log("XP Bar: " + (xpBar != null));

        /*
         * PHASE 5C:
         * Button listeners are deliberately NOT registered here.
         *
         * UpgradePanel starts inactive, so this Awake() never runs during
         * scene load. It runs later from inside Show() at
         * upgradePanel.SetActive(true) - i.e. AFTER Show() already called
         * SetButton() for all three buttons. Awake() used to append a
         * second identical listener on top, so a single click applied the
         * upgrade twice (x1.20 -> effectively x1.44).
         *
         * SetButton() is now the single registration point; it already
         * calls RemoveAllListeners() before AddListener(). The buttons are
         * only ever used through Show(), so this is sufficient.
         */
    }

    private void OnDestroy()
    {
        if (damageButton != null)
            damageButton.onClick.RemoveListener(ChooseDamage);

        if (fireRateButton != null)
            fireRateButton.onClick.RemoveListener(ChooseFireRate);

        if (maxHealthButton != null)
            maxHealthButton.onClick.RemoveListener(ChooseMaxHealth);
    }

    public void Show(
        UpgradeManager manager,
        List<UpgradeManager.UpgradeType> choices
    )
    {
        Debug.Log("UpgradeUI: SHOW");

        upgradeManager = manager;

        if (upgradePanel == null)
        {
            Debug.LogError("UpgradeUI: UpgradePanel НЕ назначен!");
            return;
        }

        if (damageButton == null)
            Debug.LogWarning("UpgradeUI: DamageButton НЕ назначен!");

        if (fireRateButton == null)
            Debug.LogWarning("UpgradeUI: FireRateButton НЕ назначен!");

        if (maxHealthButton == null)
            Debug.LogWarning("UpgradeUI: MaxHealthButton НЕ назначен!");

        SetButton(
            damageButton,
            choices.Count > 0
                ? choices[0]
                : UpgradeManager.UpgradeType.Damage
        );

        SetButton(
            fireRateButton,
            choices.Count > 1
                ? choices[1]
                : UpgradeManager.UpgradeType.FireRate
        );

        SetButton(
            maxHealthButton,
            choices.Count > 2
                ? choices[2]
                : UpgradeManager.UpgradeType.MaxHealth
        );

        if (xpBar != null)
            xpBar.SetActive(false);

        upgradePanel.SetActive(true);

        transform.SetAsLastSibling();

        upgradePanel.transform.SetAsLastSibling();

        Canvas canvas = upgradePanel.GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            upgradePanel.transform.SetAsLastSibling();

            Debug.Log(
                "UpgradeUI: Canvas найден: " + canvas.name
            );
        }
        else
        {
            Debug.LogWarning(
                "UpgradeUI: Canvas не найден!"
            );
        }

        Debug.Log(
            "UpgradeUI: Panel Active = " +
            upgradePanel.activeSelf
        );
    }

    public void Hide()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        if (xpBar != null)
            xpBar.SetActive(true);
    }

    private void SetButton(
        Button button,
        UpgradeManager.UpgradeType upgradeType
    )
    {
        if (button == null)
            return;

        button.onClick.RemoveAllListeners();

        switch (upgradeType)
        {
            case UpgradeManager.UpgradeType.Damage:
                button.onClick.AddListener(ChooseDamage);
                SetButtonText(button, "DAMAGE +20%");
                break;

            case UpgradeManager.UpgradeType.FireRate:
                button.onClick.AddListener(ChooseFireRate);
                SetButtonText(button, "FIRE RATE +15%");
                break;

            case UpgradeManager.UpgradeType.MaxHealth:
                button.onClick.AddListener(ChooseMaxHealth);
                SetButtonText(button, "MAX HP +25");
                break;

            case UpgradeManager.UpgradeType.AttackRange:
                button.onClick.AddListener(ChooseAttackRange);
                SetButtonText(button, "ATTACK RANGE +5");
                break;

            case UpgradeManager.UpgradeType.FullHeal:
                button.onClick.AddListener(ChooseFullHeal);
                SetButtonText(button, "FULL HEAL");
                break;
        }
    }

    private void SetButtonText(
        Button button,
        string text
    )
    {
        TMP_Text buttonText =
            button.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
            buttonText.text = text;
    }

    private void ChooseDamage()
    {
        if (upgradeManager != null)
            upgradeManager.ApplyUpgrade(
                UpgradeManager.UpgradeType.Damage
            );
    }

    private void ChooseFireRate()
    {
        if (upgradeManager != null)
            upgradeManager.ApplyUpgrade(
                UpgradeManager.UpgradeType.FireRate
            );
    }

    private void ChooseMaxHealth()
    {
        if (upgradeManager != null)
            upgradeManager.ApplyUpgrade(
                UpgradeManager.UpgradeType.MaxHealth
            );
    }

    private void ChooseAttackRange()
    {
        if (upgradeManager != null)
            upgradeManager.ApplyUpgrade(
                UpgradeManager.UpgradeType.AttackRange
            );
    }

    private void ChooseFullHeal()
    {
        if (upgradeManager != null)
            upgradeManager.ApplyUpgrade(
                UpgradeManager.UpgradeType.FullHeal
            );
    }

}