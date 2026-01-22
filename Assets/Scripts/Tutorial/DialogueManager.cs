using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject HolyWaterIndicator;
    [SerializeField] GameObject PlayerIndicator;
    [SerializeField] GameObject InfectionIndicator;
    public TextMeshProUGUI dialogueText;
    [SerializeField] float LetterDelay;
    public Dialogue dialogue;
    [SerializeField] TutorialManager tutorialManager;
    public bool isThisFirstDialog = true;
    public bool isThisThirdDialogue = false;

    private bool sentenceTyped = true;
    public Queue<string> sentences;
    private string currentSentence = "";

    private void Start()
    {
        sentences = new Queue<string>();
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sentenceTyped == true)
            {
                DisplayNextSentence();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                sentenceTyped = true;
            }
        }
    }

    public void StartDialogue()
    {
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0 && isThisFirstDialog && !isThisThirdDialogue)
        {
            HolyWaterIndicator.SetActive(false);
            tutorialManager.EndOfFirstDialogue();
            return;
        }
        else if(sentences.Count == 0 && !isThisFirstDialog && !isThisThirdDialogue)
        {
            tutorialManager.EndOfSecondDialogue();
            return;
        }
        if (sentences.Count == 0 && isThisThirdDialogue)
        {
            tutorialManager.EndOfThirdDialogue();
            return;
        }
        if (sentences.Count == 4 && isThisFirstDialog)
        {
            PlayerIndicator.SetActive(true);
        }
        if (sentences.Count == 3 && isThisFirstDialog)
        {
            PlayerIndicator.SetActive(false);
            InfectionIndicator.SetActive(true);
        }
        if (sentences.Count == 2 && isThisFirstDialog)
        {
            InfectionIndicator.SetActive(false);
            HolyWaterIndicator.SetActive(true);
        }

        currentSentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceTyped = false;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(LetterDelay);
        }

        sentenceTyped = true;
    }


}