using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private string nextScene = "LevelPage";

    public Button mulaibarubt;
    // Pop Up di mulaibaruUI
    public GameObject mulaipopUP1;
    public GameObject mulaipopUP2;
    public TMP_InputField inputNama;
    public GameObject warningtext;

    // Tombol lanjut di mulaipopUp2
    public Button mulaibaru_lanjutbt;

    // Tombol lanjutkan
    public Button lanjutkanbt;

    // Tombol iya di lanjutkanUI
    public Button lanjutkan_iyabt;

    // Tombol iya di keluarUI
    public Button keluar_iyabt;


    public Button leftCharacter;
    public Button rightCharacter;



    private int currentIndexOfCharacterSkin = 0;
    private bool apakahAdaData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);

        apakahAdaData = SaveLoadSystem.Instance.ApakahAdaData();
        if (apakahAdaData)
        {
            lanjutkanbt.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Data tidak ditemukan");
            lanjutkanbt.gameObject.SetActive(false);
        }
        mulaibarubt.onClick.AddListener(OnClickMulaiBaruBT);
        leftCharacter.onClick.AddListener(OnclickLeftCharacter);
        rightCharacter.onClick.AddListener(OnclickRightCharacter);
        mulaibaru_lanjutbt.onClick.AddListener(OnClickLanjutMulaiBaruBT);
        lanjutkan_iyabt.onClick.AddListener(OnClickIyaLanjutBT);
        keluar_iyabt.onClick.AddListener(OnClickIyaKeluarBT);

        PlayerPrefs.SetInt("TingkatKesulitan", 1);
    }

    private void OnClickMulaiBaruBT()
    {
        inputNama.text = "";
        if (apakahAdaData)
        {
            mulaipopUP1.SetActive(true);
            mulaipopUP2.SetActive(false);
        }
        else
        {
            mulaipopUP1.SetActive(false);
            mulaipopUP2.SetActive(true);
        }
    }

    private void OnClickLanjutMulaiBaruBT()
    {

        string nama = inputNama.text.Trim(); // Trim untuk hapus spasi di awal & akhir

        if (string.IsNullOrEmpty(nama))
        {
            warningtext.SetActive(true);
        }
        else
        {
            SaveLoadSystem.Instance.NewGame(nama);
            SceneManager.LoadScene(nextScene);
        }

    }
    private void OnClickIyaLanjutBT()
    {
        SaveLoadSystem.Instance.LoadGame();
        SceneManager.LoadScene(nextScene);
    }
    private void OnClickIyaKeluarBT()
    {
        Debug.Log("Keluar dari permainan");
        Application.Quit(); // Berfungsi saat build aplikasi
    }


    private void OnclickLeftCharacter()
    {
        GameObject[] characterSkins = GameObject.FindGameObjectsWithTag("Character");
        Transform parentCharacter;

        foreach (GameObject obj in characterSkins)
        {
            int indexOfCharacterSkin = 0;
            if (obj.activeInHierarchy)
            {
                indexOfCharacterSkin = obj.transform.GetSiblingIndex();
                parentCharacter = obj.transform.parent;

                if (indexOfCharacterSkin <= 0)
                {
                    parentCharacter.GetChild(parentCharacter.childCount - 1).gameObject.SetActive(true);
                    currentIndexOfCharacterSkin = parentCharacter.childCount - 1;
                }
                else
                {
                    parentCharacter.GetChild(indexOfCharacterSkin - 1).gameObject.SetActive(true);
                    currentIndexOfCharacterSkin = indexOfCharacterSkin - 1;
                }
                obj.SetActive(false);
            }
        }
        PlayerPrefs.SetInt("SkinCharacter", currentIndexOfCharacterSkin);
    }

    private void OnclickRightCharacter()
    {
        GameObject[] characterSkins = GameObject.FindGameObjectsWithTag("Character");
        Transform parentCharacter;

        foreach (GameObject obj in characterSkins)
        {
            int indexOfCharacterSkin = 0;
            if (obj.activeInHierarchy)
            {
                indexOfCharacterSkin = obj.transform.GetSiblingIndex();
                parentCharacter = obj.transform.parent;

                if (indexOfCharacterSkin >= parentCharacter.childCount - 1)
                {
                    parentCharacter.GetChild(0).gameObject.SetActive(true);
                    currentIndexOfCharacterSkin = 0;
                }
                else
                {
                    parentCharacter.GetChild(indexOfCharacterSkin + 1).gameObject.SetActive(true);
                    currentIndexOfCharacterSkin = indexOfCharacterSkin + 1;
                }
                obj.SetActive(false);
            }
        }
        PlayerPrefs.SetInt("SkinCharacter", currentIndexOfCharacterSkin);
    }
}



