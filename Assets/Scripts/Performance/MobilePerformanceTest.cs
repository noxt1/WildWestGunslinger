using UnityEngine;

public class MobilePerformanceTest : MonoBehaviour
{
    private float currentFPS;
    private float fpsTimer;
    private int frames;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        frames++;

        fpsTimer += Time.unscaledDeltaTime;

        if (fpsTimer >= 0.5f)
        {
            currentFPS =
                frames /
                fpsTimer;

            frames = 0;
            fpsTimer = 0f;
        }
    }

    private void OnGUI()
    {
        GUIStyle style =
            new GUIStyle();

        style.fontSize = 40;
        style.normal.textColor =
            Color.white;

        GUI.Label(
            new Rect(
                20,
                20,
                300,
                70
            ),
            $"FPS: {currentFPS:F1}",
            style
        );
    }
}