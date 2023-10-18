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
	//[SerializeField] float bufferTime;
	[SerializeField] float bonusAirTimeInterval;

	[Header("Control variables")]
	[SerializeField] bool jumping;
	[SerializeField] bool isGrounded;
	[SerializeField] float coyoteTimer;
	//[SerializeField] float bufferTimer;

	Rigidbody2D myRigidbody;

	// Start is called before the first frame update
	void Start()
    {
		jumpReference.action.performed += OnJump;
		jumpReference.action.canceled += OnJumpCanceled;

		myRigidbody = GetComponent<Rigidbody2D>();
    }

	private void OnJump(InputAction.CallbackContext context)
	{
		//Debug.Log("He pulsado saltar");
		if (coyoteTimer > 0.0f)
		{
			jumping = true;
			myRigidbody.velocity += new Vector2(0, jumpForce);
			coyoteTimer = 0;
			//bufferTimer = bufferTime;
		}
	}

	void OnJumpCanceled(InputAction.CallbackContext context)
	{
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
		coyoteTimer = isGrounded ? coyoteTime : coyoteTimer - Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Floor")
		{
			isGrounded = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Floor")
		{
			isGrounded = false;
		}
	}
}
