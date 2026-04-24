// using UnityEngine;

// public class RoadMover : MonoBehaviour {
//     public float speed = 2f;
//     public float segmentLength = 10f; 
//     public Transform otherSegment;

//     void LateUpdate() {
//         transform.position += Vector3.back * speed * Time.deltaTime;
//         if (transform.position.z <= 0f) {
//             float newZ = otherSegment.position.z + segmentLength;
//             transform.position = new Vector3(transform.position.x, 0, newZ - 0.01f);
//         }
//     }
// }

using UnityEngine;

public class RoadMover : MonoBehaviour {
    public float speed = 2f;
    public float segmentLength = 10f; 
    public Transform otherSegment;

    void LateUpdate() {
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z <= -10f) {
            float newZ = otherSegment.position.z + segmentLength;
            
            // This will print in the bottom left of Unity
            Debug.Log(gameObject.name + " is snapping! Other segment is at: " + otherSegment.position.z + ". Snapping to: " + newZ);

            transform.position = new Vector3(transform.position.x, 0, newZ - 0.01f);
        }
    }

    // This draws a red line in the Scene view so you can see the "trigger zone"
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-10, 0, 0), new Vector3(10, 0, 0));
    }
}