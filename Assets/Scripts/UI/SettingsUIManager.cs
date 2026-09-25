using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsUIManager : MonoBehaviour
{
    public static SettingsUIManager Instance { get; private set; }


[Header("Settings Canvas")]
    [SerializeField] private GameObject settingsCanvas;

    private bool openedFromPause;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Удаляем дубликат Canvas вместе с новым менеджером.
            if (settingsCanvas != null)
            {
                Destroy(settingsCanvas);
            }

            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Менеджер остаётся между сценами.
        DontDestroyOnLoad(gameObject);

        // Единственный SettingsCanvas тоже остаётся между сценами.
        if (settingsCanvas != null)
        {
            DontDestroyOnLoad(settingsCanvas);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        HideSettings();

        Debug.Log(
            "SettingsUIManager: READY"
        );
    }

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    )
    {
        // При каждом переходе между сценами
        // настройки должны быть скрыты.
        HideSettings();

        openedFromPause = false;

        Debug.Log(
            "SettingsUIManager: Scene loaded -> " +
            scene.name
        );
    }

    public void OpenFromMainMenu()
    {
        openedFromPause = false;

        OpenSettings();

        Debug.Log(
            "SettingsUIManager: OPEN FROM MAIN MENU"
        );
    }

    public void OpenFromPause()
    {
        openedFromPause = true;

        OpenSettings();

        Debug.Log(
            "SettingsUIManager: OPEN FROM PAUSE"
        );
    }

    private void OpenSettings()
    {
        if (settingsCanvas == null)
        {
            Debug.LogError(
                "SettingsUIManager: SettingsCanvas НЕ НАЗНАЧЕН!"
            );

            return;
        }

        settingsCanvas.SetActive(true);

        Canvas canvas =
            settingsCanvas.GetComponent<Canvas>();

        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
        }

        Transform settingsPanel =
            settingsCanvas.transform.Find(
                "SettingsPanel"
            );

        if (settingsPanel != null)
        {
            settingsPanel.gameObject.SetActive(true);
        }

        Debug.Log(
            "SettingsUIManager: SETTINGS OPEN"
        );
    }

    public void Back()
    {
        HideSettings();

        if (openedFromPause)
        {
            if (PauseMenuController.Instance != null)
            {
                PauseMenuController.Instance.ShowPausePanel();
            }
        }

        openedFromPause = false;

        Debug.Log(
            "SettingsUIManager: BACK"
        );
    }

    public void HideSettings()
    {
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
        }
    }

    public bool IsOpen()
    {
        return settingsCanvas != null &&
               settingsCanvas.activeSelf;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }


}
