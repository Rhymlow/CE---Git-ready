using CEutilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static GameManager;

public class GameSystem : MonoBehaviour
{
    #region Variables

    public static GameManager gameManager;
    public static AddManager addManager;
    public static GameObject player;
    public static AudioSource soundEffectUI;
    public static List<GameObject> pickeableObjects = new List<GameObject>();
    public static List<GameObject> usableObjects = new List<GameObject>();
    public static AudioClip[] spellBells = new AudioClip[17];
    public static Material highlightedMaterial;
    public static Material highlightedWrongMaterial;
    public static GameObject highlightedObject;
    public static GameObject highlightedUsableObject;
    public static GameObject pickedUpObject;
    public static GameObject pickedUpParentObject; // este debe de llenarse si se trata de un objeto que tenga un default
    public static bool isAnItemPickedUp;
    public static GameObject itemEquipped;
    public static bool enableTestAd;
    public static int islandDay;
    public static GameObject cameraOrbit;

    #endregion

    #region EXPERIMENTAL STUFF

    /// <summary>
    /// This ornly reset the usable objects right know, in the future can reset to the pickeableObjects and restore to the original set up, then you need to update teh set highlithedusable to works with all the types of usablegameobjects to diferentiate if is a usable o construction mode or all the other variations.
    /// </summary>
    public static void ResetAlTheArrays()
    {
        foreach (GameObject obj in usableObjects)
        {
            obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetIsUsableObjectHighlighted(false);
            obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetHighlightedUsable();
        }
        DebugLog("All the UsableObjets was set up like no highlithed to erase the list and no usableobject still highlighted.", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
        usableObjects = new List<GameObject>();
        /*foreach (GameObject obj in pickeableObjects)
        {
            obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetIsUsableObjectHighlighted(false);
            obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetHighlightedUsable();
        }*/
    }

    public static void EnableAllSphereColliders(bool enable)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");
        if (enable)
        {
            foreach (GameObject obj in crops)
            {
                obj.transform.Find("default").transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;

            }
            DebugLog("All the Crops SphereColliders was enabled", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
            foreach (GameObject obj in items)
            {
                obj.transform.Find("default").transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;
            }
            DebugLog("All the Items SphereColliders was disabled", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
        }
        else
        {
            foreach (GameObject obj in crops)
            {
                obj.transform.Find("default").transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = false;

            }
            DebugLog("All the Crops SphereColliders was enabled", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
            foreach (GameObject obj in items)
            {
                obj.transform.Find("default").transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = false;
            }
            DebugLog("All the Items SphereColliders was disabled", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
        }
    }

    public static void UpdateDay()
    {
        islandDay++;
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject go in rootObjects)
        {
            if (!go.CompareTag("Crop"))
                continue;

            go.GetComponent<CropBehaviour>().UpdateCrop();
        }
    }

    public static void EnablePlayerMovement(bool isPlayerMovementEnabled)
    {
        if (isPlayerMovementEnabled)
        {
            player.GetComponent<PlayerMovement>().playerMovementActivated = true;
        }
        else
        {
            player.GetComponent<PlayerMovement>().playerMovementActivated = false;
        }
    }

    public static void UnequipItem()
    {
        itemEquipped.transform.SetParent(null);
        itemEquipped.GetComponent<Rigidbody>().isKinematic = false;
        isAnItemPickedUp = false;
        Destroy(pickedUpParentObject);
        pickedUpParentObject = null;
        pickedUpObject = null;
        itemEquipped.transform.Find("default").transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetOnTriggersActivated(true);
        itemEquipped = null;
    }

    public static void PlacePrefabOfUsableItem()
    {
        if (pickedUpObject && pickedUpParentObject && pickedUpObject.GetComponent<MeshRenderer>().sharedMaterial == highlightedMaterial)
        {
            pickedUpParentObject.transform.SetParent(null);
            pickedUpParentObject.tag = "Crop";
            pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetOnTriggersActivated(true);
            pickedUpObject.GetComponent<MeshRenderer>().material = pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().GetPickeableObjectMaterial();
            pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;
            pickedUpObject.GetComponent<MeshCollider>().enabled = true;
            pickedUpObject.transform.Find("PickeableObject").GetComponent<BoxCollider>().size += new Vector3(0,0.5f,0);
            GameObject tGO = Instantiate(Resources.Load(pickedUpParentObject.GetComponent<PrefabPath>().prefabpath) as GameObject);
            pickedUpParentObject = tGO;
            pickedUpParentObject.tag = "Untagged";
            pickedUpObject = tGO.transform.GetChild(0).gameObject;
            pickedUpObject.GetComponent<MeshRenderer>().material = highlightedWrongMaterial;
        }
    }

    #endregion

    #region CONSTRUCTION MODE

    public static bool constructionModeActivated = false;
    public static void ActivateConstructionMode()
    {
        if(constructionModeActivated == false)
        {
            DebugLog("The process of change from Construction mode to Enabled beggings.", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
            //EnableAllSphereColliders(true);
            ResetAlTheArrays();

            constructionModeActivated = true;
            DebugLog("The Construction Mode function ends and Enable successfully.", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
        }
        else
        {
            DebugLog("The process of change from Construction mode to Disable beggings.", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
            //EnableAllSphereColliders(false);
            if (!pickedUpObject && !pickedUpParentObject)
            {
                constructionModeActivated = false;
                if (highlightedObject)
                {
                    highlightedObject.GetComponent<MeshRenderer>().material = highlightedObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().GetPickeableObjectMaterial();
                    pickeableObjects = new List<GameObject>();
                    highlightedObject = null;
                    
                }
            }
            DebugLog("The Construction Mode function ends and Disable successfully.", DebugFilter.All, DebugFilter.GameSystem, DebugFilter.GS_ConstructionMode);
        }
    }

    #endregion

    #region MANAGERS

    public static void ShowRewarded()
    {
        addManager.ShowAd();
    }

    #endregion

    #region SAVE/LOAD GAME

    public static string gameID;
    public static GameData gameData;

    public static void DeleteGame()
    {
        if (Directory.Exists(Application.persistentDataPath + "/" + gameID))
        {
            Directory.Delete(Application.persistentDataPath + "/" + gameID, true);
        }
    }

    public static void SaveGame()
    {
        filteredRoots = new List<GameObject>();
        FillFilteredRoots();
        if (!Directory.Exists(Application.persistentDataPath + "/" + gameID))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + gameID);
            if (!Directory.Exists(Application.persistentDataPath + "/" + gameID + "/WorldData"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/" + gameID + "/WorldData");
            }
        }
        SaveSystem.SaveGame(new GameData(gameID,new PlayerData(new MyVector3(player.transform.position)), new IslandData(islandDay ,filteredRoots)));
        filteredRoots = new List<GameObject>();
    }

    public static bool LoadGame()
    {
        gameData = SaveSystem.LoadGame();
        if (gameData != null)
        {
            #region RESET STATES
            DestroyFilteredRoots();
            pickeableObjects = new List<GameObject>();
            highlightedObject = null;
            pickedUpObject = null;
            pickedUpParentObject = null;
            #endregion
            #region LOAD GAME
            islandDay = gameData.islandData.islandDay;
            player.transform.position = new Vector3(gameData.playerData.playerSpawnPoint.x, gameData.playerData.playerSpawnPoint.y + 40, gameData.playerData.playerSpawnPoint.z);
            foreach (MyGameObject obj in gameData.islandData.SavedGameObjects)
            {
                Instantiate(Resources.Load(obj.prefabPath) as GameObject, new Vector3(obj.position.x, obj.position.y, obj.position.z), new Quaternion(obj.rotation.x, obj.rotation.y, obj.rotation.z, obj.rotation.w));
            }
            #endregion
            return true;
        }
        else
        {
            DebugLog("The game cannot be loaded", DebugFilter.SaveLoadSystem, DebugFilter.SaveLoadSystem);
            return false;
        }
    }

    public static List<GameObject> filteredRoots = new List<GameObject>();


    public static void FillFilteredRoots()
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject go in rootObjects)
        {
            if (go.CompareTag("System") || go.CompareTag("Terrain") || go.CompareTag("Player") || go.CompareTag("Untagged"))
                continue;

            filteredRoots.Add(go);
        }
    }

    public static void DestroyFilteredRoots()
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject go in rootObjects)
        {
            if (go.CompareTag("System") || go.CompareTag("Terrain") || go.CompareTag("Player") || go.CompareTag("Untagged"))
                continue;

            Destroy(go);
        }
    }

    #endregion

    #region DISCORD CONNECTION

    private string webhookUrl = "https://discord.com/api/webhooks/1333334694384893994/oWxNnIKekuRwT5v2w-9j3ElPKui5a2SD6xgGVb9ylcpEPAnWmr9sREkJwDOCg2Ro04pg";


    public void SendMessageToDiscord(string message)
    {
        StartCoroutine(SendDiscordMessageCoroutine(message));
    }

    private IEnumerator SendDiscordMessageCoroutine(string message)
    {
        // Cuerpo del mensaje (formato JSON)
        string jsonPayload = JsonUtility.ToJson(new DiscordMessage
        {
            content = message // El contenido del mensaje
        });

        // Crear el objeto UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(webhookUrl, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            DebugLog("The message was sended to Discord", DebugFilter.Discord);
        }
        else
        {
            DebugLog($"Error to send message to Discord: {request.error}", DebugFilter.Error);
        }
    }

