using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveLightHookable : LightEnemyHookable
{
    protected override void ParryingAction()
    {
        base.ParryingAction();
        gameObject.GetComponent<HealthManager>().EventDamageTaken?.Invoke(this, new Vector3(1, 0, 0));

    }

}
