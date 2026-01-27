using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static GameSystem;

public class PickObjectBehaviour : MonoBehaviour
{
    public string usableItem = "null";
    public bool isUsableObject = false;
    public bool isAnItem = false;
    public string isAnItemInstanciablePath = "null";
    bool onTriggersActivated = true;
    Material pickeableObjectMaterial;
    bool isUsableObjectSelected = false;

    int terrainCount = 0;
    int blockingObjectsCount = 0;


    Renderer rend;
    MaterialPropertyBlock mpb;

    private void Awake()
    {
        rend = this.transform.parent.GetComponent<MeshRenderer>();
        mpb = new MaterialPropertyBlock();
        pickeableObjectMaterial = transform.parent.GetComponent<MeshRenderer>().material;
    }

    public void ExecuteObjectEffect()
    {
        if (isAnItem)
        {
            highlightedUsableObject.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            highlightedUsableObject.transform.parent.transform.position = player.transform.position + new Vector3(0, 2, 0);
            highlightedUsableObject.transform.parent.gameObject.transform.SetParent(player.transform);
            itemEquipped = highlightedUsableObject.transform.parent.gameObject;
            highlightedUsableObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetOnTriggersActivated(false);
            highlightedUsableObject.GetComponent<MeshCollider>().enabled = false;
            usableObjects.RemoveAll(x => x == highlightedUsableObject);
            isAnItemPickedUp = true;
            GameObject tGO = Instantiate(Resources.Load(isAnItemInstanciablePath) as GameObject);
            pickedUpParentObject = tGO;
            pickedUpParentObject.tag = "Untagged";
            pickedUpObject = tGO.transform.GetChild(0).gameObject;
            pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = false;
            pickedUpObject.GetComponent<MeshRenderer>().material = highlightedWrongMaterial;
            highlightedUsableObject = null;
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");
            foreach (GameObject obj in crops)
            {
                obj.transform.Find("default").transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = false;
            }
            foreach (GameObject obj in items)
            {
                obj.transform.Find("default").transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = false;
            }
        }
        else if (Resources.Load(this.transform.parent.transform.parent.GetComponent<PrefabPath>().prefabpath + "ObjectEffect") != null)
        {
            Instantiate(Resources.Load(this.transform.parent.transform.parent.GetComponent<PrefabPath>().prefabpath + "ObjectEffect"));
        }
    }

    IEnumerator UpdateHighlightState()
    {
        yield return new WaitForEndOfFrame();
        if (!constructionModeActivated && !isAnItemPickedUp)
            yield break;

        if (pickedUpObject == null)
            yield break;

        if (terrainCount > 0 && blockingObjectsCount == 0)
        {
            pickedUpObject.GetComponent<MeshRenderer>().material = highlightedMaterial;
        }
        else
        {
            pickedUpObject.GetComponent<MeshRenderer>().material = highlightedWrongMaterial;
        }
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
        if (constructionModeActivated || isAnItemPickedUp)
        {
            if (pickedUpObject && other.CompareTag("Terrain"))
                terrainCount++;
            if (other.CompareTag("Item") || other.CompareTag("Crop"))
                blockingObjectsCount++;
        }
        StartCoroutine(UpdateHighlightState());
        pickedUpObject.GetComponent<MeshRenderer>().material = highlightedMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        if (constructionModeActivated)
        {
            if (other.gameObject == player && pickeableObjects.Contains(transform.parent.gameObject) && onTriggersActivated)
            {
                pickeableObjects.RemoveAll(x => x == transform.parent.gameObject);
                if (transform.parent.GetComponent<MeshRenderer>().material != pickeableObjectMaterial)
                {
                    transform.parent.GetComponent<MeshRenderer>().material = pickeableObjectMaterial;
                }
            }
            if (pickedUpObject && other.CompareTag("Terrain"))
            {
                pickedUpObject.GetComponent<MeshRenderer>().material = highlightedWrongMaterial;
            }
        }
        else if (isAnItemPickedUp)
        {
            if (pickedUpObject && other.CompareTag("Terrain"))
            {
                pickedUpObject.GetComponent<MeshRenderer>().material = highlightedWrongMaterial;
            }
        }
        else if (!constructionModeActivated && isUsableObject == true)
        {
            if (other.gameObject == player && usableObjects.Contains(transform.parent.gameObject))
            {
                usableObjects.RemoveAll(x => x == transform.parent.gameObject);
                mpb.SetColor("_EmissionColor", Color.black);
                rend.SetPropertyBlock(mpb);
                isUsableObjectSelected = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (constructionModeActivated)
        {
            if (other.gameObject == player && !pickeableObjects.Contains(transform.parent.gameObject) && onTriggersActivated)
            {
                if (DebugMode)
                {
                    Debug.LogWarning(other.name);
                    Debug.LogWarning("1");
                }
                pickeableObjects.Add(transform.parent.gameObject);
            }
        }
        else if (isAnItemPickedUp)
        {
        }
        else if (!constructionModeActivated && isUsableObject == true && player.GetComponent<PlayerMovement>().nameItemEquipped == usableItem)
        {
            if (other.gameObject == player && !usableObjects.Contains(transform.parent.gameObject))
            {
                if (DebugMode)
                {
                    Debug.LogWarning(other.name);
                    Debug.LogWarning("2");
                }
                usableObjects.Add(transform.parent.gameObject);
            }
        }
    }
}
