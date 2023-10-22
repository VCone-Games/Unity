using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wallgrab : MonoBehaviour
{

	[Header("Input system")]
	[SerializeField] InputActionReference grabWallReference;
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
	[SerializeField][ReadOnly] bool wantsToGrabWall;
	[SerializeField][ReadOnly] bool isGrabbingWall;
	[SerializeField][ReadOnly] bool isPressingWall;
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
        grabWallReference.action.performed += OnPressed;
        grabWallReference.action.canceled += OnReleased;

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
			jumpWallTimer = jumpWallTime;
			jumpWall = true;
		}
	}

	private void OnPressed(InputAction.CallbackContext context)
	{
		wantsToGrabWall = true;
	}

	private void OnReleased(InputAction.CallbackContext context)
	{
		wantsToGrabWall = false;
	}

	// Update is called once per frame
	void FixedUpdate()
    {

		bool leftWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.left, distanceMax + myCollider.bounds.extents.x, wallLayer);
		bool rightWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.right, distanceMax + myCollider.bounds.extents.x, wallLayer);

		isPressingWall = leftWall || rightWall; 

		if (!jumpScript.IsGrounded && wantsToGrabWall && isPressingWall)
		{
			myRigidbody.velocity = Vector2.zero;
			isGrabbingWall = true;
		}
		else
		{
			isPressingWall = false;
			isGrabbingWall = false;
		}

		if (jumpWall)
		{
			if (leftWall || isJumpingLeft)
			{
				myRigidbody.velocity = new Vector2(jumpWallForce + ForceAddX, jumpWallForce);
				isJumpingLeft = true;
				horizontalMovementReference.IsFacingRight = false;
			}
			else if (rightWall || isJumpingRight)
			{
				myRigidbody.velocity = new Vector2(-jumpWallForce + ForceAddX, jumpWallForce);
				isJumpingRight = true;
				horizontalMovementReference.IsFacingRight = true;
			}

			jumpWallTimer -= Time.deltaTime;
		}

		if (jumpWallTimer <= 0.0f)
		{
			jumpWall = false;
			isJumpingLeft = false;
			isJumpingRight = false;
		}
    }


}
