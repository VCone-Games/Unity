using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayerManager : HealthManager
{

	public EventHandler<int> EventHealing;
	public EventHandler<int> EventUpdateHealthUI;

    private HorizontalMovement horizontalMovementComponent;
    private Dash dashComponent;
    private Jump jumpComponent;
    private Hook hook;
    private CinemachineImpulseSource impulseSource;

    [SerializeField] private Vector2 DamageKnockbackVector;
    [SerializeField] private float DamageKnockbackForce;

    private Vector2 auxKnockback;

    [SerializeField] private float InvulnerabilityTime;
    private float InvulnerabilityTimer;

    private void Update()
    {
        if(InvulnerabilityTimer > 0)
        {
            InvulnerabilityTimer -= Time.deltaTime;
            if(InvulnerabilityTimer < 0)
            {
                canTakeDamage = true;
                OnlyTakeDmgOnce = false;
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        EventHealing += Heal;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        horizontalMovementComponent = GetComponent<HorizontalMovement>();
        dashComponent = GetComponent<Dash>();
        jumpComponent = GetComponent<Jump>();
        hook = GetComponent<Hook>();
    }
    void Restore()
    {
		current_health = max_health;
        EventUpdateHealthUI?.Invoke(this, current_health);
    }

    void Heal(object sender, int health)
    {
        int objetiveHealth = current_health + health;

		current_health = (objetiveHealth > max_health) ? max_health : objetiveHealth;
        EventUpdateHealthUI?.Invoke(this, current_health);

        //Debug.Log("Healing... " + current_health);
    }

    protected override void TakeDamage(object sender, Vector3 damageContactPoint)
    {
        base.TakeDamage(sender, damageContactPoint);
        if (OnlyTakeDmgOnce) return;
        OnlyTakeDmgOnce = true;

        TimeStop.instance.StopTime(0.05f, 10f, 0.5f);
        CameraShakeManager.instance.CameraShake(impulseSource, new Vector3(1, 0.2f, 0));
        EventUpdateHealthUI?.Invoke(this, current_health);

        //Debug.Log("Damaging..." + current_health);

        horizontalMovementComponent.DisableMovementInput();
        dashComponent.DisableDashInput();
        jumpComponent.DisableJumpInput();
        hook.DisableHookInput();
        myRigidbody.velocity = Vector2.zero;

        if (damageContactPoint.y <= 0)
        {
            auxKnockback = DamageKnockbackVector;
            Debug.Log("DERECHA");
        }
        if(damageContactPoint.y > 0) 
        {
            auxKnockback = new Vector2(-DamageKnockbackVector.x, DamageKnockbackVector.y);
            Debug.Log("IZQUIERDA");
        }
        myRigidbody.velocity = auxKnockback.normalized * DamageKnockbackForce;
    }

    public override void EndDamaging()
    {
        myAnimator.SetBool("isDamaging", false);

        horizontalMovementComponent.EnableMovementInput();
        dashComponent.EnableDashInput();
        jumpComponent.EnableJumpInput();
        hook.EnableHookInput();

        InvulnerabilityTimer = InvulnerabilityTime;
    }

}
