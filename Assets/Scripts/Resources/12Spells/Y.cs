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
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");
            foreach (GameObject obj in crops)
            {
                obj.transform.Find("default").transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;
            }
            foreach (GameObject obj in items)
            {
                obj.transform.Find("default").transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;
            }
            Destroy(this.transform.gameObject);
        }
    }
}
