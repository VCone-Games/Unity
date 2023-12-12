using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePersistManager : MonoBehaviour
{
    public static DialoguePersistManager instance;

    private Dictionary<string, int> npcCurrentText = new Dictionary<string, int>();

    public Dictionary<string, int> NPCCurrentText { get { return npcCurrentText; } }

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
