using UnityEngine;
using System.Collections.Generic;

public class RoadMover : MonoBehaviour {
    public float speed = 2.3f;
    public float segmentLength = 30f; 
    public Transform otherSegment;

    public MileTracker mileTracker; 

    [Header("Decoration Settings")]
    public GameObject[] props;

    private List<float> originalZPositions = new List<float>();

    void Awake() {
        foreach (GameObject prop in props) {
            originalZPositions.Add(prop.transform.localPosition.z);
        }
    }

    void LateUpdate() {
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z <= -(segmentLength - 3.5f)) {
            float newZ = otherSegment.position.z + segmentLength;
            transform.position = new Vector3(transform.position.x, 0, newZ - 3.5f);
            RandomizeProps();
            if (mileTracker != null) {
                mileTracker.RegisterLoop();
            }
        }
    }

    void RandomizeProps() {
        if (props == null || props.Length == 0) return;

        for (int i = 0; i < props.Length; i++) {
            GameObject prop = props[i];
            prop.SetActive(Random.value > 0.5f);

            float originalZ = originalZPositions[i];
            float randomOffset = Random.Range(-3f, 3f); 
            
            Vector3 currentPos = prop.transform.localPosition;
            prop.transform.localPosition = new Vector3(currentPos.x, currentPos.y, originalZ + randomOffset);
        }
    }
}