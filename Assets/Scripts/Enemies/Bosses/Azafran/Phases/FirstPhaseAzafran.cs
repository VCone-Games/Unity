using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPhaseAzafran : Enemy
{

	[Header("Player params")]
	[SerializeField] private GameObject playerObject;

	enum TStateAttack { CHARGE, DIG, SHOOTING, DIGGING_BACK, IDLE }
	[Header("State params")]
	[SerializeField] private TStateAttack tState = TStateAttack.IDLE;
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
	[SerializeField] private List<Transform> digPlaces;
	[SerializeField] private Transform digWaiting;
	[SerializeField] private Transform previousDigPlace;

	[Header("Azafran params")]
	[SerializeField] private Transform rightCheck;
	[SerializeField] private Transform spawnAttack;
	[SerializeField] private float circleRadius;
	[SerializeField] private LayerMask wallMask;

	[Header("Control state params")]
	[SerializeField] private float digTimer;
	[SerializeField] private float shootTimer;
	[SerializeField] private int shootCount;


	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();

		playerObject = GameObject.FindGameObjectWithTag("Player");
		stateDictionary = new Dictionary<TStateAttack, float>();
		InitializeStateDictionary(probList, stateDictionary);

		InvokeRepeating("Attack", 0.0f, decissionTime);
	}

	protected override void Attack()
	{

		if (!(tState == TStateAttack.IDLE))
		{
			Debug.Log("\tInvalid state, current state:" + tState.ToString());
			return;
		}

		Debug.Log("Procesando ataque");

		float accumulate = 0;
		foreach (var probability in probList)
		{
			accumulate += probability;
		}

		float selectedAttack = (float)UnityEngine.Random.Range(0, accumulate * 100) / 100;
		Debug.Log("prob:" + selectedAttack);

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
		Debug.Log("state: " + tState.ToString());
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

		Debug.Log("Charge");
	}

	void Dig()
	{
		tState = TStateAttack.DIG;

		myRigidbody2D.isKinematic = true;
		myRigidbody2D.velocity = new Vector2(0.0f, 0.0f);

		previousDigPlace.position = transform.position;
		transform.position = digWaiting.position;

		digTimer = digTime;
		Debug.Log("Dig");
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


		if (tState == TStateAttack.CHARGE)
		{
			myRigidbody2D.velocity = new Vector2(directionCharge.x * moveSpeed, 0);

			if (Physics2D.OverlapCircle(rightCheck.position, circleRadius, wallMask))
			{
				Debug.Log("Encuentro pared");
				GetComponent<PhaseManagerAzafran>().summonFallingStone?.Invoke(this, stonesSummoned);
				tState = TStateAttack.IDLE;
			}
		}
		else if (tState == TStateAttack.DIG && digTimer > 0.0f)
		{
			digTimer -= Time.deltaTime;
			Debug.Log("Digging...");
		}
		else if (tState == TStateAttack.DIG && digTimer < 0.0f)
		{
			int selectedPlace = UnityEngine.Random.Range(0, digPlaces.Count);


			transform.position = digPlaces[selectedPlace].position;
			Debug.Log("Disparando...");
			shootTimer = shootTime;
			tState = TStateAttack.SHOOTING;
		}
		else if (tState == TStateAttack.SHOOTING)
		{
			Shoot();
		}
		else if (tState == TStateAttack.DIGGING_BACK && digTimer > 0.0f)
		{
			digTimer -= Time.deltaTime;
			Debug.Log("Digging back...");
		}
		else if (tState == TStateAttack.DIGGING_BACK && digTimer < 0.0f)
		{
			transform.position = previousDigPlace.position;
			myRigidbody2D.isKinematic = false;
			tState = TStateAttack.IDLE;
		}

		Vector3 rotator = (facingRight) ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
		transform.rotation = Quaternion.Euler(rotator);
	}

	void Shoot()
	{
		if (shootTimer < 0.0f)
		{
			if (shootCount == maxShoots)
			{
				Debug.Log("Volviendo a DIGGING_BACK");
				shootCount = 0;
				transform.position = digWaiting.position;
				tState = TStateAttack.DIGGING_BACK;
				digTimer = digTime;
				return;
			}
			else
			{
				Debug.Log("*********Shooting");
				Debug.Log(spawnAttack.position);
				Instantiate(prefabStone, spawnAttack.position, Quaternion.identity);
				shootTimer = shootTime;
				shootCount++;
			}
		}
		else if (shootTimer >= 0.0f)
		{
			Debug.Log("Recargando...");
			shootTimer -= Time.deltaTime;
		}

	}
}
