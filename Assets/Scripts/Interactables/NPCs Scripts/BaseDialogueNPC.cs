using Cinemachine;
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
	[SerializeField] protected string nameNPC;

	private CinemachineVirtualCamera npcCamera;
	protected DialoguePersistManager dialogueManager;

	protected override void Start()
	{
		base.Start();

		dialogueManager = DialoguePersistManager.instance;
		if (!dialogueManager.NPCCurrentText.ContainsKey(nameNPC))
		{
			Debug.Log("NPC NO EXISTE, CREANDO");
			dialogueManager.NPCCurrentText.Add(nameNPC, 0);
		}
		currentText = dialogueManager.NPCCurrentText[nameNPC];

		Debug.Log("current text: " + currentText);
		GameObject dialogueCanvas = GameObject.FindGameObjectWithTag("Dialogue Box");
		dialogueController = dialogueCanvas.transform.GetChild(0).GetComponent<DialogueController>();
		npcCamera = GameObject.FindWithTag("Camera NPC").GetComponent<CinemachineVirtualCamera>();
	}

	public override void Interact()
	{
		base.Interact();
		bool startedTalking = myAnimator.GetBool("StartTalk");
		bool isTalking = myAnimator.GetBool("isTalking");

		CameraManager.Instance.DisableCamera();
		npcCamera.enabled = true;
		npcCamera.Follow = transform;

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
		//Debug.Log("TERMINANDO INTERACCION      *");
		currentText = ((currentText + 1) < _dialoguesList.Count) ? currentText + 1 : currentText;
		dialogueManager.NPCCurrentText[nameNPC] = currentText;

		myAnimator.SetBool("isTalking", false);
		dialogueController.EndAbruptly();
        npcCamera.enabled = false;
        CameraManager.Instance.EnableCamera();
    }
}
