using UnityEngine;
using UnityEngine.UI;

public class SoundOnOffManager : MonoBehaviour
{
    public Button soundOnButton;   // Tombol Sound On
    public Button soundOffButton;  // Tombol Sound Off

    private bool isSoundOn = true;

    void Start()
    {
        // Ambil status suara dari PlayerPrefs (default: On)
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        ApplySoundSetting();

        soundOnButton.onClick.AddListener(ToggleSound);
        soundOffButton.onClick.AddListener(ToggleSound);
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        ApplySoundSetting();
    }

    void ApplySoundSetting()
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;

        // Aktifkan tombol On jika suara menyala, dan disable tombol Off
        soundOnButton.gameObject.SetActive(isSoundOn);
        soundOffButton.gameObject.SetActive(!isSoundOn);
    }
}
