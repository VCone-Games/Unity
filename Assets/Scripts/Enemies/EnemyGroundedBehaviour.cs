using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyGroundedBehaviour : EnemyBaseBehaviour
{

	[Header("Grounded params")]
	[SerializeField] private Transform feet;
	[SerializeField] private Vector2 initialFeetPos;
	[SerializeField] private Vector2 inverseFeetPos;
	[SerializeField] private float distance;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private bool grounded;

	protected override void Patrol()
	{
		myRigidbody2D.velocity = (facingRight) ?
			new Vector2(moveSpeed, myRigidbody2D.velocity.y) :
			new Vector2(-moveSpeed, myRigidbody2D.velocity.y);
	}

	protected override void Chase()
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

		Debug.Log("Chase state");
	}

	protected override void CheckChangeState()
	{
		isPlayerInSight = Physics2D.Raycast(myCollider2D.bounds.center, Vector2.left, myCollider2D.bounds.extents.y + visionRange, playerLayer) ||
			Physics2D.Raycast(myCollider2D.bounds.center, Vector2.right, myCollider2D.bounds.extents.y + visionRange, playerLayer);

		if (isPlayerInSight)
		{
			chaseTimer = chaseTime;
		}
		if (Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
		{
			ChangeState(TState.ATTACK);
		}
		else if (isPlayerInSight || chaseTimer > 0.0f)
		{
			ChangeState(TState.CHASE);
		}
		else
		{
			ChangeState(TState.PATROL);
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();


		if (facingRight)
		{
			mySpriteRenderer.flipX = false;
			feet.localPosition = initialFeetPos;
		}
		else
		{
			mySpriteRenderer.flipX = true;
			feet.localPosition = inverseFeetPos;
		}

		grounded = Physics2D.Raycast(feet.position, Vector2.down, distance, groundLayer);

		if (!grounded)
		{
			facingRight = !facingRight;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		initialFeetPos = feet.localPosition;
		inverseFeetPos = initialFeetPos;
		inverseFeetPos.x = -inverseFeetPos.x;
	}
}
