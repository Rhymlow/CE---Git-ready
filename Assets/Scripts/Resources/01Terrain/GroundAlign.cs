using UnityEngine;

public class GroundAlign : MonoBehaviour
{
    public float offsetBelowHit = 1.0f;
    public float raycastDistance = 100f;
    public LayerMask groundLayer; // Puedes usarlo para filtrar el terreno si lo deseas

    void Start()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;

        if (Physics.Raycast(origin, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            Vector3 newPosition = hit.point - new Vector3(0, offsetBelowHit, 0);
            transform.parent.transform.position = newPosition;
            Destroy(this.gameObject);
        }
        else
        {
            Debug.LogWarning("No se detectó terreno debajo de " + gameObject.name);
        }
    }
}