    #endregion

    #region SPELL INPUT BUFFER

    public static void ExecuteInputBuffer()
    {
        gameManager.ExecuteBuffer();
    }

    public static void AddInputToBuffer(string inputToAdd, GameObject inputButton)
    {
        gameManager.AddToBuffer(inputToAdd, inputButton);
    }

    public static void CleanInputBuffer()
    {
        gameManager.CleanBuffer();
    }

    #endregion

    #region PICK OBJECT BEHAVIOUR

    public static void PickUpHighlightedObject()
    {
        if (highlightedObject && !pickedUpObject)
        {
            if (highlightedObject.transform.parent)
            {
                highlightedObject.transform.parent.gameObject.transform.SetParent(player.transform);
                highlightedObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetOnTriggersActivated(false);
                highlightedObject.GetComponent<MeshCollider>().enabled = false;
                pickeableObjects.RemoveAll(x => x == highlightedObject);
                pickedUpParentObject = highlightedObject.transform.parent.gameObject;
                pickedUpObject = highlightedObject;
                pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = false;
                pickedUpObject.transform.Find("PickeableObject").GetComponent<BoxCollider>().size -= new Vector3(0, 0.5f, 0);
                pickedUpObject.GetComponent<MeshRenderer>().material = highlightedWrongMaterial;
                highlightedObject = null;
            }
            else
            {
                highlightedObject.transform.SetParent(player.transform);
                highlightedObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetOnTriggersActivated(false);
                pickeableObjects.RemoveAll(x => x == highlightedObject);
                pickedUpObject = highlightedObject;
                pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = false;
                highlightedObject = null;
            }
        }
    }

