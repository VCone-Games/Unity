using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseDialogueNPC : AInteractable, ITalkable
{
	protected int currentText = 0;
	[SerializeField] protected List<DialogueText> _dialoguesList;
	[SerializeField] protected DialogueController dialogueController;

	protected override void Start()
	{
		base.Start();
		GameObject dialogueCanvas = GameObject.FindGameObjectWithTag("Dialogue Box");
		dialogueController = dialogueCanvas.transform.GetChild(0).GetComponent<DialogueController>();
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
			Talk(_dialoguesList[currentText]);
		}
	}

	protected virtual void OnAnimation_StartTalk()
	{
		myAnimator.SetBool("isTalking", true);
		myAnimator.SetBool("StartTalk", false);
		Talk(_dialoguesList[currentText]);
	}

	public void Talk(DialogueText dialogueText)
	{
		dialogueController.DisplayNexParagraph(dialogueText, this);
	}

	public override void EndInteraction()
	{
		base.EndInteraction();
		Debug.Log("TERMINANDO INTERACCION      *");
		currentText = ((currentText + 1) < _dialoguesList.Count) ? currentText + 1 : currentText;

	}
}
