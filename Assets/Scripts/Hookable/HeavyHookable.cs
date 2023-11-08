using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyHookable : AHookable
{
    protected override void ParryingAction()
    {
        playerRigidbody.velocity = parryDirection;
        if (Mathf.Abs(parryDirection.normalized.x) > 0.95)
        {
            playerRigidbody.velocity += new Vector2(0, 8);
        }
        base.ParryingAction();
    }

    protected override void HookingInteraction()
    {
        base.HookingInteraction();

        playerRigidbody.velocity = hookingSpeed * -vectorToPlayer;
    }

    public override void Hooked(GameObject hookProjectile, float hookingSpeed)
    {
        base.Hooked(hookProjectile, hookingSpeed);

        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
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
