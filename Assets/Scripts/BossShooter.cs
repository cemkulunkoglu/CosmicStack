using UnityEngine;

public class BossShooter : MonoBehaviour
{
    [Header("Ateş")]
    public EnemyBullet bulletPrefab;
    public Transform[] firePoints = new Transform[3];
    public bool aimAtPlayer = true;
    [Range(0f, 12f)] public float inaccuracyDeg = 4f;

    [Header("Sıklık")]
    public float fireInterval = 1.2f;
    public Vector2 randomJitter = new Vector2(-0.2f, 0.3f);

    [Header("SFX (opsiyonel)")]
    public AudioClip shootSfx;
    [Range(0, 1)] public float shootVolume = 0.8f;

    Transform player;

    void Awake()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        player = p ? p.transform : null;
    }

    void OnEnable() => ScheduleNext();
    void OnDisable() => CancelInvoke();

    void ScheduleNext()
    {
        CancelInvoke(nameof(FireBurst));
        float t = fireInterval + Random.Range(randomJitter.x, randomJitter.y);
        t = Mathf.Max(0.1f, t);
        Invoke(nameof(FireBurst), t);
    }

    void FireBurst()
    {
        if (!isActiveAndEnabled) return;

        foreach (var fp in firePoints)
        {
            if (!fp || !bulletPrefab) continue;

            Vector2 dir = Vector2.down;
            if (aimAtPlayer && player)
                dir = ((Vector2)player.position - (Vector2)fp.position).normalized;

            float a = Random.Range(-inaccuracyDeg, inaccuracyDeg);
            dir = Quaternion.Euler(0, 0, a) * dir;

            var b = Instantiate(bulletPrefab, fp.position, Quaternion.identity);
            b.Fire(dir);
        }

        if (shootSfx)
            AudioOneShot.Play2D(shootSfx, shootVolume);

        ScheduleNext();
    }
}
