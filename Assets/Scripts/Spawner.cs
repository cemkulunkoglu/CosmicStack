using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Ayarlarý")]
    public GameObject[] spawnObjects;
    public float spawnInterval = 1.5f;
    public float spawnXRange = 2.5f;
    public float spawnY = 6f; 

    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomObject), 1f, spawnInterval);
    }

    void SpawnRandomObject()
    {
        if (spawnObjects.Length == 0) return;

        int randomIndex = Random.Range(0, spawnObjects.Length);
        GameObject prefab = spawnObjects[randomIndex];

        float randomX = Random.Range(-spawnXRange, spawnXRange);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
