using Unity.VisualScripting;
using UnityEngine;

public class PickObjectBehaviour : MonoBehaviour
{
    public Material pickeableObjectMaterial;
    public bool onTriggersActivated = true;
    public Vector3 scaleModifier = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 positionModifier = new Vector3(0, 0, 0);
    public Vector3[] raycastposition = new Vector3[4];
    public bool isUsableObject = false;
    public bool isUsableObjectSelected = false;
    public string objectEffectPath;

    Renderer rend;
    MaterialPropertyBlock mpb;

    private void Start()
    {
        rend = this.transform.parent.GetComponent<MeshRenderer>();
        mpb = new MaterialPropertyBlock();
        pickeableObjectMaterial = transform.parent.GetComponent<MeshRenderer>().material;
    }

    public void ExecuteObjectEffect()
    {
        Debug.Log(objectEffectPath + "ObjectEffect");
        Instantiate(Resources.Load(objectEffectPath + "ObjectEffect"));
    }

    public void SetHighlightedUsable()
    {
        if (isUsableObjectSelected)
        {
            mpb.SetColor("_EmissionColor", Color.gray3);
        }
        else
        {
            mpb.SetColor("_EmissionColor", Color.black);
        }
        rend.SetPropertyBlock(mpb);
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
        else if (!GameSystem.constructionModeActivated && isUsableObject == true)
        {
            if (other.gameObject == GameSystem.player && GameSystem.usableObjects.Contains(transform.parent.gameObject))
            {
                GameSystem.usableObjects.RemoveAll(x => x == transform.parent.gameObject);
                mpb.SetColor("_EmissionColor", Color.black);
                rend.SetPropertyBlock(mpb);
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
            if (!onTriggersActivated && other.transform.tag == "Terrain")
            {
                Debug.Log(other.name);
                Debug.Log(GameSystem.pickedUpObject.transform.parent.name);
                GameSystem.pickedUpObject.GetComponent<MeshRenderer>().material = GameSystem.highlightedMaterial;
            }
            
        }
        else if (!GameSystem.constructionModeActivated && isUsableObject == true)
        {
            if (other.gameObject == GameSystem.player && !GameSystem.usableObjects.Contains(transform.parent.gameObject))
            {
                GameSystem.usableObjects.Add(transform.parent.gameObject);
            }
        }
    }
}
