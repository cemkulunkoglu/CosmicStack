using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BossController : MonoBehaviour
{
    [Header("Hareket")]
    public float moveSpeed = 1.5f;
    public float topPadding = 1.2f;
    public float sidePadding = 0.5f;

    float yFixed;
    float leftX, rightX;
    float dir = 1f;

    void OnEnable()
    {
        SnapInsideCamera();
    }

    void Update()
    {
        Vector3 p = transform.position;
        p.x += dir * moveSpeed * Time.deltaTime;

        if (p.x > rightX) { p.x = rightX; dir = -1f; }
        if (p.x < leftX) { p.x = leftX; dir = 1f; }

        p.y = yFixed;
        p.z = 0f;
        transform.position = p;
    }

    public void SnapInsideCamera()
    {
        var cam = Camera.main;
        if (!cam) return;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        var sr = GetComponent<SpriteRenderer>();
        float halfBossW = sr ? (sr.bounds.size.x * 0.5f) : 0.5f;

        leftX = -halfW + sidePadding + halfBossW;
        rightX = halfW - sidePadding - halfBossW;

        yFixed = halfH - topPadding;

        transform.position = new Vector3(0f, yFixed, 0f);
    }
}
