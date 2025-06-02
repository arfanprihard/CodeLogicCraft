using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Win : MonoBehaviour
{
    [System.Serializable]
    public class DataSolusiWin
    {
        public int[] solusi = new int[5];

    }
    public DataSolusiWin[] dataSolusiWin;
    [SerializeField] GameObject Body, Top, Bintang1, Bintang2, Bintang3;
    [SerializeField] ParticleSystem Firework;
    private MovementCharacter movementCharacter;
    public Button restartButton;
    public Button nextButton;
    private InGameManager inGameManager;
    public TMP_Text level;
    public TMP_Text apresiasi;
    public TMP_Text isi;

    private int bintangYangDidapat;

    public AudioClip winSound;
    public AudioClip star1Sound;
    public AudioClip star2Sound;
    public AudioClip star3Sound;

    public AudioClip yeaySound;

    public AudioSource audioSource;



    void Start()
    {
        movementCharacter = FindObjectOfType<MovementCharacter>();
        inGameManager = FindObjectOfType<InGameManager>();

        restartButton.onClick.AddListener(Restart);
        nextButton.onClick.AddListener(Next);

    }

    void Restart()
    {
        movementCharacter.ResetPosisi();
        gameObject.SetActive(false);
    }
    void Next()
    {
        int tingkatKesulitan = PlayerPrefs.GetInt("TingkatKesulitan");
        int level = PlayerPrefs.GetInt("Level");
        if (level >= 5 && tingkatKesulitan <= 3)
        {
            tingkatKesulitan += 1;
            level = 1;
        }
        else
        {
            level += 1;
        }
        PlayerPrefs.SetInt("TingkatKesulitan", tingkatKesulitan);
        PlayerPrefs.SetInt("Level", level);
        inGameManager.UpdateLevel();
        movementCharacter.ResetPosisi();
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Reset();  // Reset ukuran saat objek dinonaktifkan
    }
    void OnEnable()
    {
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
        Top.transform.localScale = Vector3.zero;
        Body.transform.localScale = Vector3.zero;
        Bintang1.transform.localScale = Vector3.zero;
        Bintang2.transform.localScale = Vector3.zero;
        Bintang3.transform.localScale = Vector3.zero;
        Firework.Stop();
        Firework.gameObject.SetActive(false);
    }
    void TopAnim()
    {
        Firework.Play();
        Firework.gameObject.SetActive(true);

        AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
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


        // Bintang 1
        LeanTween.delayedCall(0f, () =>
        {
            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(star1Sound);
            // AudioSource.PlayClipAtPoint(star1Sound, Camera.main.transform.position);
            LeanTween.scale(Bintang1, new Vector3(30f, 30f, 30f), 2f)
                .setEase(LeanTweenType.easeOutElastic);
        });

        // Bintang 2
        LeanTween.delayedCall(0.1f, () =>
        {
            audioSource.pitch = 1.2f;
            audioSource.PlayOneShot(star2Sound);
            // AudioSource.PlayClipAtPoint(star2Sound, Camera.main.transform.position);
            LeanTween.scale(Bintang2, new Vector3(30f, 30f, 30f), 2f)
                .setEase(LeanTweenType.easeOutElastic);
        });

        // Bintang 3
        LeanTween.delayedCall(0.2f, () =>
        {
            audioSource.pitch = 2f;
            audioSource.PlayOneShot(star3Sound);
            // AudioSource.PlayClipAtPoint(star3Sound, Camera.main.transform.position);
            LeanTween.scale(Bintang3, new Vector3(30f, 30f, 30f), 2f)
                .setEase(LeanTweenType.easeOutElastic);
        });

        // Reset pitch agar tidak mempengaruhi suara selanjutnya
        LeanTween.delayedCall(0.6f, () =>
        {
            AudioSource.PlayClipAtPoint(yeaySound, Camera.main.transform.position);
        });


    }



}
