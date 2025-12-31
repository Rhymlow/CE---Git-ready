using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using CEutilities;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{

    public static GameManager gameManager;
    public static AddManager addManager;
    public static GameObject player;
    public static AudioSource soundEffectUI;
    public static List<GameObject> pickeableObjects = new List<GameObject>();
    public static AudioClip[] spellBells = new AudioClip[17];
    public static Material highlightedMaterial;
    public static Material highlightedWrongMaterial;
    public static GameObject highlightedObject;
    public static GameObject pickedUpObject;
    public static GameObject pickedUpParentObject; // este debe de llenarse si se trata de un objeto que tnega un default
    public static GameObject cameraOrbit;

    #region FILTER ROOTS

    

    

    #endregion

    #region MANAGERS

    public static void ShowRewarded()
    {
        addManager.ShowAd();
    }

    #endregion

    #region DEBUGMODE

    public static bool DebugMode = false;

    public static void EnableDebugMode(bool ActivateDebugMode)
    {
        DebugMode = ActivateDebugMode;
    }

    #endregion

    #region SAVE/LOAD GAME

    public static string gameID;
    public static GameData gameData;

    public static void SaveGame()
    {
        FillFilteredRoots();
        if (!Directory.Exists(Application.persistentDataPath + "/" + gameID))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + gameID);
            if (!Directory.Exists(Application.persistentDataPath + "/" + gameID + "/WorldData"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/" + gameID + "/WorldData");
            }
        }
        SaveSystem.SaveGame(new GameData(gameID,new PlayerData(player.transform.position), new IslandData(filteredRoots)));
        filteredRoots = new List<GameObject>();
    }

    public static void LoadGame()
    {
        gameData = SaveSystem.LoadGame();
        if (gameData != null)
        {
            //Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No se pudo cargar el archivo");
        }
    }

    public static List<GameObject> filteredRoots = new List<GameObject>();
    public static void FillFilteredRoots()
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject go in rootObjects)
        {
            if (go.CompareTag("System") || go.CompareTag("Terrain") || go.CompareTag("Player"))
                continue;

            filteredRoots.Add(go);
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
            Debug.Log("Mensaje enviado exitosamente a Discord.");
        }
        else
        {
            Debug.LogWarning($"Error al enviar mensaje: {request.error}");
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
                highlightedObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().ontriggersActivated = false;
                pickeableObjects.RemoveAll(x => x == highlightedObject);
                pickedUpParentObject = highlightedObject.transform.parent.gameObject;
                pickedUpObject = highlightedObject;
                pickedUpObject.GetComponent<MeshRenderer>().material = highlightedWrongMaterial;
                pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = false;
                highlightedObject = null;
            }
            else
            {
                highlightedObject.transform.SetParent(player.transform);
                highlightedObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().ontriggersActivated = false;
                pickeableObjects.RemoveAll(x => x == highlightedObject);
                pickedUpObject = highlightedObject;
                pickedUpObject.GetComponent<MeshRenderer>().material = highlightedWrongMaterial;
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
            pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().ontriggersActivated = true;
            pickedUpObject.GetComponent<MeshRenderer>().material = pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().pickeableObjectMaterial;
            pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;
            pickedUpParentObject = null;
            pickedUpObject = null;
        }
        else if(pickedUpObject.GetComponent<MeshRenderer>().sharedMaterial == highlightedMaterial)
        {
            pickedUpObject.transform.SetParent(null);
            pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().ontriggersActivated = true;
            pickedUpObject.GetComponent<MeshRenderer>().material = pickedUpObject.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().pickeableObjectMaterial;
            pickedUpObject.transform.Find("PickeableObject").GetComponent<SphereCollider>().enabled = true;
            pickedUpObject = null;
        }
    }

    public static void HighlightPickeableObject()
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
                    obj.GetComponent<MeshRenderer>().material = obj.transform.Find("PickeableObject").GetComponent<PickObjectBehaviour>().pickeableObjectMaterial;
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

    #endregion
}

#region Data

[System.Serializable]
public class DiscordMessage
{
    public string content;
}
#endregion