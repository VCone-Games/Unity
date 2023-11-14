using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagerAzafran : HealthManager
{
    public EventHandler EventSecondPhase;
	[Header("Phases")]
	[SerializeField] private bool SecondPhase = false;

	protected override void TakeDamage(object sender, int damage)
	{
		if (!canTakeDamage || myAnimator.GetBool("isDamaging")) return;
		myAnimator.SetBool("isDamaging", true);
		int objetiveHealth = current_health - damage;
		current_health = (objetiveHealth < 0) ? 0 : objetiveHealth;

		if (!SecondPhase && current_health <= 0)
		{
			SecondPhase = true;
			EventSecondPhase?.Invoke(this, null);
			current_health = max_health;
			canTakeDamage = false;
		}
		else if (SecondPhase && current_health <= 0)
		{
			EventDie?.Invoke(this, null);
		}
		Debug.Log("Damage received " + current_health);
	}
}
