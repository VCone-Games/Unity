using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayerManager : HealthManager
{

	public EventHandler<int> EventHealing;
	public EventHandler<int> EventUpdateHealthUI;

    protected override void Start()
    {
        base.Start();
        EventHealing += Heal;
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

        Debug.Log("Healing... " + current_health);
    }

    protected override void TakeDamage(object sender, int damage)
    {
        base.TakeDamage(sender, damage);
        EventUpdateHealthUI?.Invoke(this, current_health);

        Debug.Log("Damaging..." + current_health);
    }

}
