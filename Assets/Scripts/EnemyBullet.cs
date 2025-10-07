using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;
    public float life = 3f;
    public int damage = 1;

    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Fire(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.down;
        dir = dir.normalized;

        transform.up = dir;
        rb.linearVelocity = dir * speed;
        Destroy(gameObject, life);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var hp = other.GetComponent<PlayerHealth>();
        if (hp) hp.TakeDamage(damage);

        Destroy(gameObject);
    }
}
