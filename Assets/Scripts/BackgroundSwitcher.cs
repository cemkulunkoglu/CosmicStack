using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    [Header("Hedef Renderer (arkaplan)")]
    public SpriteRenderer targetRenderer;

    [Header("Seviye Arkaplanlarý (sýrasýyla Level1..Level5)")]
    public Sprite[] levelBackgrounds = new Sprite[5];

    [Tooltip("Test için: 0 = kapalý, 1..5 = zorla bu seviyeyi kullan")]
    public int debugLevelOverride = 0;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<SpriteRenderer>();

        // last_level yoksa 1
        int level = debugLevelOverride > 0 ? debugLevelOverride : PlayerPrefs.GetInt("last_level", 1);
        int idx = Mathf.Clamp(level - 1, 0, levelBackgrounds.Length - 1);

        if (targetRenderer != null && levelBackgrounds[idx] != null)
            targetRenderer.sprite = levelBackgrounds[idx];
        else
            Debug.LogWarning("BackgroundSwitcher: Sprite veya Renderer eksik!");
    }
}
