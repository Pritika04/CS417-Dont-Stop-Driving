using UnityEngine;

public class HitogataSpawner : MonoBehaviour {
    public float spawnInterval = 8f;
    public float activeDuration = 5f;
    public GameObject visuals;

    [Header("Juicy Feedback")]
    public AudioSource spawnSound;
    public ParticleSystem spawnBurst;

    void Start() {
        visuals.SetActive(false);
        InvokeRepeating("TrySpawn", spawnInterval, spawnInterval);
    }

    void TrySpawn() {
        float x = Random.Range(-3f, 3f); 
        transform.localPosition = new Vector3(x, 0, 22f); 
        visuals.SetActive(true);
        if (spawnSound != null) {
            spawnSound.Play();
        }
        if (spawnBurst != null) {
            spawnBurst.Play();
        }
        Invoke("Hide", activeDuration);
    }

    void Hide() {
        visuals.SetActive(false);
    }
}