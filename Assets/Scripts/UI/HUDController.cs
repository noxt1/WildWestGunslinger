using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;

    [Header("XP")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TMP_Text levelText;

    private PlayerHealth playerHealth;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        UpdateHealth();
    }

    private void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        if (playerHealth == null)
            return;

        int currentHealth = playerHealth.CurrentHealth;
        int maxHealth = playerHealth.MaxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}