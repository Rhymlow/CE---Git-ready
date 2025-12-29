using UnityEngine;

public class GroundChecker : GameSystem
{
    public float rayLength = 0.3f;
    public LayerMask groundLayer;
    private bool isOnGround;

    void Update()
    {
        // Centro del objeto + offset de altura
        Vector3 center = transform.position + new Vector3(0, 0.0f, 0);

        // Posiciones de los 4 puntos alrededor del centro, separados por 0.5 unidades
        Vector3[] raycastPoints = new Vector3[]
        {
            center + new Vector3(0.5f, 0, 0),  // Derecha
            center + new Vector3(-0.5f, 0, 0), // Izquierda
            center + new Vector3(0, 0, 0.5f),  // Adelante
            center + new Vector3(0, 0, -0.5f), // Atrás
        };

        // Bandera para saber si todos los raycasts tocan suelo
        bool allTouchingGround = true;

        foreach (var point in raycastPoints)
        {
            Ray ray = new Ray(point, Vector3.down);
            bool hit = Physics.Raycast(ray, rayLength, groundLayer);

            // Dibuja los rayos en verde si tocan el suelo, rojo si no
            Debug.DrawRay(point, Vector3.down * rayLength, hit ? Color.green : Color.red);

            if (!hit)
            {
                allTouchingGround = false;
            }
        }

        isOnGround = allTouchingGround;

        // Opcional: puedes imprimir o usar `isOnGround` como quieras
        Debug.Log("¿Se puede colocar el objeto? " + isOnGround);
    }
}
