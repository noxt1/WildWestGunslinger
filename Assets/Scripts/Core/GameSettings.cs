using UnityEngine;

public static class GameSettings
{
    public enum Difficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2,
        Nightmare = 3
    }

    private const string DifficultyKey = "Difficulty";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";
    private const string GraphicsKey = "GraphicsQuality";
    private const string VibrationKey = "Vibration";

    public static Difficulty CurrentDifficulty
    {
        get
        {
            return (Difficulty)PlayerPrefs.GetInt(
                DifficultyKey,
                (int)Difficulty.Normal
            );
        }
        set
        {
            PlayerPrefs.SetInt(
                DifficultyKey,
                (int)value
            );

            PlayerPrefs.Save();
        }
    }

    public static float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(
                MusicKey,
                1f
            );
        }
        set
        {
            PlayerPrefs.SetFloat(
                MusicKey,
                Mathf.Clamp01(value)
            );

            PlayerPrefs.Save();
        }
    }

    public static float SFXVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(
                SFXKey,
                1f
            );
        }
        set
        {
            PlayerPrefs.SetFloat(
                SFXKey,
                Mathf.Clamp01(value)
            );

            PlayerPrefs.Save();
        }
    }

    public static int GraphicsQuality
    {
        get
        {
            return PlayerPrefs.GetInt(
                GraphicsKey,
                1
            );
        }
        set
        {
            PlayerPrefs.SetInt(
                GraphicsKey,
                value
            );

            PlayerPrefs.Save();
        }
    }

    public static bool Vibration
    {
        get
        {
            return PlayerPrefs.GetInt(
                VibrationKey,
                1
            ) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(
                VibrationKey,
                value ? 1 : 0
            );

            PlayerPrefs.Save();
        }
    }

    public static void ResetToDefaults()
    {
        CurrentDifficulty =
            Difficulty.Normal;

        MusicVolume = 1f;
        SFXVolume = 1f;
        GraphicsQuality = 1;
        Vibration = true;
    }
}