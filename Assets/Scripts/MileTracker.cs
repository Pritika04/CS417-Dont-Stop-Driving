using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

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
    public int health = 5;

    // controllers
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor leftController;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor rightController;

    // keys
    public GameObject greenKey;
    public GameObject blueKey;
    public GameObject redKey;

    // key grab states
    private bool greenKeyGrabbed = false;
    private bool blueKeyGrabbed = false;
    private bool redKeyGrabbed = false;

    // interactables
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable greenKeyInteractable;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable blueKeyInteractable;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable redKeyInteractable;

    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort.TunnelingVignetteController vignette;

    // NEW: Game end flag to stop lighting overrides
    private bool gameEnded = false;


    public void RegisterLoop() {
        currentLoops++;
        if (currentLoops >= loopsPerMile) {
            totalMiles++;
            currentLoops = 0;
            MarkMileDistinctly();
        }
    }


    void Start()
    {
        // GREEN KEY
        if (greenKeyInteractable != null)
        {
            greenKeyInteractable.selectEntered.AddListener((args) => greenKeyGrabbed = true);
            greenKeyInteractable.selectExited.AddListener((args) => greenKeyGrabbed = false);
        }

        // BLUE KEY
        if (blueKeyInteractable != null)
        {
            blueKeyInteractable.selectEntered.AddListener((args) => blueKeyGrabbed = true);
            blueKeyInteractable.selectExited.AddListener((args) => blueKeyGrabbed = false);
        }

        // RED KEY
        if (redKeyInteractable != null)
        {
            redKeyInteractable.selectEntered.AddListener((args) => redKeyGrabbed = true);
            redKeyInteractable.selectExited.AddListener((args) => redKeyGrabbed = false);
        }
    }


    int ApplyKeyPenalty(bool correctKeyGrabbed)
    {
        bool anyKeyGrabbed = redKeyGrabbed || blueKeyGrabbed || greenKeyGrabbed;

        if (!anyKeyGrabbed)
            return -1;

        if (correctKeyGrabbed)
        {
            int keysHeld = 0;
            if (redKeyGrabbed) keysHeld++;
            if (blueKeyGrabbed) keysHeld++;
            if (greenKeyGrabbed) keysHeld++;

            if (keysHeld > 1)
                return -2;

            return 0;
        }

        return -2;
    }


    void MarkMileDistinctly() {
        mileDisplay.text = "MILE: " + totalMiles + " / 3";

        if (textUpdateEffect != null) textUpdateEffect.Play();
        if (mileTransitionEffect != null) mileTransitionEffect.Play();

        int penalty = 0;

        switch (totalMiles) {

            case 1:
                penalty = ApplyKeyPenalty(redKeyGrabbed);
                health += penalty;

                if (health <= 0) { LoseGame(); return; }

                healthDisplay.text = "Health: " + health + " / 5";

                vignette.defaultParameters.apertureSize = 0.8f;
                creepyRadio.TriggerSpookyStation(1);

                if (carInteriorLight != null) {
                    carInteriorLight.color = Color.red;
                    carInteriorLight.intensity = 5f;
                    if (lightUpdateEffect != null) lightUpdateEffect.Play();
                }
                break;

            case 2:
                penalty = ApplyKeyPenalty(blueKeyGrabbed);
                health += penalty;

                if (health <= 0) { LoseGame(); return; }

                healthDisplay.text = "Health: " + health + " / 5";

                vignette.defaultParameters.apertureSize = 0.5f;
                creepyRadio.TriggerSpookyStation(2);

                if (carInteriorLight != null) {
                    carInteriorLight.color = Color.orange;
                    carInteriorLight.intensity = 1f;
                    if (lightUpdateEffect != null) lightUpdateEffect.Play();
                }
                break;

            case 3:
                penalty = ApplyKeyPenalty(greenKeyGrabbed);
                health += penalty;

                if (health <= 0) { LoseGame(); return; }

                healthDisplay.text = "Health: " + health + " / 5";

                vignette.defaultParameters.apertureSize = 0.3f;
                creepyRadio.TriggerSpookyStation(3);

                WinGame();
                break;
        }
    }


    void UpdateInteriorLightColorBasedOnKey()
    {
        if (carInteriorLight == null)
            return;

        if (redKeyGrabbed) {
            carInteriorLight.color = Color.red;
            return;
        }

        if (blueKeyGrabbed) {
            carInteriorLight.color = Color.blue;
            return;
        }

        if (greenKeyGrabbed) {
            carInteriorLight.color = Color.green;
            return;
        }

        carInteriorLight.color = Color.white;
    }


    void Update()
    {
        if (gameEnded)
            return; // STOP all lighting overrides

        UpdateInteriorLightColorBasedOnKey();
    }


    void WinGame() {
        gameEnded = true;

        if (carInteriorLight != null) {
            carInteriorLight.color = Color.green;
            carInteriorLight.intensity = 1f;
            if (lightUpdateEffect != null) lightUpdateEffect.Play();
        }

        mileDisplay.text = "YOU SURVIVED!";
        StartCoroutine(WaitAndQuit());
    }

    void LoseGame() {
        gameEnded = true;

        if (carInteriorLight != null) {
            carInteriorLight.color = Color.red;
            carInteriorLight.intensity = 1f;
            if (lightUpdateEffect != null) lightUpdateEffect.Play();
        }

        mileDisplay.text = "YOU LOST!";
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
