using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Atamalar")]
    public GameObject gameOverPanel;

    [Header("SFX")]
    public AudioClip gameOverSfx;
    [Range(0f, 1f)] public float gameOverVolume = 0.9f;

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

        if (gameOverSfx)
        {
#if UNITY_EDITOR
            AudioOneShot.Play2D(gameOverSfx, gameOverVolume);
#else
            AudioSource.PlayClipAtPoint(
                gameOverSfx,
                Camera.main ? Camera.main.transform.position : Vector3.zero,
                gameOverVolume
            );
#endif
        }

        if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("last_level", 1);
        PlayerPrefs.Save();

        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
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
