using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagerCurcuma : HealthEnemyManager
{
    // Start is called before the first frame update
    public override void TakeDamage(int damage)
    {
		current_health -= damage;
		if (current_health <= max_health/2)
		{
			enemyComponent.EventSecondPhase?.Invoke(this, null);
		} else
		{
			enemyComponent.EventDie?.Invoke(this, null);
		}
	}
}
