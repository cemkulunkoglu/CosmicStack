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

    [Header("SFX")]
    public AudioClip fireSfx;
    public AudioClip[] fireSfxVariants;
    [Range(0f, 1f)] public float sfxVolume = 0.6f;
    public Vector2 pitchRange = new Vector2(0.98f, 1.02f);
    [Range(0f, 1f)] public float sfxSpatialBlend = 0f;

    Transform player;
    bool visible;
    AudioSource _audio;

    void Awake()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        player = p ? p.transform : null;

        if (!TryGetComponent(out _audio))
            _audio = gameObject.AddComponent<AudioSource>();

        _audio.playOnAwake = false;
        _audio.spatialBlend = sfxSpatialBlend;
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

        PlayFireSfx();

        ScheduleNext();
    }

    void PlayFireSfx()
    {
        if (_audio == null) return;

        AudioClip clip = null;
        if (fireSfxVariants != null && fireSfxVariants.Length > 0)
            clip = fireSfxVariants[Random.Range(0, fireSfxVariants.Length)];
        else
            clip = fireSfx;

        if (!clip) return;

        _audio.pitch = Random.Range(pitchRange.x, pitchRange.y);
        _audio.PlayOneShot(clip, sfxVolume);
    }
}
