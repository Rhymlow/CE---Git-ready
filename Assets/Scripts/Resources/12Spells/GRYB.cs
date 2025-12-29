using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRYB : GameSystem
{
    /*GameObject catedral;
    Vector3 playerPosition;
    Quaternion playerRotation;
    bool isCatedralSpawning = false;

    void Start()
    {
        // Instanciar el prefab
        catedral = Instantiate(Resources.Load("12/GRYB/Catedral")) as GameObject;
        if (catedral != null)
        {
            // Obtener la posición y rotación inicial del jugador
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                playerPosition = player.transform.position;
                playerRotation = player.transform.rotation;

                // Colocar la catedral frente al jugador
                catedral.transform.position = GetPositionInFront(playerPosition, playerRotation, 150f);
                catedral.transform.rotation = playerRotation;
                catedral.transform.position += new Vector3(0f, -80.5f, 0f);
                isCatedralSpawning = true;
            }
        }
    }

    void Update()
    {
        if (catedral != null && isCatedralSpawning == true)
        {
            // Animar la catedral si está debajo del nivel del suelo
            if (catedral.transform.position.y < 0)
            {
                catedral.transform.position += new Vector3(0f, 5f * Time.deltaTime, 0f);
            }
        }
    }

    /// <summary>
    /// Calcula una posición frente a un objeto basado en su posición, rotación y una distancia.
    /// </summary>
    Vector3 GetPositionInFront(Vector3 position, Quaternion rotation, float distance)
    {
        // Calcula el frente del jugador usando la rotación
        Vector3 forward = rotation * Vector3.right;
        return position - forward * distance;
    }*/
}
