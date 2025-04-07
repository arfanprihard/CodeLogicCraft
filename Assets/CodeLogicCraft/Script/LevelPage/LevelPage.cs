using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelPage : MonoBehaviour
{
    [System.Serializable]
    public class PerTingkatKesulitan
    {
        public GameObject tingkatKesulitan;
        public Button[] levels = new Button[5];
        public GameObject[] locklevels = new GameObject[5];
    }
    public TMP_Text topText;
    public TMP_Text totalBintangTxt;
    public Button backbt;
    public PerTingkatKesulitan[] perTingkatKesulitans;

    void Start()
    {
        totalBintangTxt.text = SaveLoadSystem.Instance.GetTotalBintang() + "/60";
        backbt.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        int tingkatanKesulitanSekarang = PlayerPrefs.GetInt("TingkatKesulitan");

        // Loop setiap PerLevel dalam array perlevel
        int indexLevel = 0;
        foreach (PerTingkatKesulitan perTingkatKesulitan in perTingkatKesulitans)
        {
            if (indexLevel == tingkatanKesulitanSekarang - 1)
            {
                perTingkatKesulitan.tingkatKesulitan.SetActive(true);
            }
            else
            {
                perTingkatKesulitan.tingkatKesulitan.SetActive(false);
            }
            for (int i = 0; i < perTingkatKesulitan.levels.Length; i++)
            {
                int index = i + 1;
                perTingkatKesulitan.levels[i].onClick.AddListener(() => OnClickLevel(GetIndexKesulitan(perTingkatKesulitan.tingkatKesulitan), index));

                if (ApakahLevelSebelumnyaAdaBintang(GetIndexKesulitan(perTingkatKesulitan.tingkatKesulitan), index))
                {
                    int banyakBintang = SaveLoadSystem.Instance.GetBintang(GetIndexKesulitan(perTingkatKesulitan.tingkatKesulitan), index);
                    perTingkatKesulitan.levels[i].transform.parent.gameObject.SetActive(true);
                    perTingkatKesulitan.locklevels[i].SetActive(false);
                    PengisianBintangPerLevel(perTingkatKesulitan.levels[i].transform.parent.gameObject, banyakBintang);
                }
                else
                {
                    perTingkatKesulitan.levels[i].transform.parent.gameObject.SetActive(false);
                    perTingkatKesulitan.locklevels[i].SetActive(true);
                }
            }
            indexLevel++;
        }
    }

    void Update()
    {
        int tingkatanKesulitanSekarang = PlayerPrefs.GetInt("TingkatKesulitan");
        if (tingkatanKesulitanSekarang == 1 || tingkatanKesulitanSekarang == 0)
        {
            topText.text = "Dasar - Pergerakan";
        }
        else if (tingkatanKesulitanSekarang == 2)
        {
            topText.text = "Perulangan - Kode Berulang";
        }
        else if (tingkatanKesulitanSekarang == 3)
        {
            topText.text = "Percabangan - Pilihan Jalan";
        }
        else if (tingkatanKesulitanSekarang == 4)
        {
            topText.text = "Method - Panggil Bantuan";
        }
    }

    void OnClickLevel(int tingkatKesulitan, int level)
    {
        Debug.Log($"Tingkat Kesulitan: {tingkatKesulitan}, Level: {level} diklik");
        PlayerPrefs.SetInt("TingkatKesulitan", tingkatKesulitan);
        PlayerPrefs.SetInt("Level", level);
        SceneManager.LoadScene("InGame");
    }

    private int GetIndexKesulitan(GameObject tingkatKesulitan)
    {
        string namaKesulitan = tingkatKesulitan.name; // Ambil nama objek

        switch (namaKesulitan)
        {
            case "Mudah": return 1;
            case "Sedang": return 2;
            case "Sulit": return 3;
            case "Ekstrem": return 4;
            default: return 0;
        }
    }


    private bool ApakahLevelSebelumnyaAdaBintang(int tingkatKesulitan, int level)
    {
        if (level == 1 && tingkatKesulitan == 1) return true;
        if (level > 1)
        {
            level -= 1;
        }
        else if (level <= 1)
        {
            tingkatKesulitan -= 1;
            level = 5;
        }

        return SaveLoadSystem.Instance.GetBintang(tingkatKesulitan, level) != 0;
    }

    private void PengisianBintangPerLevel(GameObject parent, int banyakBintang)
    {
        for (int i = 0; i < 3; i++)
        {
            parent.transform.GetChild(2).GetChild(i).gameObject.SetActive(i < banyakBintang);
        }
    }
}
