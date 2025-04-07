using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
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

    public void UpdateLevel(){
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
