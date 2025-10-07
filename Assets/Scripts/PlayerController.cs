using UnityEngine;

public class PlayerControllerFull : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 10f;
    public Vector2 boundaryX = new Vector2(-2.5f, 2.5f);
    public Vector2 boundaryY = new Vector2(-4f, 4f);

    [Header("Thruster Ayarlarý")]
    public ParticleSystem[] thrusters;
    public SpriteRenderer[] thrusterSprites;
    public float minSpeedToFlame = 0.5f;
    public float idleGrace = 0.1f;

    private Vector3 targetPosition;
    private float lastMoveTime;
    private bool isDragging;

    void Start()
    {
        targetPosition = transform.position;
        SetFlame(false);
    }

    void Update()
    {
        isDragging = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            isDragging = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)
            );
            targetPosition = mousePos;
        }
#else
        if (Input.touchCount > 0)
        {
            isDragging = true;
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, 10f)
            );
            targetPosition = touchPos;
        }
#endif

        if (!isDragging)
            targetPosition = transform.position;

        Vector3 newPos = isDragging
            ? Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed)
            : transform.position;

        newPos.x = Mathf.Clamp(newPos.x, boundaryX.x, boundaryX.y);
        newPos.y = Mathf.Clamp(newPos.y, boundaryY.x, boundaryY.y);
        newPos.z = 0f;

        Vector3 delta = newPos - transform.position;
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
                if (ps == null) continue;
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
                if (sr == null) continue;
                sr.enabled = on;
            }
        }
    }
}
