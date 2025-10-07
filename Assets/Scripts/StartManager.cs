using UnityEngine;
using TMPro;

public class StartManager : MonoBehaviour
{
    [Header("UI Referanslarý")]
    public GameObject tapToStartText;
    public GameObject levelText;

    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f;

        tapToStartText.SetActive(true);
        levelText.SetActive(true);
    }

    void Update()
    {
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;
        tapToStartText.SetActive(false);
    }
}
