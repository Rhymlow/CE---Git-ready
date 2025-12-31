using UnityEngine;
using System.IO;
using CEutilities;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region SAVE/LOAD GAME DATA

    public static void SaveGame(GameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + GameSystem.gameID + "/gameData.ssl";
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData data = new GameData(gameData);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string camino = Application.persistentDataPath + "/" + GameSystem.gameID + "/gameData.ssl";
        if (File.Exists(camino))
        {
            BinaryFormatter formateador = new BinaryFormatter();
            FileStream Corriente = new FileStream(camino, FileMode.Open);
            GameData datos = formateador.Deserialize(Corriente) as GameData;
            Corriente.Close();
            return datos;
        }
        else
        {
            Debug.LogError("El archivo de salvado no se encontro en " + camino);
            return null;
        }
    }

    #endregion
}
