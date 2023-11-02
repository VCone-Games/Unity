using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
	[Header("Health params")]
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;

	public EventHandler<int> EventUpdateHealth;

	public int MaxHealth {  get { return maxHealth; } set {  maxHealth = value; } }
	public int CurrentHealth { get { return currentHealth; } set {  currentHealth = value; } }

	// Start is called before the first frame update
	void Start()
	{
		currentHealth = maxHealth;
	}

	void Restore()
	{
		currentHealth = maxHealth;
		EventUpdateHealth?.Invoke(this, currentHealth);
	}

	void Heal(int health)
	{
		int objetiveHealth = currentHealth + health;

		currentHealth = (objetiveHealth > maxHealth) ? maxHealth : objetiveHealth;
		EventUpdateHealth?.Invoke(this, currentHealth);

		Debug.Log("Healing... " + currentHealth);
	}

	void Damage(int damage)
	{
		int objetiveHealth = currentHealth - damage;

		currentHealth = (objetiveHealth < 0) ? 0 : objetiveHealth;
		EventUpdateHealth?.Invoke(this, currentHealth);

		Debug.Log("Damaging..." + currentHealth);
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetKey(KeyCode.UpArrow))
		//{
		//	Heal(1);
		//}
		//
		//if (Input.GetKey(KeyCode.DownArrow))
		//{
		//	Damage(1);
		//}
	}
}
