using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Saðlýk")]
    public int maxHealth = 4;
    public UnityEvent onDeath;

    [Header("I-frame (çakýþma sonrasý kýsa dokunulmazlýk)")]
    public float invincibleTime = 0.2f;

    [Header("UI")]
    public Slider healthBar;

    [Header("Ölüm FX")]
    public AudioClip deathSfx;
    [Range(0f, 1f)] public float deathVolume = 0.9f;
    public GameObject deathVfx;

    [Header("Hit FX")]
    public AudioClip hitSfx;
    [Range(0f, 1f)] public float hitVolume = 0.8f;

    public int CurrentHealth { get; private set; }

    bool invincible;
    bool dead;
    SpriteRenderer sr;
    Collider2D col;

    void Awake()
    {
        CurrentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (healthBar != null)
        {
            healthBar.wholeNumbers = true;
            healthBar.maxValue = maxHealth;
            healthBar.value = CurrentHealth;
        }
        UpdateHealthUI();
    }

    public void TakeDamage(int dmg)
    {
        if (dead || invincible || dmg <= 0 || CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - dmg);
        UpdateHealthUI();

        if (CurrentHealth > 0 && hitSfx)
        {
#if UNITY_EDITOR
            AudioOneShot.Play2D(hitSfx, hitVolume);
#else
            AudioSource.PlayClipAtPoint(
                hitSfx,
                Camera.main ? Camera.main.transform.position : transform.position,
                hitVolume
            );
#endif
        }

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        if (invincibleTime > 0f)
            StartCoroutine(IFrames(invincibleTime));
    }

    void Die()
    {
        if (dead) return;
        dead = true;

        onDeath?.Invoke();

        if (col) col.enabled = false;
        if (sr) sr.enabled = false;

        if (deathSfx)
        {
#if UNITY_EDITOR
            AudioOneShot.Play2D(deathSfx, deathVolume);
#else
            AudioSource.PlayClipAtPoint(
                deathSfx,
                Camera.main ? Camera.main.transform.position : transform.position,
                deathVolume
            );
#endif
        }

        if (deathVfx) Instantiate(deathVfx, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }

    System.Collections.IEnumerator IFrames(float time)
    {
        invincible = true;
        if (sr != null)
        {
            float t = 0f;
            while (t < time)
            {
                sr.enabled = !sr.enabled;
                yield return new WaitForSeconds(0.05f);
                t += 0.05f;
            }
            sr.enabled = true;
        }
        invincible = false;
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = CurrentHealth;
        }
    }

    public void ResetHealth()
    {
        dead = false;
        CurrentHealth = maxHealth;
        if (sr) sr.enabled = true;
        if (col) col.enabled = true;
        UpdateHealthUI();
    }

    public void AddHealth(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0) return;
        int before = CurrentHealth;
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        if (CurrentHealth != before) UpdateHealthUI();
    }

    public bool IsFull() => CurrentHealth >= maxHealth;
    public float HealthRatio() => maxHealth > 0 ? (float)CurrentHealth / maxHealth : 0f;
}
