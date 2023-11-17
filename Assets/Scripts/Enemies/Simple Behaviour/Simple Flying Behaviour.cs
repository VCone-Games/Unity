using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleFlyingBehaviour : Enemy
{
	[Header("Enemy simple flying behaviour")]
	[SerializeField] protected float circleRadius;
	[SerializeField] protected LayerMask groundMask;
	[SerializeField] protected LayerMask wallMask;
	[SerializeField] protected LayerMask enemyMask;
	[SerializeField] protected float hitTime;

	[Header("Checks collisions")]
	[SerializeField] protected GameObject rightCheck;
	[SerializeField] protected GameObject groundCheck;
	[SerializeField] protected GameObject roofCheck;

	[Header("Direction")]
	[SerializeField] protected Vector2 initDirection;

	[Header("Control variables")]
	[SerializeField] protected Vector2 currentDirection;
	[SerializeField] protected bool rightTouch;
	[SerializeField] protected bool groundTouch;
	[SerializeField] protected bool roofTouch;
	[SerializeField] protected float hitTimer;


	// Update is called once per frame
	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (isBeingHooked || isDead) return;

		HitCollision();

		if (rightTouch)
		{
			transform.Rotate(new Vector3(0, 180,0));
			facingRight = !facingRight;
			currentDirection.x *= -1; 
		}

		if ((roofTouch || groundTouch) && hitTimer < 0.0f)
		{
			currentDirection.y *= -1;
			hitTimer = hitTime;
		}
		else if (hitTimer >= 0.0f)
		{
			hitTimer -= Time.deltaTime;
		}

		myRigidbody2D.velocity = currentDirection.normalized * moveSpeed;
	}

	void HitCollision()
	{
		rightTouch = Physics2D.OverlapCircle(rightCheck.transform.position, circleRadius, groundMask) ||
			Physics2D.OverlapCircle(rightCheck.transform.position, circleRadius, wallMask) ||
			Physics2D.OverlapCircle(rightCheck.transform.position, circleRadius, enemyMask);

		groundTouch = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundMask);
		roofTouch = Physics2D.OverlapCircle(roofCheck.transform.position, circleRadius, groundMask);
	}

	private void Awake()
	{
		base.Awake();
		currentDirection = initDirection;
	}
}
