using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("Difficulty")]
    [SerializeField] private TMP_Dropdown difficultyDropdown;

    [Header("Audio")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Graphics")]
    [SerializeField] private TMP_Dropdown graphicsDropdown;

    [Header("Controls")]
    [SerializeField] private Toggle vibrationToggle;

    private void Start()
    {
        LoadSettingsIntoUI();
    }

    public void LoadSettingsIntoUI()
    {
        if (difficultyDropdown != null)
        {
            difficultyDropdown.value =
                (int)GameSettings.CurrentDifficulty;

            difficultyDropdown.RefreshShownValue();
        }

        if (musicSlider != null)
        {
            musicSlider.value =
                GameSettings.MusicVolume;
        }

        if (sfxSlider != null)
        {
            sfxSlider.value =
                GameSettings.SFXVolume;
        }

        if (graphicsDropdown != null)
        {
            graphicsDropdown.value =
                GameSettings.GraphicsQuality;

            graphicsDropdown.RefreshShownValue();
        }

        if (vibrationToggle != null)
        {
            vibrationToggle.isOn =
                GameSettings.Vibration;
        }

        ApplyAudio();
        ApplyGraphics();
    }

    public void SetDifficulty(int value)
    {
        if (value < 0 || value > 3)
            return;

        GameSettings.CurrentDifficulty =
            (GameSettings.Difficulty)value;

        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.ApplyDifficulty();
        }
    }

    public void SetMusicVolume(float value)
    {
        GameSettings.MusicVolume =
            value;

        ApplyAudio();
    }

    public void SetSFXVolume(float value)
    {
        GameSettings.SFXVolume =
            value;
    }

    public void SetGraphicsQuality(int value)
    {
        GameSettings.GraphicsQuality =
            value;

        ApplyGraphics();
    }

    public void SetVibration(bool value)
    {
        GameSettings.Vibration =
            value;
    }

    public void ResetSettings()
    {
        GameSettings.ResetToDefaults();

        LoadSettingsIntoUI();
    }

    private void ApplyAudio()
    {
        AudioListener.volume =
            GameSettings.MusicVolume;
    }

    private void ApplyGraphics()
    {
        int quality =
            Mathf.Clamp(
                GameSettings.GraphicsQuality,
                0,
                QualitySettings.names.Length - 1
            );

        QualitySettings.SetQualityLevel(
            quality,
            true
        );
    }
}