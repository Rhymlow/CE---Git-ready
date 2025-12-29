using UnityEngine;

[ExecuteAlways]
public class SolarBillboard : MonoBehaviour
{
    public Light sunLight;
    public float distance = 100f;

    void Update()
    {
        if (sunLight == null || Camera.main == null)
            return;

        // Dirección opuesta al frente de la luz
        Vector3 sunDir = -sunLight.transform.forward;

        // Posicionar el sol lejos en esa dirección
        transform.position = sunLight.transform.position + sunDir * distance;

        // Siempre mira a la cámara
        transform.LookAt(Camera.main.transform);
    }
}
