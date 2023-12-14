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
	[SerializeField] bool _CurcumaDefeated;
	[SerializeField] bool _AzafranDefeated;
	[SerializeField] bool _BothDefeated;

	[SerializeField] private GameObject toPerejil;

	protected override void Start()
	{
		base.Start();

		DatabaseMetrics database = DatabaseMetrics.Singleton;
		_AzafranDefeated = (database.DataBoss["Azafran"]);
		_CurcumaDefeated = (database.DataBoss["Curcuma"]);

		_BothDefeated = (_AzafranDefeated && _CurcumaDefeated);

		if (dialogueManager.JengibreCanDialogueList[0]) _dialoguesList = _dialogueBase;
		else if (dialogueManager.JengibreCanDialogueList[3] && _BothDefeated) _dialoguesList = _dialogueBothBosses;
		else if (dialogueManager.JengibreCanDialogueList[2] && _AzafranDefeated) _dialoguesList = _dialogueAzafran;
		else if (dialogueManager.JengibreCanDialogueList[1] && _CurcumaDefeated) _dialoguesList = _dialogueCurcuma;
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
		

		if (dialogueManager.JengibreCanDialogueList[0] && dialogueManager.JengibreFirstAssingsList[0])
		{
			Debug.Log("Asignando PRIMER dialogo");			
			dialogueManager.JengibreFirstAssingsList[0] = false;
			dialogueManager.NPCCurrentText[nameNPC] = 0;
			currentText = 0;
			_dialoguesList = _dialogueBase;
		}
		if (dialogueManager.JengibreCanDialogueList[2] && _AzafranDefeated && dialogueManager.JengibreFirstAssingsList[2] && !dialogueManager._DialogueBossOccupied)
		{
			Debug.Log("Asignando AZAFRAN DERROTADO dialogo");
			dialogueManager._DialogueBossOccupied = true;
			dialogueManager.JengibreFirstAssingsList[2] = false;
			dialogueManager.NPCCurrentText[nameNPC] = 0;
			currentText = 0;
			_dialoguesList = _dialogueAzafran;
		} else	if (dialogueManager.JengibreCanDialogueList[1] && _CurcumaDefeated && dialogueManager.JengibreFirstAssingsList[1] && !dialogueManager._DialogueBossOccupied)
		{
			Debug.Log("Asignando CURCUMA DERROTADO dialogo");
			dialogueManager._DialogueBossOccupied = true;
			dialogueManager.JengibreFirstAssingsList[1] = false;
			dialogueManager.NPCCurrentText[nameNPC] = 0;
			currentText = 0;
			_dialoguesList = _dialogueCurcuma;
		}
		if(dialogueManager.JengibreCanDialogueList[3] && _BothDefeated && dialogueManager.JengibreFirstAssingsList[3])
		{
			Debug.Log("Asignando AMBOS DERROTADOS dialogo");
			dialogueManager.JengibreFirstAssingsList[3] = false;
			dialogueManager.NPCCurrentText[nameNPC] = 0;
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
		if (currentText == 0 && dialogueManager.JengibreCanDialogueList[0])
		{
			PlayerInfo.Instance.CanWallGrab = true;
			GameObject.FindObjectOfType<OnSceneLoad>().SetPowers();
		}

		if (DialogoTerminado && dialogueManager.JengibreCanDialogueList[0])
		{
			Debug.Log("Fin del dialogo. HABILITANDO FIRST/SECOND BOSS");
			dialogueManager.JengibreCanDialogueList[2] = true;
			dialogueManager.JengibreCanDialogueList[1] = true;
			dialogueManager.JengibreCanDialogueList[0] = false;
			dialogueManager._DialogueBossOccupied = false;
		}
		else if (DialogoTerminado && dialogueManager.JengibreCanDialogueList[2] && _AzafranDefeated)
		{
			Debug.Log("Fin del dialogo AZAFRAN. HABILITANDO END");
			dialogueManager._AzafranEndConversationReached = true;
			dialogueManager.JengibreCanDialogueList[2] = false;
			dialogueManager._DialogueBossOccupied = false;
		} else if (DialogoTerminado && dialogueManager.JengibreCanDialogueList[1] && _CurcumaDefeated)
		{
			Debug.Log("Fin del dialogo CURCUMA. HABILITANDO END");
			dialogueManager._CurcmuaEndConversationReached = true;
			dialogueManager.JengibreCanDialogueList[1] = false;
		}
		if (DialogoTerminado &&
			dialogueManager._AzafranEndConversationReached && dialogueManager._CurcmuaEndConversationReached &&
			_BothDefeated && !dialogueManager.JengibreCanDialogueList[3])
		{
			Debug.Log("Fin del dialogo. HABILITANDO BOTH-");
			dialogueManager.JengibreCanDialogueList[3] = true;
		} else if (DialogoTerminado && dialogueManager.JengibreCanDialogueList[3])
		{
			Debug.Log("He llegado al ultimo dialogo. UwU");
			toPerejil.GetComponent<TPPLAYERBACK>().TeleportBack();
		}
	}
}
