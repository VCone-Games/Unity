using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaDialogueNPC : BaseDialogueNPC
{
	[SerializeField] private List<DialogueText> _dialogueBase;
	[SerializeField] private List<DialogueText> _dialogueFirstBoss;
	[SerializeField] private List<DialogueText> _dialogueSecondBoss;


	[Header("Test bools")]
	[SerializeField] bool _CanDialogueBase = true;
	[SerializeField] bool _CanDialogueFirstBoss;
	[SerializeField] bool _CanDialogueSecondBoss;

	[SerializeField] bool _DialogueBaseAssing = true;
	[SerializeField] bool _DialogueFirstAssing = true;
	[SerializeField] bool _DialogueSecondAssing = true;

	[SerializeField] bool _firstBossDefeated;
	[SerializeField] bool _secondBossDefeated;
	protected override void OnAnimation_StartTalk()
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

	public override void EndInteraction()
	{
		CheckEndDialogue();
		base.EndInteraction();
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
