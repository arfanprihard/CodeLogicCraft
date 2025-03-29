using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string nama = "";
    public int[] bintangKesulitan1 = new int[5];
    public int[] bintangKesulitan2 = new int[5];
    public int[] bintangKesulitan3 = new int[5];
    public int[] bintangKesulitan4 = new int[5];

}

public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem Instance; // Singleton instance

    public GameData gameData;

    private void Awake()
    {
        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    // ==============================
    // Save Game
    // ==============================
    public void SaveGame()
    {
        string jsonData = JsonUtility.ToJson(gameData, true); // Pretty Print
        string dataPath = GetPath();
        File.WriteAllText(dataPath, jsonData);
        Debug.Log(dataPath);

    }

    // ==============================
    // Load Game
    // ==============================
    public void LoadGame()
    {
        if (File.Exists(GetPath()))
        {
            string json = File.ReadAllText(GetPath());

            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Load berhasil!");

        }
        else
        {
            Debug.LogWarning("File savegame.json tidak ditemukan!");
        }
    }

    // ==============================
    // Simpan Bintang
    // ==============================
    public void SaveBintang(int tingkatKesulitan, int level, int bintang)
    {
        level -= 1; // menjadikan index
        if (tingkatKesulitan == 1)
        {
            gameData.bintangKesulitan1[level] = bintang;
        }
        else if (tingkatKesulitan == 2)
        {
            gameData.bintangKesulitan2[level] = bintang;
        }
        else if (tingkatKesulitan == 3)
        {
            gameData.bintangKesulitan3[level] = bintang;
        }
        else if (tingkatKesulitan == 4)
        {
            gameData.bintangKesulitan4[level] = bintang;
        }
        SaveGame();
        Debug.Log($"Bintang tersimpan: Kesulitan {tingkatKesulitan}, Level {level + 1}, Bintang {bintang}");
    }

    // ==============================
    // Mendapatkan Bintang
    // ==============================
    public int GetBintang(int tingkatKesulitan, int level)
    {
        int bintangPerLevel = 0;
        level -= 1; // menjadikan index
        if (tingkatKesulitan == 1)
        {
            bintangPerLevel = gameData.bintangKesulitan1[level];
        }
        else if (tingkatKesulitan == 2)
        {
            bintangPerLevel = gameData.bintangKesulitan2[level];
        }
        else if (tingkatKesulitan == 3)
        {
            bintangPerLevel = gameData.bintangKesulitan3[level];
        }
        else if (tingkatKesulitan == 4)
        {
            bintangPerLevel = gameData.bintangKesulitan4[level];
        }
        return bintangPerLevel;
    }

    // ==============================
    // Mendapatkan Total Bintang
    // ==============================
    public int GetTotalBintang()
    {
        int total = 0;

        for (int i = 0; i < gameData.bintangKesulitan1.Length; i++)
        {
            total += gameData.bintangKesulitan1[i];
            total += gameData.bintangKesulitan2[i];
            total += gameData.bintangKesulitan3[i];
            total += gameData.bintangKesulitan4[i];
        }

        return total;
    }

    // ==============================
    // Buat Game Baru
    // ==============================
    public void NewGame(string nama)
    {
        gameData = new GameData();
        gameData.nama = nama;
        SaveGame();
        Debug.Log("Game baru dibuat.");
    }

    // ==============================
    // Cek Apakah ada data tersimpan
    // ==============================
    public bool ApakahAdaData()
    {
        LoadGame();
        Debug.Log("Apakah Nama ada = " + gameData.nama != "");
        if (File.Exists(GetPath()) && gameData.nama != "")
        {
            return true;
        }
        else
        {
            Debug.LogWarning("File savegame.json tidak ditemukan!");
            return false;

        }
    }
}
