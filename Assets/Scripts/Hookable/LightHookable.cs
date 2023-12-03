using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHookable : AHookable
{

    protected override void ParryingAction()
    {
        myRigidbody.velocity = parryDirection;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRigidbody.velocity = new Vector3(-parryDirection.x, parryDirection.y, parryDirection.z) * 0.25f;

        playerRigidbody.velocity += new Vector2(0, 8);

        base.ParryingAction();

       

    }

    protected override void HookingInteraction()
    {
        base.HookingInteraction();
        myRigidbody.velocity = hookingSpeed * vectorToHookGun;

    }

    public override void Hooked(GameObject hookProjectile, float hookingSpeed)
    {
        base.Hooked(hookProjectile, hookingSpeed);

        myRigidbody.constraints = RigidbodyConstraints2D.None;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

}
