using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitToCamera2D : MonoBehaviour
{
    public Camera cam;
    [Tooltip("Ekraný tamamen doldur (cover) ya da tam sýðdýr (contain)")]
    public bool cover = true;
    [Tooltip("Z dönüþü 90°/270° ise en-boyu takas et")]
    public bool handleRightAngleRotation = true;

    void Start() { FitNow(); }
#if UNITY_EDITOR
    void OnValidate() { FitNow(); }
#endif

    void FitNow()
    {
        if (!isActiveAndEnabled) return;
        if (cam == null) cam = Camera.main;

        var sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null || cam == null) return;

        float worldH = cam.orthographicSize * 2f;
        float worldW = worldH * cam.aspect;

        Vector2 size = sr.sprite.bounds.size;

        float z = Mathf.Abs(transform.eulerAngles.z % 180f);
        if (handleRightAngleRotation && Mathf.Abs(z - 90f) < 0.01f)
        {
            float tmp = size.x; size.x = size.y; size.y = tmp;
        }

        float sx = worldW / size.x;
        float sy = worldH / size.y;
        float s = cover ? Mathf.Max(sx, sy) : Mathf.Min(sx, sy);

        transform.localScale = new Vector3(s, s, 1f);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
    }

    public void Refit()
    {
        FitNow();
    }
}
