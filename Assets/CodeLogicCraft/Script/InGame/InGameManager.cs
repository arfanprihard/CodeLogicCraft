using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public TMP_Text totalBintangTxt;
    public TMP_Text tingkatKesulitanTxt;
    public TMP_Text levelTxt;
    public Button backButton;
    public GameObject[] parent_tingkatKesulitan;

    [System.Serializable]
    public class MapLevel
    {
        public GameObject tingkatKesulitan;
        public GameObject[] mapPerLevels;
    }
    public MapLevel[] mapLevels;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backButton.onClick.AddListener(OnClickBackBT);
        UpdateLevel();

    }

    void Update()
    {
        totalBintangTxt.text = SaveLoadSystem.Instance.GetTotalBintang() + "/60";
        levelTxt.text = "Level " + PlayerPrefs.GetInt("Level");
        int tingkatanKesulitanSekarang = PlayerPrefs.GetInt("TingkatKesulitan");
        if (tingkatanKesulitanSekarang == 1)
        {
            tingkatKesulitanTxt.text = "Dasar - Pergerakan";
        }
        else if (tingkatanKesulitanSekarang == 2)
        {
            tingkatKesulitanTxt.text = "Perulangan - Kode Berulang";

        }
        else if (tingkatanKesulitanSekarang == 3)
        {
            tingkatKesulitanTxt.text = "Percabangan - Pilihan Jalan";
        }
        else if (tingkatanKesulitanSekarang == 4)
        {
            tingkatKesulitanTxt.text = "Method - Panggil Bantuan";
        }
    }

    public void UpdateLevel()
    {
        int tingkatKesulitan = PlayerPrefs.GetInt("TingkatKesulitan");
        int level = PlayerPrefs.GetInt("Level");

        for (int i = 0; i < parent_tingkatKesulitan.Length; i++)
        {
            if (i == tingkatKesulitan - 1)
            {
                parent_tingkatKesulitan[i].SetActive(true);
                mapLevels[i].tingkatKesulitan.SetActive(true);
                for (int j = 0; j < mapLevels[i].mapPerLevels.Length; j++)
                {
                    if (j == level - 1)
                    {
                        mapLevels[i].mapPerLevels[j].SetActive(true);
                    }
                    else
                    {
                        mapLevels[i].mapPerLevels[j].SetActive(false);
                    }
                }
            }
            else
            {
                parent_tingkatKesulitan[i].SetActive(false);
                mapLevels[i].tingkatKesulitan.SetActive(false);
                for (int j = 0; j < mapLevels[i].mapPerLevels.Length; j++)
                {
                    mapLevels[i].mapPerLevels[j].SetActive(false);
                }
            }
        }
    }

    public void OnClickBackBT()
    {
        SceneManager.LoadScene("LevelPage");
    }

}
