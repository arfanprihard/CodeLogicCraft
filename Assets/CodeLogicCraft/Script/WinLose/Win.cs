using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject Body, Top, Bintang1, Bintang2, Bintang3;
    [SerializeField] ParticleSystem Firework1, SubEmitter1, Firework2, SubEmitter2;

    private MovementCharacter movementCharacter;
    public Button restartButton;

    void Start()
    {
        movementCharacter = FindObjectOfType<MovementCharacter>();
        restartButton.onClick.AddListener(Restart);
    }
    void Restart()
    {
        movementCharacter.ResetPosisi();
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Reset();  // Reset ukuran saat objek dinonaktifkan
    }
    void OnEnable()
    {
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
        Firework1.Stop();
        Firework2.Stop();
        SubEmitter1.Stop();
        SubEmitter2.Stop();
        Firework1.gameObject.SetActive(false);
        Firework2.gameObject.SetActive(false);
        SubEmitter1.gameObject.SetActive(false);
        SubEmitter2.gameObject.SetActive(false);
    }
    void TopAnim()
    {
        Firework1.gameObject.SetActive(true);
        Firework2.gameObject.SetActive(true);
        SubEmitter1.gameObject.SetActive(true);
        SubEmitter2.gameObject.SetActive(true);
        LaunchRocket();
        LeanTween.scale(Top, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(CompleteAnim);
        LeanTween.moveLocal(Top, new Vector3(-6f, 371f, 2f), 1f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(Top, new Vector3(2f, 2f, 2f), 2f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic);
    }

    void CompleteAnim()
    {

        LeanTween.scale(Body, new Vector3(1f, 1f, 1f), 1f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(Staranim);

    }


    void Staranim()
    {

        LeanTween.scale(Bintang1, new Vector3(30f, 30f, 30f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(Bintang2, new Vector3(30f, 30f, 30f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(Bintang3, new Vector3(30f, 30f, 30f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);

    }


    void LaunchRocket()
    {
        Firework1.Play();  // Main roket pas mulai terbang
        Firework2.Play();  // Main roket pas mulai terbang

        // Nunggu roket selesai buat meledak jadi kembang api
        LeanTween.delayedCall(0f, () =>  // Roket terbang selama 2.5 detik
        {
            // Firework1.Stop();  // Berhenti di puncak
            // Firework2.Stop();  // Berhenti di puncak

            // Nunggu buat mulai kembang api
            LeanTween.delayedCall(0f, () =>  // Delay 1 detik sebelum kembang api
            {
                SubEmitter1.transform.position = Firework1.transform.position;  // Pindah kembang api ke roket
                SubEmitter1.Play();  // Mulai kembang api 🎆
                SubEmitter2.transform.position = Firework2.transform.position;  // Pindah kembang api ke roket
                SubEmitter2.Play();  // Mulai kembang api 🎆
            });
        });
    }


}
