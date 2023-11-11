using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagerAzafran : HealthEnemyManager
{
    public EventHandler EventSecondPhase;
	[Header("Phases")]
	[SerializeField] private bool SecondPhase = false;

	public override void TakeDamage(int damage)
	{
		if (!canTakeDamage) return;
		current_health -= damage;
		if (!SecondPhase && current_health <= 0)
		{
			SecondPhase = true;
			EventSecondPhase?.Invoke(this, null);
			current_health = max_health;
			canTakeDamage = false;
		}
		else if (SecondPhase && current_health <= 0)
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
