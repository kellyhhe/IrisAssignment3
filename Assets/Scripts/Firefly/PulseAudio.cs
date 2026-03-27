using UnityEngine;
using System.Collections.Generic;

public class PulseAudio : MonoBehaviour
{
    public AudioSource musicSource;
    public FireflySpawner spawner;

    [Range(64, 2048)] public int sampleSize = 512;
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;

    public float onsetMultiplier = 1.5f;
    public float minAmplitude = 0.008f;
    public float minPulseInterval = 0.08f;
    public int historyLength = 12;

    float[] spectrum;
    float[] outputSamples;

    Queue<float> energyHistory = new Queue<float>();
    float energySum = 0f;
    float lastPulseTime = -999f;

    void Start()
    {
        spectrum = new float[sampleSize];
        outputSamples = new float[sampleSize];
    }

    void Update()
    {
        if (musicSource == null || spawner == null || !musicSource.isPlaying)
            return;

        musicSource.GetOutputData(outputSamples, 0);

        float amplitude = 0f;
        for (int i = 0; i < outputSamples.Length; i++)
            amplitude += Mathf.Abs(outputSamples[i]);

        amplitude /= outputSamples.Length;

        musicSource.GetSpectrumData(spectrum, 0, fftWindow);

        float energy = 0f;
        for (int i = 2; i < sampleSize / 5; i++)
            energy += spectrum[i];

        energyHistory.Enqueue(energy);
        energySum += energy;

        if (energyHistory.Count > historyLength)
            energySum -= energyHistory.Dequeue();

        float avgEnergy = energySum / energyHistory.Count;

        bool onsetDetected =
            energyHistory.Count >= historyLength &&
            energy > avgEnergy * onsetMultiplier &&
            amplitude > minAmplitude &&
            Time.time - lastPulseTime > minPulseInterval;

        if (onsetDetected)
        {
            spawner.PulseAll();
            lastPulseTime = Time.time;
        }
    }
}