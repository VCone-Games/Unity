using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOGENGIBRE : AInteractable
{
    [SerializeField] private SceneObject changingScene;
    [SerializeField] private int EnterPosition;
    public override void Interact()
    {
        base.Interact();

        EndInteraction();

        SceneChanger.Instance.ChangeSceneByMoving(changingScene, EnterPosition);
    }
}
