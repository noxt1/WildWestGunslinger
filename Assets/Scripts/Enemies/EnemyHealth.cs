using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("XP")]
    [SerializeField] private GameObject xpPrefab;
    [SerializeField] private int xpReward = 1;

    [Header("Health Bar")]
    [SerializeField] private bool showHealthBar = true;
    [SerializeField] private float healthBarHeight = 2f;
    [SerializeField] private float healthBarWidth = 1.5f;
    [SerializeField] private float healthBarHeightUI = 0.12f;
    [SerializeField] private float healthBarScale = 0.01f;

    [Header("Wave Scaling")]
    [SerializeField] private float hpGrowthPerWave = 0.08f;
    [SerializeField] private float maxHpMultiplier = 3f;

    [Header("Elite")]
    [SerializeField] private float eliteHealthMultiplier = 2f;
    [SerializeField] private int eliteXpMultiplier = 3;

    private int baseMaxHealth;
    private int baseXpReward;
    private int currentHealth;

    private bool isElite;

    private GameObject healthBarObject;
    private RectTransform fillRect;

    private Camera mainCamera;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsElite => isElite;

    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        baseMaxHealth = maxHealth;
        baseXpReward = xpReward;

        currentHealth = maxHealth;

        mainCamera = Camera.main;

        if (showHealthBar)
        {
            CreateHealthBar();
        }
    }

    private void Start()
    {
        UpdateHealthBar();

        OnHealthChanged?.Invoke(
            currentHealth,
            maxHealth
        );
    }

    public void SetElite(bool elite)
    {
        isElite = elite;

        if (!isElite)
        {
            maxHealth = baseMaxHealth;
            xpReward = baseXpReward;
            currentHealth = maxHealth;

            UpdateHealthBar();

            OnHealthChanged?.Invoke(
                currentHealth,
                maxHealth
            );

            return;
        }

        maxHealth =
            Mathf.RoundToInt(
                baseMaxHealth *
                eliteHealthMultiplier
            );

        xpReward =
            baseXpReward *
            eliteXpMultiplier;

        currentHealth =
            maxHealth;

        UpdateHealthBar();

        OnHealthChanged?.Invoke(
            currentHealth,
            maxHealth
        );

        Debug.Log(
            $"{gameObject.name}: ELITE. " +
            $"HP = {maxHealth}, XP = {xpReward}"
        );
    }

    public void ApplyWaveScaling(int wave)
    {
        if (wave < 1)
        {
            wave = 1;
        }

        float multiplier =
            1f +
            (wave - 1) *
            hpGrowthPerWave;

        multiplier =
            Mathf.Min(
                multiplier,
                maxHpMultiplier
            );

        maxHealth =
            Mathf.RoundToInt(
                baseMaxHealth *
                multiplier
            );

        if (isElite)
        {
            maxHealth =
                Mathf.RoundToInt(
                    maxHealth *
                    eliteHealthMultiplier
                );

            xpReward =
                baseXpReward *
                eliteXpMultiplier;
        }
        else
        {
            xpReward =
                baseXpReward;
        }

        currentHealth =
            maxHealth;

        UpdateHealthBar();

        OnHealthChanged?.Invoke(
            currentHealth,
            maxHealth
        );

        Debug.Log(
            $"{gameObject.name}: Wave {wave} HP = {maxHealth}"
        );
    }

    private void LateUpdate()
    {
        if (healthBarObject == null)
        {
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return;
            }
        }

        healthBarObject.transform.position =
            transform.position +
            Vector3.up *
            healthBarHeight;

        Vector3 direction =
            healthBarObject.transform.position -
            mainCamera.transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            healthBarObject.transform.rotation =
                Quaternion.LookRotation(
                    direction
                );
        }
    }

    private void CreateHealthBar()
    {
        healthBarObject =
            new GameObject(
                "EnemyHealthBar"
            );

        Canvas canvas =
            healthBarObject.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.WorldSpace;

        canvas.sortingOrder =
            100;

        RectTransform canvasRect =
            healthBarObject.GetComponent<RectTransform>();

        canvasRect.sizeDelta =
            new Vector2(
                healthBarWidth,
                healthBarHeightUI
            );

        canvasRect.localScale =
            Vector3.one *
            healthBarScale;

        GameObject backgroundObject =
            new GameObject(
                "Background"
            );

        backgroundObject.transform.SetParent(
            healthBarObject.transform
        );

        Image background =
            backgroundObject.AddComponent<Image>();

        background.color =
            new Color(
                0f,
                0f,
                0f,
                0.9f
            );

        RectTransform backgroundRect =
            backgroundObject.GetComponent<RectTransform>();

        backgroundRect.anchorMin =
            Vector2.zero;

        backgroundRect.anchorMax =
            Vector2.one;

        backgroundRect.offsetMin =
            Vector2.zero;

        backgroundRect.offsetMax =
            Vector2.zero;

        GameObject fillObject =
            new GameObject(
                "Fill"
            );

        fillObject.transform.SetParent(
            backgroundObject.transform
        );

        Image fill =
            fillObject.AddComponent<Image>();

        fill.color =
            isElite
                ? new Color(
                    1f,
                    0.65f,
                    0.05f,
                    1f
                )
                : new Color(
                    0.9f,
                    0.1f,
                    0.1f,
                    1f
                );

        fillRect =
            fillObject.GetComponent<RectTransform>();

        fillRect.anchorMin =
            new Vector2(
                0f,
                0f
            );

        fillRect.anchorMax =
            new Vector2(
                0f,
                1f
            );

        fillRect.pivot =
            new Vector2(
                0f,
                0.5f
            );

        fillRect.anchoredPosition =
            Vector2.zero;

        fillRect.sizeDelta =
            new Vector2(
                healthBarWidth,
                0f
            );
    }

    private void UpdateHealthBar()
    {
        if (fillRect == null)
        {
            return;
        }

        float healthPercent =
            maxHealth > 0
                ? (float)currentHealth /
                  maxHealth
                : 0f;

        healthPercent =
            Mathf.Clamp01(
                healthPercent
            );

        fillRect.sizeDelta =
            new Vector2(
                healthBarWidth *
                healthPercent,
                0f
            );
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0,
                maxHealth
            );

        Debug.Log(
            $"{gameObject.name}: получил {damage} урона. " +
            $"HP = {currentHealth}/{maxHealth}"
        );

        UpdateHealthBar();

        OnHealthChanged?.Invoke(
            currentHealth,
            maxHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SpawnXP();

        if (healthBarObject != null)
        {
            Destroy(
                healthBarObject
            );
        }

        Destroy(
            gameObject
        );
    }

    private void SpawnXP()
    {
        if (xpPrefab == null)
        {
            Debug.LogWarning(
                "XP Prefab не назначен в EnemyHealth."
            );

            return;
        }

        GameObject xp =
            Instantiate(
                xpPrefab,
                transform.position,
                Quaternion.identity
            );

        XPOrb xpOrb =
            xp.GetComponent<XPOrb>();

        if (xpOrb != null)
        {
            xpOrb.SetXP(
                xpReward
            );
        }
    }
}