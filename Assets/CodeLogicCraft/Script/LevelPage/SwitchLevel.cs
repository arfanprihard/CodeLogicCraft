using UnityEngine;
using UnityEngine.UI;

public class SwitchLevel : MonoBehaviour
{
    public GameObject[] tingkatKesulitan;

    public Button leftLevelbt;
    public Button rightLevelbt;

    private int indexTingkatKesulitanYangAktif;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftLevelbt.onClick.AddListener(OnClickLeftLevelBT);
        rightLevelbt.onClick.AddListener(OnClickRightLevelBT);
    }

    void Update()
    {
        for (int i = 0; i < tingkatKesulitan.Length; i++)
        {
            if (tingkatKesulitan[i].activeInHierarchy)
            {
                indexTingkatKesulitanYangAktif = i;
            }
        }
        if (indexTingkatKesulitanYangAktif == 0)
        {
            leftLevelbt.gameObject.SetActive(false);
            rightLevelbt.gameObject.SetActive(true);
        }
        else if (indexTingkatKesulitanYangAktif == 3)
        {
            leftLevelbt.gameObject.SetActive(true);
            rightLevelbt.gameObject.SetActive(false);
        }
        else
        {
            leftLevelbt.gameObject.SetActive(true);
            rightLevelbt.gameObject.SetActive(true);
        }
    }

    void OnClickLeftLevelBT()
    {

        if (indexTingkatKesulitanYangAktif > 0)
        {
            tingkatKesulitan[indexTingkatKesulitanYangAktif - 1].SetActive(true);
            tingkatKesulitan[indexTingkatKesulitanYangAktif].SetActive(false);

            PlayerPrefs.SetInt("TingkatKesulitan", indexTingkatKesulitanYangAktif);
        }

    }

    void OnClickRightLevelBT()
    {
        if (indexTingkatKesulitanYangAktif < 3)
        {
            tingkatKesulitan[indexTingkatKesulitanYangAktif + 1].SetActive(true);
            tingkatKesulitan[indexTingkatKesulitanYangAktif].SetActive(false);
            PlayerPrefs.SetInt("TingkatKesulitan", indexTingkatKesulitanYangAktif + 2);
        }
    }
}
