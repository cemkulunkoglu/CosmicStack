using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Atamalar")]
    public GameObject gameOverPanel;

    bool shown;

    void Awake()
    {
        Time.timeScale = 1f;
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (shown) return;
        shown = true;

        if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("last_level", 1);
        PlayerPrefs.Save();

        var current = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(current.buildIndex);
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
