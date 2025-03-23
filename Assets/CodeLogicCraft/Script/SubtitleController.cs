using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SubtitleController : MonoBehaviour
{
    public PlayableDirector timeline;   // Untuk Timeline
    public GameObject subtitlePanel;    // Panel UI Subtitle
    public Text subtitleText;           // Text di Subtitle

    private bool isWaitingForInput = false;

    void Start()
    {
        // Sembunyikan panel subtitle di awal
        subtitlePanel.SetActive(false);
    }

    // Fungsi untuk menampilkan subtitle (dipanggil dari Signal Emitter)
    public void ShowSubtitle(string text)
    {
        // Pastikan subtitle hanya muncul jika timeline sedang berjalan
        if (timeline.state == PlayState.Playing)
        {
            Debug.Log("ShowSubtitle dipanggil: " + text);
            subtitlePanel.SetActive(true); // Tampilkan panel subtitle
            subtitleText.text = text;      // Atur teks subtitle

            timeline.Pause();              // Pause Timeline saat subtitle muncul
            isWaitingForInput = true;      // Aktifkan mode menunggu input
        }
    }

    void Update()
    {
        // Lanjutkan Timeline jika pengguna mengklik
        if (isWaitingForInput && Input.GetMouseButtonDown(0))
        {
            ContinueTimeline();
        }
    }

    // Fungsi untuk melanjutkan timeline
    public void ContinueTimeline()
    {
        subtitlePanel.SetActive(false); // Sembunyikan panel subtitle
        timeline.Play();                // Lanjutkan Timeline
        isWaitingForInput = false;      // Matikan mode menunggu input
    }
}