using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testNPC : AInteractable, ITalkable
{
    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private DialogueController dialogueController;
    public override void Interact()
    {
        base.Interact();
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        dialogueController.DisplayNexParagraph(dialogueText, this);
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
    }
}
