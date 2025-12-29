using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmothRotation : MonoBehaviour
{
    public float rotationSpeed = 0.1f;  // Controla la velocidad de la rotación
    public float changeDirectionInterval = 2f;  // Intervalo para cambiar la dirección de rotación
    public string buttonID;

    private Quaternion targetRotation;  // La rotación hacia la que el objeto se va a mover
    private float timeSinceLastChange = 0f;  // Temporizador para cambiar la dirección

    void Start()
    {
        // Inicializamos con una rotación aleatoria
        targetRotation = Random.rotation;
    }

    void Update()
    {
        // Actualizamos el temporizador para cambiar la dirección
        timeSinceLastChange += Time.deltaTime;

        // Si ha pasado el tiempo de intervalo, cambiamos la dirección de rotación
        if (timeSinceLastChange >= changeDirectionInterval)
        {
            targetRotation = Random.rotation;  // Asignamos una nueva rotación aleatoria
            timeSinceLastChange = 0f;  // Reseteamos el temporizador
        }

        // Realizamos la rotación suave hacia la nueva rotación
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
