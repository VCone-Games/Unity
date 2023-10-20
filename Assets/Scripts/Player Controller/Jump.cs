using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
	[SerializeField] int maxJumps;
	[SerializeField] float bonusAirTimeInterval;
	[SerializeField] float raycastFeetLength;

	[Header("Control variables")]
	[SerializeField][ReadOnly] bool jumping;
	[SerializeField][ReadOnly] bool isGrounded;
	[SerializeField][ReadOnly] bool keyPressed;
	[SerializeField][ReadOnly] float coyoteTimer;
	[SerializeField][ReadOnly] float bufferTimer;
	[SerializeField][ReadOnly] int jumpCount;

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
		jumpCount++;
		if (jumpCount < maxJumps)
		{
			coyoteTimer = coyoteTime;
		}

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

		if (isGrounded)
		{
			coyoteTimer = coyoteTime;
			jumpCount = 0; // Reinicia el contador de saltos cuando tocas el suelo.
		}
		else
		{
			coyoteTimer -= Time.deltaTime;

			if (myRigidbody.velocity.y <= bonusAirTimeInterval && -bonusAirTimeInterval <= myRigidbody.velocity.y)
			{
				myRigidbody.gravityScale = 1.5f;
			}
			else
			{
				myRigidbody.gravityScale = 3;
			}
		}

		bufferTimer -= Time.deltaTime;

		JumpMethod();
	}

	void JumpMethod()
	{
		if (keyPressed && (bufferTimer > 0.0f && coyoteTimer > 0.0f))
		{
			jumping = true;
			myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
		}
	}

}
