using System;
using System.Collections;
using TMPro;
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

    public Button nextButtonSukses;
    public GameObject suksesUI;
    public GameObject gagalUI;
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
        nextButtonSukses.onClick.AddListener(OnClickButtonSukses);
    }

    void Update()
    {
        if (!movementCharacter.IsMoving() && movementCharacter.IsOnStartPosition())
        {
            playButton.gameObject.SetActive(true);
            reloadButton.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (movementCharacter.CekFallArea())
        {
            ActionFailed();
        }
    }



    void OnClickButtonSukses()
    {
        foreach (Transform child in main.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void OnPlayClicked()
    {
        if (isPlaying) return; // Mencegah duplikasi eksekusi

        isPlaying = true;

        // Nonaktifkan tombol Play dan aktifkan tombol Pause
        playButton.gameObject.SetActive(false);
        reloadButton.gameObject.SetActive(true);
        playMode.SetActive(true);

        MainDragAndDrop mainDragAndDrop = FindObjectOfType<MainDragAndDrop>();
        int totalButtonMain = mainDragAndDrop.HitungSemuaButton(main.transform);
        int totalButtonMethod = 0;
        if (method != null)
        {
            totalButtonMethod = mainDragAndDrop.HitungSemuaButton(method.transform);
        }
        PlayerPrefs.SetInt("TotalKode", totalButtonMain + totalButtonMethod);
        Debug.Log("Banyak Kode = " + (totalButtonMain + totalButtonMethod));

        // Jalankan aksi secara berurutan
        StartCoroutine(ExecuteButtonMain());
    }

    IEnumerator ExecuteButtonMain()
    {
        for (int i = 0; i < main.transform.childCount; i++)
        {
            Transform child = main.transform.GetChild(i);
            string name = child.name;

            if (name == "LoopIn")
            {
                Transform imgLoop = child.Find("loopImg");
                Transform buttonLoop = imgLoop.Find("jumlahLoop");

                TMP_Text jumlahLoop = buttonLoop.GetChild(0).GetComponent<TMP_Text>();

                int loopCount = int.Parse(jumlahLoop.text);
                for (int j = 0; j < loopCount; j++)
                {
                    yield return StartCoroutine(ExecuteButtonLoop(child));
                }
            }
            else if (name == "Percabangan" && movementCharacter.CekPercabangan())
            {
                Transform childPercabangan = child.Find("isi").Find("isi");
                yield return StartCoroutine(ExecuteButtonPercabangan(childPercabangan));
            }
            else if (name == "Step")
            {
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
            else if (name == "Take")
            {
                if (movementCharacter.CekTakeItem())
                {
                    movementCharacter.TakeItem();
                }
                else
                {
                    ActionFailed();
                }
                yield return new WaitForSeconds(2.5f);
            }
            else if (name == "Method")
            {
                yield return StartCoroutine(ExecuteButtonMethod());
            }
        }

        isPlaying = false;


        // Periksa apakah karakter berada di area finish
        playButton.gameObject.SetActive(false);
        reloadButton.gameObject.SetActive(true);
        playMode.SetActive(false);

        if (movementCharacter.CekFinish() && !IsItemActiveInHierarchy())
        {
            suksesUI.SetActive(true);
        }
        else
        {
            gagalUI.SetActive(true);
        }
    }

    IEnumerator ExecuteButtonMethod()
    {
        for (int i = 0; i < method.transform.childCount; i++)
        {
            Transform child = method.transform.GetChild(i);
            string name = child.name;

            if (name == "LoopIn")
            {
                Transform imgLoop = child.Find("loopImg");
                Transform buttonLoop = imgLoop.Find("jumlahLoop");

                TMP_Text jumlahLoop = buttonLoop.GetChild(0).GetComponent<TMP_Text>();

                int loopCount = int.Parse(jumlahLoop.text);
                for (int j = 0; j < loopCount; j++)
                {
                    yield return StartCoroutine(ExecuteButtonLoop(child));
                }
            }
            else if (name == "Percabangan" && movementCharacter.CekPercabangan())
            {
                Transform childPercabangan = child.Find("isi").Find("isi");
                yield return StartCoroutine(ExecuteButtonPercabangan(childPercabangan));
            }
            else if (name == "Step")
            {
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
            else if (name == "Take")
            {
                if (movementCharacter.CekTakeItem())
                {
                    movementCharacter.TakeItem();
                }
                else
                {
                    ActionFailed();
                }
                yield return new WaitForSeconds(2.5f);
            }
        }
    }

    IEnumerator ExecuteButtonLoop(Transform loopParent)
    {
        for (int i = 0; i < loopParent.childCount; i++)
        {
            Transform child = loopParent.GetChild(i);
            string name = child.name;

            if (name == "Step")
            {
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
            else if (name == "LoopIn")
            {
                Transform imgLoop = child.Find("loopImg");
                Transform buttonLoop = imgLoop.Find("jumlahLoop");
                TMP_Text jumlahLoop = buttonLoop.GetChild(0).GetComponent<TMP_Text>();

                int loopCount = int.Parse(jumlahLoop.text);
                for (int j = 0; j < loopCount; j++)
                {
                    yield return StartCoroutine(ExecuteButtonLoop(child));
                }
            }
            else if (name == "Percabangan" && movementCharacter.CekPercabangan())
            {
                Transform childPercabangan = child.Find("isi").Find("isi");
                yield return StartCoroutine(ExecuteButtonPercabangan(childPercabangan));
            }
            else if (name == "Method")
            {
                yield return StartCoroutine(ExecuteButtonMethod());
            }
            else if (name == "Take")
            {
                if (movementCharacter.CekTakeItem())
                {
                    movementCharacter.TakeItem();
                }
                else
                {
                    ActionFailed();
                }
                yield return new WaitForSeconds(2.5f);
            }
        }
    }

    IEnumerator ExecuteButtonPercabangan(Transform parentPercabangan)
    {
        Debug.Log("Banyak child Percabangan = " + parentPercabangan.childCount);
        if (parentPercabangan.childCount > 0)
        {
            Transform child = parentPercabangan.GetChild(0);
            string name = child.name;

            if (name == "Step")
            {
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
                yield return StartCoroutine(ExecuteButtonMethod());
            }
            else if (name == "Take")
            {
                if (movementCharacter.CekTakeItem())
                {
                    movementCharacter.TakeItem();
                }
                else
                {
                    ActionFailed();
                }
                yield return new WaitForSeconds(2.5f);
            }
        }
    }


    void OnReloadClicked()
    {
        StopAllCoroutines(); // Hentikan semua Coroutine di GameManager
        movementCharacter.StopAllActions(); // Hentikan semua aksi di karakter
        movementCharacter.ResetPosisi(); // Reset posisi karakter

        isPlaying = false;

        playButton.gameObject.SetActive(true);
        reloadButton.gameObject.SetActive(false);
        playMode.SetActive(false);
    }

    void ActionFailed()
    {
        StopAllCoroutines(); // Hentikan semua Coroutine
        movementCharacter.StopAllActions(); // Hentikan semua aksi di karakter
        gagalUI.SetActive(true); // Tampilkan UI gagal
        playButton.gameObject.SetActive(true); // Tampilkan tombol Play
        reloadButton.gameObject.SetActive(false); // Sembunyikan tombol Reload
        playMode.SetActive(false); // Matikan play mode
        isPlaying = false;
    }
    bool IsItemActiveInHierarchy()
    {
        // Cari semua objek dengan tag "Item"
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        // Periksa apakah objek tersebut aktif di dalam hierarki
        foreach (GameObject item in items)
        {
            if (item.activeInHierarchy)
            {
                return true; // Jika ada objek aktif, return true
            }
        }

        return false; // Tidak ada objek aktif, return false
    }
}
