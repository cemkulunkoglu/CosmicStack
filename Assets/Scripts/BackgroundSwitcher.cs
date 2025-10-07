using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    [Header("Hedef Renderer (arkaplan)")]
    public SpriteRenderer targetRenderer;

    [Header("Seviye Arkaplanlarý (Level1..LevelN)")]
    public Sprite[] levelBackgrounds = new Sprite[5];

    [Tooltip("Test: 0=kapalý, 1..N=bu seviyeyi zorla")]
    public int debugLevelOverride = 0;

    void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        int level = debugLevelOverride > 0
            ? debugLevelOverride
            : PlayerPrefs.GetInt("last_level", 1);

        ApplyLevel(level);
    }

    public void ApplyLevel(int level)
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<SpriteRenderer>();

        if (levelBackgrounds == null || levelBackgrounds.Length == 0)
        {
            Debug.LogWarning("BackgroundSwitcher: levelBackgrounds boþ!");
            return;
        }

        int idx = Mathf.Clamp(level - 1, 0, levelBackgrounds.Length - 1);
        var sprite = levelBackgrounds[idx];
        if (sprite == null)
        {
            Debug.LogWarning($"BackgroundSwitcher: Level {level} için sprite atanmadý.");
            return;
        }

        targetRenderer.sprite = sprite;

        var fit = GetComponent<FitToCamera2D>();
        if (fit) fit.Refit();
    }
}