    public static void PlacePickedUpObject()
    {
        if (pickedUpObject && pickedUpParentObject && pickedUpObject.GetComponent<MeshRenderer>().sharedMaterial == highlightedMaterial)
        {
            pickedUpParentObject.transform.SetParent(null);
            pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetOnTriggersActivated(true);
            pickedUpObject.GetComponent<MeshRenderer>().material = pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().GetPickeableObjectMaterial();
            pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;
            pickedUpObject.GetComponent<MeshCollider>().enabled = true;
            pickedUpObject.transform.Find("PickeableObject").GetComponent<BoxCollider>().size += new Vector3(0, 0.5f, 0);
            pickedUpParentObject = null;
            pickedUpObject = null;
        }
        else if(pickedUpObject.GetComponent<MeshRenderer>().sharedMaterial == highlightedMaterial)
        {
            pickedUpObject.transform.SetParent(null);
            pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetOnTriggersActivated(true);
            pickedUpObject.GetComponent<MeshRenderer>().material = pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().GetPickeableObjectMaterial();
            pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;
            pickedUpObject = null;
        }
    }

    public static void HighlightPickeableObject()
    {
        if (constructionModeActivated)
        {
            if (pickeableObjects.Count > 0 && !pickedUpObject)
            {
                highlightedObject = null;
                float minDistance = float.MaxValue;

                foreach (GameObject obj in pickeableObjects)
                {
                    if (obj == null) continue;

                    float distance = Vector3.Distance(
                        player.transform.position,
                        obj.transform.position
                    );

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        highlightedObject = obj;
                    }
                }
                foreach (GameObject obj in pickeableObjects)
                {
                    if (obj.GetComponent<MeshRenderer>().sharedMaterial == highlightedMaterial)
                    {
                        obj.GetComponent<MeshRenderer>().material = obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().GetPickeableObjectMaterial();
                    }
                }
                highlightedObject.GetComponent<MeshRenderer>().material = highlightedMaterial;
            }
            else if (pickedUpParentObject)
            {
                float invertedObjLookAt = (cameraOrbit.transform.Find("Main Camera").transform.position.y - player.transform.position.y);
                pickedUpParentObject.transform.position = player.transform.position + player.transform.right * -2.0f + new Vector3(0, -invertedObjLookAt + 2.0f, 0);
            }
            else if (pickedUpObject)
            {
                float invertedObjLookAt = (cameraOrbit.transform.Find("Main Camera").transform.position.y - player.transform.position.y);
                pickedUpObject.transform.position = player.transform.position + player.transform.right * -2.0f + new Vector3(0, -invertedObjLookAt + 2.0f, 0);
            }
            else
            {
                highlightedObject = null;
            }
        }
        else if (isAnItemPickedUp)
        {
            float invertedObjLookAt = (cameraOrbit.transform.Find("Main Camera").transform.position.y - player.transform.position.y);
            pickedUpParentObject.transform.position = player.transform.position + player.transform.right * -2.0f + new Vector3(0, -invertedObjLookAt + 2.0f, 0);
        }
    }

    #endregion

    #region USABLE OBJECT BEHAVIOUR

    public static void HighlightUsableObject()
    {
        if (!constructionModeActivated)
        {
            if (usableObjects.Count > 0)
            {
                highlightedUsableObject = null;
                float minDistance = float.MaxValue;

                foreach (GameObject obj in usableObjects)
                {
                    if (obj == null) continue;

                    float distance = Vector3.Distance(
                        player.transform.position,
                        obj.transform.position
                    );

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        highlightedUsableObject = obj;
                    }
                }
                foreach (GameObject obj in usableObjects)
                {
                    if (obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().GetIsUsableObjectSelected() == true)
                    {
                        obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetIsUsableObjectHighlighted(false);
                        obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetHighlightedUsable();
                    }
                }
                highlightedUsableObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetIsUsableObjectHighlighted(true);
                highlightedUsableObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().SetHighlightedUsable();
            }
            else
            {
                highlightedUsableObject = null;
                usableObjects.Clear();
            }
        }
    }

    #endregion

    #region DEBUGMODE

    public enum DebugFilter
    {
        None,
        All,
        Error,
        SaveLoadSystem,
        Discord,
        Ads,
        GameSystem,
        GS_ConstructionMode,
        PickeableObjectBehaviour,
        POB_IsSomethingInsideBoxCollider,
        POB_Ontrigger,
    }

    public static void DebugLog(string message, params DebugFilter[] filters)
    {
        if (gameManager.DebugModeFilter == DebugFilter.None)
            return;

        if (gameManager.DebugModeFilter == DebugFilter.All)
        {
            Debug.LogWarning(message);
            return;
        }

        foreach (var f in filters)
        {
            if (gameManager.DebugModeFilter == f)
            {
                if (f == DebugFilter.Error)
                {
                    Debug.LogError(message);
                    return;
                }
                Debug.LogWarning(message);
                return;
            }
        }
    }

    #endregion

}

#region Data

[System.Serializable]
public class DiscordMessage
{
    public string content;
}
#endregion