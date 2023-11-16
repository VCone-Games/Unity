using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagerAzafran : HealthManager
{
	[Header("Phases")]
	[SerializeField] private bool SecondPhase = false;

	protected override void TakeDamage(object sender, Vector3 damage)
	{
		if (!canTakeDamage || myAnimator.GetBool("isDamaging")) return;
		myAnimator.SetBool("isDamaging", true);
		int objetiveHealth = current_health - (int)damage.x;
		current_health = (objetiveHealth < 0) ? 0 : objetiveHealth;

		if (!SecondPhase && current_health <= 0)
		{
			SecondPhase = true;
			myAnimator.SetBool("secondPhase", true);
			current_health = max_health;
			canTakeDamage = false;
		}
		else if (SecondPhase && current_health <= 0)
		{
			EventDie?.Invoke(this, null);
		}
		Debug.Log("Damage received " + current_health);
	}

	void DamageReceived()
	{
		myAnimator.SetBool("isDamaging", false);
	}

	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.F))
			EventDamageTaken?.Invoke(this, new Vector3(1, 0, 0));
	}
}
