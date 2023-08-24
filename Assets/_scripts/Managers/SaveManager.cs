using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    string saveFile;
    GameData gameData = new GameData();
    private bool _fileExist = false;

    private void Awake()
    {
        saveFile = Application.dataPath + "/gamedata.json";
        ReadFile();
    }

    public void ReadFile()
    {
        if (File.Exists(saveFile))
        {
            string fileContents = File.ReadAllText(saveFile);
            gameData = JsonUtility.FromJson<GameData>(fileContents);
            _fileExist = true;
        }
        else
        {
            _fileExist = false;
        }
    }

    public void WriteFile()
    {
        string jsonString = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFile, jsonString);
    }

    public GameData GetData() => gameData;
    public void SetGameData(GameData newData)
    {
        gameData = newData;
        WriteFile();
    }
    public void ClearData() => File.Delete(saveFile);
}

[System.Serializable]
public class GameData
{
    public int lvl;
    public int coins = 300;
    public int[] hunters;
}