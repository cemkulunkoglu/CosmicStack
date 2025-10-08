using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour
{
    [Header("Etkisi")]
    public int healAmount = 1;

    [Header("Hareket/Ömür")]
    public float fallSpeed = 2.5f;
    public float lifeTime = 12f;

    [Header("SFX/VFX")]
    public AudioClip pickupSfx;
    [Range(0f, 1f)] public float sfxVolume = 0.9f;
    public GameObject pickupVfx;

    bool picked;

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
        if (picked) return;

        var hp = other.GetComponent<PlayerHealth>();
        if (!hp) hp = other.GetComponentInParent<PlayerHealth>();
        if (!hp) return;

        hp.AddHealth(healAmount);
        picked = true;

        if (pickupSfx)
            AudioSource.PlayClipAtPoint(
                pickupSfx,
                Camera.main ? Camera.main.transform.position : transform.position,
                sfxVolume
            );

        if (pickupVfx) Instantiate(pickupVfx, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
