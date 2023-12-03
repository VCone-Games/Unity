using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeavyEnemyHookable : HeavyHookable
{
    protected override void ParryingAction()
    {
        gameObject.GetComponent<HealthManager>().EventDamageTaken?.Invoke(this, new Vector3(1, 0, 0));
        base.ParryingAction();

       
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        float xVelocity = parryDirection.x >= 0 ? 1 : -1;
        myRigidbody.velocity = new Vector3(xVelocity * 3, 7);
    }

    public override void Unhook()
    {
        base.Unhook();
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

}
