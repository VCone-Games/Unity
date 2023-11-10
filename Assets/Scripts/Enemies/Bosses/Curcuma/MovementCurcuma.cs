using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementCurcuma : Enemy
{
	/*
	private enum TState { CROW_A, CROW_B, CROW_C, ENERGY_CROW }
	[Header("State params")]
	[SerializeField] private TState tState;
	[SerializeField] private List<float> probAttacks;
	[SerializeField] private List<float> probAttacksSecondPhase;
	[SerializeField] private Dictionary<TState, float> stateDictionary;
	[SerializeField] private Dictionary<TState, float> stateDictionarySecondPhase;
	[SerializeField] private float attackTimer;
	[SerializeField] private float attackTimerSecondPhase;

	[Header("Flying patrol params")]
	[SerializeField] private List<Transform> patrolPoints;
	[SerializeField] private int currentPatrolPoint;
	[SerializeField] private float patrolDistance;

	[Header("AI flying params")]
	[SerializeField] private float nextWaypointDistance = 3.0f;
	[SerializeField] private int currentWayPoint;
	[SerializeField] private Path path;
	[SerializeField] private Seeker seeker;
	[SerializeField] private bool reachedEndOfPath;

	[Header("Control variables")]
	[SerializeField] private float attackTime;
	[SerializeField] private bool secondPhase = false;

	protected override void Attack()
	{
		float accumulate = 0;
		List<float> list = (secondPhase) ? probAttacksSecondPhase : probAttacks;
		foreach (var probability in list)
		{
			accumulate += probability;
		}

		float selectAttack = (float)UnityEngine.Random.Range(0, accumulate*100) /100;
		Debug.Log(selectAttack);

		Dictionary<TState, float> dictonary = (secondPhase) ? stateDictionarySecondPhase : stateDictionary;
		foreach (var state in dictonary)
		{
			if (selectAttack < state.Value)
			{
				tState = state.Key;
				Attack(tState);
				break;
			}

		}
	}
	private void Attack(TState tState)
	{
		switch (tState)
		{
			case TState.CROW_A:
				Debug.Log("Invocar crow A");
				break;
			case TState.CROW_B:
				Debug.Log("Invocar crow B");
				break;
			case TState.CROW_C:
				Debug.Log("Invocar crow C");
				break;
			case TState.ENERGY_CROW:
				Debug.Log("Invocar energy crow");
				break;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		EventSecondPhase += SecondPhaseActivation;
		stateDictionary = new Dictionary<TState, float>();

		stateDictionary[TState.CROW_A] = probAttacks[0];
		stateDictionary[TState.CROW_B] = probAttacks[0] + probAttacks[1];
		stateDictionary[TState.CROW_C] = probAttacks[0] + probAttacks[1] + probAttacks[2];
		stateDictionary[TState.ENERGY_CROW] = probAttacks[0] + probAttacks[1] + probAttacks[2] + probAttacks[3];

		stateDictionarySecondPhase[TState.CROW_A] = probAttacksSecondPhase[0];
		stateDictionarySecondPhase[TState.CROW_B] = probAttacksSecondPhase[0] + probAttacksSecondPhase[1];
		stateDictionarySecondPhase[TState.CROW_C] = probAttacksSecondPhase[0] + probAttacksSecondPhase[1] + probAttacksSecondPhase[2];
		stateDictionarySecondPhase[TState.ENERGY_CROW] = probAttacksSecondPhase[0] + probAttacksSecondPhase[1] + probAttacksSecondPhase[2] + probAttacksSecondPhase[3];

		seeker = GetComponent<Seeker>();
		InvokeRepeating("UpdatePath", 0f, .5f);
		InvokeRepeating("Attack", 0f, attackTimer);

	}

	private void SecondPhaseActivation(object sender, EventArgs e)
	{
		moveSpeed += 2;
		secondPhase = true;
		CancelInvoke("Attack");
		InvokeRepeating("Attack", 0f, attackTimerSecondPhase);

		Debug.Log("Spawn del exoesqueleto");
	}

	void UpdatePath()
	{
		seeker.StartPath(myRigidbody2D.position, patrolPoints[currentPatrolPoint].position, OnPathComplete);
	}

	void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWayPoint = 0;
		}
	}

	private void FixedUpdate()
	{
		if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) <= patrolDistance)
		{
			int previousCurrentPatrolPoint = currentPatrolPoint;
			do
			{
				currentPatrolPoint = UnityEngine.Random.Range(0, patrolPoints.Count);
			} while (previousCurrentPatrolPoint == currentPatrolPoint);
		}

		Movement();
		Flip();
	}

	private void Movement()
	{
		if (path == null) return;

		if (currentWayPoint >= path.vectorPath.Count)
		{
			return;
		}

		Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - myRigidbody2D.position).normalized;

		myRigidbody2D.velocity = direction * moveSpeed;

		float distance = Vector2.Distance(myRigidbody2D.position, path.vectorPath[currentWayPoint]);
		if (distance <= nextWaypointDistance)
		{
			currentWayPoint++;
		}
	}

	private void Flip()
	{
		if (myRigidbody2D.velocity.x >= 0.0f)
		{
			facingRight = true;
			mySpriteRenderer.flipX = false;
		}
		else
		{
			facingRight = false;
			mySpriteRenderer.flipX = true;
		}
	}*/
	protected override void Attack()
	{
		throw new NotImplementedException();
	}
}
