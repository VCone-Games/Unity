 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAFlyPatrol : MovementFlyIA
{
	protected enum TState { PATROL, ATTACK }

	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected float attackRange;

	[Header("Flying patrol params")]
	[SerializeField] protected List<Transform> patrolPoints;
	[SerializeField] protected int currentPatrolPoint;
	[SerializeField] protected float patrolDistance;

	protected override void UpdatePath()
	{
		if (seeker.IsDone())
			seeker.StartPath(myRigidbody2D.position, patrolPoints[currentPatrolPoint].position, OnPathComplete);
	}

	protected override void FixedUpdate()
	{
		CheckState();
		if (tState == TState.ATTACK)
		{
			Attack();
			return;
		}
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

	protected virtual void CheckState()
	{
		if (Vector3.Distance(transform.position, playerObject.transform.position) < attackRange)
		{
			tState = TState.ATTACK;
		}
	}

	protected override void StopAttack()
	{
		base.StopAttack();
		tState = TState.PATROL;
		Debug.Log("Cambiando a patrulla");
	}
}
