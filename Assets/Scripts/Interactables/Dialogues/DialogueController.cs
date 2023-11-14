using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCDialogueText;
    [SerializeField] private float typeSpeed;

    private IInteractable interactable;

    private Queue<string> paragraphs = new Queue<string>();

    private bool conversationEnded;
    private bool isTyping;

    private string p;

    private Coroutine typeCoroutine;

    private const string HTML_ALPHA = "<color=#00000000>";

    private const float MAX_TYPE_TIME = 0.1f;

    public void DisplayNexParagraph(DialogueText dialogueText, IInteractable interactable)
    {
        if (paragraphs.Count == 0)
        {
            if (!conversationEnded)
            {
                this.interactable = interactable;
                StartConversation(dialogueText);
            }
            else if(conversationEnded && !isTyping) 
            {
                EndConversation();
            }
        }

        if (!isTyping && !conversationEnded)
        {
            p = paragraphs.Dequeue();

            typeCoroutine = StartCoroutine(TypeDialogue(p));
        }

        else
        {
            FinishParagraphEarly();
        }

        if (paragraphs.Count == 0)
        {
            conversationEnded = true;
        }


    }

    private void StartConversation(DialogueText dialogueText)
    {

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        NPCNameText.text = dialogueText.speakerName;


        for (int i = 0; i < dialogueText.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(dialogueText.paragraphs[i]);
        }
    }

    private void EndConversation()
    {
        interactable.EndInteraction();

        paragraphs.Clear();

        conversationEnded = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeDialogue(string p)
    {
        isTyping = true;

        NPCDialogueText.text = "";

        string originalText = p;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            NPCDialogueText.text = originalText;

            displayedText = NPCDialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            NPCDialogueText.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        StopCoroutine(typeCoroutine);

        NPCDialogueText.text = p;

        isTyping = false;
    }
}
