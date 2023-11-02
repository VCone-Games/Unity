using EZCameraShake;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [Header("Is Disabled")]
    private bool DISABLED;

    [Header("Input system")]
    [SerializeField] InputActionReference dashReference;

    [Header("Player Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private Jump jumpReference;
    [SerializeField] private HorizontalMovement horizontalMovementComponent;
    [SerializeField] private WallGrab wallGrabComponent;

    [Header("Dash params")]
    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;

    [Header("Control variables")]
    [SerializeField] bool isDashing;
    [SerializeField] bool hasDashed;
    [SerializeField] bool hasParred;
    [SerializeField] float dashTimer;
    private float normalGravityScale;


    public bool HasParred { set { hasParred = value; } }
    



    public bool IsDashing
    {
        get { return isDashing; }
    }

    // Start is called before the first frame update
    void Start()
    {
        dashReference.action.performed += OnDashing;
        normalGravityScale = myRigidbody.gravityScale;
        horizontalMovementComponent.enabled = false;
        horizontalMovementComponent.enabled = true;
    }

    private void OnDashing(InputAction.CallbackContext context)
    {
        if (!hasDashed || hasParred)
        {
            hasParred = false;
            CameraShaker.Instance.ShakeOnce(1f, 30f, .1f, 0.3f);
            isDashing = true;
            hasDashed = true;
            dashTimer = dashDuration;
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.gravityScale = 0;
            myRigidbody.velocity = Vector3.zero;
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


            if (!horizontalMovementComponent.IsFacingRight)
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

        if (dashTimer <= 0.0f && (jumpReference.IsGrounded || wallGrabComponent.IsGrabbingWall))
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
