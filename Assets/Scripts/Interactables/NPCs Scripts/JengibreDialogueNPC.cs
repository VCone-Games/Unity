using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengibreDialogueNPC : BaseDialogueNPC
{
	[SerializeField] private List<DialogueText> _dialogueBase;
	[SerializeField] private List<DialogueText> _dialogueAzafran;
	[SerializeField] private List<DialogueText> _dialogueCurcuma;
	[SerializeField] private List<DialogueText> _dialogueBothBosses;

	[Header("Test bools")]
	[SerializeField] bool _CanDialogueBase = true;
	[SerializeField] bool _CanDialogueCurcumaBoss;
	[SerializeField] bool _CanDialogueAzafranBoss;
	[SerializeField] bool _CanDialogueBothBoss;

	[SerializeField] bool _DialogueBaseAssing = true;
	[SerializeField] bool _DialogueCurcumaAssing = true;
	[SerializeField] bool _DialogueAzafranAssing = true;
	[SerializeField] bool _DialogueBothAssing = true;

	[SerializeField] bool _AzafranReached;
	[SerializeField] bool _CurcumaReached;

	[SerializeField] bool _CurcumaDefeated;
	[SerializeField] bool _AzafranDefeated;
	[SerializeField] bool _BothDefeated;

	protected override void OnAnimation_StartTalk()
	{

		myAnimator.SetBool("isTalking", true);
		myAnimator.SetBool("StartTalk", false);

		SelectTalk();
		Talk(_dialoguesList[currentText]);
	}
	void SelectTalk()
	{
		//_AzafranDefeated = DataBase.Singleton.DataBoss["Azafran"];
		//_CurcumaDefeated = DataBase.Singleton.DataBoss["Curcuma"];
		
		_BothDefeated = (_AzafranDefeated && _CurcumaReached);

		if (_CanDialogueBase && _DialogueBaseAssing)
		{
			_DialogueBaseAssing = false;
			currentText = 0;
			_dialoguesList = _dialogueBase;
		}
		if (_CanDialogueAzafranBoss && _AzafranDefeated && _DialogueAzafranAssing)
		{
			_DialogueAzafranAssing = false;
			currentText = 0;
			_dialoguesList = _dialogueAzafran;
		} else	if (_CanDialogueCurcumaBoss && _CurcumaDefeated && _DialogueCurcumaAssing)
		{
			_DialogueCurcumaAssing = false;
			currentText = 0;
			_dialoguesList = _dialogueCurcuma;
		}
		if(_CanDialogueBothBoss && _BothDefeated && _DialogueBothAssing)
		{
			_DialogueBothAssing = false;
			currentText = 0;
			_dialoguesList = _dialogueBothBosses;
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
			_CanDialogueAzafranBoss = true;
			_CanDialogueCurcumaBoss = true;
			_CanDialogueBase = false;
		}
		else if (DialogoTerminado && _CanDialogueAzafranBoss && _AzafranDefeated)
		{
			_AzafranReached = true;
			_CanDialogueAzafranBoss = false;
		} else if (DialogoTerminado && _CanDialogueCurcumaBoss && _CurcumaDefeated)
		{
			_CurcumaReached = true;
			_CanDialogueCurcumaBoss = false;
		}
		if (DialogoTerminado && _AzafranReached && _CurcumaReached && _BothDefeated)
		{
			_CanDialogueBothBoss = true;
		}
	}
}
