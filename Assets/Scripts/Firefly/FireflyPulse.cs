using UnityEngine;
using System.Collections;

public class FireflyPulse : MonoBehaviour
{
    public float pulseMultiplier = 2f;
    public float growSpeed = 20f;
    public float shrinkSpeed = 10f;

    Vector3 baseScale;
    Coroutine pulseRoutine;

    void Start()
    {
        baseScale = transform.localScale;
    }

    public void SetBaseScale(float size)
    {
        baseScale = Vector3.one * size;
        transform.localScale = baseScale;
    }

    public void TriggerPulse()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        pulseRoutine = StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        Vector3 pulseScale = baseScale * pulseMultiplier;

        while ((transform.localScale - pulseScale).sqrMagnitude > 0.0001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, pulseScale, growSpeed * Time.deltaTime);
            yield return null;
        }

        while ((transform.localScale - baseScale).sqrMagnitude > 0.0001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, baseScale, shrinkSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = baseScale;
        pulseRoutine = null;
    }
}