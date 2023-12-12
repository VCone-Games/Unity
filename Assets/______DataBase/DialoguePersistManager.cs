using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePersistManager : MonoBehaviour
{
    public static DialoguePersistManager instance;

    private Dictionary<string, int> npcCurrentText = new Dictionary<string, int>();

    [SerializeField] private List<bool> pandaFirstAssingsList = new List<bool>() { true, true, true };
	[SerializeField] private List<bool> pandaCanDialogueList = new List<bool>() { true, false, false };


	[SerializeField] private List<bool> jengibreFirstAssingsList = new List<bool>() { true, true, true, true };
	[SerializeField] private List<bool> jengibreCanDialogueList = new List<bool>() { true, false, false, false };
	private bool azafranReached;
	private bool curcumaReached;

	public Dictionary<string, int> NPCCurrentText { get { return npcCurrentText; } }
    public List<bool> PandaFirstAssingsList { get { return pandaFirstAssingsList; } }
    public List<bool> PandaCanDialogueList { get { return pandaCanDialogueList; } }

	public List<bool> JengibreFirstAssingsList { get { return jengibreFirstAssingsList; } }
	public List<bool> JengibreCanDialogueList { get { return jengibreCanDialogueList; } }

	public bool _AzafranEndConversationReached { get { return azafranReached; } set { azafranReached = value; } }
	public bool _CurcmuaEndConversationReached { get { return curcumaReached; } set { curcumaReached = value; } }

	private void Awake()
	{
		if (instance == null)
			instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
