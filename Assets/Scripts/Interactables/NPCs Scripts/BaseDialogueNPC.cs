using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseDialogueNPC : AInteractable, ITalkable
{
	private int currentText = 0;
	[SerializeField] private List<DialogueText> _dialoguesList;
	[SerializeField] private DialogueController dialogueController;

	protected override void Start()
	{
		base.Start();
		dialogueController = GameObject.FindGameObjectWithTag("Dialogue Box").
			GetComponent<DialogueController>();
	}
	public override void Interact()
	{
		base.Interact();
		bool startedTalking = myAnimator.GetBool("StartTalk");
		bool isTalking = myAnimator.GetBool("isTalking");

		if (!startedTalking && !isTalking)
			myAnimator.SetBool("StartTalk", true);
		if (isTalking)
		{
			currentText = ((currentText + 1) < _dialoguesList.Count) ? currentText + 1 : currentText;
			Talk(_dialoguesList[currentText]);
		}
	}

	public void OnAnimation_StartTalk()
	{

		myAnimator.SetBool("isTalking", true);
		myAnimator.SetBool("StartTalk", false);
		currentText = ((currentText + 1) < _dialoguesList.Count) ? currentText + 1 : currentText;
		Talk(_dialoguesList[currentText]);
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
