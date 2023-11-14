using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
	public EventHandler EventDie;
	public EventHandler<int> EventDamageTaken;

	[Header("Health params")]
	[SerializeField] protected int max_health;
	[SerializeField] protected int current_health;
	[SerializeField] protected bool canTakeDamage = true;

	[Header("Components")]
	[SerializeField] protected Animator myAnimator;

	public int MaxHealth { get { return max_health; } set { max_health = value; } }
	public int CurrentHealth { get { return current_health; } set { current_health = value; } }

	// Start is called before the first frame update
	protected virtual void Start()
	{
		current_health = max_health;
		EventDamageTaken += TakeDamage;
		myAnimator = GetComponent<Animator>();
	}

	protected virtual void TakeDamage(object sender, int damage)
	{
		if (!canTakeDamage || myAnimator.GetBool("isDamaging")) return;
		Debug.Log(gameObject + ": Me han dañado");

		// Implementa cómo el enemigo maneja el daño
		myAnimator.SetBool("isDamaging", true);
		int objetiveHealth = current_health - damage;
		current_health = (objetiveHealth < 0) ? 0 : objetiveHealth;

		if (current_health <= 0)
		{
			EventDie?.Invoke(this, null);
		}
	}

	public void EndDamaging()
	{
		myAnimator.SetBool("isDamaging", false);
	}

}
