using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirDash : MonoBehaviour
{

	[Header("Input system")]
	[SerializeField] InputActionReference dashReference;

	[Header("Dash params")]
	[SerializeField] float dashForce;
	[SerializeField] float dashDuration;

    [Header("Control variables")]
    [SerializeField] bool isDashing;
    [SerializeField] bool hasDashed;
    [SerializeField] float dashTimer;

	Jump jumpReference;
	Rigidbody2D myRigidbody;

	public bool IsDashing {  get { return isDashing; } }

	// Start is called before the first frame update
	void Start()
    {
        dashReference.action.performed += OnDashing;  
		jumpReference = GetComponent<Jump>();

		myRigidbody = GetComponent<Rigidbody2D>();
    }

	private void OnDashing(InputAction.CallbackContext context)
	{
		if (!hasDashed)
		{
			isDashing = true;
			hasDashed = true;
			dashTimer = dashDuration;
		}
	}



	// Update is called once per frame
	void FixedUpdate()
    {
        if(dashTimer > 0.0f)
		{
			if (transform.localScale.x < 0.0f)
				myRigidbody.velocity = new Vector2(-dashForce, myRigidbody.velocity.y);
			else
				myRigidbody.velocity = new Vector2(dashForce, myRigidbody.velocity.y);

			dashTimer -= Time.deltaTime;
		}
		else if (dashTimer <= 0.0f) 
		{
			isDashing = false;
			myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
		}

		if (dashTimer <= 0.0f && jumpReference.IsGrounded)
		{
			isDashing = false;
			hasDashed = false;
		}



    }
}
