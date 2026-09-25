using UnityEngine;
using TMPro;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Wave Settings")]
    [SerializeField] private int startingEnemies = 3;
    [SerializeField] private int enemiesIncreasePerWave = 2;
    [SerializeField] private float timeBetweenWaves = 3f;

    [Header("UI")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text waveCompleteText;

    private int currentWave = 0;
    private bool waveActive = false;
    private bool waitingForNextWave = false;

    private void Start()
    {
        if (waveCompleteText != null)
        {
            waveCompleteText.gameObject.SetActive(false);
        }

        if (enemySpawner == null)
        {
            enemySpawner = FindFirstObjectByType<EnemySpawner>();
        }

        StartCoroutine(StartFirstWaveNextFrame());
    }

    private IEnumerator StartFirstWaveNextFrame()
    {
        yield return null;

        if (ArenaTacticalMap.Instance != null &&
            !ArenaTacticalMap.Instance.IsBuilt)
        {
            ArenaTacticalMap.Instance.Build();
        }

        StartNextWave();
    }

    private void Update()
    {
        if (!waveActive)
            return;

        if (waitingForNextWave)
            return;

        if (enemySpawner == null)
        {
            enemySpawner = FindFirstObjectByType<EnemySpawner>();

            if (enemySpawner == null)
                return;
        }

        int aliveEnemies =
            enemySpawner.GetLiveEnemyCount();

        if (aliveEnemies <= 0)
        {
            StartCoroutine(NextWaveDelay());
        }
    }

    private void StartNextWave()
    {
        currentWave++;

        int enemyCount =
            startingEnemies +
            (currentWave - 1) * enemiesIncreasePerWave;

        waveActive = true;
        waitingForNextWave = false;

        UpdateWaveUI();

        Debug.Log(
            $"WAVE {currentWave} STARTED. " +
            $"Enemies: {enemyCount}"
        );

        if (enemySpawner != null)
        {
            enemySpawner.SetCurrentWave(currentWave);
            enemySpawner.SpawnEnemies(enemyCount);

            Debug.Log(
                $"WAVE {currentWave} SPAWNED. " +
                $"LIVE ENEMIES: " +
                enemySpawner.GetLiveEnemyCount()
            );
        }
        else
        {
            Debug.LogError(
                "EnemySpawner не назначен в WaveManager."
            );

            waveActive = false;
        }
    }

    private IEnumerator NextWaveDelay()
    {
        if (waitingForNextWave)
            yield break;

        waitingForNextWave = true;
        waveActive = false;

        Debug.Log(
            $"WAVE {currentWave} COMPLETE"
        );

        ShowWaveComplete();

        yield return new WaitForSecondsRealtime(
            timeBetweenWaves
        );

        HideWaveComplete();

        StartNextWave();
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text =
                $"WAVE {currentWave}";
        }
    }

    private void ShowWaveComplete()
    {
        if (waveCompleteText != null)
        {
            waveCompleteText.gameObject.SetActive(true);
        }
    }

    private void HideWaveComplete()
    {
        if (waveCompleteText != null)
        {
            waveCompleteText.gameObject.SetActive(false);
        }
    }
}