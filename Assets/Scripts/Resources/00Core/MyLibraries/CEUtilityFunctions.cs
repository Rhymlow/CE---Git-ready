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
    }

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

    public class PlayerData
    {
        public Vector3 playerSpawnPoint;
        
        public PlayerData()
        {
            playerSpawnPoint = new Vector3();
        }

        public PlayerData(Vector3 tPlayerSpawnPoint)
        {
            playerSpawnPoint = tPlayerSpawnPoint;
        }
    }
}


