using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveManager : MonoBehaviour
{
    // Create a save data container to store data
    public static List<GameData> dataList = new List<GameData>();

    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// Save player progress
    /// </summary>
    public static void SaveGame(GameData currentGameData)
    {
        string savePath = Application.persistentDataPath + "/Save/";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        dataList.Clear();
        dataList.Add(currentGameData);
        FileStream file = File.Create(savePath + "snake.sav");
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, dataList);
        file.Close();

        print("Saved.");
    }

    /// <summary>
    /// Load the saved files
    /// </summary>
    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/Save/snake.sav"))
        {
            FileStream file = new FileStream(Application.persistentDataPath + "/Save/snake.sav", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            dataList = bf.Deserialize(file) as List<GameData>;
            file.Close();

            print("Loaded.");
        }
        else
        {
            print("Cannot found the save file.");
        }
    }

}

[System.Serializable]
public class GameData
{
    // Info
    public static GameData currentGame = new GameData();
    public string UserName;
    public int Coin;
    public int HighestScore;

    // Skin
    public Dictionary<string, bool> BuySkin;
    public string CurrentUsedSkin;

    // Ability
    public int maxVision;
    public int currentVision;
}