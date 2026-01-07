using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    int daysHasBeenPlanted = 0; 
    public int daysToBeMature;
    public string cropPath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCrop()
    {
        daysHasBeenPlanted++;
        if(daysHasBeenPlanted >= daysToBeMature / 4)
        {
            //Instantiate(Resources.Load(cropPath), new Vector3(0,0,0), new Quaternion(0,0,0,0));
        }
    }
}
