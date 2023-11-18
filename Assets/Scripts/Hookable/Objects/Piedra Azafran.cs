using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiedraAzafran : LightHookable
{
    public float forceMultiplicator;
    protected override void ParryingAction()
    {
        if(GetComponent<ThrowStone>() != null)
        {
            GetComponent<ThrowStone>().CanDamageBoss = true;
        }

        if (GetComponent<FallingStone>() != null)
        {
            GetComponent<FallingStone>().CanDamageBoss = true;
        }

        myRigidbody.velocity = parryDirection * forceMultiplicator;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRigidbody.velocity = new Vector3(-parryDirection.x, parryDirection.y, parryDirection.z) * 0.25f;

        playerRigidbody.velocity += new Vector2(0, 8);

        base.ParryingAction();

    }

    public override void Unhook()
    {
        if (!isHooked) return;

        //if (isParried)
        gameObject.layer = normalLayer;
        isParried = false;

        if (hookProjectile != null)
        {
            hookProjectile.GetComponent<HookProjectile>().DestroyProjectile();
        }
        else
        {
            playerGO.GetComponent<Hook>().HookDestroyed();
        }

        hookProjectile = null;
        hookingSpeed = 0;
        isHooked = false;
        if (enemyComponent != null) enemyComponent.SetUnhookTimer();

        timeStopped = false;
        playerGO.GetComponent<Interact>().enabled = true;
    }

    public override void Hooked(GameObject hookProjectile, float hookingSpeed)
    {
        if (GetComponent<ThrowStone>() != null)
        {
            GetComponent<ThrowStone>().hooked = true;
        }

        if (GetComponent<FallingStone>() != null)
        {
            GetComponent<FallingStone>().hooked = true;
        }
        base.Hooked(hookProjectile, hookingSpeed);

    }
}
