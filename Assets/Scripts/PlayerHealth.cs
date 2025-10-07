using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Saðlýk")]
    public int maxHealth = 4;
    public UnityEvent onDeath;

    [Header("Ýframe (çakýþma sonrasý kýsa dokunulmazlýk)")]
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
        UpdateHealthUI();
    }

    public void TakeDamage(int dmg)
    {
        if (invincible || dmg <= 0) return;

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
}
