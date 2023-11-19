using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testNPC : AInteractable, ITalkable
{
    [SerializeField] private List<DialogueText> dialogueTextList;
    private int currentText = 0;
    [SerializeField] private DialogueController dialogueController;
    public override void Interact()
    {
        base.Interact();
        Talk(dialogueTextList[currentText]);
        if (currentText == 0) currentText++;
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
