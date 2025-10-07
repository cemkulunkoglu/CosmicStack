using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Ateş Ayarları")]
    public EnemyBullet bulletPrefab;
    public Transform firePoint;
    public float rateMin = 1.2f;
    public float rateMax = 2.0f;
    public float inaccuracyDeg = 6f;
    public bool aimAtPlayer = true;

    Transform player;
    bool visible;

    void Awake()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        player = p ? p.transform : null;
    }

    void OnEnable() => ScheduleNext();
    void OnDisable() => CancelInvoke();

    void OnBecameVisible() => visible = true;
    void OnBecameInvisible() => visible = false;

    void ScheduleNext()
    {
        CancelInvoke(nameof(FireOnce));
        Invoke(nameof(FireOnce), Random.Range(rateMin, rateMax));
    }

    void FireOnce()
    {
        if (!isActiveAndEnabled) return;
        if (!visible) { ScheduleNext(); return; }

        Vector3 pos = firePoint ? firePoint.position : transform.position + Vector3.down * 0.2f;

        Vector2 dir;
        if (aimAtPlayer && player)
            dir = ((Vector2)player.position - (Vector2)pos).normalized;
        else
            dir = Vector2.down;

        float a = Random.Range(-inaccuracyDeg, inaccuracyDeg);
        dir = Quaternion.Euler(0, 0, a) * dir;

        var b = Instantiate(bulletPrefab, pos, Quaternion.identity);
        b.Fire(dir);

        ScheduleNext();
    }
}
