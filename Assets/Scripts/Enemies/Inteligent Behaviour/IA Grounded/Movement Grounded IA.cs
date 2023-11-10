using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementGroundedIA : Enemy
{
	[Header("Player params")]
	[SerializeField] protected GameObject playerObject;
	[SerializeField] protected LayerMask playerLayer;

	[Header("Grounded params")]
	[SerializeField] protected Transform feet;
	[SerializeField] protected LayerMask groundLayer;
	[SerializeField] protected float distance;

	[Header("Control grounded variables")]
	[SerializeField] protected bool grounded;

	protected virtual void Patrol()
	{
		myRigidbody2D.velocity = (facingRight) ?
			new Vector2(moveSpeed, myRigidbody2D.velocity.y) :
			new Vector2(-moveSpeed, myRigidbody2D.velocity.y);
	}

	protected abstract void CheckState();
	protected virtual void FixedUpdate()
	{
		CheckState(); 

		grounded = Physics2D.Raycast(feet.position, Vector2.down, distance, groundLayer);

		if (!grounded)
		{
			facingRight = !facingRight;
			transform.Rotate(new Vector3(0, 180, 0));
		}
	}
}
