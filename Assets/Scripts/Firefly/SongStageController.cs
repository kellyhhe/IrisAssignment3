using UnityEngine;

public class SongStageController : MonoBehaviour
{
    public AudioSource musicSource;
    public FireflySpawner spawner;

    public float chorus1Start = 50f;
    public float chorus1End = 69f;
    public float chorus2Start = 100f;
    public float chorus2End = 157f;

    public float finalChorusStart = 176f;

    public float fallStart = 200f;
    public float fallSpeed = 0.25f;
    public float groundY = -3f;

    bool fallTriggered = false;

    void Update()
    {
        if (musicSource == null || spawner == null || !musicSource.isPlaying) return;

        float t = musicSource.time;

        bool flowerMode =
            (t >= chorus1Start && t < chorus1End) ||
            (t >= chorus2Start && t < chorus2End) ||
            (t >= finalChorusStart);

        for (int i = 0; i < spawner.switchers.Count; i++)
        {
            if (spawner.switchers[i] == null) continue;

            if (flowerMode) spawner.switchers[i].ShowFlower();
            else spawner.switchers[i].ShowFirefly();
        }

        if (!fallTriggered && t >= fallStart)
        {
            fallTriggered = true;

            for (int i = 0; i < spawner.drifters.Count; i++)
            {
                if (spawner.drifters[i] != null)
                    spawner.drifters[i].StartFalling(fallSpeed, groundY);
            }
        }
    }
}