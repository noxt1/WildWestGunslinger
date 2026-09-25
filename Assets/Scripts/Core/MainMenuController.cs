using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;

    [Header("Game Scene")]
    [SerializeField] private string gameSceneName = "TestArena";

    private void Start()
    {
        ShowMainMenu();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.OpenFromMainMenu();
        }
        else
        {
            Debug.LogError(
                "MainMenuController: SettingsUIManager не найден!"
            );
        }
    }

    public void CloseSettings()
    {
        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.HideSettings();
        }

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        if (mainPanel != null)
        {
            mainPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}