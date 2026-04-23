using UnityEngine;

public class RoadMover : MonoBehaviour {
    public float speed = 15f; 
    public float roadLength = 20f;
    public Transform otherSegment;

    void Update() {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z <= -roadLength) {
            float newZ = otherSegment.position.z + roadLength;
            transform.position = new Vector3(0, 0, newZ);

            RandomizeDecor();
        }
    }

    void RandomizeDecor() {
        // TODO: randomize trees/humanoid monster, etc every 10 seconds
    }
}