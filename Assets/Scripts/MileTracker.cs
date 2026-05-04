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

    public void RegisterLoop() {
        currentLoops++;
        if (currentLoops >= loopsPerMile) {
            totalMiles++;
            currentLoops = 0;
            MarkMileDistinctly();
        }
    }


    // keep track of key states
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

        // If NO key is held → -1
        if (!anyKeyGrabbed)
            return -1;

        // If correct key is held BUT other keys are also held → -2
        if (correctKeyGrabbed)
        {
            int keysHeld = 0;
            if (redKeyGrabbed) keysHeld++;
            if (blueKeyGrabbed) keysHeld++;
            if (greenKeyGrabbed) keysHeld++;

            if (keysHeld > 1)
                return -2; // correct + wrong = penalty

            return 0; // correct key ONLY
        }

        // If only wrong keys are held → -2
        return -2;
    }





    void MarkMileDistinctly() {
        mileDisplay.text = "MILE: " + totalMiles + " / 3";

        if (textUpdateEffect != null) textUpdateEffect.Play();
        if (mileTransitionEffect != null) mileTransitionEffect.Play();

        int penalty = 0;

        switch (totalMiles) {

            case 1:
                // REQUIRE RED KEY
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
                // REQUIRE BLUE KEY
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
                // REQUIRE GREEN KEY
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

        // Priority: if multiple keys are grabbed, pick one in a consistent order
        if (redKeyGrabbed)
        {
            carInteriorLight.color = Color.red;
            return;
        }

        if (blueKeyGrabbed)
        {
            carInteriorLight.color = Color.blue;
            return;
        }

        if (greenKeyGrabbed)
        {
            carInteriorLight.color = Color.green;
            return;
        }

        // If no key is held, optional default
        carInteriorLight.color = Color.white;
    }


    void Update()
    {
        UpdateInteriorLightColorBasedOnKey();
    }



    void WinGame() {
        // change interior light color
        if (carInteriorLight != null) {
            carInteriorLight.color = Color.green;
            carInteriorLight.intensity = 1f;
            if (lightUpdateEffect != null) {
                lightUpdateEffect.Play();
            }
        }

        mileDisplay.text = "YOU SURVIVED!";
        StartCoroutine(WaitAndQuit());
    }

    void LoseGame() {
        // change interior light color
        if (carInteriorLight != null) {
            carInteriorLight.color = Color.red;
            carInteriorLight.intensity = 1f;
            if (lightUpdateEffect != null) {
                lightUpdateEffect.Play();
            }
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
