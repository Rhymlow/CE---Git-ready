using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace CEutilities
{
    [System.Serializable]
    public class GameData
    {
        public string gameID;
        public PlayerData playerData;
        public IslandData islandData;

        public GameData()
        {
            gameID = "null";
            playerData = new PlayerData();
            islandData = new IslandData();
        }
        public GameData(GameData gameData)
        {
            gameID = gameData.gameID;
            playerData = gameData.playerData;
            islandData = gameData.islandData;
        }

        public GameData(string tGameID, PlayerData tPlayerData, IslandData tIslandData)
        {
            gameID = tGameID;
            playerData = tPlayerData;
            islandData = tIslandData;
        }
    }

    [System.Serializable]
    public class IslandData
    {
        public List<MyGameObject> SavedGameObjects;

        public IslandData()
        {
            SavedGameObjects = new List<MyGameObject>();
        }

        public IslandData(List<GameObject> tSavedGameObjects)
        {
            SavedGameObjects = new List<MyGameObject>();
            MyGameObject myGameObject;
            foreach(GameObject obj in tSavedGameObjects)
            {
                myGameObject = new MyGameObject(obj);
                SavedGameObjects.Add(myGameObject);
            }
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public MyVector3 playerSpawnPoint;
        
        public PlayerData()
        {
            playerSpawnPoint = new MyVector3();
        }

        public PlayerData(MyVector3 tPlayerSpawnPoint)
        {
            playerSpawnPoint = tPlayerSpawnPoint;
        }
    }

    [System.Serializable]
    public class MyVector3
    {
        public float x;
        public float y;
        public float z;

        public MyVector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        public MyVector3(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }
    }

    [System.Serializable]
    public class MyQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public MyQuaternion()
        {
            x = 0;
            y = 0;
            z = 0;
            w = 0;
        }

        public MyQuaternion(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }
    }

    [System.Serializable]
    public class MyGameObject
    {
        public string prefabPath;
        public MyVector3 position;
        public MyQuaternion rotation;

        public MyGameObject()
        {
            prefabPath = "";
            position = new MyVector3();
            rotation = new MyQuaternion();
        }

        public MyGameObject(MyGameObject myGameObject)
        {
            prefabPath = myGameObject.prefabPath;
            position = myGameObject.position;
            rotation = myGameObject.rotation;
        }

        public MyGameObject(GameObject gameObject)
        {
            prefabPath = gameObject.GetComponent<PrefabPath>().prefabpath;
            position = new MyVector3(gameObject.transform.position);
            rotation = new MyQuaternion(gameObject.transform.rotation);
        }
    }
}


