using System.IO;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    string path;
    GameData gameData;

    private void Awake()
    {
        path = Application.persistentDataPath + "/savegame.json";
    }

    public void LoadGame(int tingkatKesulitan, int levelPerKesulitan)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            gameData = new GameData(tingkatKesulitan, levelPerKesulitan); // Buat data baru jika belum ada
            SaveGame();
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(path, json);
    }

    public void SaveBintang(int tingkatKesulitan, int levelIndex, int bintang)
    {
        if (gameData != null &&
            tingkatKesulitan < gameData.bintangPerLevel.Length &&
            levelIndex < gameData.bintangPerLevel[tingkatKesulitan].Length)
        {
            // Simpan hanya jika skor baru lebih tinggi
            gameData.bintangPerLevel[tingkatKesulitan][levelIndex] = Mathf.Max(gameData.bintangPerLevel[tingkatKesulitan][levelIndex], bintang);
            SaveGame();
        }
    }

    public int GetBintang(int tingkatKesulitan, int levelIndex)
    {
        if (gameData != null &&
            tingkatKesulitan < gameData.bintangPerLevel.Length &&
            levelIndex < gameData.bintangPerLevel[tingkatKesulitan].Length)
        {
            return gameData.bintangPerLevel[tingkatKesulitan][levelIndex];
        }
        return 0;
    }

    public int GetTotalBintang()
    {
        return gameData != null ? gameData.GetTotalBintang() : 0;
    }

    public void NewGame(int tingkatKesulitan, int levelPerKesulitan)
    {
        gameData = new GameData(tingkatKesulitan, levelPerKesulitan);
        SaveGame();
    }
}
