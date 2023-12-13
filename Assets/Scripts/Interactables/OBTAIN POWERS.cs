using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBTAINPOWERS : AInteractable
{
    [Header("0.Wall Grab, 1. Dash, 2. Double Jump")]
    [SerializeField] private int power;
    public override void Interact()
    {
        base.Interact();

        EndInteraction();
        Destroy(gameObject);

    }
}
