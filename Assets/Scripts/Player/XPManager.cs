using UnityEngine;
using System;

public class XPManager : MonoBehaviour
{
    [Header("XP")]
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int requiredXP = 10;

    [Header("Level")]
    [SerializeField] private int currentLevel = 1;

    public int CurrentXP => currentXP;
    public int RequiredXP => requiredXP;
    public int CurrentLevel => currentLevel;

    public event Action OnLevelUp;

    public void AddXP(int amount)
    {
        if (amount <= 0)
            return;

        currentXP += amount;

        Debug.Log(
            $"Player получил {amount} XP. " +
            $"XP: {currentXP}/{requiredXP}"
        );

        while (currentXP >= requiredXP)
        {
            currentXP -= requiredXP;

            LevelUp();

            requiredXP =
                Mathf.RoundToInt(
                    requiredXP * 1.35f
                );
        }
    }

    private void LevelUp()
    {
        currentLevel++;

        Debug.Log(
            $"LEVEL UP! Новый уровень: {currentLevel}"
        );

        OnLevelUp?.Invoke();
    }
}