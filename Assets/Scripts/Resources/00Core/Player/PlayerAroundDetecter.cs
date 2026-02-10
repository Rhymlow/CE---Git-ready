using UnityEngine;
using System.Collections;
using static GameSystem;

public class PlayerAroundDetecter : MonoBehaviour
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

    SphereCollider sphereCollider;
    Collider[] overlapResults = new Collider[20];

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
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

    bool IsSomethingInsideSphereCollider()
    {
        Vector3 center = sphereCollider.transform.TransformPoint(sphereCollider.center);

        float radius = sphereCollider.radius * Mathf.Max(
            sphereCollider.transform.lossyScale.x,
            sphereCollider.transform.lossyScale.y,
            sphereCollider.transform.lossyScale.z
        );

        int count = Physics.OverlapSphereNonAlloc(
            center,
            radius,
            overlapResults
        );
        DebugLog(this.transform.root + " the number of Colliders inside of this GameObject is " + count + " without filter by tag or ignore the self BoxCollider", DebugFilter.POB_IsSomethingInsideBoxCollider);

        for (int i = 0; i < count; i++)
        {
            Collider c = overlapResults[i];

            if (c == null)
            {
                DebugLog("This collider is null", DebugFilter.POB_IsSomethingInsideBoxCollider, DebugFilter.PickeableObjectBehaviour);
                continue;
            }

            if (c != null && (c.transform.root.tag == "Item" || c.transform.root.tag == "Crop"))
            {
                DebugLog("This Collider is the BoxCollider of this GameObject, so was ignored.", DebugFilter.POB_IsSomethingInsideBoxCollider, DebugFilter.PickeableObjectBehaviour);
                return true;
            }
        }
        DebugLog(this.transform.root + " don't have Items or Crops inside him.", DebugFilter.POB_IsSomethingInsideBoxCollider, DebugFilter.PickeableObjectBehaviour);
        return false; // no hay nada
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

    public Material GetPickeableObjectMaterial()
    {
        return pickeableObjectMaterial;
    }

    public void SetOnTriggersActivated(bool tOnTriggersActivated)
    {
        onTriggersActivated = tOnTriggersActivated;
    }

    public void SetIsUsableObjectHighlighted(bool tisUsableObjectSelected)
    {
        isUsableObjectSelected = tisUsableObjectSelected;
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
        if (constructionModeActivated)
        {
            if (other.gameObject == player && !pickeableObjects.Contains(transform.parent.gameObject) && onTriggersActivated)
            {
                pickeableObjects.Add(transform.parent.gameObject);
            }
        }
        else if (!constructionModeActivated && isUsableObject == true && player.GetComponent<PlayerMovement>().nameItemEquipped == usableItem)
        {
            if (other.gameObject == player && !usableObjects.Contains(transform.parent.gameObject))
            {
                usableObjects.Add(transform.parent.gameObject);
            }
        }


        DebugLog(other.transform.root.name + " was enter to the SphereCollider of " + this.gameObject.transform.root.name, DebugFilter.PickeableObjectBehaviour, DebugFilter.POB_Ontrigger);
        if (constructionModeActivated || isAnItemPickedUp)
        {
            if (pickedUpObject && other.CompareTag("Terrain"))
                terrainCount++;
            if (other.CompareTag("Item") || other.CompareTag("Crop"))
                blockingObjectsCount++;
        }
        StartCoroutine(UpdateHighlightState());
        if (pickedUpObject != null)
        {
            pickedUpObject.GetComponent<MeshRenderer>().material = highlightedMaterial;
        }

        if (constructionModeActivated)
        {
            if (other.gameObject == player && !pickeableObjects.Contains(transform.parent.gameObject) && onTriggersActivated)
            {
                pickeableObjects.Add(transform.parent.gameObject);
            }
        }
        if (!constructionModeActivated && isUsableObject == true && player.GetComponent<PlayerMovement>().nameItemEquipped == usableItem)
        {
            if (other.gameObject == player && !usableObjects.Contains(transform.parent.gameObject))
            {
                usableObjects.Add(transform.parent.gameObject);
            }
        }
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
}
