using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public float life = 3f;
    public int defaultDamage = 1;

    Rigidbody2D rb;
    Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        col.isTrigger = true;
    }

    public void Init(Vector2 dir, float speed, float lifeOverride = -1f)
    {
        if (lifeOverride > 0f) life = lifeOverride;

        dir = dir.sqrMagnitude > 0f ? dir.normalized : Vector2.up;
        transform.up = dir;
        rb.linearVelocity = dir * speed;

        Destroy(gameObject, life);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() || other.GetComponentInParent<PlayerHealth>())
            return;

        var hp = other.GetComponent<EnemyHealth>() ?? other.GetComponentInParent<EnemyHealth>();
        if (hp)
        {
            int dmg = defaultDamage;
            var dd = GetComponent<DamageDealer>();
            if (dd) dmg = Mathf.Max(dmg, dd.damage);

            hp.ApplyDamage(dmg);
            Destroy(gameObject);
            return;
        }

        if (other.GetComponent<FallingObject>() || other.GetComponent<DamageDealer>())
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
