using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LevelTimer : MonoBehaviour
{
    [Header("Timer")]
    public float levelDuration = 10f;
    public bool autoStart = true;

    [Header("UI")]
    public Slider progress;
    public TMP_Text timeText;

    [Header("Events")]
    public UnityEvent onTimerStart;
    public UnityEvent onTimerFinished;

    float timeRemaining;
    bool running;

    void Awake()
    {
        if (progress != null)
        {
            progress.minValue = 0f;
            progress.wholeNumbers = true;
            progress.maxValue = Mathf.CeilToInt(levelDuration);
            progress.interactable = false;
        }
        ResetTimer();
        if (autoStart) StartTimer();
    }

    public void ResetTimer()
    {
        timeRemaining = levelDuration;
        if (progress != null)
            progress.maxValue = Mathf.CeilToInt(levelDuration);
        running = false;
        UpdateUI();
    }


    public void StartTimer()
    {
        if (levelDuration <= 0f) levelDuration = 1f;
        running = true;
        onTimerStart?.Invoke();
    }

    public void PauseTimer() { running = false; }
    public void ResumeTimer() { running = true; }
    public void SetDuration(float seconds) { levelDuration = seconds; ResetTimer(); }

    void Update()
    {
        if (!running) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            running = false;
            UpdateUI();
            onTimerFinished?.Invoke();
            return;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (progress != null)
            progress.value = Mathf.CeilToInt(timeRemaining);

        if (timeText != null)
            timeText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }

}
