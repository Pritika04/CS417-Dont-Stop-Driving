using UnityEngine;
using TMPro;
using System.Collections;

public class MileTracker : MonoBehaviour {
    public TextMeshProUGUI mileDisplay;
    public CreepyRadio creepyRadio; 
    public Light carInteriorLight;
    public int loopsPerMile = 3;
    
    private int currentLoops = 0;
    private int totalMiles = 0;

    public RoadMover[] roads;

    public void RegisterLoop() {
        currentLoops++;
        if (currentLoops >= loopsPerMile) {
            totalMiles++;
            currentLoops = 0;
            MarkMileDistinctly();
        }
    }

    void MarkMileDistinctly() {
        mileDisplay.text = "MILE: " + totalMiles + " / 3";

        switch (totalMiles) {
            case 1:
                creepyRadio.TriggerSpookyStation(1);
                if (carInteriorLight != null) {
                    carInteriorLight.color = Color.red;
                    carInteriorLight.intensity = 5f;
                }
                break;
            case 2:
                creepyRadio.TriggerSpookyStation(2);
                if (carInteriorLight != null) {
                    carInteriorLight.color = Color.orange;
                    carInteriorLight.intensity = 1f;
                }
                break;
            case 3:
                creepyRadio.TriggerSpookyStation(3);
                if (carInteriorLight != null) {
                    carInteriorLight.color = Color.green;
                    carInteriorLight.intensity = 1f;
                }
                WinGame();
                break;
        }
    }

    void WinGame() {
        mileDisplay.text = "YOU SURVIVED!";

        if (roads != null) {
            foreach (RoadMover road in roads) {
                road.speed = 0f; 
            }
        }

        StartCoroutine(WaitAndQuit());
    }

    IEnumerator WaitAndQuit() {
        yield return new WaitForSeconds(10f);

        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }   
}