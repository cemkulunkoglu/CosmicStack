using UnityEngine;

public class PlayerControllerFull : MonoBehaviour
{
    [Header("Yatay Hareket")]
    public float xSpeed = 12f;
    public Vector2 xBounds = new Vector2(-2.5f, 2.5f);

    [Header("Dikey Limit (küçük bant)")]
    public bool useStartYAsCenter = true;
    public float yCenter = -3.2f;
    public float yRange = 0.6f; 
    public float yFollowSpeed = 6f;
    [Range(0f, 1f)] public float yFollowFactor = 0.5f;

    [Header("Thruster Ayarlarý")]
    public ParticleSystem[] thrusters;
    public SpriteRenderer[] thrusterSprites;
    public float minSpeedToFlame = 0.5f;
    public float idleGrace = 0.1f;

    Vector3 targetWorld;
    float lastMoveTime;
    bool isDragging;
    float startY;

    void Start()
    {
        startY = transform.position.y;
        if (useStartYAsCenter) yCenter = startY;
        targetWorld = transform.position;
        SetFlame(false);
    }

    void Update()
    {
        isDragging = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            isDragging = true;
            targetWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)
            );
        }
#else
        if (Input.touchCount > 0)
        {
            isDragging = true;
            var touch = Input.GetTouch(0);
            targetWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, 10f)
            );
        }
#endif

        float targetX = Mathf.Clamp(targetWorld.x, xBounds.x, xBounds.y);

        float yMin = yCenter - yRange;
        float yMax = yCenter + yRange;

        float rawY = targetWorld.y;
        float biasedY = Mathf.Lerp(yCenter, rawY, yFollowFactor);
        float targetY = Mathf.Clamp(biasedY, yMin, yMax);

        Vector3 pos = transform.position;
        float newX = isDragging ? Mathf.Lerp(pos.x, targetX, Time.deltaTime * xSpeed) : pos.x;
        float newY = isDragging ? Mathf.Lerp(pos.y, targetY, Time.deltaTime * yFollowSpeed) : pos.y;

        Vector3 newPos = new Vector3(newX, newY, 0f);

        Vector3 delta = newPos - pos;
        float speed = delta.magnitude / Mathf.Max(Time.deltaTime, 1e-5f);
        bool moving = speed > minSpeedToFlame;
        if (moving) lastMoveTime = Time.time;

        bool showFlame = moving || (Time.time - lastMoveTime < idleGrace);
        SetFlame(showFlame);

        transform.position = newPos;
    }

    void SetFlame(bool on)
    {
        if (thrusters != null)
        {
            foreach (var ps in thrusters)
            {
                if (!ps) continue;
                var em = ps.emission;
                em.enabled = on;
                if (on && !ps.isPlaying) ps.Play();
                if (!on && ps.isPlaying) ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        if (thrusterSprites != null)
        {
            foreach (var sr in thrusterSprites)
            {
                if (!sr) continue;
                sr.enabled = on;
            }
        }
    }
}
