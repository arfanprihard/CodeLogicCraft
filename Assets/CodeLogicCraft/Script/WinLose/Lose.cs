using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lose : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject Body, TopGagal;

    private MovementCharacter movementCharacter;
    public TMP_Text level;
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
        int indexLevel = PlayerPrefs.GetInt("Level") - 1;

        level.text = "Level " + (indexLevel + 1);
        Reset();  // Reset ukuran saat diaktifkan
        TopAnim(); // Jalankan animasi dari awal lagi
    }
    void TopAnim()
    {
        LeanTween.scale(TopGagal, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(.4f).setEase(LeanTweenType.easeOutElastic).setOnComplete(BodyAnim);
        LeanTween.moveLocal(TopGagal, new Vector3(-6f, 367f, 2f), 1f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(TopGagal, new Vector3(1f, 1f, 1f), 2f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic);
    }
    void Reset()
    {
        TopGagal.transform.localScale = Vector3.zero;
        Body.transform.localScale = Vector3.zero;
    }
    void BodyAnim()
    {
        LeanTween.scale(Body, new Vector3(1f, 1f, 1f), 1f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);


    }

}
