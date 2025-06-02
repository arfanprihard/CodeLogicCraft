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
        public Button story;
        public GameObject lockStory;
        public Button[] levels = new Button[5];
        public GameObject[] locklevels = new GameObject[5];
    }
    public TMP_Text topText;
    public TMP_Text totalBintangTxt;
    public Button homebt;
    public PerTingkatKesulitan[] perTingkatKesulitans;

    void Start()
    {
        totalBintangTxt.text = SaveLoadSystem.Instance.GetTotalBintang() + "/60";
        homebt.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        int tingkatanKesulitanSekarang = PlayerPrefs.GetInt("TingkatKesulitan");

        // Loop setiap PerLevel dalam array perlevel
        int indexTingkatKesulitan = 0;
        foreach (PerTingkatKesulitan perTingkatKesulitan in perTingkatKesulitans)
        {
            if (indexTingkatKesulitan == tingkatanKesulitanSekarang - 1)
            {
                perTingkatKesulitan.tingkatKesulitan.SetActive(true);
            }
            else
            {
                perTingkatKesulitan.tingkatKesulitan.SetActive(false);
            }
            int tingkatKesulitanStory = indexTingkatKesulitan + 1;
            perTingkatKesulitan.story.onClick.AddListener(() => OnClickStory(perTingkatKesulitan.story, tingkatKesulitanStory));
            if (StorySudahKebuka(tingkatKesulitanStory))
            {
                perTingkatKesulitan.story.transform.parent.gameObject.SetActive(true);
                perTingkatKesulitan.lockStory.SetActive(false);
            }
            else
            {
                perTingkatKesulitan.story.transform.parent.gameObject.SetActive(false);
                perTingkatKesulitan.lockStory.SetActive(true);
            }

            for (int i = 0; i < perTingkatKesulitan.levels.Length; i++)
            {
                int index = i + 1;
                int tingkatKesulitan = indexTingkatKesulitan + 1;

                perTingkatKesulitan.levels[i].onClick.AddListener(() => OnClickLevel(tingkatKesulitan, index));

                bool levelSudahKebuka = LevelSudahKebuka(tingkatKesulitan, index);

                if (levelSudahKebuka)
                {
                    int banyakBintang = SaveLoadSystem.Instance.GetBintang(tingkatKesulitan, index);
                    PengisianBintangPerLevel(perTingkatKesulitan.levels[i].transform.parent.gameObject, banyakBintang);
                    perTingkatKesulitan.levels[i].transform.parent.gameObject.SetActive(true);
                    perTingkatKesulitan.locklevels[i].SetActive(false);
                }
                else
                {
                    perTingkatKesulitan.levels[i].transform.parent.gameObject.SetActive(false);
                    perTingkatKesulitan.locklevels[i].SetActive(true);
                }

            }
            indexTingkatKesulitan++;
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

    void OnClickStory(Button story, int tingkatKesulitan)
    {
        string namaButton = story.gameObject.name;
        PlayerPrefs.SetInt("TingkatKesulitan", tingkatKesulitan);
        Debug.Log("Button Story di Klik dengan nama button = " + namaButton + ", Dengan tingkat Kesulitan = " + tingkatKesulitan);
        if (namaButton == "mulaiDasar")
        {
            SceneManager.LoadScene("StoryDasar");
        }
        else if (namaButton == "mulaiPerulangan")
        {
            SceneManager.LoadScene("StoryPerulangan");
        }
        else if (namaButton == "mulaiPercabangan")
        {
            SceneManager.LoadScene("StoryPercabangan");
        }
        else if (namaButton == "mulaiMethod")
        {
            SceneManager.LoadScene("StoryMethod");
        }

    }
    void OnClickLevel(int tingkatKesulitan, int level)
    {
        Debug.Log($"Tingkat Kesulitan: {tingkatKesulitan}, Level: {level} diklik");
        PlayerPrefs.SetInt("TingkatKesulitan", tingkatKesulitan);
        PlayerPrefs.SetInt("Level", level);
        SceneManager.LoadScene("InGame");
    }

    private bool StorySudahKebuka(int tingkatKesulitan)
    {
        if (tingkatKesulitan == 1) return true;

        return SaveLoadSystem.Instance.GetBintang(tingkatKesulitan - 1, 5) != 0;
    }

    private bool LevelSudahKebuka(int tingkatKesulitan, int level)
    {
        if (level > 1)
        {
            level -= 1;
            return SaveLoadSystem.Instance.GetBintang(tingkatKesulitan, level) != 0;
        }
        else
        {
            return SaveLoadSystem.Instance.GetSudahBukaStory(tingkatKesulitan);
        }
    }

    private void PengisianBintangPerLevel(GameObject parent, int banyakBintang)
    {
        for (int i = 0; i < 3; i++)
        {
            parent.transform.GetChild(2).GetChild(i).gameObject.SetActive(i < banyakBintang);
        }
    }
}
