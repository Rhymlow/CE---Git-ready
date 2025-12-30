using JetBrains.Annotations;
using UnityEngine;

namespace CEutilities
{
    [System.Serializable]
    public class GameData
    {
        public string gameID;
        public string playerName;
        public Vector3 playerSpawnPoint;
        public string[] playerInventory;

        public GameData()
        {
            gameID = "null";
            playerName = "null";
            playerSpawnPoint = new Vector3();
            playerInventory = null;
        }
        public GameData(GameData gameData)
        {
            gameID = gameData.gameID;
            playerName = gameData.playerName;
            playerSpawnPoint = gameData.playerSpawnPoint;
            playerInventory = gameData.playerInventory;
        }
    }

    public class IslandData
    {  
    }
}


