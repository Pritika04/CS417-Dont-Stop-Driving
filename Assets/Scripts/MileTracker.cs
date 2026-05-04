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
    private int health = 5;   // actual health tracking

    [Header("Juicy Feedback")]
    public ParticleSystem mileTransitionEffect;
    public ParticleSystem textUpdateEffect;
    public ParticleSystem lightUpdateEffect;

    public TextMeshProUGUI healthDisplay;

    // controllers
public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor leftController;
public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor rightController;
    // keys
    public GameObject greenKey;
    public GameObject blueKey;
    public GameObject redKey;

    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort.TunnelingVignetteController vignette;

    public void RegisterLoop() {
        currentLoops++;
        if (currentLoops >= loopsPerMile) {
            totalMiles++;
            currentLoops = 0;
            MarkMileDistinctly();
        }
    }

    // Debug helper to see what XR thinks you're holding
    void DebugHeldObject()
    {
        var left = leftController.firstInteractableSelected;
        var right = rightController.firstInteractableSelected;

        Debug.Log($"LEFT HELD: {(left != null ? left.transform.name : "none")}");
        Debug.Log($"RIGHT HELD: {(right != null ? right.transform.name : "none")}");
    }

    // Generic helper for holding a key
    bool IsHolding(UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor controller, GameObject target)
    {
        if (controller == null || target == null)
            return false;

        var held = controller.firstInteractableSelected;
        if (held == null)
            return false;

        return held.transform.gameObject == target;
    }

    // Specific helpers
    public bool IsHoldingGreen() => IsHolding(leftController, greenKey) || IsHolding(rightController, greenKey);
    public bool IsHoldingBlue()  => IsHolding(leftController, blueKey)  || IsHolding(rightController, blueKey);
    public bool IsHoldingRed()   => IsHolding(leftController, redKey)   || IsHolding(rightController, redKey);

    // Returns: 0 = none, 1 = red, 2 = blue, 3 = green
    int GetHeldKey()
    {
        if (IsHoldingRed()) return 1;
        if (IsHoldingBlue()) return 2;
        if (IsHoldingGreen()) return 3;
        return 0;
    }

    // Health loss rules
    int CalculateHealthLoss(int requiredKey, int heldKey)
    {
        if (heldKey == requiredKey)
            return 0;   // correct key

        if (heldKey == 0)
            return 1;   // no key

        return 2;       // wrong key
    }

    void MarkMileDistinctly() {
        mileDisplay.text = "MILE: " + totalMiles + " / 3";

        if (textUpdateEffect != null) textUpdateEffect.Play();
        if (mileTransitionEffect != null) mileTransitionEffect.Play();

        DebugHeldObject(); // TEMP DEBUG

        // 1 = red, 2 = blue, 3 = green
        int requiredKey = totalMiles;
        int heldKey = GetHeldKey();

        // Set interior light color based on held key
        switch (heldKey)
        {
            case 1: carInteriorLight.color = Color.red; break;
            case 2: carInteriorLight.color = Color.blue; break;
            case 3: carInteriorLight.color = Color.green; break;
            default: carInteriorLight.color = Color.white; break;
        }

        // Health logic
        int loss = CalculateHealthLoss(requiredKey, heldKey);
        health -= loss;
        if (health < 0) health = 0;

        healthDisplay.text = "Health: " + health + "/5";

        if (health <= 0)
        {
            LoseGame();
            return;
        }

        // Mile-specific spooky stuff
        switch (totalMiles)
        {
            case 1:
                vignette.defaultParameters.apertureSize = 0.8f;
                creepyRadio.TriggerSpookyStation(1);
                break;

            case 2:
                vignette.defaultParameters.apertureSize = 0.5f;
                creepyRadio.TriggerSpookyStation(2);
                break;

            case 3:
                vignette.defaultParameters.apertureSize = 0.3f;
                creepyRadio.TriggerSpookyStation(3);
                WinGame();
                break;
        }

        if (lightUpdateEffect != null) lightUpdateEffect.Play();
    }

    void WinGame() {
        if (carInteriorLight != null) {
            carInteriorLight.color = Color.green;
            carInteriorLight.intensity = 1f;
            if (lightUpdateEffect != null) lightUpdateEffect.Play();
        }

        mileDisplay.text = "YOU SURVIVED!";
        StartCoroutine(WaitAndQuit());
    }

    void LoseGame() {
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
