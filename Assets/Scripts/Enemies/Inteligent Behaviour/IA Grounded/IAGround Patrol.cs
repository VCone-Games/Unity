using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAGroundPatrol : MovementGroundedIA
{
	protected enum TState { PATROL, ATTACK }
	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected float attackRange;

	protected override void CheckState()
	{
		if (Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
		{
			tState = TState.ATTACK;
		}
	}

	protected virtual void StopAttack()
	{
		base.StopAttack();
		tState = TState.PATROL;
		Debug.Log("Cambiando a patrulla");
	}

	protected virtual void FixedUpdate()
	{
		base.FixedUpdate();

		switch (tState)
		{
			case TState.PATROL:
				Patrol();
				break;
			case TState.ATTACK:
				if (!canAttack) return;
				Attack();
				break;
		}
	}
}
