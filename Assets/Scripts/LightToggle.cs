using UnityEngine;

public class LightToggle : MonoBehaviour
{
    public Light carLight;
    public ParticleSystem burstParticles;

    private bool isOn = false;

    public void ToggleLight()
    {
        isOn = !isOn;
        carLight.enabled = isOn;

        if (burstParticles != null)
            burstParticles.Play();
    }
}
