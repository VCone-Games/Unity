using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCollectionable : AInteractable
{
	public override void Interact()
	{
		base.Interact();
        DataBase.Singleton.Coleccionables++;
		Destroy(gameObject);
	}
}
