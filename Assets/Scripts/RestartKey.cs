using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartKey : MonoBehaviour {
    public InputActionReference action;

    void Start() {
        action.action.Enable();

        action.action.performed += (ctx) => {
            Restart();
        };
    }

    void Restart() {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}