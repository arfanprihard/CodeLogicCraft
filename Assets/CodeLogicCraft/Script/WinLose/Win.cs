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
    [SerializeField] ParticleSystem Firework1, SubEmitter1, Firework2, SubEmitter2;
    private MovementCharacter movementCharacter;
    public Button restartButton;
    public Button nextButton;
    private InGameManager inGameManager;
    public TMP_Text level;
    public TMP_Text apresiasi;
    public TMP_Text isi;

    
    private int bintangYangDidapat;

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
        if(level >= 5 && tingkatKesulitan <= 3){
            tingkatKesulitan += 1;
            level = 1;
        }else{
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
        if(totalKode <= solusiKode){
            Bintang1.SetActive(true);
            Bintang2.SetActive(true);
            Bintang3.SetActive(true);

            bintangYangDidapat = 3;
            apresiasi.text = "SEMPURNA";
            isi.text = "Kamu berhasil menemukan solusi yang efisien. Kamu telah menggunakan "+ totalKode + " kode!";
        }else if(totalKode > solusiKode && totalKode <= solusiKode + 3){
            Bintang1.SetActive(true);
            Bintang2.SetActive(true);
            Bintang3.SetActive(false);

            bintangYangDidapat = 2;
            apresiasi.text = "HEBAT";
            isi.text = "Kamu telah menggunakan " + totalKode +" kode. Dapatkan 3 bintang dengan menyelesaikan tantangan menggunakan "+solusiKode+" kode atau kurang.";
        }else{
            Bintang1.SetActive(true);
            Bintang2.SetActive(false);
            Bintang3.SetActive(false);

            bintangYangDidapat = 1;
            apresiasi.text = "CUKUP BAGUS";
            isi.text = "Kamu telah menggunakan " + totalKode +" kode. Dapatkan 3 bintang dengan menyelesaikan tantangan menggunakan "+solusiKode+" kode atau kurang.";
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
