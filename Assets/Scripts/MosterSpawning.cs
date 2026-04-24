using UnityEngine;
using System.Collections;

public class HitogataFlicker : MonoBehaviour {
    public GameObject hitogataVisuals;
    public Transform player;

    public float activeDuration = 2f;
    public float waitDuration = 10f;

    public float roadLength = 20f;

    public float sideOffset = 6f;

    void Start() {
        hitogataVisuals.SetActive(false);
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine() {
        while (true) {
            yield return new WaitForSeconds(waitDuration);

            float x = Random.Range(-sideOffset, sideOffset);

            float z = player.position.z +
                      Random.Range(roadLength, roadLength * 2f);

            transform.position = new Vector3(x, 0, z);

            hitogataVisuals.SetActive(true);

            yield return new WaitForSeconds(activeDuration);

            hitogataVisuals.SetActive(false);
        }
    }
}