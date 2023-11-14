using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagerCurcuma : HealthManager
{
	public EventHandler EventSecondPhase;
	[Header("Phases")]
	[SerializeField] private bool SecondPhase = false;

    // Start is called before the first frame update
    protected override void TakeDamage(object sender, int damage)
    {
		if (!canTakeDamage || myAnimator.GetBool("isDamaging")) return;
		myAnimator?.SetBool("isDamaging", true);
		int objetiveHealth = current_health - damage;
		current_health = (objetiveHealth < 0) ? 0 : objetiveHealth;

		if (current_health <= max_health/2 && !SecondPhase)
		{
			SecondPhase = true;
			EventSecondPhase?.Invoke(this, null);
		} else if (current_health <= 0)
		{
			EventDie?.Invoke(this, null);
		}

		Debug.Log("Damage received " + current_health);
	}
}
