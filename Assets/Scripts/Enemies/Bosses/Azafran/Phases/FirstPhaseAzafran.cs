using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPhaseAzafran : Enemy
{
	[Header("Player params")]
	[SerializeField] private GameObject playerObject;

	enum TStateAttack {  CHARGE, DIG, IDLE }
	[Header("State params")]
	[SerializeField] private TStateAttack tState = TStateAttack.IDLE;
	[SerializeField] private List<float> probList;
	[SerializeField] private Dictionary<TStateAttack, float> stateDictionary;
	[SerializeField] private float decissionTime;
	[SerializeField] private Vector3 directionCharge;
	[SerializeField] private int shootCounts;

	[Header("Azafran params")]
	[SerializeField] private Transform rightCheck;
	[SerializeField] private float circleRadius;
	[SerializeField] private LayerMask wallMask;

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
		if (!(tState == TStateAttack.IDLE)) return;

		Debug.Log("Procesando ataque");

		float accumulate = 0;
		foreach (var probability in probList)
		{
			accumulate += probability;
		}

		float selectedAttack = (float) Random.Range(0, accumulate * 100) / 100;
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
				Debug.Log("Dig");
				break;
		}
	}

	void Charge()
	{
		tState = TStateAttack.CHARGE;
		directionCharge = (playerObject.transform.position - transform.position).normalized;

		Debug.Log("Charge");
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

		if (tState == TStateAttack.IDLE || tState == TStateAttack.DIG)
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
				tState = TStateAttack.IDLE;
			}
		}

		Vector3 rotator = (facingRight) ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
		transform.rotation = Quaternion.Euler(rotator);
	}
}
