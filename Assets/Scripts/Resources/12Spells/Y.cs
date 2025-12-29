using UnityEngine;

public class Y : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EffectTest();
    }

    void EffectTest()
    {
        //Efecto que debe hacer este script
        Destroy(this.transform.gameObject);
    }
}
