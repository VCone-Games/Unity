using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthPlayerManager : HealthManager
{

    public EventHandler<int> EventHealing;
    public EventHandler<int> EventUpdateHealthUI;

    private HorizontalMovement horizontalMovementComponent;
    private Dash dashComponent;
    private Jump jumpComponent;
    private Hook hook;
    private CinemachineImpulseSource impulseSource;


	[Header("Audio Management")]
    [SerializeField] private PlayerSoundManager soundManager;

    protected override void Start()
    {
        base.Start();
        EventHealing += Heal;
        EventDie += Die;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        horizontalMovementComponent = GetComponent<HorizontalMovement>();
        dashComponent = GetComponent<Dash>();
        jumpComponent = GetComponent<Jump>();
        hook = GetComponent<Hook>();

        max_health = PlayerInfo.Instance.MaxHealth;
		current_health = PlayerInfo.Instance.CurrentHealth;

		HealthUI.HealthUISingleton.EventInitialiteUI?.Invoke(this, this);
	}
	public void Restore()
    {
        Debug.Log($"Restoring.../CurrentHealth: {current_health} /MaxHealth {max_health}");
        current_health = max_health;
        PlayerInfo.Instance.CurrentHealth = max_health;
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
        soundManager.PlayHit();
        horizontalMovementComponent.DisableMovementInput();
        dashComponent.DisableDashInput();
        jumpComponent.DisableJumpInput();
        if (hook.hookProjectile != null)
            hook.hookProjectile.GetComponent<HookProjectile>().DestroyProjectile();
        GameObject hookedObject = GetComponent<Hook>().HookedObject;
        if (hookedObject != null)
        {
            hookedObject.GetComponent<AHookable>().Unhook();
        }
        hook.HookDestroyed();
        hook.DisableHookInput();
        Time.timeScale = 1;

        base.TakeDamage(sender, damageContactPoint);
        if (OnlyTakeDmgOnce) return;
        OnlyTakeDmgOnce = true;

        TimeStop.instance.StopTime(0.05f, 7f, 0.75f);
        CameraShakeManager.instance.CameraShake(impulseSource, new Vector3(1, 0.2f, 0));
        EventUpdateHealthUI?.Invoke(this, current_health);

        gameObject.layer = 11;
    }

    public override void EndDamaging()
    {
        horizontalMovementComponent.EnableMovementInput();
        dashComponent.EnableDashInput();
        jumpComponent.EnableJumpInput();
        hook.EnableHookInput();
        base.EndDamaging();
    }


    protected override void Update()
    {
        if (InvulnerabilityTimer > 0)
        {
            InvulnerabilityTimer -= Time.deltaTime;
            if (InvulnerabilityTimer < 0)
            {
                canTakeDamage = true;
                OnlyTakeDmgOnce = false;
                gameObject.layer = 6;
            }
        }
    }

    protected void Die(object sender, GameObject gameObject)
    {
        horizontalMovementComponent.DisableMovementInput();
        dashComponent.DisableDashInput();
        jumpComponent.DisableJumpInput();
        hook.DisableHookInput();
        myRigidbody.velocity = Vector3.zero;
        myAnimator.SetTrigger("Dead");

		//DataBase.Singleton.DeathCount++;
        PlayerInfo.Instance.IsDead = true;
    }

    protected void EndDieAnimation()
    {
        SceneChanger.Instance.ChangeSceneByDeath();
    }
}
