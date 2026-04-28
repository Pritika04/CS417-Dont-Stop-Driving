using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayNote : MonoBehaviour
{
    public float frequency = 440f; // A4
    public float duration = 0.15f; // seconds

    private AudioSource audioSource;
    private float time;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    public void PlayBeep()
    {
        time = 0f;
        audioSource.Play();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        float sampleRate = AudioSettings.outputSampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            if (time > duration)
            {
                data[i] = 0;
                continue;
            }

            float sample = Mathf.Sin(2 * Mathf.PI * frequency * time) * 0.5f;

            for (int c = 0; c < channels; c++)
                data[i + c] = sample;

            time += 1f / sampleRate;
        }
    }
}
