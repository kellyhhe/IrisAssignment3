using System.Collections.Generic;
using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bluePrefab;
    public GameObject whitePrefab;
    public GameObject yellowPrefab;

    [Header("Spawn Count")]
    public int count = 100;

    [Header("Spawn Area")]
    public Vector3 center = new Vector3(0f, 1f, 8f);
    public Vector3 areaSize = new Vector3(16f, 9f, 2f);

    [Header("Random Base Size")]
    public float minSize = 0.10f;
    public float maxSize = 0.22f;

    [Header("Color Weights")]
    public float blueWeight = 0.72f;
    public float whiteWeight = 0.20f;
    public float yellowWeight = 0.08f;

    [HideInInspector] public List<FireflyPulse> fireflies = new List<FireflyPulse>();
    [HideInInspector] public List<FireflyFlowerSwitch> switchers = new List<FireflyFlowerSwitch>();
    [HideInInspector] public List<FireflyDrift> drifters = new List<FireflyDrift>();

    void Start()
    {
        SpawnFireflies();
    }

    void SpawnFireflies()
    {
        fireflies.Clear();
        switchers.Clear();
        drifters.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject chosenPrefab = GetWeightedRandomPrefab();
            if (chosenPrefab == null) continue;

            Vector3 pos = center + new Vector3(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f),
                Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
            );

            GameObject obj = Instantiate(chosenPrefab, pos, Quaternion.identity, transform);

            float randomSize = Random.Range(minSize, maxSize);

            FireflyPulse pulse = obj.GetComponent<FireflyPulse>();
            if (pulse != null)
            {
                pulse.SetBaseScale(randomSize);
                fireflies.Add(pulse);
            }

            FireflyFlowerSwitch s = obj.GetComponent<FireflyFlowerSwitch>();
            if (s != null)
            {
                s.ShowFirefly();
                switchers.Add(s);
            }

            FireflyDrift d = obj.GetComponent<FireflyDrift>();
            if (d != null)
            {
                d.SetSpawnArea(center, areaSize);
                drifters.Add(d);
            }
        }
    }

    GameObject GetWeightedRandomPrefab()
    {
        float totalWeight = blueWeight + whiteWeight + yellowWeight;
        float randomValue = Random.Range(0f, totalWeight);

        if (randomValue < blueWeight)
            return bluePrefab;

        randomValue -= blueWeight;

        if (randomValue < whiteWeight)
            return whitePrefab;

        return yellowPrefab;
    }

    public void PulseAll()
    {
        for (int i = 0; i < fireflies.Count; i++)
        {
            if (fireflies[i] != null)
                fireflies[i].TriggerPulse();
        }
    }
}