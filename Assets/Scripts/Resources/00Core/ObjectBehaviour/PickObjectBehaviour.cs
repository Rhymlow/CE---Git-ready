using Unity.VisualScripting;
using UnityEngine;

public class PickObjectBehaviour : MonoBehaviour
{
    public Material pickeableObjectMaterial;
    public bool onTriggersActivated = true;
    public bool onTriggerStaySwitch = false;
    public Vector3 scaleModifier = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 positionModifier = new Vector3(0, 0, 0);
    public Vector3[] raycastposition = new Vector3[4];


    private void Start()
    {
        pickeableObjectMaterial = transform.parent.GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameSystem.constructionModeActivated)
        {
            if (GameSystem.pickedUpObject && other.transform.tag == "Terrain")
            {
                GameSystem.pickedUpObject.GetComponent<MeshRenderer>().material = GameSystem.highlightedMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameSystem.constructionModeActivated)
        {
            if (other.gameObject == GameSystem.player && GameSystem.pickeableObjects.Contains(transform.parent.gameObject) && onTriggersActivated)
            {
                GameSystem.pickeableObjects.RemoveAll(x => x == transform.parent.gameObject);
                if (transform.parent.GetComponent<MeshRenderer>().material != pickeableObjectMaterial)
                {
                    transform.parent.GetComponent<MeshRenderer>().material = pickeableObjectMaterial;
                }
            }
            if (GameSystem.pickedUpObject && other.transform.tag == "Terrain")
            {
                GameSystem.pickedUpObject.GetComponent<MeshRenderer>().material = GameSystem.highlightedWrongMaterial;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameSystem.constructionModeActivated)
        {
            if (other.gameObject == GameSystem.player && !GameSystem.pickeableObjects.Contains(transform.parent.gameObject) && onTriggersActivated)
            {
                GameSystem.pickeableObjects.Add(transform.parent.gameObject);
            }
            if(GameSystem.pickedUpObject && other.name != "default" && !onTriggersActivated && other.transform.tag != "Terrain" && onTriggerStaySwitch == false)
            {
                GameSystem.pickedUpObject.GetComponent<MeshRenderer>().material = GameSystem.highlightedWrongMaterial;
                onTriggerStaySwitch = true;
            }
        }
    }
}
