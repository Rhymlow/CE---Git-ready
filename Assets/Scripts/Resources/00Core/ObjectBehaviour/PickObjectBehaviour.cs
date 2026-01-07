using Unity.VisualScripting;
using UnityEngine;

public class PickObjectBehaviour : MonoBehaviour
{
    public string usableItem = "null";
    public bool isUsableObject = false;
    bool onTriggersActivated = true;
    Material pickeableObjectMaterial;
    bool isUsableObjectSelected = false;

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
        Instantiate(Resources.Load(this.transform.parent.transform.parent.GetComponent<PrefabPath>().prefabpath + "ObjectEffect"));
    }

    public bool GetIsUsableObjectSelected()
    {
        return isUsableObjectSelected;
    }

    public void SetIsUsableObjectSelected(bool tisUsableObjectSelected)
    {
        isUsableObjectSelected = tisUsableObjectSelected;
    }

    public Material GetPickeableObjectMaterial()
    {
        return pickeableObjectMaterial;
    }

    public void SetOnTriggersActivated(bool tOnTriggersActivated)
    {
        onTriggersActivated = tOnTriggersActivated;
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
                isUsableObjectSelected = false;
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
        else if (!GameSystem.constructionModeActivated && isUsableObject == true && GameSystem.player.GetComponent<PlayerMovement>().itemEquipped == usableItem /*|| u*/)
        {
            if (other.gameObject == GameSystem.player && !GameSystem.usableObjects.Contains(transform.parent.gameObject))
            {
                GameSystem.usableObjects.Add(transform.parent.gameObject);
            }
        }
    }
}
