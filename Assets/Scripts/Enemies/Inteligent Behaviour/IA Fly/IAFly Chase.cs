using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAFlyChase : MovementFlyIA
{
	protected enum TState { CHASE, ATTACK }

	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected float attackRange;

	protected override void UpdatePath()
	{
		if (seeker.IsDone())
			seeker.StartPath(myRigidbody2D.position, playerObject.transform.position, OnPathComplete);
	}

	protected override void FixedUpdate()
	{
		CheckState();
		if (tState == TState.ATTACK) return;
		IAWorking();
	}

	private void CheckState()
	{
		if (Vector3.Distance(transform.position, playerObject.transform.position) < attackRange)
		{
			tState = TState.ATTACK;
		}
		else
		{
			tState = TState.CHASE;
		}
	}
}
