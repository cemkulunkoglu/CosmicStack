using UnityEngine;

public class BossDamage : MonoBehaviour
{
    [Header("Hit SFX/VFX")]
    public AudioClip hitSfx;
    [Range(0f, 1f)] public float hitVolume = 0.8f;
    public GameObject hitVfx;

    [Header("Flash (opsiyonel)")]
    public bool flashOnHit = true;
    public Color flashColor = Color.white;
    public float flashTime = 0.06f;

    [Header("Spam Koruması")]
    public float minInterval = 0.05f;

    float lastPlay;
    SpriteRenderer[] srs;
    Color[] original;

    void Awake()
    {
        if (flashOnHit)
        {
            srs = GetComponentsInChildren<SpriteRenderer>(true);
            original = new Color[srs.Length];
            for (int i = 0; i < srs.Length; i++)
                original[i] = srs[i].color;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isPlayerBullet = other.GetComponent<Bullet>() && !other.GetComponent<EnemyBullet>();
        if (!isPlayerBullet) return;

        PlayHitFx(other.transform.position);
    }

    void PlayHitFx(Vector3 at)
    {
        if (Time.time - lastPlay < minInterval) return;
        lastPlay = Time.time;

        if (hitSfx)
        {
#if UNITY_EDITOR
            AudioOneShot.Play2D(hitSfx, hitVolume);
#else
            AudioSource.PlayClipAtPoint(
                hitSfx,
                Camera.main ? Camera.main.transform.position : at,
                hitVolume
            );
#endif
        }

        if (hitVfx) Instantiate(hitVfx, at, Quaternion.identity);

        if (flashOnHit) StartCoroutine(Flash());
    }

    System.Collections.IEnumerator Flash()
    {
        if (srs == null || srs.Length == 0) yield break;
        for (int i = 0; i < srs.Length; i++) if (srs[i]) srs[i].color = flashColor;
        yield return new WaitForSeconds(flashTime);
        for (int i = 0; i < srs.Length; i++) if (srs[i]) srs[i].color = original[i];
    }
}
