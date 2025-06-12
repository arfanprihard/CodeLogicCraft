using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoopButton : MonoBehaviour
{
    public GameObject isi;
    private ContentSizeFitter contentSizeFitter;
    public Button jumlahLoopButton;
    private TMP_Text buttonText;
    private AudioSource audioButtonClick;

    void Start()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        buttonText = jumlahLoopButton.GetComponentInChildren<TMP_Text>();
        jumlahLoopButton.onClick.AddListener(OnLoopBtClicked);
        GameObject audioObj1 = GameObject.Find("AudioButtonClick");

        if (audioObj1 != null)
        {
            audioButtonClick = audioObj1.GetComponent<AudioSource>();
        }
    }


    void Update()
    {
        // Nonaktifkan dan aktifkan ulang ContentSizeFitter
        contentSizeFitter.enabled = false;
        contentSizeFitter.enabled = true;

        // Paksa pembaruan layout
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        if (gameObject.transform.childCount > 2)
        {
            isi.SetActive(false);
        }
        else
        {
            isi.SetActive(true);
        }
    }
    void OnLoopBtClicked()
    {
        audioButtonClick.Play();
        string angkaString = buttonText.text;
        int angka = int.Parse(angkaString);
        if (angka < 2 || angka > 8)
        {
            buttonText.text = "2";
        }
        else
        {
            buttonText.text = "" + (angka + 1);
        }

    }
}
