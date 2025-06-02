using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogLine
    {
        public string text;
        public float delayBetweenLines;
    }

    public DialogLine[] dialogLines;
    public TextMeshProUGUI dialogText;
    public GameObject dialogPanel;

    public bool apakahBisaDiskip = false;
    public float typingSpeed = 0.05f;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialogPanel.SetActive(true);
        typingCoroutine = StartCoroutine(ShowDialog());
    }

    void Update()
    {
        // Cek input untuk skip (klik mouse kiri di mana saja)
        if (apakahBisaDiskip && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Langsung tampilkan semua teks jika masih mengetik
                StopCoroutine(typingCoroutine);
                dialogText.text = dialogLines[currentLineIndex].text;
                isTyping = false;
            }
            else
            {
                // Lanjut ke dialog berikutnya
                currentLineIndex++;
                if (currentLineIndex < dialogLines.Length)
                {
                    typingCoroutine = StartCoroutine(ShowDialog());
                }
                else
                {
                    EndDialog();
                }
            }
        }
    }


    IEnumerator ShowDialog()
    {
        isTyping = true;
        dialogPanel.SetActive(true);
        dialogText.text = "";

        string line = dialogLines[currentLineIndex].text;

        foreach (char c in line)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        yield return new WaitForSeconds(dialogLines[currentLineIndex].delayBetweenLines);

        // Lanjut otomatis hanya jika tidak bisa diskip
        if (!apakahBisaDiskip)
        {
            currentLineIndex++;
            if (currentLineIndex < dialogLines.Length)
            {
                typingCoroutine = StartCoroutine(ShowDialog());
            }
            else
            {
                EndDialog();
            }
        }
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
    }
}
