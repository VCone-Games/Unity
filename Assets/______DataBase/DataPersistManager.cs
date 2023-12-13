using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistManager : MonoBehaviour
{
    public static DataPersistManager instance;


	/// <summary>
	/// DIALOGOS
	/// </summary>
    private Dictionary<string, int> npcCurrentText = new Dictionary<string, int>();

    [SerializeField] private List<bool> pandaFirstAssingsList = new List<bool>() { true, true, true };
	[SerializeField] private List<bool> pandaCanDialogueList = new List<bool>() { true, false, false };


	[SerializeField] private List<bool> jengibreFirstAssingsList = new List<bool>() { true, true, true, true };
	[SerializeField] private List<bool> jengibreCanDialogueList = new List<bool>() { true, false, false, false };
	private bool dialogueOccuped;
	private bool azafranReached;
	private bool curcumaReached;

	public Dictionary<string, int> NPCCurrentText { get { return npcCurrentText; } }
    public List<bool> PandaFirstAssingsList { get { return pandaFirstAssingsList; } }
    public List<bool> PandaCanDialogueList { get { return pandaCanDialogueList; } }

	public List<bool> JengibreFirstAssingsList { get { return jengibreFirstAssingsList; } }
	public List<bool> JengibreCanDialogueList { get { return jengibreCanDialogueList; } }

	public bool _AzafranEndConversationReached { get { return azafranReached; } set { azafranReached = value; } }
	public bool _CurcmuaEndConversationReached { get { return curcumaReached; } set { curcumaReached = value; } }
	public bool _DialogueBossOccupied {  get { return dialogueOccuped; } set { dialogueOccuped = value; } }


	/// <summary>
	/// INTERACTABLES
	/// </summary>
	private Dictionary<int, bool> interactablesActive = new Dictionary<int, bool>();
	public Dictionary<int, bool> InteractablesActive {  get { return interactablesActive; } }


	private void Awake()
	{
		if (instance == null)
			instance = this;
	}

	public bool CheckActiveIDInteractable(int id)
	{
		if (!interactablesActive.ContainsKey(id))
			interactablesActive.Add(id, true);

		return interactablesActive[id];
	}
}
