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

    public int CurrentHealth { get; private set; }

    bool invincible;
    SpriteRenderer sr;

    void Awake()
    {
        CurrentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
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
        if (invincible || dmg <= 0 || CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - dmg);
        UpdateHealthUI();

        if (CurrentHealth <= 0)
        {
            onDeath?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        if (invincibleTime > 0f)
            StartCoroutine(IFrames(invincibleTime));
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
        CurrentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void AddHealth(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0) return;
        int before = CurrentHealth;
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);

        if (CurrentHealth != before)
            UpdateHealthUI();
    }

    public bool IsFull() => CurrentHealth >= maxHealth;
    public float HealthRatio() => maxHealth > 0 ? (float)CurrentHealth / maxHealth : 0f;
}
