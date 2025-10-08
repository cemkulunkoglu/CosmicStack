using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;
    public float fallSpeed = 2.5f;
    public float lifeTime = 12f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        if (Camera.main)
        {
            float bottom = -Camera.main.orthographicSize;
            if (transform.position.y < bottom - 1f) Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var hp = other.GetComponent<PlayerHealth>();
        if (!hp) hp = other.GetComponentInParent<PlayerHealth>();
        if (!hp) return;

        hp.AddHealth(healAmount);
        Destroy(gameObject);
    }
}
