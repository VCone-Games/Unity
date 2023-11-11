using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagerCurcuma : HealthEnemyManager
{
	public EventHandler EventSecondPhase;
	[Header("Phases")]
	[SerializeField] private bool SecondPhase = false;

    // Start is called before the first frame update
    public override void TakeDamage(int damage)
    {
		current_health -= damage;
		if (current_health <= max_health/2 && !SecondPhase)
		{
			SecondPhase = true;
			EventSecondPhase?.Invoke(this, null);
		} else if (current_health <= 0)
		{
			current_health = 0;
			EventDie?.Invoke(this, null);
		}
		Debug.Log("Damage received " + current_health);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Z))
		{
			TakeDamage(1);
		}
	}
}
