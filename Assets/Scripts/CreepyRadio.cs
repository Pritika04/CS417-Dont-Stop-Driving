using UnityEngine;

public class CreepyRadio : MonoBehaviour {
    public AudioSource spookySource; 
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;

    public void TriggerSpookyStation(int mile) {
        if (mile == 1) {
            spookySource.clip = sound1;
        } else if (mile == 2) {
            spookySource.clip = sound2;
        } else if (mile == 3) {
            spookySource.clip = sound3;
        }
        spookySource.Play();
    }
}