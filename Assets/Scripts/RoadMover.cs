using UnityEngine;

public class RoadMover : MonoBehaviour {
    public float speed = 5f;
    public float segmentLength = 5f;
    // We remove "otherSegment" to prevent one segment from "pulling" the other into a gap
    
    void LateUpdate() {
        // 1. Move backward
        transform.position += Vector3.back * speed * Time.deltaTime;

        // 2. The Hard Reset
        // If the segment center passes -5, we snap it exactly 5 units ahead of the start.
        if (transform.position.z <= -segmentLength) {
            // Instead of relative math, we use a clean jump.
            // This ensures that even after 100 loops, the gap never grows.
            transform.position = new Vector3(0, 0, segmentLength);
        }
    }
}