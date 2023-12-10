using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemyHookable : LightHookable
{
    protected override void ParryingAction()
    {
        if (!gameObject.CompareTag("crow"))
        {
            gameObject.GetComponent<HealthManager>().EventDamageTaken?.Invoke(this, new Vector3(1, 0, 0));
        }
            
        base.ParryingAction();

        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRigidbody.velocity = new Vector3(0.1f, 7);
    }

    public override void Unhook()
    {
        base.Unhook();
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
