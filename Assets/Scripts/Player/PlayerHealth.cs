using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Death UI")]
    [SerializeField] private DeathUIController deathUI;

    private int currentHealth;
    private bool isDead;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;

        Time.timeScale = 1f;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        if (damage <= 0)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log(
            $"Player получил {damage} урона. " +
            $"HP: {currentHealth}/{maxHealth}"
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        if (amount <= 0)
            return;

        maxHealth += amount;
        currentHealth += amount;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        Debug.Log(
            $"Max HP increased! HP: {currentHealth}/{maxHealth}"
        );
    }

    public void RestoreFullHealth()
    {
        if (isDead)
            return;

        currentHealth = maxHealth;

        Debug.Log(
            $"Full Heal! HP: {currentHealth}/{maxHealth}"
        );
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("PLAYER DIED");

        if (deathUI != null)
        {
            deathUI.ShowDeathScreen();
        }
        else
        {
            Debug.LogWarning(
                "DeathUIController не подключён к PlayerHealth."
            );

            Time.timeScale = 0f;
        }
    }

}