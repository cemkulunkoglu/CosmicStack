using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 1;

    [Header("FX")]
    public AudioClip explosionSfx;
    [Range(0f, 1f)] public float explosionVolume = 0.9f;
    public GameObject explosionVfx;

    public UnityEvent onDeath;

    int hp;

    void Awake() { hp = Mathf.Max(1, maxHP); }

    public void ApplyDamage(int dmg)
    {
        if (dmg <= 0) return;
        hp -= dmg;
        if (hp <= 0) Die();
    }

    public void Die()
    {
        onDeath?.Invoke();

        AudioOneShot.Play2D(explosionSfx, explosionVolume);

        if (explosionVfx) Instantiate(explosionVfx, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
