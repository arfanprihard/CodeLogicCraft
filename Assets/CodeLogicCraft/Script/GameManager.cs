using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button playButton; // Assign di Inspector
    public Button reloadButton; // Assign di Inspector
    public GameObject playMode;
    public GameObject main;
    public GameObject method;

    private MovementCharacter movementCharacter;
    private bool isPlaying = false;

    void Start()
    {
        movementCharacter = FindObjectOfType<MovementCharacter>();

        // Pastikan di awal, play aktif dan pause nonaktif
        playButton.gameObject.SetActive(true);
        reloadButton.gameObject.SetActive(false);

        // Tambah event listener ke tombol play dan pause
        playButton.onClick.AddListener(OnPlayClicked);
        reloadButton.onClick.AddListener(OnReloadClicked);
    }

    void OnPlayClicked()
    {
        if (isPlaying) return; // Mencegah duplikasi eksekusi

        isPlaying = true;

        // Nonaktifkan tombol Play dan aktifkan tombol Pause
        playButton.gameObject.SetActive(false);
        reloadButton.gameObject.SetActive(true);
        playMode.SetActive(true);

        // Jalankan aksi secara berurutan
        StartCoroutine(ExecuteButtonMain());
    }

    IEnumerator ExecuteButtonMain()
    {
        for (int i = 0; i < main.transform.childCount; i++)
        {
            Transform child = main.transform.GetChild(i);
            string name = child.name;

            if (name == "Step")
            {
                Debug.Log("Nama Child: " + name);
                movementCharacter.Langkah();
                yield return new WaitUntil(() => !movementCharacter.IsMoving());
            }
            else if (name == "HadapKiri")
            {
                movementCharacter.HadapKiri();
                yield return new WaitForSeconds(0.5f);
            }
            else if (name == "HadapKanan")
            {
                movementCharacter.HadapKanan();
                yield return new WaitForSeconds(0.5f);
            }
            else if (name == "Method")
            {
                // Menunggu hingga ExecuteButtonMethod selesai
                yield return StartCoroutine(ExecuteButtonMethod());
            }
        }

        isPlaying = false;

        // Jika Karakter sudah selesai bergerak
        playButton.gameObject.SetActive(false);
        reloadButton.gameObject.SetActive(true);
        playMode.SetActive(false);
    }

    IEnumerator ExecuteButtonMethod()
    {
        for (int i = 0; i < method.transform.childCount; i++)
        {
            Transform child = method.transform.GetChild(i);
            string name = child.name;

            if (name == "Step")
            {
                Debug.Log("Nama Child: " + name);
                movementCharacter.Langkah();
                yield return new WaitUntil(() => !movementCharacter.IsMoving());
            }
            else if (name == "HadapKiri")
            {
                movementCharacter.HadapKiri();
                yield return new WaitForSeconds(0.5f);
            }
            else if (name == "HadapKanan")
            {
                movementCharacter.HadapKanan();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void OnReloadClicked()
    {
        if (!isPlaying) return; // Hanya reset jika sedang berjalan

        StopAllCoroutines();                // Hentikan semua Coroutine di GameManager
        movementCharacter.StopAllActions(); // Hentikan semua aksi di karakter
        movementCharacter.ResetPosisi();    // Reset posisi karakter

        isPlaying = false;

        // Aktifkan tombol Play dan nonaktifkan tombol Reload
        playButton.gameObject.SetActive(true);
        reloadButton.gameObject.SetActive(false);
        playMode.SetActive(false);
    }
}
