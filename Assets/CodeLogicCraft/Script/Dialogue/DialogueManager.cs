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
    public GameObject nextObject;

    public float typingSpeed = 0.05f;
    public AudioClip typingSound;
    public AudioSource audioSource;

    public int soundInterval = 3;

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
        if (apakahBisaDiskip && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogText.text = dialogLines[currentLineIndex].text;
                isTyping = false;
            }
            else
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
    }

    IEnumerator ShowDialog()
    {
        isTyping = true;
        dialogPanel.SetActive(true);
        dialogText.text = "";

        string line = dialogLines[currentLineIndex].text;
        int i = 0;
        int charCount = 0;

        while (i < line.Length)
        {
            if (line[i] == '<')
            {
                int tagEnd = line.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    string tag = line.Substring(i, tagEnd - i + 1);
                    dialogText.text += tag;
                    i = tagEnd + 1;
                    continue;
                }
            }

            dialogText.text += line[i];

            if (!char.IsWhiteSpace(line[i]))
            {
                charCount++;
                if (charCount % soundInterval == 0 && typingSound != null && audioSource != null)
                {
                    audioSource.pitch = Random.Range(1.8f, 2f);
                    audioSource.PlayOneShot(typingSound, 0.3f);
                }
            }

            i++;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        yield return new WaitForSeconds(dialogLines[currentLineIndex].delayBetweenLines);

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
        if (apakahBisaDiskip && nextObject != null)
        {
            nextObject.SetActive(true);
        }
        dialogPanel.SetActive(false);
    }
}
