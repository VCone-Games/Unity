using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPhaseAzafran : Enemy
{

	[Header("Player params")]
	[SerializeField] private GameObject playerObject;

	enum TStateAttack { CHARGE, DIG, DIG_TRAVEL, DIGGING_BACK, SHOOTING, IDLE }
	enum TStateMovingToDigPoint { GROUND, WALL, PLATAFORM, NONE }
	[Header("State params")]
	[SerializeField] private TStateAttack tState = TStateAttack.IDLE;
	[SerializeField] private TStateMovingToDigPoint tStateMoving = TStateMovingToDigPoint.NONE;

	[SerializeField] private List<float> probList;
	[SerializeField] private Dictionary<TStateAttack, float> stateDictionary;
	[SerializeField] private float decissionTime;
	[SerializeField] private Vector3 directionCharge;

	[Header("PREFABS")]
	[SerializeField] private GameObject prefabStone;

	[Header("CHASE STATE")]
	[SerializeField] private int stonesSummoned;

	[Header("SHOOT STATE")]
	[SerializeField] private float shootTime;
	[SerializeField] private int maxShoots;

	[Header("DIG STATE")]
	[SerializeField] private float digTime;
	[SerializeField] private float digSpeedGround;
	[SerializeField] private float digSpeedWall;
	[SerializeField] private List<Transform> digPlaces;
	[SerializeField] private Transform digObjetive;
	[SerializeField] private Transform previousDigPlace;
	[SerializeField] private Vector2 digDirection;
	[SerializeField] private float distanceToChangeMovementState;

	[Header("Azafran params")]
	[SerializeField] private Transform rightCheck;
	[SerializeField] private Transform spawnAttack;
	[SerializeField] private float circleRadius;
	[SerializeField] private bool rightSide;
	[SerializeField] private LayerMask wallMask;
	[SerializeField] private float raycastWallLenght;
	[SerializeField] private float gravityScale;

	[Header("Control state params")]
	[SerializeField] private float digTimer;
	[SerializeField] private float shootTimer;
	[SerializeField] private int shootCount;


	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();

		gravityScale = myRigidbody2D.gravityScale;
		playerObject = GameObject.FindGameObjectWithTag("Player");
		stateDictionary = new Dictionary<TStateAttack, float>();
		InitializeStateDictionary(probList, stateDictionary);

		InvokeRepeating("Attack", decissionTime, decissionTime);
	}

	protected override void Attack()
	{

		if (!(tState == TStateAttack.IDLE))
		{
			return;
		}


		float accumulate = 0;
		foreach (var probability in probList)
		{
			accumulate += probability;
		}

		float selectedAttack = (float)UnityEngine.Random.Range(0, accumulate * 100) / 100;

		foreach (var state in stateDictionary)
		{
			if (selectedAttack < state.Value)
			{
				tState = state.Key;
				Attack(tState);
				break;
			}
		}
	}

	private void Attack(TStateAttack tState)
	{
		switch (tState)
		{
			case TStateAttack.CHARGE:
				Charge();
				break;
			case TStateAttack.DIG:
				Dig();
				break;
		}
	}

	void Charge()
	{
		tState = TStateAttack.CHARGE;
		directionCharge = (playerObject.transform.position - transform.position).normalized;
		myAnimator.SetBool("isCharging", true);
	}

	void Dig()
	{
		tState = TStateAttack.DIG;
		myRigidbody2D.gravityScale = 0.0f;
		myAnimator.SetBool("beginDigging", true);
		previousDigPlace.position = transform.position;
	}

	void StartDigging()
	{
		tState = (tState == TStateAttack.DIG) ?
			TStateAttack.DIG_TRAVEL :
			TStateAttack.DIGGING_BACK;
		if (tState == TStateAttack.DIGGING_BACK) shootCount = 0;

		tStateMoving = (tState == TStateAttack.DIG_TRAVEL) ?
			TStateMovingToDigPoint.GROUND :
			TStateMovingToDigPoint.PLATAFORM;

		myAnimator.SetBool("beginDigging", false);
		myAnimator.SetBool("isDigging", true);

		int selectedPlace = UnityEngine.Random.Range(0, digPlaces.Count);

		digObjetive.position = (tState == TStateAttack.DIG_TRAVEL) ?
			digPlaces[selectedPlace].position :
			previousDigPlace.position;

		digDirection = digObjetive.position;
	}

	private void InitializeStateDictionary(List<float> probabilities, Dictionary<TStateAttack, float> dictionary)
	{
		float cumulativeProbability = 0f;

		for (int i = 0; i < probabilities.Count; i++)
		{
			cumulativeProbability += probabilities[i];
			dictionary[(TStateAttack)i] = cumulativeProbability;
		}
	}

	void ChargeLogic()
	{
		myRigidbody2D.velocity = new Vector2(directionCharge.x * moveSpeed, 0);

		if (Physics2D.OverlapCircle(rightCheck.position, circleRadius, wallMask))
		{
			myAnimator.SetBool("isCharging", false);
			GetComponent<PhaseManagerAzafran>().summonFallingStone?.Invoke(this, stonesSummoned);
			tState = TStateAttack.IDLE;
		}
	}
	/// <summary>
	/// ///////////////////////////////////
	/// </summary>

	void AscendWall(bool RightWall, bool LeftWall, bool condition)
	{
		Vector3 rotator;
		if (condition)
		{
			if (RightWall)
			{
				if (tState == TStateAttack.DIG_TRAVEL) rightSide = true;
				rotator = new Vector3(0, 0, 90);
				transform.rotation = Quaternion.Euler(rotator);
				tStateMoving = TStateMovingToDigPoint.WALL;
			}
			else
			{
				myRigidbody2D.velocity = new Vector2(digSpeedGround, 0.0f);
			}
		}
		else
		{

			if (LeftWall)
			{
				if (tState == TStateAttack.DIG_TRAVEL) rightSide = false;
				rotator = new Vector3(0, 180, 90);
				transform.rotation = Quaternion.Euler(rotator);
				tStateMoving = TStateMovingToDigPoint.WALL;
			}
			else
			{
				myRigidbody2D.velocity = new Vector2(-digSpeedGround, 0.0f);
			}
		}
	}
	void TravelToWall()
	{
		bool LeftWall = Physics2D.Raycast(myCollider2D.bounds.center, Vector2.left,
			myCollider2D.bounds.extents.x + raycastWallLenght, wallMask);
		bool RightWall = Physics2D.Raycast(myCollider2D.bounds.center, Vector2.right,
			myCollider2D.bounds.extents.x + raycastWallLenght, wallMask);

		switch (tState)
		{
			case TStateAttack.DIG_TRAVEL:
				AscendWall(RightWall, LeftWall, (digDirection.x >= 0));
				break;
			case TStateAttack.DIGGING_BACK:
				AscendWall(RightWall, LeftWall, rightSide);
				break;
		}
	}
	void TravelOnWall()
	{
		Vector2 destiny = new Vector2(0.0f, digObjetive.position.y);
		Vector2 origin = new Vector2(0.0f, transform.position.y);

		if (Vector2.Distance(origin, destiny) >= distanceToChangeMovementState)
		{
			myRigidbody2D.velocity = (tState == TStateAttack.DIG_TRAVEL) ? 
				new Vector2(0.0f, digSpeedWall) : 
				new Vector2(0.0f, -digSpeedWall);
		}
		else
		{
			tStateMoving = (tState == TStateAttack.DIG_TRAVEL)?
				TStateMovingToDigPoint.PLATAFORM:
				TStateMovingToDigPoint.GROUND;
		}
	}
	private void TravelToDestiny()
	{
		Vector2 destiny = new Vector2(digObjetive.position.x, 0.0f);
		Vector2 origin = new Vector2(transform.position.x, 0.0f);

		Vector2 direction = destiny - origin;
		Vector3 rotator;
		if (Vector2.Distance(destiny, origin) >= distanceToChangeMovementState)
		{
			myRigidbody2D.velocity = (direction.x >= 0) ?
				new Vector2(digSpeedGround, 0.0f) :
				new Vector2(-digSpeedGround, 0.0f);

			rotator = (direction.x > 0) ?
				new Vector3(0, 0, 0) :
				new Vector3(0, 180, 0);

			transform.rotation = Quaternion.Euler(rotator);
		}
		else
		{
			myRigidbody2D.velocity = Vector2.zero;
			myRigidbody2D.gravityScale = gravityScale;
			myAnimator.SetBool("isDigging", false);
			myAnimator.SetBool("endDigging", true);
			tStateMoving = TStateMovingToDigPoint.NONE;

			tState = (tState == TStateAttack.DIG_TRAVEL) ?
				TStateAttack.SHOOTING :
				TStateAttack.IDLE;
		}
	}
	void TravellingToPoint()
	{
		switch (tStateMoving)
		{
			case TStateMovingToDigPoint.GROUND:
				TravelToWall();
				break;
			case TStateMovingToDigPoint.WALL:
				TravelOnWall();
				break;
			case TStateMovingToDigPoint.PLATAFORM:
				TravelToDestiny();
				break;
		}
	}
	void ReturnToPoint()
	{
		switch (tStateMoving)
		{
			case TStateMovingToDigPoint.PLATAFORM:
				TravelToWall();
				break;
			case TStateMovingToDigPoint.WALL:
				TravelOnWall();
				break;
			case TStateMovingToDigPoint.GROUND:
				TravelToDestiny();
				break;
		}
	}

	/// <summary>
	/// ///////////////////////////////////
	/// </summary>
	void EndDigging()
	{
		myAnimator.SetBool("endDigging", false);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 direction;

		if (tState == TStateAttack.IDLE || tState == TStateAttack.SHOOTING)
		{
			direction = playerObject.transform.position - transform.position;
			facingRight = (direction.x > 0) ? true : false;
		}
		else if (tState == TStateAttack.CHARGE)
		{
			direction = myRigidbody2D.velocity;
			facingRight = (direction.x > 0) ? true : false;
		}

		switch (tState)
		{
			case TStateAttack.CHARGE:
				ChargeLogic();
				break;
			case TStateAttack.DIG_TRAVEL:
				TravellingToPoint();
				break;
			case TStateAttack.SHOOTING:
				Shoot();
				break;
			case TStateAttack.DIGGING_BACK:
				ReturnToPoint();
				break;
		}

		if (tStateMoving != TStateMovingToDigPoint.NONE) return;
		Vector3 rotator = (facingRight) ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
		transform.rotation = Quaternion.Euler(rotator);
	}

	void Shoot()
	{
		if (shootTimer < 0.0f)
		{
			if (shootCount != maxShoots)
			{
				myAnimator.SetBool("isAttacking", true);
			}
			else
			{
				myAnimator.SetBool("beginDigging", true);
			}
		}
		else if (shootTimer >= 0.0f)
		{
			shootTimer -= Time.deltaTime;
		}

	}

	public override void StopAttack()
	{
		base.StopAttack();
		shootTimer = shootTime;
		shootCount++;
	}

	void InsantiateProjectile()
	{
		Instantiate(prefabStone, spawnAttack.position, Quaternion.identity);
	}
}
