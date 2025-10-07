using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    public float moveSpeed = 3f;
    public bool followPlayer = true;

    [Tooltip("D��ey a�a��dan izin verilen sapma (derece). 0 = tam dik a�a��")]
    public float maxDeviationDeg = 12f;

    private Vector2 direction;

    void Start()
    {
        if (followPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 toPlayer = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

                float angleToPlayer = Vector2.SignedAngle(Vector2.down, toPlayer);

                float clamped = Mathf.Clamp(angleToPlayer, -maxDeviationDeg, maxDeviationDeg);

                direction = Quaternion.Euler(0, 0, clamped) * Vector2.down;
            }
            else
            {
                direction = Vector2.down;
            }
        }
        else
        {
            float rand = Random.Range(-maxDeviationDeg, maxDeviationDeg);
            direction = Quaternion.Euler(0, 0, rand) * Vector2.down;
        }
    }

    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (transform.position.y < -6f || Mathf.Abs(transform.position.x) > 6f)
            Destroy(gameObject);
    }
}
