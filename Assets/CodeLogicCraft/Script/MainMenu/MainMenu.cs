using System.IO;
using System.Text.RegularExpressions;
using TMPro;
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
    public GameObject[] character;
    public GameObject currentCharacter;

    private bool apakahAdaData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
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
        string nameKarakter = currentCharacter.transform.GetChild(0).name;
        int indexNameKarakter = 0;
        Match match = Regex.Match(nameKarakter, @"\d+$");

        if (match.Success)
        {
            indexNameKarakter = int.Parse(match.Value);
            Debug.Log("Angka di akhir: " + indexNameKarakter);
        }
        else
        {
            Debug.Log("Tidak ada angka di akhir");
        }

        GameObject karakterSekarang = currentCharacter.transform.GetChild(0).gameObject;

        // Simpan posisi, rotasi, dan skala
        Vector3 posisi = karakterSekarang.transform.position;
        Quaternion rotasi = karakterSekarang.transform.rotation;
        Vector3 skala = karakterSekarang.transform.localScale;


        GameObject karakterBaru;
        if (indexNameKarakter <= 0)
        {
            karakterBaru = character[character.Length - 1];
        }
        else
        {
            karakterBaru = character[indexNameKarakter - 1];
        }

        // Hapus objek lama
        DestroyImmediate(karakterSekarang);

        // Buat objek baru
        GameObject newCharacter = Instantiate(karakterBaru, currentCharacter.transform);
        newCharacter.name = karakterBaru.name;
        newCharacter.transform.rotation = rotasi;
        newCharacter.transform.localScale = skala;
        newCharacter.transform.position = posisi;
    }

    private void OnclickRightCharacter()
    {
        string nameKarakter = currentCharacter.transform.GetChild(0).name;
        int indexNameKarakter = 0;
        Match match = Regex.Match(nameKarakter, @"\d+$");

        if (match.Success)
        {
            indexNameKarakter = int.Parse(match.Value);
            Debug.Log("Angka di akhir: " + indexNameKarakter);
        }
        else
        {
            Debug.Log("Tidak ada angka di akhir");
        }

        GameObject karakterSekarang = currentCharacter.transform.GetChild(0).gameObject;

        // Simpan posisi, rotasi, dan skala
        Vector3 posisi = karakterSekarang.transform.position;
        Quaternion rotasi = karakterSekarang.transform.rotation;
        Vector3 skala = karakterSekarang.transform.localScale;

        GameObject karakterBaru;
        if (indexNameKarakter < character.Length - 1)
        {
            karakterBaru = character[indexNameKarakter + 1];
        }
        else
        {
            karakterBaru = character[0];
        }

        // Hapus objek lama
        DestroyImmediate(karakterSekarang);

        // Buat objek baru
        GameObject newCharacter = Instantiate(karakterBaru, currentCharacter.transform);
        newCharacter.name = karakterBaru.name;
        newCharacter.transform.rotation = rotasi;
        newCharacter.transform.localScale = skala;
        newCharacter.transform.position = posisi;
    }
}



