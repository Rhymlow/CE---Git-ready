using UnityEngine;

public class G : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        EffectTest();
        
    }

    void EffectTest()
    {
        GameSystem.player.GetComponent<PlayerMovement>().UpdatePlayerGravitySpeed(10.0f);
        Destroy(this.transform.gameObject);
    }
}
