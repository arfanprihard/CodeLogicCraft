using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableDirector nextDirector;
    [Header("UI References")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Data")]
    public string characterName = "NPC";
    [TextArea(3, 10)]
    public List<string> dialogueLines;

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;

    private int currentLine = 0;
    private bool isTyping = false;
    private bool dialogueEnded = false;
    private Coroutine typingCoroutine;



    void Start()
    {
        // director.Pause();
        director.gameObject.SetActive(false); // Ini bikin semua track & binding dilepas
        StartDialogue();
    }

    void Update()
    {
        if (dialogueEnded) return;

        if (Input.GetMouseButtonDown(0)) // tap layar
        {
            if (isTyping)
            {
                // Skip typing, langsung tampilkan seluruh teks
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[currentLine];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        currentLine = 0;
        dialogueEnded = false;
        characterNameText.text = characterName;
        StartTyping(dialogueLines[currentLine]);
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Count)
        {
            StartTyping(dialogueLines[currentLine]);
        }
        else
        {
            nextDirector.gameObject.SetActive(true);
            nextDirector.Play();
            EndDialogue();
        }
    }

    void StartTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueText.text = "";
        characterNameText.text = "";
        dialogueEnded = true;
        // director.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Debug.Log("Dialog selesai!");
        // Bisa trigger event lain di sini
    }
}
