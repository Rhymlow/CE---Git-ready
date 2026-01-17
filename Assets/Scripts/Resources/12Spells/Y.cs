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
        if (!GameSystem.isAnItemPickedUp)
        {
            GameSystem.ActivateConstructionMode();
            Destroy(this.transform.gameObject);
        }
        else
        {
            GameSystem.UnequipItem();
            Destroy(this.transform.gameObject);
        }
    }
}
