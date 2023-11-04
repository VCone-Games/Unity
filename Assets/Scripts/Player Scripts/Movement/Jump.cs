using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    [Header("Is Disabled")]
    private bool DISABLED;
    private bool disableBonusAirTime;

    [Header("Input system")]
    [SerializeField] private InputActionReference jumpReference;

    [Header("Player Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private WallGrab grabWallComponent;

    [Header("Jump params")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float bufferTime;
    [SerializeField] private int maxJumps;
    [SerializeField] private float bonusAirTimeInterval;
    [SerializeField] private float raycastFeetLength;
    [SerializeField] private float bonusAirTimeGravityScale;
    [SerializeField] private float normalGravityScale;

    [Header("Jump layerMask")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Control variables")]
    [SerializeField] private bool jumping;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool jumpInputPressed;
    [SerializeField] private float coyoteTimer;
    [SerializeField] private float bufferTimer;
    [SerializeField] private int jumpCount;
    [SerializeField] private bool hasParred;

    [Header("Animator Components")]
    [SerializeField] private Animator animator;


    public bool HasParred { set { hasParred = value; } }




    public bool IsJumping
    {
        get { return jumping; }
        set { jumping = value; }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    public float NormalGravityScale
    {
        get { return normalGravityScale; }
    }


    // Start is called before the first frame update
    void Start()
    {
        jumpReference.action.performed += OnJump;
        jumpReference.action.canceled += OnJumpCanceled;

        normalGravityScale = myRigidbody.gravityScale;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpInputPressed = true;

        bufferTimer = bufferTime;
        if (coyoteTimer < 0)
        {
            jumpCount++;
        }
        if (jumpCount < maxJumps || hasParred)
        {
            hasParred = false;
            coyoteTimer = coyoteTime;
            animator.SetTrigger("Jump Trigger");
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
        if (DISABLED) return;

        isGrounded = Physics2D.Raycast(myCollider.bounds.center, Vector2.down,
            myCollider.bounds.extents.y + raycastFeetLength, groundLayer);

        if (GetComponent<Dash>().IsDashing) return;

        if (!isGrounded)
        {
            animator.SetBool("Is Airborne", true);
        }
        else
        {
            animator.SetBool("Is Airborne", false);
        }


        if (isGrounded || grabWallComponent.IsGrabbingWall)
        {
            coyoteTimer = coyoteTime;
            jumpCount = 0; // Reinicia el contador de saltos cuando tocas el suelo.
        }
        else
        {
            if (coyoteTimer > 0)
                coyoteTimer -= Time.deltaTime;

            if (!disableBonusAirTime)
            {
                if (myRigidbody.velocity.y <= bonusAirTimeInterval && -bonusAirTimeInterval <= myRigidbody.velocity.y)
                {
                    myRigidbody.gravityScale = bonusAirTimeGravityScale;
                }
                else
                {
                    myRigidbody.gravityScale = normalGravityScale;
                }
            }

        }

        if (bufferTimer > 0)
            bufferTimer -= Time.deltaTime;


        if (jumpInputPressed)
        {
            JumpMethod();
        }

    }

    void JumpMethod()
    {
        if ((bufferTimer > 0.0f && coyoteTimer > 0.0f))
        {
            // GetComponent<HorizontalMovement>().DisableMovementInput();
            jumping = true;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
        }
    }

    public void DisableJumpInput()
    {
        jumpReference.action.Disable();
        DISABLED = true;
    }
    public void EnableJumpInput()
    {
        jumpReference.action.Enable();
        DISABLED = false;
    }

    public void DisableBonusAirTime()
    {
        disableBonusAirTime = true;
    }
    public void EnableBonusAirTime()
    {
        disableBonusAirTime = false;
    }

}
