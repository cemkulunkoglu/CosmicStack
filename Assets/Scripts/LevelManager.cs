using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Bağlantılar")]
    public LevelTimer levelTimer;
    public PlayerHealth playerHealth;
    public Spawner spawner;
    public BackgroundSwitcher background;
    public TMP_Text levelLabel;
    public GameObject tapPanel;
    public TMP_Text tapText;

    [Header("Seviye Ayarları")]
    public int maxLevels = 5;
    public float[] levelDurations = { 10, 10, 10, 10, 10 };
    public GameObject[] level1Enemies, level2Enemies, level3Enemies, level4Enemies, level5Enemies;
    public float[] spawnIntervals = { 1.5f, 1.3f, 1.1f, 0.9f, 0.8f };

    [Header("Level Completed UI")]
    public GameObject completePanel;
    public TMP_Text completeText;
    public bool clickAnywhereToContinue = true;

    int currentLevel;
    bool waitingContinue = false;

    void Awake()
    {
        if (!levelTimer) levelTimer = FindFirstObjectByType<LevelTimer>();
        if (levelTimer) levelTimer.onTimerFinished.AddListener(OnLevelFinished);
        else Debug.LogError("LevelManager: LevelTimer atanmadı!");

        if (completePanel) completePanel.SetActive(false);

        currentLevel = Mathf.Clamp(PlayerPrefs.GetInt("last_level", 1), 1, maxLevels);
        SetupLevel(currentLevel);
        ShowTap();
    }

    void SetupLevel(int level)
    {
        if (levelLabel) levelLabel.text = $"LEVEL {level}";
        if (background) background.ApplyLevel(level);

        if (levelTimer)
        {
            float dur = level <= levelDurations.Length ? levelDurations[level - 1] : levelDurations[^1];
            levelTimer.SetDuration(dur);
            levelTimer.PauseTimer();
        }

        if (playerHealth) playerHealth.ResetHealth();

        if (spawner)
        {
            spawner.StopSpawning();
            spawner.SetSpawnList(GetEnemiesForLevel(level),
                level <= spawnIntervals.Length ? spawnIntervals[level - 1] : spawnIntervals[^1]);
        }

        ClearEnemiesInScene();

        var go = FindFirstObjectByType<GameOverUI>();
        if (go && go.gameOverPanel && go.gameOverPanel.activeSelf) go.gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    GameObject[] GetEnemiesForLevel(int level) => level switch
    {
        1 => level1Enemies,
        2 => level2Enemies,
        3 => level3Enemies,
        4 => level4Enemies,
        5 => level5Enemies,
        _ => level5Enemies
    };

    void ShowTap()
    {
        if (!tapPanel) return;

        tapPanel.SetActive(true);
        tapPanel.transform.SetAsLastSibling();

        var cg = tapPanel.GetComponent<CanvasGroup>();
        if (cg) { cg.alpha = 1f; cg.blocksRaycasts = true; cg.interactable = true; }

        var startBtn = tapPanel.GetComponentInChildren<Button>(true);
        if (startBtn)
        {
            startBtn.onClick.RemoveAllListeners();
            startBtn.onClick.AddListener(StartLevel);
        }

        if (tapText) tapText.enabled = true;

        if (levelTimer) levelTimer.PauseTimer();
        Time.timeScale = 1f;
    }

    public void StartLevel()
    {
        if (tapPanel) tapPanel.SetActive(false);
        if (tapText) tapText.enabled = false;

        if (levelTimer) { levelTimer.ResetTimer(); levelTimer.StartTimer(); }
        if (spawner) spawner.StartSpawning(1f);
    }

    public void OnLevelFinished()
    {
        ShowComplete();
    }

    void ShowComplete()
    {
        if (spawner) spawner.StopSpawning();
        if (levelTimer) levelTimer.PauseTimer();

        if (completePanel)
        {
            completePanel.SetActive(true);
            completePanel.transform.SetAsLastSibling();

            var btn = completePanel.GetComponentInChildren<Button>(true);
            if (btn)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(NextLevel);
            }
        }

        if (completeText)
            completeText.text = (currentLevel >= maxLevels)
                ? "ALL LEVELS COMPLETED!"
                : $"LEVEL {currentLevel} COMPLETED";

        waitingContinue = true;
    }

    public void NextLevel()
    {
        if (!waitingContinue) return;
        waitingContinue = false;

        if (completePanel) completePanel.SetActive(false);

        if (currentLevel >= maxLevels)
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("last_level", currentLevel);
            PlayerPrefs.Save();

            SetupLevel(currentLevel);
            ShowTap();
            return;
        }

        currentLevel = Mathf.Min(currentLevel + 1, maxLevels);
        PlayerPrefs.SetInt("last_level", currentLevel);
        PlayerPrefs.Save();

        SetupLevel(currentLevel);
        ShowTap();
    }

    IEnumerator OpenTapNextFrame() { yield return null; ShowTap(); }

    void ClearEnemiesInScene()
    {
        foreach (var e in FindObjectsOfType<FallingObject>())
            Destroy(e.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) ShowTap();
        if (Input.GetKeyDown(KeyCode.Y)) StartLevel();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("last_level", 1);
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
        }

        if (waitingContinue && clickAnywhereToContinue &&
            (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            NextLevel();
        }
    }

    public void OnTimerFinished()
    {
        OnLevelFinished();
    }
}
