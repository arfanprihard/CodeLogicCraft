using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LoopButton : MonoBehaviour
{
    private ContentSizeFitter contentSizeFitter;
    public GameObject shadow;

    public Button jumlahLoopButton;
    private TMP_Text buttonText;


    private int lastChildCount;
    private Vector2 lastSizeDelta;

    void Start()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();

        if (contentSizeFitter == null)
        {
            Debug.LogWarning("Content Size Fitter tidak ditemukan di " + gameObject.name);
        }

        // Simpan jumlah awal child dan ukuran awal
        lastChildCount = GetTotalChildCount(transform);
        lastSizeDelta = ((RectTransform)transform).sizeDelta;

        buttonText = jumlahLoopButton.GetComponentInChildren<TMP_Text>();
        if (buttonText == null)
        {
            Debug.LogError("TextMeshPro di dalam Button tidak ditemukan!");
            return;
        }
        jumlahLoopButton.onClick.AddListener(OnLoopBtClicked);
    }
    void OnLoopBtClicked()
    {
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

    void Update()
    {
        // Cek apakah jumlah child (termasuk nested child) berubah
        int currentChildCount = GetTotalChildCount(transform);
        Vector2 currentSizeDelta = ((RectTransform)transform).sizeDelta;

        // Cek perubahan pada parent untuk mengatur tag
        if (transform.parent.name != "Canvas")
        {
            shadow.tag = "Button";
        }
        else
        {
            shadow.tag = "Untagged";
        }

        // Atur visibilitas shadow berdasarkan jumlah child
        bool shouldShowShadow = currentChildCount <= 12;
        if (shadow.activeSelf != shouldShowShadow)
        {
            shadow.SetActive(shouldShowShadow);
            RefreshLayout(); // Refresh layout jika shadow berubah
        }

        // Jika ada perubahan jumlah child atau ukuran, perbarui layout
        if (currentChildCount != lastChildCount || currentSizeDelta != lastSizeDelta)
        {
            RefreshLayout();
            lastChildCount = currentChildCount;
            lastSizeDelta = currentSizeDelta; // Update ukuran terakhir
        }
    }

    void RefreshLayout()
    {
        if (contentSizeFitter != null)
        {
            // Nonaktifkan dan aktifkan ulang ContentSizeFitter
            contentSizeFitter.enabled = false;
            contentSizeFitter.enabled = true;

            // Paksa pembaruan layout
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);

            Debug.Log("Layout diperbarui di " + gameObject.name);
        }
    }

    // Hitung total child termasuk child dalam child (rekursif)
    int GetTotalChildCount(Transform parent)
    {
        int count = 0;
        foreach (Transform child in parent)
        {
            count++;
            count += GetTotalChildCount(child); // Hitung child di dalam child
        }
        return count;
    }
}
