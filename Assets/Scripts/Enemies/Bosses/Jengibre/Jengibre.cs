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
		Attack();
	}

	protected override void Attack()
	{
		base.Attack();
		myAnimator.SetBool("isIDLE", false);
	}

	// Update is called once per frame
	void Update()
	{
	}
	
	

	void ONANIMATION_SelectAttack()
	{
		if (ActionInProgress) return;
		ActionInProgress = true;
		switch (utilitySystem.action)
		{
			case JengibreUtilitySystemCalculator.CharacterAction.DROP_STONES:
				myAnimator.SetTrigger("ATK_DropStones");
				break;
			case JengibreUtilitySystemCalculator.CharacterAction.THROW_STONES:
				myAnimator.SetTrigger("ATK_ThrowStones");
				break;
			case JengibreUtilitySystemCalculator.CharacterAction.PROTECT:
				myAnimator.SetTrigger("ATK_Protect");
				break;
			case JengibreUtilitySystemCalculator.CharacterAction.ENERGY_BALL:
				myAnimator.SetTrigger("ATK_EnergyBall");
				break;
			case JengibreUtilitySystemCalculator.CharacterAction.PHYSICAL_ATTACK:
				myAnimator.SetTrigger("ATK_PhysicalAttack");
				break;
		}
	}
	void ONANIMATION_RETURN_TO_IDLE()
	{
		myAnimator.SetBool("isIDLE", true);
		ActionInProgress = false;
	}
	void ONANIMATION_DROPSTONES_ACTION()
	{
		Debug.Log("Drop_Stones");
	}
	void ONANIMATION_THROWSTONES_ACTION()
	{

	}
	void ONANIMATION_PROTECT_ACTION()
	{

	}
	void ONANIMATION_ENERGYBALL_ACTION()
	{

	}

}
