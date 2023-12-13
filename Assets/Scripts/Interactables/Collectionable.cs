using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectionable : AInteractable
{
    [Header("Collectionable params")]
    [SerializeField] private int id;
    [SerializeField] private bool active;

    public override void Interact()
    {
        base.Interact();

        active = false;
        DataPersistManager.instance.InteractablesActive[id] = active;
        DatabaseMetrics.Singleton.CollectionablePickeds++;
        gameObject.SetActive(active);

	}

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        active = DataPersistManager.instance.CheckActiveIDInteractable(id);
        gameObject.SetActive(active);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
