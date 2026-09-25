using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private RawImage fill;
    [SerializeField] private RawImage background;
    [SerializeField] private EnemyHealth enemyHealth;

    private RectTransform fillRect;
    private RectTransform backgroundRect;

    private void Awake()
    {
        if (fill == null)
        {
            Transform fillTransform = transform.Find("HPFill");

            if (fillTransform != null)
            {
                fill = fillTransform.GetComponent<RawImage>();
            }
        }

        if (background == null)
        {
            Transform backgroundTransform = transform.Find("HPBackground");

            if (backgroundTransform != null)
            {
                background = backgroundTransform.GetComponent<RawImage>();
            }
        }

        if (enemyHealth == null)
        {
            enemyHealth = GetComponentInParent<EnemyHealth>();
        }

        if (fill != null)
        {
            fillRect = fill.rectTransform;
            fill.texture = Texture2D.whiteTexture;
        }

        if (background != null)
        {
            backgroundRect = background.rectTransform;
            background.texture = Texture2D.whiteTexture;
        }
    }

    private void OnEnable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged += UpdateBar;
        }
    }

    private void Start()
    {
        if (enemyHealth != null)
        {
            UpdateBar(
                enemyHealth.CurrentHealth,
                enemyHealth.MaxHealth
            );
        }
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged -= UpdateBar;
        }
    }

    private void UpdateBar(int currentHealth, int maxHealth)
    {
        if (fill == null || maxHealth <= 0)
            return;

        float percent = (float)currentHealth / maxHealth;

        Vector2 backgroundSize = backgroundRect.sizeDelta;

        fillRect.sizeDelta = new Vector2(
            backgroundSize.x * percent,
            backgroundSize.y
        );

        fillRect.anchoredPosition = new Vector2(
            -backgroundSize.x * (1f - percent) * 0.5f,
            0f
        );

        fill.color = Color.green;

        if (background != null)
        {
            background.color = Color.black;
        }
    }
}