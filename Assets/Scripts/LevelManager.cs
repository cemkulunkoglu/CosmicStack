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

    int currentLevel;

    void Awake()
    {
        if (!levelTimer) levelTimer = FindFirstObjectByType<LevelTimer>();
        if (levelTimer) levelTimer.onTimerFinished.AddListener(OnLevelFinished);
        else Debug.LogError("LevelManager: LevelTimer atanmadı!");

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
        if (spawner) spawner.StopSpawning();

        currentLevel = Mathf.Min(currentLevel + 1, maxLevels);
        PlayerPrefs.SetInt("last_level", currentLevel);
        PlayerPrefs.Save();

        SetupLevel(currentLevel);

        StartCoroutine(OpenTapNextFrame());
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
    }

    public void OnTimerFinished()
    {
        if (spawner) spawner.StopSpawning();
        ShowTap();
    }

}
