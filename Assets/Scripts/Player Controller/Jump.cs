using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
	[Header("Input system")]
	[SerializeField] InputActionReference jumpReference;

	[Header("Jump params")]
	[SerializeField] float jumpForce;
	[SerializeField] float coyoteTime;
	[SerializeField] float bufferTime;
	[SerializeField] float bonusAirTimeInterval;
	[SerializeField] float raycastFeetLength;

	[Header("Control variables")]
	[SerializeField] bool jumping;
	[SerializeField] bool isGrounded;
	[SerializeField] bool keyPressed;
	[SerializeField] float coyoteTimer;
	[SerializeField] float bufferTimer;

	Rigidbody2D myRigidbody;
	Collider2D myCollider;

	public bool IsGrounded { get { return isGrounded; } }

	[Header("Jump layer mask")]
	[SerializeField] LayerMask groundLayer;

	// Start is called before the first frame update
	void Start()
    {
		jumpReference.action.performed += OnJump;
		jumpReference.action.canceled += OnJumpCanceled;

		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
    }

	private void OnJump(InputAction.CallbackContext context)
	{
		bufferTimer = bufferTime;
		keyPressed = true;
	}

	void OnJumpCanceled(InputAction.CallbackContext context)
	{
		keyPressed = false;
		if (jumping)
		{
			jumping = false;

			if (myRigidbody.velocity.y < 0) return;

			myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
		}

	}

	// Update is called once per frame
	void FixedUpdate()
    {
		isGrounded = Physics2D.Raycast(myCollider.bounds.center, Vector2.down,
			myCollider.bounds.extents.y + raycastFeetLength, groundLayer);

		coyoteTimer = isGrounded ? coyoteTime : coyoteTimer - Time.deltaTime;
		bufferTimer -= Time.deltaTime;

		JumpMethod();
	}

	void JumpMethod()
	{
		if (keyPressed && (bufferTimer > 0.0f && coyoteTimer > 0.0f))
		{
			jumping = true;
			myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
			coyoteTimer = 0;
		}
	}

}
