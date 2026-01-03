using UnityEngine;

public class ObjectEffect10010101 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.LogError("Se ejecuto el efecto de la cama");
        Destroy(this.transform.gameObject);
    }
}