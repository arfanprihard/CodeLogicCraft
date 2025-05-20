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

    public float typingSpeed = 0.05f;

    private int currentLineIndex = 0;

    void Start()
    {
        dialogPanel.SetActive(true);
        StartCoroutine(ShowDialog());
    }

    IEnumerator ShowDialog()
    {
        while (currentLineIndex < dialogLines.Length)
        {
            dialogPanel.SetActive(true);
            dialogText.text = "";

            string line = dialogLines[currentLineIndex].text;

            foreach (char c in line)
            {
                dialogText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(dialogLines[currentLineIndex].delayBetweenLines);

            currentLineIndex++;
        }

        EndDialog();
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        // Tambahkan aksi setelah dialog selesai (misalnya lanjut cutscene)
    }
}
