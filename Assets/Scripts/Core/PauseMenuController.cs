using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance { get; private set; }

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;

    private bool isPaused;

    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.HideSettings();
        }
    }

    public void OpenSettings()
    {
        if (!isPaused)
        {
            PauseGame();
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.OpenFromPause();
        }
        else
        {
            Debug.LogError(
                "PauseMenuController: SettingsUIManager не найден!"
            );
        }
    }

    public void CloseSettings()
    {
        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.HideSettings();
        }

        if (isPaused && pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void ShowPausePanel()
    {
        if (isPaused && pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        isPaused = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        isPaused = false;

        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.HideSettings();
        }

        SceneManager.LoadScene(
            "MainMenu"
        );
    }
}