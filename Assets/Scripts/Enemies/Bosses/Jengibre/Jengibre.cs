using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jengibre : Enemy
{
	private HealthManager myHealthManager;
	[Header("Player components")]
	[SerializeField] private GameObject playerObject;
	[SerializeField] private HealthPlayerManager playerHealth;
	[SerializeField] private Hook playerHook;

	JengibreUtilitySystemCalculator utilitySystem;
	[Header("Jengibre params")]
	[SerializeField] private float UpdateUtilitySystemTime;


	[Header("Control variables")]
	[SerializeField] private int stonesInGame;
	[SerializeField] private bool ActionInProgress = false;


	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		utilitySystem = new JengibreUtilitySystemCalculator();
		myHealthManager = GetComponent<HealthManager>();

		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerHealth = playerObject.GetComponent<HealthPlayerManager>();
		playerHook = playerObject.GetComponent<Hook>();

		InvokeRepeating("UpdateUtilitySystem", UpdateUtilitySystemTime, UpdateUtilitySystemTime);
	}

	void UpdateUtilitySystem()
	{
		if (ActionInProgress) return;

		float distanceToPlayer = Vector3.Distance(gameObject.transform.position, playerObject.transform.position);
		float playerHealthPercentage = playerHealth.CurrentHealth / playerHealth.MaxHealth;
		float ownHealthPercentage = myHealthManager.CurrentHealth / myHealthManager.MaxHealth;
		bool hookingSomething = playerHook.ShootingHook;

		utilitySystem.SetChances(stonesInGame, distanceToPlayer,
			playerHealthPercentage, ownHealthPercentage, hookingSomething);
	}

	// Update is called once per frame
	void Update()
	{
		SelectAttack();
	}

	void SelectAttack()
	{
		if (ActionInProgress) return;
		ActionInProgress = true;
		switch (utilitySystem.action)
		{
			case JengibreUtilitySystemCalculator.CharacterAction.DROP_STONES:
				break;
			case JengibreUtilitySystemCalculator.CharacterAction.THROW_STONES:
				break;
			case JengibreUtilitySystemCalculator.CharacterAction.PROTECT:
				break;
			case JengibreUtilitySystemCalculator.CharacterAction.ENERGY_BALL:
				break;
			case JengibreUtilitySystemCalculator.CharacterAction.PHYSICAL_ATTACK:
				break;
		}
	}
	void RETURN_TO_IDLE()
	{
		ActionInProgress = false;
	}
	void DROP_STONES_ACTION()
	{
		Debug.Log("Drop_Stones");
	}
}
