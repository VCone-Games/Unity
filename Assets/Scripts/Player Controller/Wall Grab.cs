using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wallgrab : MonoBehaviour
{

	[Header("Input system")]
	[SerializeField] InputActionReference jumpReference;

	[Header("Params")]
	[SerializeField] float distanceMax;
	[SerializeField] float jumpWallForce;
	[SerializeField] float ForceAddX;
	[SerializeField] float jumpWallTime;

	[Header("Wall Layermask")]
	[SerializeField] LayerMask wallLayer;

	[Header("Control variables")]
	[SerializeField][ReadOnly] bool jumpWall;
	[SerializeField][ReadOnly] bool isGrabbingWall;
	[SerializeField][ReadOnly] bool leftWall;
	[SerializeField][ReadOnly] bool rightWall;
	[SerializeField][ReadOnly] bool isJumpingLeft;
	[SerializeField][ReadOnly] bool isJumpingRight;
	[SerializeField][ReadOnly] float jumpWallTimer;

	Rigidbody2D myRigidbody;
	Collider2D myCollider;
	Jump jumpScript;

	HorizontalMovement horizontalMovementReference;

	public bool JumpWall { get { return jumpWall; } set { jumpWall = true; } }

	// Start is called before the first frame update
	void Start()
    {
		jumpReference.action.performed += WallJump;

		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		jumpScript = GetComponent<Jump>();

		horizontalMovementReference = GetComponent<HorizontalMovement>();
    }

	private void WallJump(InputAction.CallbackContext context)
	{
		if (isGrabbingWall)
		{
			if (rightWall)
			{
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				horizontalMovementReference.IsFacingRight = false;
			}
			if (leftWall)
			{
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
				horizontalMovementReference.IsFacingRight = true;
			}

			jumpWallTimer = jumpWallTime;
			jumpWall = true;
			jumpScript.IsJumping = true;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
    {

		leftWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.left, distanceMax + myCollider.bounds.extents.x, wallLayer);
		rightWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.right, distanceMax + myCollider.bounds.extents.x, wallLayer);


		if (!jumpScript.IsGrounded && (leftWall || rightWall))
		{
			myRigidbody.velocity = Vector2.zero;
			isGrabbingWall = true;
		}
		else
		{
			leftWall = false;
			rightWall = false;
			isGrabbingWall = false;
		}

		if (jumpWall && jumpScript.IsJumping)
		{
			if (leftWall || isJumpingLeft)
			{
				myRigidbody.velocity = new Vector2(jumpWallForce + ForceAddX, jumpWallForce);
				isJumpingLeft = true;
			}
			else if (rightWall || isJumpingRight)
			{
				myRigidbody.velocity = new Vector2(-jumpWallForce + ForceAddX, jumpWallForce);
				isJumpingRight = true;
			}

			jumpWallTimer -= Time.deltaTime;
		}

		if (jumpWallTimer <= 0.0f || !jumpScript.IsJumping)
		{
			jumpWall = false;
			isJumpingLeft = false;
			isJumpingRight = false;
		}
    }


}
