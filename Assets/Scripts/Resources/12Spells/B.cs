using UnityEngine;

public class B : MonoBehaviour
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
        if (GameSystem.pickedUpObject)
        {
            GameSystem.PlacePickedUpObject();
        }
        else
        {
            GameSystem.PickUpHighlightedObject();
        }
        Destroy(this.transform.gameObject);
    }
}
