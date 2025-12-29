using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Tooltip("Duración del día completo en segundos (día + noche).")]
    public float fullCycleDuration = 1080f; // 12 min día + 6 min noche

    private float currentRotation = 0f;

    void Update()
    {
        // Tiempo delta normalizado a 360°
        float deltaAnglePerSecond = 360f / fullCycleDuration;

        // Determina si estamos en la parte nocturna (ángulo entre 180 y 360)
        bool isNight = currentRotation >= 180f && currentRotation < 360f;

        // Si es noche, avanza el doble de rápido
        float speedMultiplier = isNight ? 2f : 1f;

        // Aplicar rotación
        float rotationDelta = deltaAnglePerSecond * speedMultiplier * Time.deltaTime;
        currentRotation += rotationDelta;
        currentRotation %= 360f; // Limita el ángulo a [0,360)

        // Aplicar al eje X
        transform.rotation = Quaternion.Euler(currentRotation, 0f, 0f);
    }
}
