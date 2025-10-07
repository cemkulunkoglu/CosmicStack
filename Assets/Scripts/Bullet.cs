using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public float life = 3f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (other.GetComponent<PlayerControllerFull>() != null) return;

        if (other.GetComponent<DamageDealer>() != null || other.GetComponent<FallingObject>() != null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
