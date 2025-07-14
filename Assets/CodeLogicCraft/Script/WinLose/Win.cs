using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [System.Serializable]
    public class DataSolusiWin
    {
        public int[] solusi = new int[5];

    }
    public DataSolusiWin[] dataSolusiWin;
    [SerializeField] GameObject Body, Top, Bintang1, Bintang2, Bintang3;
    private ParticleSystem konfeti;
    private MovementCharacter movementCharacter;
    public GameObject endingTransition;
    public Button restartButton;
    public Button nextButton;
    private InGameManager inGameManager;
    public TMP_Text level;
    public TMP_Text apresiasi;
    public TMP_Text isi;

    private int bintangYangDidapat;

    private bool sudahMenang = false;
    public AudioSource audioSourceWin;
    public AudioSource audioSourceStar1;
    public AudioSource audioSourceStar2;
    public AudioSource audioSourceStar3;
    public AudioSource audioSourceTrompet;

    void Start()
    {

        movementCharacter = FindFirstObjectByType<MovementCharacter>();
        inGameManager = FindFirstObjectByType<InGameManager>();
        restartButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(Restart);
        nextButton.onClick.AddListener(Next);

    }

    void Restart()
    {
        sudahMenang = false;
        movementCharacter.ResetPosisi();
        audioSourceWin.Stop();
        audioSourceStar1.Stop();
        audioSourceStar2.Stop();
        audioSourceStar3.Stop();
        audioSourceTrompet.Stop();
        restartButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    void FullScreenEnding()
    {
        if (endingTransition == null)
        {
            Debug.LogError("Transition tidak ditemukan!");
            return;
        }
        endingTransition.SetActive(true);
    }
    void Next()
    {
        sudahMenang = false;
        audioSourceWin.Stop();
        audioSourceStar1.Stop();
        audioSourceStar2.Stop();
        audioSourceStar3.Stop();
        audioSourceTrompet.Stop();
        restartButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        int tingkatKesulitan = PlayerPrefs.GetInt("TingkatKesulitan");
        int level = PlayerPrefs.GetInt("Level");
        if (tingkatKesulitan == 4 && level == 5)
        {
            tingkatKesulitan += 1;
            PlayerPrefs.SetInt("TingkatKesulitan", tingkatKesulitan);
            FullScreenEnding();
            SceneManager.LoadSceneAsync("StoryEnding");
        }
        if (level >= 5 && tingkatKesulitan <= 3)
        {
            tingkatKesulitan += 1;
            level = 1;
            PlayerPrefs.SetInt("TingkatKesulitan", tingkatKesulitan);
            PlayerPrefs.SetInt("Level", level);
            SceneManager.LoadScene("LevelPage");
        }
        else
        {
            level += 1;
            PlayerPrefs.SetInt("TingkatKesulitan", tingkatKesulitan);
            PlayerPrefs.SetInt("Level", level);
            inGameManager.UpdateLevel();
            movementCharacter.ResetPosisi();
            gameObject.SetActive(false);
        }


    }

    void OnDisable()
    {
        Reset();  // Reset ukuran saat objek dinonaktifkan
    }
    void OnEnable()
    {
        GameObject[] semuaKamera = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject kamera in semuaKamera)
        {
            if (kamera.activeInHierarchy)
            {
                Debug.Log("Kamera aktif ditemukan: " + kamera.name);

                if (kamera.transform.childCount > 0)
                {
                    Transform childPertama = kamera.transform.GetChild(0);
                    konfeti = childPertama.GetComponent<ParticleSystem>();
                    konfeti.transform.localPosition = new Vector3(0f, 3f, 8f);
                    konfeti.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogWarning("Kamera tidak memiliki child.");
                }

                break;
            }
        }
        if (sudahMenang) return;
        sudahMenang = true;
        int indexTingkatKesulitan = PlayerPrefs.GetInt("TingkatKesulitan") - 1;
        int indexLevel = PlayerPrefs.GetInt("Level") - 1;
        int solusiKode = dataSolusiWin[indexTingkatKesulitan].solusi[indexLevel];
        int totalKode = PlayerPrefs.GetInt("TotalKode");

        level.text = "Level " + (indexLevel + 1);
        if (totalKode <= solusiKode)
        {
            Bintang1.SetActive(true);
            Bintang2.SetActive(true);
            Bintang3.SetActive(true);

            bintangYangDidapat = 3;
            apresiasi.text = "SEMPURNA";
            isi.text = "Kamu berhasil menemukan solusi yang efisien. Kamu telah menggunakan " + totalKode + " kode!";
        }
        else if (totalKode > solusiKode && totalKode <= solusiKode + 3)
        {
            Bintang1.SetActive(true);
            Bintang2.SetActive(true);
            Bintang3.SetActive(false);

            bintangYangDidapat = 2;
            apresiasi.text = "HEBAT";
            isi.text = "Kamu telah menggunakan " + totalKode + " kode. Dapatkan 3 bintang dengan menyelesaikan tantangan menggunakan " + solusiKode + " kode atau kurang.";
        }
        else
        {
            Bintang1.SetActive(true);
            Bintang2.SetActive(false);
            Bintang3.SetActive(false);

            bintangYangDidapat = 1;
            apresiasi.text = "CUKUP BAGUS";
            isi.text = "Kamu telah menggunakan " + totalKode + " kode. Dapatkan 3 bintang dengan menyelesaikan tantangan menggunakan " + solusiKode + " kode atau kurang.";
        }
        SaveLoadSystem.Instance.SaveBintang(indexTingkatKesulitan + 1, indexLevel + 1, bintangYangDidapat);
        Reset();  // Reset ukuran saat diaktifkan
        TopAnim(); // Jalankan animasi dari awal lagi
    }
    void Reset()
    {
        sudahMenang = false;
        Top.transform.localScale = Vector3.zero;
        Body.transform.localScale = Vector3.zero;
        Bintang1.transform.localScale = Vector3.zero;
        Bintang2.transform.localScale = Vector3.zero;
        Bintang3.transform.localScale = Vector3.zero;
        konfeti.Stop();

        konfeti.gameObject.SetActive(false);
    }
    void TopAnim()
    {
        konfeti.Play();
        konfeti.gameObject.SetActive(true);

        audioSourceWin.Play();
        // Menunggu 1 detik setelah pemanggilan LaunchRocket, baru lanjutkan animasi berikutnya
        LeanTween.delayedCall(1f, () =>
        {

            LeanTween.scale(Top, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(CompleteAnim);
            LeanTween.moveLocal(Top, new Vector3(-6f, 371f, 2f), 1f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
            LeanTween.scale(Top, new Vector3(2f, 2f, 2f), 2f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic);
        });
    }

    void CompleteAnim()
    {

        LeanTween.scale(Body, new Vector3(1f, 1f, 1f), 1f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(Staranim);

    }





    void Staranim()
    {
        if (bintangYangDidapat == 1)
        {
            // Bintang 1
            LeanTween.delayedCall(0f, () =>
                    {
                        audioSourceStar1.Play();
                        LeanTween.scale(Bintang1, new Vector3(30f, 30f, 30f), 2f)
                            .setEase(LeanTweenType.easeOutElastic);
                    });
            // Trompet
            LeanTween.delayedCall(0.5f, () =>
            {
                audioSourceTrompet.Play();
                restartButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);
            });

        }
        else if (bintangYangDidapat == 2)
        {
            // Bintang 1
            LeanTween.delayedCall(0f, () =>
                    {
                        audioSourceStar1.Play();
                        LeanTween.scale(Bintang1, new Vector3(30f, 30f, 30f), 2f)
                            .setEase(LeanTweenType.easeOutElastic);
                    });

            // Bintang 2
            LeanTween.delayedCall(1f, () =>
            {
                audioSourceStar2.Play();
                LeanTween.scale(Bintang2, new Vector3(30f, 30f, 30f), 2f)
                    .setEase(LeanTweenType.easeOutElastic);
            });
            // Trompet
            LeanTween.delayedCall(1.5f, () =>
            {
                audioSourceTrompet.Play();
                restartButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);
            });

        }
        else if (bintangYangDidapat == 3)
        {
            // Bintang 1
            LeanTween.delayedCall(0f, () =>
                    {

                        audioSourceStar1.Play();
                        LeanTween.scale(Bintang1, new Vector3(30f, 30f, 30f), 2f)
                            .setEase(LeanTweenType.easeOutElastic);
                    });

            // Bintang 2
            LeanTween.delayedCall(1f, () =>
            {
                audioSourceStar2.Play();
                LeanTween.scale(Bintang2, new Vector3(30f, 30f, 30f), 2f)
                    .setEase(LeanTweenType.easeOutElastic);
            });
            // Bintang 3
            LeanTween.delayedCall(2f, () =>
            {
                audioSourceStar3.Play();
                LeanTween.scale(Bintang3, new Vector3(30f, 30f, 30f), 2f)
                    .setEase(LeanTweenType.easeOutElastic);
            });
            // Trompet
            LeanTween.delayedCall(2.5f, () =>
            {
                audioSourceTrompet.Play();
                restartButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);
            });

        }



    }




}
