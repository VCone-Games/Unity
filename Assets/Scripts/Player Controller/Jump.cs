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
    [SerializeField] float bonusAirTimeGravityScale;
    [SerializeField] float normalGravityScale;

    [Header("Jump layerMask")]
    [SerializeField] LayerMask groundLayer;

	[Header("Control variables")]
	[SerializeField][ReadOnly] bool jumping;
	[SerializeField][ReadOnly] bool isGrounded;
    [SerializeField][ReadOnly] bool keyPressed;
	[SerializeField][ReadOnly] float coyoteTimer;
	[SerializeField][ReadOnly] float bufferTimer;
	[SerializeField][ReadOnly] int jumpCount;

    Rigidbody2D myRigidbody;
    Collider2D myCollider;

	public bool IsJumping { get { return jumping; } set { jumping = value; } }

	public bool IsGrounded { get { return isGrounded; } }

	public float NormalGravityScale {  get { return normalGravityScale; } }


    // Start is called before the first frame update
    void Start()
    {
        jumpReference.action.performed += OnJump;
        jumpReference.action.canceled += OnJumpCanceled;

        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();

        myRigidbody.gravityScale = normalGravityScale;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpInputPressed = true;

        bufferTimer = bufferTime;
        jumpCount++;
        if (jumpCount < maxJumps)
        {
            coyoteTimer = coyoteTime;
        }

    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        jumpInputPressed = false;
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

        if (GetComponent<AirDash>().IsDashing) return;

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
                myRigidbody.gravityScale = bonusAirTimeGravityScale;
            }
            else
            {
                myRigidbody.gravityScale = normalGravityScale;
            }
        }

        bufferTimer -= Time.deltaTime;

        if (jumpInputPressed)
        {
            JumpMethod();
        }

    }

    void JumpMethod()
    {
        if ((bufferTimer > 0.0f && coyoteTimer > 0.0f) || isWalled)
        {
            // GetComponent<HorizontalMovement>().DisableMovementInput();
            jumping = true;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
        }
    }

}
