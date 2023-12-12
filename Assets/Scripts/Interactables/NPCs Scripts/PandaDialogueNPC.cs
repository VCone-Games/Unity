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
	[SerializeField] bool _firstBossDefeated;
	[SerializeField] bool _secondBossDefeated;

	protected override void Start()
	{
		base.Start();

		DatabaseMetrics database = DatabaseMetrics.Singleton;
		_firstBossDefeated = (database.DataBoss["Azafran"]) || (database.DataBoss["Curcuma"]);
		_secondBossDefeated = (database.DataBoss["Azafran"]) && (database.DataBoss["Curcuma"]);

		if (dialogueManager.PandaCanDialogueList[0]) _dialoguesList = _dialogueBase;
		else if (dialogueManager.PandaCanDialogueList[1] && _firstBossDefeated) _dialoguesList = _dialogueFirstBoss;
		else if (dialogueManager.PandaCanDialogueList[2] && _secondBossDefeated) _dialoguesList = _dialogueSecondBoss;
	}
	protected override void OnAnimation_StartTalk()
	{

		myAnimator.SetBool("isTalking", true);
		myAnimator.SetBool("StartTalk", false);

		SelectTalk();
		Talk(_dialoguesList[currentText]);
	}
	void SelectTalk()
	{
		
		if (dialogueManager.PandaCanDialogueList[0] && dialogueManager.PandaFirstAssingsList[0])
		{
			Debug.Log("Asignando PRIMER dialogo");
			dialogueManager.PandaFirstAssingsList[0] = false;
			dialogueManager.NPCCurrentText[nameNPC] = 0;
			currentText = 0;
			_dialoguesList = _dialogueBase;
		}
		if (dialogueManager.PandaCanDialogueList[1] && _firstBossDefeated && dialogueManager.PandaFirstAssingsList[1])
		{
			Debug.Log("Asignando SEGUNDO dialogo");
			dialogueManager.PandaFirstAssingsList[1] = false;
			dialogueManager.NPCCurrentText[nameNPC] = 0;
			currentText = 0;
			_dialoguesList = _dialogueFirstBoss;
		}
		if (dialogueManager.PandaCanDialogueList[2] && _secondBossDefeated && dialogueManager.PandaFirstAssingsList[2])
		{
			Debug.Log("Asignando TERCER dialogo");
			dialogueManager.PandaFirstAssingsList[2] = false;
			dialogueManager.NPCCurrentText[nameNPC] = 0;
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
		if (DialogoTerminado && dialogueManager.PandaCanDialogueList[0])
		{
			Debug.Log("Fin del dialogo. HABILITANDO FIRST BOSS");
			dialogueManager.PandaCanDialogueList[1] = true;
			dialogueManager.PandaCanDialogueList[0] = false;
		} else if (DialogoTerminado && dialogueManager.PandaCanDialogueList[1] && _firstBossDefeated)
		{
			Debug.Log("Fin del dialogo. HABILITANDO SEGUNDO BOSS");
			dialogueManager.PandaCanDialogueList[2] = true;
			dialogueManager.PandaCanDialogueList[1] = false;
		} 
	}
}
