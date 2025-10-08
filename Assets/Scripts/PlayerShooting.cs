using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float yOffset = 0.5f;
    public float bulletSpeed = 12f;
    public float bulletLife = 3f;

    [Header("SFX")]
    public AudioClip fireSfx;
    public AudioClip[] fireSfxVariants;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;
    public Vector2 pitchRange = new Vector2(0.98f, 1.02f);

    AudioSource _audio;

    void Awake()
    {
        if (!TryGetComponent(out _audio))
            _audio = gameObject.AddComponent<AudioSource>();

        _audio.playOnAwake = false;
        _audio.spatialBlend = 0f;
    }

    void Update()
    {
        if (!LevelManager.IsPlaying) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(1))
            Fire();
#endif
    }

    void Fire()
    {
        Vector3 spawnPos = firePoint
            ? firePoint.position
            : transform.position + Vector3.up * yOffset;

        var go = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        var b = go.GetComponent<Bullet>();
        if (b != null)
            b.Init(Vector2.up, bulletSpeed, bulletLife);

        PlayFireSfx();
    }

    void PlayFireSfx()
    {
        AudioClip clip = null;

        if (fireSfxVariants != null && fireSfxVariants.Length > 0)
            clip = fireSfxVariants[Random.Range(0, fireSfxVariants.Length)];
        else
            clip = fireSfx;

        if (!clip || _audio == null) return;

        _audio.pitch = Random.Range(pitchRange.x, pitchRange.y);
        _audio.PlayOneShot(clip, sfxVolume);
    }
}
