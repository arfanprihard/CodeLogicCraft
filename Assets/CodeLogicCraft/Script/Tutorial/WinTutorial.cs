using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinTutorial : MonoBehaviour
{
    public int solusiKode;

    [SerializeField] GameObject Body, Top, Bintang1, Bintang2, Bintang3;
    [SerializeField] ParticleSystem Firework;
    private MovementCharacter movementCharacter;
    public Button nextButton;
    public TMP_Text apresiasi;
    public TMP_Text isi;


    public AudioClip winSound;
    public AudioClip star1Sound;
    public AudioClip star2Sound;
    public AudioClip star3Sound;

    public AudioClip trompetSound;

    private int totalBintang;


    void Start()
    {
        movementCharacter = FindFirstObjectByType<MovementCharacter>();

        nextButton.onClick.AddListener(Next);

    }

    void Next()
    {
        SceneManager.LoadScene("LevelPage");
        movementCharacter.ResetPosisi();
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Reset();  // Reset ukuran saat objek dinonaktifkan
    }
    void OnEnable()
    {
        int totalKode = PlayerPrefs.GetInt("TotalKode");

        if (totalKode <= solusiKode)
        {
            Bintang1.SetActive(true);
            Bintang2.SetActive(true);
            Bintang3.SetActive(true);
            totalBintang = 3;
            apresiasi.text = "SEMPURNA";
            isi.text = "Kamu berhasil menemukan solusi yang efisien. Kamu telah menggunakan " + totalKode + " kode!";
        }
        else if (totalKode > solusiKode && totalKode <= solusiKode + 3)
        {
            Bintang1.SetActive(true);
            Bintang2.SetActive(true);
            Bintang3.SetActive(false);
            totalBintang = 2;
            apresiasi.text = "HEBAT";
            isi.text = "Kamu telah menggunakan " + totalKode + " kode. Dapatkan 3 bintang dengan menyelesaikan tantangan menggunakan " + solusiKode + " kode atau kurang.";
        }
        else
        {
            Bintang1.SetActive(true);
            Bintang2.SetActive(false);
            Bintang3.SetActive(false);
            totalBintang = 1;
            apresiasi.text = "CUKUP BAGUS";
            isi.text = "Kamu telah menggunakan " + totalKode + " kode. Dapatkan 3 bintang dengan menyelesaikan tantangan menggunakan " + solusiKode + " kode atau kurang.";
        }

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
        if (totalBintang == 1)
        {
            // Bintang 1
            LeanTween.delayedCall(0f, () =>
                    {
                        AudioSource.PlayClipAtPoint(star1Sound, Camera.main.transform.position);
                        LeanTween.scale(Bintang1, new Vector3(30f, 30f, 30f), 2f)
                            .setEase(LeanTweenType.easeOutElastic);
                    });
            // Trompet
            LeanTween.delayedCall(0.5f, () =>
            {
                AudioSource.PlayClipAtPoint(trompetSound, Camera.main.transform.position);
                nextButton.gameObject.SetActive(true);
            });

        }
        else if (totalBintang == 2)
        {
            // Bintang 1
            LeanTween.delayedCall(0f, () =>
                    {
                        AudioSource.PlayClipAtPoint(star1Sound, Camera.main.transform.position);
                        LeanTween.scale(Bintang1, new Vector3(30f, 30f, 30f), 2f)
                            .setEase(LeanTweenType.easeOutElastic);
                    });

            // Bintang 2
            LeanTween.delayedCall(1f, () =>
            {
                AudioSource.PlayClipAtPoint(star2Sound, Camera.main.transform.position);
                LeanTween.scale(Bintang2, new Vector3(30f, 30f, 30f), 2f)
                    .setEase(LeanTweenType.easeOutElastic);
            });
            // Trompet
            LeanTween.delayedCall(1.5f, () =>
            {
                AudioSource.PlayClipAtPoint(trompetSound, Camera.main.transform.position);
                nextButton.gameObject.SetActive(true);
            });

        }
        else if (totalBintang == 3)
        {
            // Bintang 1
            LeanTween.delayedCall(0f, () =>
                    {
                        AudioSource.PlayClipAtPoint(star1Sound, Camera.main.transform.position);
                        LeanTween.scale(Bintang1, new Vector3(30f, 30f, 30f), 2f)
                            .setEase(LeanTweenType.easeOutElastic);
                    });

            // Bintang 2
            LeanTween.delayedCall(1f, () =>
            {
                AudioSource.PlayClipAtPoint(star2Sound, Camera.main.transform.position);
                LeanTween.scale(Bintang2, new Vector3(30f, 30f, 30f), 2f)
                    .setEase(LeanTweenType.easeOutElastic);
            });
            // Bintang 3
            LeanTween.delayedCall(2f, () =>
            {
                AudioSource.PlayClipAtPoint(star3Sound, Camera.main.transform.position);
                LeanTween.scale(Bintang3, new Vector3(30f, 30f, 30f), 2f)
                    .setEase(LeanTweenType.easeOutElastic);
            });
            // Trompet
            LeanTween.delayedCall(2.5f, () =>
            {
                AudioSource.PlayClipAtPoint(trompetSound, Camera.main.transform.position);
                nextButton.gameObject.SetActive(true);
            });

        }



    }



}
