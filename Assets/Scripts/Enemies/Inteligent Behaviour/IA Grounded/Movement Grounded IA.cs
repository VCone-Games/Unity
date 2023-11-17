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
	[SerializeField] protected bool edgeDetector;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected bool canAttack = true;

	protected virtual void Patrol()
	{
		myRigidbody2D.velocity = (facingRight) ?
			new Vector2(moveSpeed, myRigidbody2D.velocity.y) :
			new Vector2(-moveSpeed, myRigidbody2D.velocity.y);
	}

	protected abstract void CheckState();
	protected override void FixedUpdate()
	{
        base.FixedUpdate();

        isGrounded = Physics2D.Raycast(myCollider2D.bounds.center, Vector2.down,
           myCollider2D.bounds.extents.y + 0.05f, groundLayer);
        if (!isGrounded) return;

        edgeDetector = Physics2D.Raycast(feet.position, Vector2.down, distance, groundLayer);



        if (isBeingHooked || isDead) return;

        CheckState(); 


		if (!edgeDetector)
		{
			facingRight = !facingRight;
		}

		if (facingRight)
		{
			Vector3 rotator = new Vector3(0, 0, 0);
			transform.rotation = Quaternion.Euler(rotator);
		} else
		{
			Vector3 rotator = new Vector3(0, 180, 0);
			transform.rotation = Quaternion.Euler(rotator);
		}
	}

	protected override void Awake()
	{
		base.Awake();

		playerObject = GameObject.FindGameObjectWithTag("Player");
	}
}
