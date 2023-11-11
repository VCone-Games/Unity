using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemyManager : MonoBehaviour
{
	public EventHandler EventDie;

	[Header("Health enemy params")]
    [SerializeField] protected int max_health;
    [SerializeField] protected int current_health;
    [SerializeField] protected bool canTakeDamage = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
		current_health = max_health;
    }

	public virtual void TakeDamage(int damage)
	{
        if (!canTakeDamage) return;
		// Implementa cómo el enemigo maneja el daño
		current_health -= damage;
		if (current_health <= 0)
		{
            EventDie?.Invoke(this, null);
		}
	}

}
