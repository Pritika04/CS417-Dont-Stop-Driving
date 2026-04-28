using UnityEngine;

public class RadioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] stations;   // 3 stations
    private int currentStation = 0;
    private bool isOn = false;

    public void ToggleRadio()
    {
        isOn = !isOn;

        if (isOn)
        {
            audioSource.clip = stations[currentStation];
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void NextStation()
    {
        if (!isOn) return; // do nothing if radio is off

        currentStation = (currentStation + 1) % stations.Length;
        audioSource.clip = stations[currentStation];
        audioSource.Play();
    }
}