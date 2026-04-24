using UnityEngine;

public class PlayerAnchor : MonoBehaviour {
    void Update() {
        transform.position = new Vector3(transform.position.x, 0, 0);
    }
}