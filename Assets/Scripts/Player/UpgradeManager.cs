using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public enum UpgradeType
    {
        Damage,
        FireRate,
        MaxHealth,
        AttackRange,
        FullHeal
    }

    [Header("References")]
    [SerializeField] private XPManager xpManager;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GunController gunController;
    [SerializeField] private UpgradeUI upgradeUI;

    [Header("Upgrade Values")]
    [SerializeField] private float damageMultiplier = 1.20f;
    [SerializeField] private float fireRateMultiplier = 1.15f;
    [SerializeField] private int maxHealthIncrease = 25;
    [SerializeField] private float attackRangeIncrease = 5f;

    private void Awake()
    {
        if (xpManager == null)
            xpManager = GetComponent<XPManager>();

        if (xpManager == null)
            xpManager = FindFirstObjectByType<XPManager>();

        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (gunController == null)
            gunController = GetComponent<GunController>();

        /*
         * PHASE 5B.1 (ISSUE-22):
         * GetComponent выше уже даёт канонический
         * GunController игрока. Fallback ниже больше не
         * ищет GunController по всей сцене: FindFirstObjectByType
         * при двух инстансах отдавал произвольный.
         * Идём от PlayerController — это детерминировано.
         */
        if (gunController == null)
        {
            var owner =
                FindFirstObjectByType<PlayerController>();

            if (owner != null)
                gunController =
                    owner.GetComponent<GunController>();
        }

        if (upgradeUI == null)
        {
            upgradeUI = FindFirstObjectByType<UpgradeUI>(
                FindObjectsInactive.Include
            );
        }
    }

    private void OnEnable()
    {
        if (xpManager != null)
        {
            xpManager.OnLevelUp += HandleLevelUp;
        }
        else
        {
            Debug.LogWarning(
                "UpgradeManager: XPManager �� ������."
            );
        }
    }

    private void OnDisable()
    {
        if (xpManager != null)
        {
            xpManager.OnLevelUp -= HandleLevelUp;
        }
    }

    private void HandleLevelUp()
    {
        Debug.Log(
            "LEVEL UP! ��������� ����� ���������."
        );

        if (upgradeUI == null)
        {
            upgradeUI = FindFirstObjectByType<UpgradeUI>(
                FindObjectsInactive.Include
            );
        }

        if (upgradeUI == null)
        {
            Debug.LogError(
                "UpgradeManager: UpgradeUI �� ������!"
            );

            return;
        }

        List<UpgradeType> choices = GetRandomUpgrades(3);

        Time.timeScale = 0f;

        upgradeUI.Show(
            this,
            choices
        );
    }

    private List<UpgradeType> GetRandomUpgrades(int count)
    {
        List<UpgradeType> availableUpgrades =
            new List<UpgradeType>
            {
            UpgradeType.Damage,
            UpgradeType.FireRate,
            UpgradeType.MaxHealth,
            UpgradeType.AttackRange,
            UpgradeType.FullHeal
            };

        List<UpgradeType> selectedUpgrades =
            new List<UpgradeType>();

        while (
            selectedUpgrades.Count < count &&
            availableUpgrades.Count > 0
        )
        {
            int randomIndex = Random.Range(
                0,
                availableUpgrades.Count
            );

            selectedUpgrades.Add(
                availableUpgrades[randomIndex]
            );

            availableUpgrades.RemoveAt(
                randomIndex
            );
        }

        return selectedUpgrades;
    }

    public void ApplyUpgrade(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.Damage:
                ApplyDamageUpgrade();
                break;

            case UpgradeType.FireRate:
                ApplyFireRateUpgrade();
                break;

            case UpgradeType.MaxHealth:
                ApplyMaxHealthUpgrade();
                break;

            case UpgradeType.AttackRange:
                ApplyAttackRangeUpgrade();
                break;

            case UpgradeType.FullHeal:
                ApplyFullHealUpgrade();
                break;
        }

        if (upgradeUI != null)
            upgradeUI.Hide();

        Time.timeScale = 1f;
    }

    private void ApplyDamageUpgrade()
    {
        if (gunController == null)
        {
            Debug.LogWarning(
                "UpgradeManager: GunController �� ������."
            );

            return;
        }

        Debug.Log("UPGRADE: Damage +20%");

        gunController.IncreaseDamage(
            damageMultiplier
        );
    }

    private void ApplyFireRateUpgrade()
    {
        if (gunController == null)
        {
            Debug.LogWarning(
                "UpgradeManager: GunController �� ������."
            );

            return;
        }

        Debug.Log("UPGRADE: Fire Rate +15%");

        gunController.IncreaseFireRate(
            fireRateMultiplier
        );
    }

    private void ApplyMaxHealthUpgrade()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning(
                "UpgradeManager: PlayerHealth �� ������."
            );

            return;
        }

        Debug.Log("UPGRADE: Max HP +25");

        playerHealth.IncreaseMaxHealth(
            maxHealthIncrease
        );
    }

    private void ApplyAttackRangeUpgrade()
    {
        if (gunController == null)
        {
            Debug.LogWarning(
                "UpgradeManager: GunController �� ������."
            );

            return;
        }

        Debug.Log("UPGRADE: Attack Range +5");

        gunController.IncreaseAttackRange(
            attackRangeIncrease
        );
    }

    private void ApplyFullHealUpgrade()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning(
                "UpgradeManager: PlayerHealth �� ������."
            );

            return;
        }

        Debug.Log("UPGRADE: Full Heal");

        playerHealth.RestoreFullHealth();
    }

}