using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Jengibre : Enemy
{
	private enum TStateStone { GOING_JENGIBRE, ATTACK_PLAYER, PROTECT, NONE };
	private TStateStone StoneState;


	public enum CharacterAction
	{
		DROP_STONES,
		THROW_STONES,
		PROTECT,
		ENERGY_BALL,
		PHYSICAL_ATTACK,
		IDLE
	}
	public CharacterAction action;

	private HealthManager myHealthManager;
	[Header("Player components")]
	[SerializeField] private GameObject playerObject;
	[SerializeField] private HealthPlayerManager playerHealth;
	[SerializeField] private Hook playerHook;

	JengibreUtilitySystemCalculator utilitySystem;
	[Header("Jengibre params")]
	[SerializeField] private Transform TelekinesisTransform;
	[SerializeField] private float UpdateUtilitySystemTime;
	[SerializeField] private float ActionTime;
	[SerializeField] private float ProtectTime;
	[SerializeField] private float speedToTelekinesis;
	[SerializeField] private float speedToPlayer;
	[SerializeField] private List<Transform> spawnStonesPoint;
	[SerializeField] private GameObject prefabStone;
	[SerializeField] private int stonesPerPoint;

	[Header("Control variables")]
	[SerializeField] private int stonesInGame;

	[SerializeField] private float protectTimer;


	[SerializeField] private float offSetX = 2.5f;
	[SerializeField] private float offSetY = 2.5f;

	[SerializeField] private bool ActionInProgress = false;

	[SerializeField] List<GameObject> stoneList = new List<GameObject>();
	[SerializeField] GameObject throwingStone;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		utilitySystem = new JengibreUtilitySystemCalculator();
		myHealthManager = GetComponent<HealthManager>();

		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerHealth = playerObject.GetComponent<HealthPlayerManager>();
		playerHook = playerObject.GetComponent<Hook>();

		//InvokeRepeating("UpdateUtilitySystem", UpdateUtilitySystemTime, UpdateUtilitySystemTime);
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
	}

	// Update is called once per frame
	void Update()
	{
		/*if (utilitySystem.action == JengibreUtilitySystemCalculator.CharacterAction.THROW_STONES ||
			utilitySystem.action == JengibreUtilitySystemCalculator.CharacterAction.PROTECT)
		{
			UpdateStoneMovement();
		}*/
		if (action == CharacterAction.THROW_STONES ||
			action == CharacterAction.PROTECT )
		{
			UpdateStoneMovement();
		}

	}

	private void UpdateStoneMovement()
	{
		if (throwingStone == null) return;
		switch (StoneState)
		{
			case TStateStone.GOING_JENGIBRE:
				GoingTelekinesisPoint();
				break;
			case TStateStone.ATTACK_PLAYER:
				GoingToPlayer();
				break;
			case TStateStone.PROTECT:
				ProtectJengibre();
				break;
			case TStateStone.NONE:
				break;
		}
	}

	private void GoingTelekinesisPoint()
	{
		throwingStone.GetComponent<JengibreStone>().GoJengibre(TelekinesisTransform, speedToTelekinesis);
		if (Vector3.Distance(throwingStone.transform.position, TelekinesisTransform.position) < 0.5f)
		{
			//StoneState = (utilitySystem.action == JengibreUtilitySystemCalculator.CharacterAction.THROW_STONES) ?
			StoneState = (action == CharacterAction.THROW_STONES) ?
				TStateStone.ATTACK_PLAYER : TStateStone.PROTECT;
		}
	}

	private void GoingToPlayer()
	{
		throwingStone.GetComponent<JengibreStone>().AttackPlayer(playerObject.transform, speedToTelekinesis);
	}

	private void ProtectJengibre()
	{
		if (protectTimer > 0.0f)
		{
			throwingStone.GetComponent<JengibreStone>().Protect();
			protectTimer -= Time.deltaTime;
		} else
		{
			StoneState = TStateStone.ATTACK_PLAYER;
		}
	}


	void ONANIMATION_SelectAttack()
	{
		if (ActionInProgress) return;
		ActionInProgress = true;
		/*switch (utilitySystem.action)
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
		}*/

		
		switch (action)
		{
			case CharacterAction.DROP_STONES:
				myAnimator.SetTrigger("ATK_DropStones");
				break;
			case CharacterAction.THROW_STONES:
				myAnimator.SetTrigger("ATK_ThrowStones");
				break;
			case CharacterAction.PROTECT:
				myAnimator.SetTrigger("ATK_Protect");
				break;
			case CharacterAction.ENERGY_BALL:
				myAnimator.SetTrigger("ATK_EnergyBall");
				break;
			case CharacterAction.PHYSICAL_ATTACK:
				myAnimator.SetTrigger("ATK_PhysicalAttack");
				break;
		}
	}
	public void ONANIMATION_RETURN_TO_IDLE()
	{
		Debug.Log("Stop animacion de ataque");
		myAnimator.SetBool("isAttacking", false);
	}

	public void LIBERATE_ATTACK()
	{
		Debug.Log("Liberar ataque");
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
				JengibreStone stoneLogic = stone.GetComponent<JengibreStone>();
				stoneLogic.OnDestroy += RemoveStone;
				stoneList.Add(stone);
			}
		}
	}

	void RemoveStone(object sender, GameObject stone)
	{
		if (stoneList.Remove(stone))
		{
			Destroy(stone);
		}
		if(StoneState != TStateStone.NONE)
		{
			StoneState = TStateStone.NONE;
			LIBERATE_ATTACK();
		}
	}

	private GameObject selectRandomStone()
	{
		int selectedRandom = Random.Range(0, stoneList.Count - 1);
		return stoneList[selectedRandom];
	}

	void ONANIMATION_THROWSTONES_ACTION()
	{
		Debug.Log("JENGIBRE:\tTHROW STONES");
		throwingStone = selectRandomStone();
		StoneState = TStateStone.GOING_JENGIBRE;

	}
	void ONANIMATION_PROTECT_ACTION()
	{
		Debug.Log("JENGIBRE:\tPROTECT");

		throwingStone = selectRandomStone();
		StoneState = TStateStone.GOING_JENGIBRE;
		protectTimer = ProtectTime;
	}
	void ONANIMATION_ENERGYBALL_ACTION()
	{
		Debug.Log("JENGIBRE:\tENERGY BALL");

	}

	private void ONANIMATION_PHYSICALATTACKL_ACTION()
	{
		Debug.Log("JENGIBRE:\tPHYSICAL ATTACK");
	}
}
