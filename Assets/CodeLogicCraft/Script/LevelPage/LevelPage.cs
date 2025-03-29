using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPage : MonoBehaviour
{
    [System.Serializable]
    public class PerLevel
    {
        public string tingkatKesulitan;
        public Button[] levels = new Button[5];
        public GameObject[] locklevels = new GameObject[5];
    }
    public Button backbt;
    public PerLevel[] perlevel;

    void Start()
    {
        backbt.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        // Loop setiap PerLevel dalam array perlevel
        foreach (PerLevel level in perlevel)
        {
            for (int i = 0; i < level.levels.Length; i++)
            {
                int index = i + 1;
                level.levels[i].onClick.AddListener(() => OnClickLevel(level.tingkatKesulitan, index));

                if (ApakahLevelSebelumnyaAdaBintang(GetIndexKesulitan(level.tingkatKesulitan), index))
                {
                    int banyakBintang = SaveLoadSystem.Instance.GetBintang(GetIndexKesulitan(level.tingkatKesulitan), index);
                    level.levels[i].transform.parent.gameObject.SetActive(true);
                    level.locklevels[i].SetActive(false);
                    PengisianBintangPerLevel(level.levels[i].transform.parent.gameObject, banyakBintang);
                }
                else
                {
                    level.levels[i].transform.parent.gameObject.SetActive(false);
                    level.locklevels[i].SetActive(true);
                }
            }
        }
    }

    void OnClickLevel(string tingkatKesulitan, int level)
    {
        Debug.Log($"Tingkat Kesulitan: {tingkatKesulitan}, Level: {level} diklik");
    }

    private int GetIndexKesulitan(string tingkatKesulitan)
    {
        switch (tingkatKesulitan)
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
        if (level > 1) level -= 1;
        else tingkatKesulitan -= 1;

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
