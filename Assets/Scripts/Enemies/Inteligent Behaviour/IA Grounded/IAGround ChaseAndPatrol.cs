using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAGroundChaseAndPatrol : MovementGroundedIA
{

    [SerializeField] protected float chaseTimer;
	protected enum TState { PATROL, CHASE, ATTACK }
	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected float visionRange;
	[SerializeField] protected float attackRange;

	[Header("Control ground IA chase")]
    [SerializeField] protected float chaseTime;
    [SerializeField] protected bool isPlayerInSight;

	protected void Chase()
	{
		// Persigue al jugador
		chaseTimer -= Time.deltaTime;
		Vector3 destiny = playerObject.transform.position - gameObject.transform.position;

		if (destiny.x > 0)
		{
			myRigidbody2D.velocity = new Vector2(moveSpeed, myRigidbody2D.velocity.y);
			facingRight = true;
		}
		else
		{
			myRigidbody2D.velocity = new Vector2(-moveSpeed, myRigidbody2D.velocity.y);
			facingRight = false;
		}

		//Debug.Log("Chase state");
	}

	protected override void CheckState()
	{
		isPlayerInSight = Physics2D.Raycast(myCollider2D.bounds.center, Vector2.left, myCollider2D.bounds.extents.y + visionRange, playerLayer) ||
			Physics2D.Raycast(myCollider2D.bounds.center, Vector2.right, myCollider2D.bounds.extents.y + visionRange, playerLayer);
		if (myAnimator.GetBool("isAttacking")) return;

		if (isPlayerInSight)
		{
			chaseTimer = chaseTime;
		}
		if (Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
		{
			if (!canAttack) return;
			tState = TState.ATTACK;
		}
		else if (isPlayerInSight || chaseTimer > 0.0f)
		{
			tState = TState.CHASE;
		}
		else
		{
			tState = TState.PATROL;
		}
	}
	public override void StopAttack()
	{
		base.StopAttack();
		tState = TState.CHASE;
		Debug.Log("Cambiando a patrulla");
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
        if (isBeingHooked || isDead) return;

        switch (tState)
		{
			case TState.PATROL:
				Patrol();
				break;
			case TState.CHASE:
				Chase();
				break;
			case TState.ATTACK:
				Attack();
				break;
		}
	}
}
