using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAFlyChaseAndPatrol : MovementFlyIA
{
	protected enum TState { PATROL, CHASE, ATTACK }

	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected float attackRange;
	[SerializeField] protected float visionRange;

	[Header("Flying patrol params")]
	[SerializeField] protected List<Transform> patrolPoints;
	[SerializeField] protected int currentPatrolPoint;
	[SerializeField] protected float patrolDistance;
	[SerializeField] protected bool isPlayerInSight;


	protected override void UpdatePath()
	{
		if (seeker.IsDone())
			switch (tState)
			{
				case TState.CHASE:
					Chase();
					break;

				case TState.PATROL:
					Patrol();
					break;
			}
	}

	protected void Patrol()
	{
		seeker.StartPath(myRigidbody2D.position, patrolPoints[currentPatrolPoint].position, OnPathComplete);
	}
	protected void Chase()
	{
		seeker.StartPath(myRigidbody2D.position, playerObject.transform.position, OnPathComplete);
	}

	protected override void FixedUpdate()
	{
		CheckState();
		if (tState == TState.ATTACK) return;
		if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < patrolDistance)
		{
			int previousCurrentPatrolPoint = currentPatrolPoint;
			do
			{
				currentPatrolPoint = Random.Range(0, patrolPoints.Count);
			} while (previousCurrentPatrolPoint == currentPatrolPoint);
		}

		IAWorking();
	}
	protected void CheckState()
	{

		isPlayerInSight = Vector3.Distance(transform.position, playerObject.transform.position) < visionRange;

		if (Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
		{
			tState = TState.ATTACK;
		}
		else if (isPlayerInSight)
		{
			tState = TState.CHASE;
		}
		else
		{
			tState = TState.PATROL;
		}
	}
}
