using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    public float EnemyHealthMultiplier { get; private set; }
    public float EnemyDamageMultiplier { get; private set; }
    public float EnemySpeedMultiplier { get; private set; }
    public float SpawnCountMultiplier { get; private set; }
    public float XPMultiplier { get; private set; }
    public float EliteChanceMultiplier { get; private set; }

    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        ApplyDifficulty();
    }

    public void ApplyDifficulty()
    {
        switch (GameSettings.CurrentDifficulty)
        {
            case GameSettings.Difficulty.Easy:

                EnemyHealthMultiplier = 0.75f;
                EnemyDamageMultiplier = 0.75f;
                EnemySpeedMultiplier = 0.90f;
                SpawnCountMultiplier = 0.75f;
                XPMultiplier = 1.10f;
                EliteChanceMultiplier = 0.50f;

                break;

            case GameSettings.Difficulty.Normal:

                EnemyHealthMultiplier = 1.00f;
                EnemyDamageMultiplier = 1.00f;
                EnemySpeedMultiplier = 1.00f;
                SpawnCountMultiplier = 1.00f;
                XPMultiplier = 1.00f;
                EliteChanceMultiplier = 1.00f;

                break;

            case GameSettings.Difficulty.Hard:

                EnemyHealthMultiplier = 1.40f;
                EnemyDamageMultiplier = 1.30f;
                EnemySpeedMultiplier = 1.15f;
                SpawnCountMultiplier = 1.25f;
                XPMultiplier = 0.90f;
                EliteChanceMultiplier = 1.50f;

                break;

            case GameSettings.Difficulty.Nightmare:

                EnemyHealthMultiplier = 2.00f;
                EnemyDamageMultiplier = 1.70f;
                EnemySpeedMultiplier = 1.35f;
                SpawnCountMultiplier = 1.50f;
                XPMultiplier = 0.80f;
                EliteChanceMultiplier = 2.00f;

                break;
        }
    }

    public string GetDifficultyName()
    {
        switch (GameSettings.CurrentDifficulty)
        {
            case GameSettings.Difficulty.Easy:
                return "Easy";

            case GameSettings.Difficulty.Normal:
                return "Normal";

            case GameSettings.Difficulty.Hard:
                return "Hard";

            case GameSettings.Difficulty.Nightmare:
                return "Nightmare";
        }

        return "Normal";
    }
}