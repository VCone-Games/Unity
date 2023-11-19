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
    protected override void TakeDamage(object sender, Vector3 damage)
    {
		if (!canTakeDamage || myAnimator.GetBool("isDamaging")) return;
		myAnimator.SetBool("isDamaging", true);
		Debug.Log("CURCUMA RECIBE: " + (int)damage.x);
		int objetiveHealth = current_health - (int)damage.x;
		current_health = (objetiveHealth < 0) ? 0 : objetiveHealth;

		if (current_health <= max_health/2 && !SecondPhase)
		{
			SecondPhase = true;
			EventSecondPhase?.Invoke(this, null);
		}
		if (current_health <= 0)
		{
			EventDie?.Invoke(this, null);
		}

		Debug.Log("CURCUUUUUUUMAAAAA::: Damage received " + current_health);
	}

	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.F)) EventDamageTaken(this, new Vector3(4, 0, 0));
	}
}
