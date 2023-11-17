using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedSkeletonCurcuma : MovementGroundedIA
{
	private enum TState { PATROL, FALLING };
	[Header("State params")]
	[SerializeField] private TState tState;
	protected override void CheckState()
	{

		if (myRigidbody2D.velocity.y < 0)
		{
			tState = TState.FALLING;
		} else
		{
			tState = TState.PATROL;
		}
	}

	//protected enum TState { PATROL, ATTACK }

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

        if (isBeingHooked || isDead) return;

        switch (tState)
		{
			case TState.PATROL:
				Patrol();
				break;
			case TState.FALLING:
				Falling();
				break;

		}
	}

	private void Falling()
	{
		myAnimator.SetBool("isFalling", true);
		myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
	}

	protected override void Patrol()
	{
		base.Patrol();
		myAnimator.SetBool("isFalling", false);
	}
}
