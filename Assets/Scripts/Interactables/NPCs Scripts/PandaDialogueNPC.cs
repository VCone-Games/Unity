using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaDialogueNPC : AInteractable, ITalkable
{
	[SerializeField] private int currentText = 0;
	[SerializeField] private List<DialogueText> _dialogueBase;
	[SerializeField] private List<DialogueText> _dialogueFirstBoss;
	[SerializeField] private List<DialogueText> _dialogueSecondBoss;

	[SerializeField] List<DialogueText> _dialoguesList;
	[SerializeField] private DialogueController dialogueController;

	[Header("Test bools")]
	[SerializeField] bool _CanDialogueBase = true;
	[SerializeField] bool _CanDialogueFirstBoss;
	[SerializeField] bool _CanDialogueSecondBoss;

	[SerializeField] bool _DialogueBaseAssing = true;
	[SerializeField] bool _DialogueFirstAssing = true;
	[SerializeField] bool _DialogueSecondAssing = true;

	[SerializeField] bool _firstBossDefeated;
	[SerializeField] bool _secondBossDefeated;

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
			Talk(_dialoguesList[currentText]);
		}
	}


	public void OnAnimation_StartTalk()
	{

		myAnimator.SetBool("isTalking", true);
		myAnimator.SetBool("StartTalk", false);

		SelectTalk();
		Talk(_dialoguesList[currentText]);
	}
	void SelectTalk()
	{
		//_firstBossDefeated = (DataBase.Singleton.DataBoss["Azafran"]) || (DataBase.Singleton.DataBoss["Curcuma"]);
		//_secondBossDefeated = (DataBase.Singleton.DataBoss["Azafran"]) && (DataBase.Singleton.DataBoss["Curcuma"]);

		if (_CanDialogueBase && _DialogueBaseAssing)
		{
			_DialogueBaseAssing = false;
			currentText = 0;
			_dialoguesList = _dialogueBase;
		}
		if (_CanDialogueFirstBoss && _firstBossDefeated && _DialogueFirstAssing)
		{
			_DialogueFirstAssing = false;
			currentText = 0;
			_dialoguesList = _dialogueFirstBoss;
		}
		if (_CanDialogueSecondBoss && _secondBossDefeated && _DialogueSecondAssing)
		{
			_DialogueSecondAssing = false;
			currentText = 0;
			_dialoguesList = _dialogueSecondBoss;
		}

	}

	public void Talk(DialogueText dialogueText)
	{
		dialogueController.DisplayNexParagraph(dialogueText, this);
	}

	public override void EndInteraction()
	{
		Debug.Log("TERMINANDO INTERACCION      *");
		base.EndInteraction();

		CheckEndDialogue();
		currentText = ((currentText + 1) < _dialoguesList.Count) ? currentText + 1 : currentText;
	}

	private void CheckEndDialogue()
	{
		bool DialogoTerminado = (currentText + 1) == _dialoguesList.Count;
		if (DialogoTerminado && _CanDialogueBase)
		{
			_CanDialogueFirstBoss = true;
			_CanDialogueBase = false;
		} else if (DialogoTerminado && _CanDialogueFirstBoss && _firstBossDefeated)
		{
			_CanDialogueSecondBoss = true;
			_CanDialogueFirstBoss = false;
		} 
	}
}
