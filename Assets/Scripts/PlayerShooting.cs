using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float yOffset = 0.5f;
    public float bulletSpeed = 12f;
    public float bulletLife = 3f;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(1))
            Fire();
#endif
    }

    void Fire()
    {
        Vector3 spawnPos = firePoint
            ? firePoint.position
            : transform.position + Vector3.up * yOffset;

        var go = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        var b = go.GetComponent<Bullet>();
        if (b != null)
            b.Init(Vector2.up, bulletSpeed, bulletLife);
    }
}
