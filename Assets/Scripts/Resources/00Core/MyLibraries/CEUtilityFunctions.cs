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
        List<GameObject> SavedGameObjects;

        public IslandData()
        {
            SavedGameObjects = new List<GameObject>();
        }

        public IslandData(List<GameObject> tSavedGameObjects)
        {
            SavedGameObjects = tSavedGameObjects;
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
}


