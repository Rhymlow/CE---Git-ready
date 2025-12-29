using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{
    [Header("Skybox Materials")]
    public Material daySkybox;
    public Material nightSkybox;

    [Header("Directional Light (Sun Source)")]
    public Light sunLight;

    private bool isNight = false;

    void Update()
    {
        if (sunLight == null || daySkybox == null || nightSkybox == null)
            return;

        // Usamos el eje Y del vector forward para ver si el sol está arriba (> 0) o abajo (< 0)
        float sunHeight = sunLight.transform.forward.y;

        // Cambiamos solo si hay un cruce real (de día a noche o viceversa)
        if (!isNight && sunHeight < 0f)
        {
            isNight = true;
            RenderSettings.skybox = nightSkybox;
            DynamicGI.UpdateEnvironment();
            Debug.Log(" Cambió a Skybox de noche");
        }
        else if (isNight && sunHeight >= 0f)
        {
            isNight = false;
            RenderSettings.skybox = daySkybox;
            DynamicGI.UpdateEnvironment();
            Debug.Log(" Cambió a Skybox de día");
        }
    }
}
