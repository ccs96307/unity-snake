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
            print(Application.persistentDataPath);
        }
        else
        {
            print("Cannot found the save file.");

            // Initialize a new GameData object
            GameData newGameData = new GameData();

            // Info
            newGameData.UserName = "player";
            newGameData.Coin = 0;
            newGameData.HighestScore = 0;

            // Skin
            newGameData.BuySkin.Add(0, true);
            newGameData.BuySkin.Add(1, true);
            newGameData.BuySkin.Add(2, false);
            newGameData.BuySkin.Add(3, false);
            newGameData.BuySkin.Add(4, false);
            newGameData.BuySkin.Add(5, false);
            newGameData.BuySkin.Add(6, false);
            newGameData.BuySkin.Add(7, false);
            newGameData.BuySkin.Add(8, false);
            newGameData.BuySkin.Add(9, false);
            newGameData.BuySkin.Add(10, false);
            newGameData.BuySkin.Add(11, false);
            newGameData.CurrentUsedSkin = 1;

            // Ability
            newGameData.maxVision = 0;
            newGameData.currentVision = 0;

            //!!!!!Add to List!!!!
            dataList.Add(newGameData);
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
    public Dictionary<int, bool> BuySkin = new Dictionary<int, bool>();
    public int CurrentUsedSkin;

    // Ability
    public int maxVision;
    public int currentVision;
}