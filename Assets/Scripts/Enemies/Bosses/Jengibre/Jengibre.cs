using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
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
	[SerializeField] private float ActionTime;


	[Header("Control variables")]
	[SerializeField] private int stonesInGame;
	[SerializeField] private List<Transform> spawnStonesPoint;
	[SerializeField] private GameObject prefabStone;
	
	[SerializeField] private float offSetX = 2.5f;
	[SerializeField] private float offSetY = 2.5f;

	[SerializeField] private int stonesPerPoint;
	[SerializeField] private bool ActionInProgress = false;

	List<GameObject> stoneList = new List<GameObject>();

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
		InvokeRepeating("Attack", ActionTime, ActionTime);
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
		for (int i = 0; i < stonesPerPoint; i++)
		{
			for (int j = 0; j < spawnStonesPoint.Count; j++)
			{
				float valOffSetX = (float)Random.Range(-offSetX, offSetX);
				float valOffSetY = (float)Random.Range(-offSetY, offSetY);

				Vector3 position = new Vector3(spawnStonesPoint[j].position.x + valOffSetX, spawnStonesPoint[j].position.y + valOffSetY);
				GameObject stone = Instantiate(prefabStone, position, Quaternion.identity);
				stoneList.Add(stone);
			}
		}
	}

	void RemoveStone(GameObject stone)
	{
		if (stoneList.Remove(stone))
		{
			Destroy(stone);
		}
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
