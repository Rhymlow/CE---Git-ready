using UnityEngine;
using UnityEngine.UI;

public class BR : MonoBehaviour
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
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
            //player.GetComponent<PlayerMovement>().UpdatePlayerGravitySpeed(10.0f);
            Destroy(this.transform.gameObject);
        //}
    }
}