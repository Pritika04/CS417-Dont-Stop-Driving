using UnityEngine;
using System.Collections;

public class LightToggle : MonoBehaviour
{
    public Light carLight;
    public ParticleSystem burstParticles;

    private bool headlightsOn = false;
    private bool hazardsOn = false;

    private Coroutine hazardBlinkRoutine;

    // -------------------------
    // HEADLIGHT SWITCH
    // -------------------------
    public void ToggleHeadlights()
    {
        headlightsOn = !headlightsOn;
        UpdateLight();

        if (burstParticles != null)
            burstParticles.Play();
    }

    // -------------------------
    // HAZARD SWITCH
    // -------------------------
    public void ToggleHazards()
    {
        hazardsOn = !hazardsOn;

        if (hazardsOn)
        {
            hazardBlinkRoutine = StartCoroutine(HazardBlink());
        }
        else
        {
            if (hazardBlinkRoutine != null)
                StopCoroutine(hazardBlinkRoutine);

            hazardBlinkRoutine = null;
        }

        UpdateLight();

        if (burstParticles != null)
            burstParticles.Play();
    }

    // -------------------------
    // CORE LIGHT STATE
    // -------------------------
    private void UpdateLight()
    {
        if (carLight == null) return;

        bool shouldBeOn = headlightsOn || hazardsOn;
        carLight.enabled = shouldBeOn;

        if (!shouldBeOn)
            return;

        // color priority: hazards > headlights
        carLight.color = hazardsOn ? Color.red : Color.yellow;
    }

    // -------------------------
    // BLINKING HAZARDS
    // -------------------------
    private IEnumerator HazardBlink()
    {
        while (hazardsOn)
        {
            if (carLight != null)
                carLight.enabled = !carLight.enabled;

            yield return new WaitForSeconds(0.5f);
        }

        // ensure final state is correct when exiting
        UpdateLight();
    }
}