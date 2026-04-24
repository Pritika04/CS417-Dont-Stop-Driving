using UnityEngine;

public class RoadMover : MonoBehaviour {
    public float speed = 2f;
    public float segmentLength = 30f; 
    public Transform otherSegment;

    void LateUpdate() {
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z <= -(segmentLength - 3.5f)) {
            float newZ = otherSegment.position.z + segmentLength;
            transform.position = new Vector3(transform.position.x, 0, newZ - 3.5f);
        }
    }
}