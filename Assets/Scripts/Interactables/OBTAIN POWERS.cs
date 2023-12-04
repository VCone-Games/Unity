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
        switch (power)
        {
            case 0:
                PlayerInfo.Instance.CanWallGrab = true;
                break;
            case 1:
                PlayerInfo.Instance.CanDash = true;
                break;
            case 2:
                PlayerInfo.Instance.CanDoubleJump = true;
                break;

        }

        GameObject.FindObjectOfType<OnSceneLoad>().SetPowers();
        EndInteraction();
        Destroy(gameObject);

    }
}
