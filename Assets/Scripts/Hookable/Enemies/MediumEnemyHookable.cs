using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemyHookable : MediumHookable
{
    protected override void ParryingAction()
    {
        gameObject.GetComponent<HealthManager>().EventDamageTaken?.Invoke(this, new Vector3(1, 0, 0));
        base.ParryingAction();

       
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    public override void Unhook()
    {
        base.Unhook();
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
