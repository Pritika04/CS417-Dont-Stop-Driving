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

    [Header("Juicy Feedback")]
    public ParticleSystem mileTransitionEffect;
    public ParticleSystem textUpdateEffect;
    public ParticleSystem lightUpdateEffect;

    public TextMeshProUGUI healthDisplay;

    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort.TunnelingVignetteController vignette;

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

        if (textUpdateEffect != null) {
            textUpdateEffect.Play();
        }

        if (mileTransitionEffect != null) {
            mileTransitionEffect.Play();
        }
        
        switch (totalMiles) {
            case 1:
                healthDisplay.text = "Health: 4/5";

                vignette.defaultParameters.apertureSize = 0.8f;
                creepyRadio.TriggerSpookyStation(1);
                if (carInteriorLight != null) {
                    carInteriorLight.color = Color.red;
                    carInteriorLight.intensity = 5f;
                    if (lightUpdateEffect != null) {
                        lightUpdateEffect.Play();
                    }
                }
                break;

            case 2:
                healthDisplay.text = "Health: 3/5";

                vignette.defaultParameters.apertureSize = 0.5f;
                creepyRadio.TriggerSpookyStation(2);
                if (carInteriorLight != null) {
                    carInteriorLight.color = Color.orange;
                    carInteriorLight.intensity = 1f;
                    if (lightUpdateEffect != null) {
                        lightUpdateEffect.Play();
                    }
                }
                break;

            case 3:
                healthDisplay.text = "Health: 2/5";

                vignette.defaultParameters.apertureSize = 0.3f;
                creepyRadio.TriggerSpookyStation(3);
                if (carInteriorLight != null) {
                    carInteriorLight.color = Color.green;
                    carInteriorLight.intensity = 1f;
                    if (lightUpdateEffect != null) {
                        lightUpdateEffect.Play();
                    }
                }
                WinGame();
                break;
        }
    }

    void WinGame() {
        mileDisplay.text = "YOU SURVIVED!";
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
