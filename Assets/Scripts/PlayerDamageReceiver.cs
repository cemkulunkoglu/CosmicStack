using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDamageReceiver : MonoBehaviour
{
    PlayerHealth health;

    void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    void HandleHit(GameObject otherGO)
    {
        if (health == null || otherGO == null) return;

        var dealer = otherGO.GetComponent<DamageDealer>();
        if (dealer == null) return;

        Debug.Log($"HIT -> {otherGO.name}  damage:{dealer.damage}");

        health.TakeDamage(dealer.damage);

        if (dealer.destroyOnHit)
            Destroy(otherGO);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        HandleHit(col.collider.gameObject);
    }
}
