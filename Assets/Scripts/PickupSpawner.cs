using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject healthPickupPrefab;

    [Header("Zamanlama")]
    public Vector2 intervalRange = new Vector2(6f, 10f);
    public float initialDelay = 2f;

    [Header("Konum")]
    public float spawnXRange = 2.5f;
    public float yOffset = 0.5f;

    [Header("Koşullar")]
    public bool onlyWhenHurt = true;
    public float spawnChance = 0.6f;

    PlayerHealth player;
    bool running;
    float nextTime;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerHealth>();
    }

    public void StartSpawning()
    {
        running = true;
        ScheduleNext(initialDelay);
    }

    public void StopSpawning()
    {
        running = false;
    }

    void ScheduleNext(float delay = -1f)
    {
        float dt = (delay >= 0f) ? delay : Random.Range(intervalRange.x, intervalRange.y);
        nextTime = Time.time + dt;
    }

    void Update()
    {
        if (!running) return;
        if (Time.time < nextTime) return;

        TrySpawn();
        ScheduleNext();
    }

    void TrySpawn()
    {
        if (!healthPickupPrefab) return;

        if (onlyWhenHurt && player != null)
        {
            var ch = player.CurrentHealth;
            var mh = player.maxHealth;
            if (ch >= mh) return;
        }

        if (Random.value > spawnChance) return;

        float top = Camera.main ? Camera.main.orthographicSize : 5f;
        float x = Random.Range(-spawnXRange, spawnXRange);
        Vector3 pos = new Vector3(x, top + yOffset, 0f);

        Instantiate(healthPickupPrefab, pos, Quaternion.identity);
    }
}
