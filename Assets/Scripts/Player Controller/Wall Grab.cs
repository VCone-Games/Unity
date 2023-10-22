using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wallgrab : MonoBehaviour
{
	[Header("Input system")]
	[SerializeField] InputActionReference wallJumpReference;

	[Header("Params")]
	[SerializeField] private float distanceMax;
	[SerializeField] private float jumpWallForce;
	[SerializeField] private float ForceAddX;
	[SerializeField] private float jumpWallTime;

	[Header("Wall Layermask")]
	[SerializeField] LayerMask wallLayer;

	[Header("Control variables")]
	[SerializeField][ReadOnly] private bool jumpWall;
	[SerializeField][ReadOnly] private bool isGrabbingWall;
	[SerializeField][ReadOnly] private bool leftWall;
	[SerializeField][ReadOnly] private bool rightWall;
	[SerializeField][ReadOnly] private bool isJumpingLeft;
	[SerializeField][ReadOnly] private bool isJumpingRight;
	[SerializeField][ReadOnly] private float jumpWallTimer;

    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;
    private Jump jumpScript;

    [SerializeField] private bool DISABLED;

    HorizontalMovement horizontalMovementReference;
	AirDash airDashMovementReference;

	public bool JumpWall { get { return jumpWall; } set { jumpWall = true; } }

	// Start is called before the first frame update
	void Start()
    {
		wallJumpReference.action.performed += WallJump;

		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		jumpScript = GetComponent<Jump>();

		horizontalMovementReference = GetComponent<HorizontalMovement>();
		airDashMovementReference = GetComponent<AirDash>();

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

            horizontalMovementReference.DisableMovementInput();
			airDashMovementReference.DisableDashInput();


            jumpScript.IsJumping = true;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		if(DISABLED) return;

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
			horizontalMovementReference.EnableMovementInput();
			airDashMovementReference.EnableDashInput();
		}
    }


    public void DisableWallGrabInput()
    {
        wallJumpReference.action.Disable();
        DISABLED = true;
    }
    public void EnableWallGrabInput()
    {
        wallJumpReference.action.Enable();
        DISABLED = false;
    }
}
