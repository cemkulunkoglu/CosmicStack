using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Ayarlarý")]
    public float spawnInterval = 1.5f;
    public float spawnXRange = 2.5f;
    public float spawnY = 6f;

    GameObject[] spawnObjects = System.Array.Empty<GameObject>();
    bool running;

    public void SetSpawnList(GameObject[] list, float interval)
    {
        spawnObjects = list ?? System.Array.Empty<GameObject>();
        spawnInterval = interval;
    }

    public void StartSpawning(float delay = 0.5f)
    {
        running = true;
        CancelInvoke(nameof(SpawnRandomObject));
        InvokeRepeating(nameof(SpawnRandomObject), delay, spawnInterval);
    }

    public void StopSpawning()
    {
        running = false;
        CancelInvoke(nameof(SpawnRandomObject));
    }

    void SpawnRandomObject()
    {
        if (!running || spawnObjects == null || spawnObjects.Length == 0) return;

        int randomIndex = Random.Range(0, spawnObjects.Length);
        GameObject prefab = spawnObjects[randomIndex];

        float randomX = Random.Range(-spawnXRange, spawnXRange);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
