using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIController : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;

    private void Awake()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    public void ShowDeathScreen()
    {
        Time.timeScale = 0f;

        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}