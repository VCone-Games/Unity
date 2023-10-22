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

    private float normalGravityScale;

    Jump jumpReference;
    Rigidbody2D myRigidbody;

    private bool DISABLED;

    private HorizontalMovement horizontalMovementComponent;
    private Wallgrab wallGrabComponent;


    public bool IsDashing { get { return isDashing; } }

    // Start is called before the first frame update
    void Start()
    {
        dashReference.action.performed += OnDashing;
        jumpReference = GetComponent<Jump>();
        normalGravityScale = jumpReference.NormalGravityScale;

        myRigidbody = GetComponent<Rigidbody2D>();

        horizontalMovementComponent = GetComponent<HorizontalMovement>();
        horizontalMovementComponent.enabled = false;
        horizontalMovementComponent.enabled = true;

        wallGrabComponent = GetComponent<Wallgrab>();
    }

    private void OnDashing(InputAction.CallbackContext context)
    {
        if (!hasDashed)
        {
            isDashing = true;
            hasDashed = true;
            dashTimer = dashDuration;
            myRigidbody.velocity = Vector2.zero;
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (DISABLED) return;
        if (dashTimer > 0.0f)
        {
            horizontalMovementComponent.DisableMovementInput();
            wallGrabComponent.DisableWallGrabInput();

            myRigidbody.gravityScale = 0;
            if (transform.localScale.x < 0.0f)
                myRigidbody.velocity = new Vector2(-dashForce, myRigidbody.velocity.y);
            else
                myRigidbody.velocity = new Vector2(dashForce, myRigidbody.velocity.y);

            dashTimer -= Time.deltaTime;
        }
        else if (dashTimer <= 0.0f)
        {
            myRigidbody.gravityScale = normalGravityScale;
            isDashing = false;
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);

            horizontalMovementComponent.EnableMovementInput();
            wallGrabComponent.EnableWallGrabInput();
        }

        if (dashTimer <= 0.0f && jumpReference.IsGrounded)
        {
            isDashing = false;
            hasDashed = false;

            horizontalMovementComponent.EnableMovementInput();
            wallGrabComponent.EnableWallGrabInput();
        }



    }

    public void DisableDashInput()
    {
        dashReference.action.Disable();
        DISABLED = true;
    }
    public void EnableDashInput()
    {
        dashReference.action.Enable();
        DISABLED = false;
    }
}
