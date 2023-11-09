using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeavyEnemyHookable : HeavyHookable
{
    protected override void ParryingAction()
    {
        base.ParryingAction();

        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        myRigidbody.velocity = new Vector3(-parryDirection.x, parryDirection.y, parryDirection.z) * 0.25f;
    }

    public override void Unhook()
    {
        base.Unhook();
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

}
