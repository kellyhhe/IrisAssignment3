using UnityEngine;

public class FireflyDrift : MonoBehaviour
{
    [Header("Drift Area")]
    public Vector3 center = new Vector3(0f, 1f, 8f);
    public Vector3 areaSize = new Vector3(16f, 9f, 2f);

    [Header("Movement")]
    public float driftSpeedMin = 0.12f;
    public float driftSpeedMax = 0.35f;
    public float noiseAmount = 0.7f;
    public float noiseScale = 0.5f;

    Vector3 direction;
    float speed;

    float noiseOffsetX;
    float noiseOffsetY;
    float noiseOffsetZ;

    bool falling = false;
    public float fallSpeed = 0.25f;
    public float groundY = -3f;

    void Start()
    {
        speed = Random.Range(driftSpeedMin, driftSpeedMax);

        direction = Random.onUnitSphere;
        direction.z *= 0.25f;
        direction.Normalize();

        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetY = Random.Range(100f, 200f);
        noiseOffsetZ = Random.Range(200f, 300f);
    }

    void Update()
    {
        if (falling)
        {
            Vector3 p = transform.position;
            p.y -= fallSpeed * Time.deltaTime;
            if (p.y < groundY) p.y = groundY;
            transform.position = p;
            return;
        }

        float t = Time.time * noiseScale;

        Vector3 noise = new Vector3(
            Mathf.PerlinNoise(noiseOffsetX, t) - 0.5f,
            Mathf.PerlinNoise(noiseOffsetY, t) - 0.5f,
            Mathf.PerlinNoise(noiseOffsetZ, t) - 0.5f
        ) * 2f;

        Vector3 desiredDirection = (direction + noise * noiseAmount).normalized;

        transform.position += desiredDirection * speed * Time.deltaTime;

        KeepInsideBounds();
    }

    void KeepInsideBounds()
    {
        Vector3 half = areaSize * 0.5f;
        Vector3 localOffset = transform.position - center;

        if (localOffset.x > half.x || localOffset.x < -half.x)
            direction.x *= -1f;

        if (localOffset.y > half.y || localOffset.y < -half.y)
            direction.y *= -1f;

        if (localOffset.z > half.z || localOffset.z < -half.z)
            direction.z *= -1f;
    }

    public void SetSpawnArea(Vector3 newCenter, Vector3 newAreaSize)
    {
        center = newCenter;
        areaSize = newAreaSize;
    }

    public void StartFalling(float speed, float y)
    {
        falling = true;
        fallSpeed = speed;
        groundY = y;
    }
}