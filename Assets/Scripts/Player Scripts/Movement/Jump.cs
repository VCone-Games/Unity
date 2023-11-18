
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    [SerializeField] private bool asciende;

    [Header("Animator Components")]
    [SerializeField] private Animator animator;

    [Header("Audio Management")]
    [SerializeField] private PlayerSoundManager soundManager;

    [Header("Camera Management")]
    [SerializeField] private CameraFollowObject cameraFollow;
    [SerializeField] private float fallSpeedYDumpingChangeThreshold;


    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private bool ShakeOnImpact;
    [SerializeField] private bool SpartaOnImpact;

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
        impulseSource = GetComponent<CinemachineImpulseSource>();

        jumpReference.action.performed += OnJump;
        jumpReference.action.canceled += OnJumpCanceled;

        normalGravityScale = myRigidbody.gravityScale;
        fallSpeedYDumpingChangeThreshold = CameraManager.Instance.fallSpeedDampingChangeThreshhold;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (jumping) return;
        jumpInputPressed = true;

        bufferTimer = bufferTime;

        jumpCount++;

        if (jumpCount <= maxJumps || hasParred)
        {
            if (!IsGrounded && coyoteTimer > 0f && maxJumps > 1)
            {
                jumpCount--;
                Debug.Log("COYOTE SALTO");
            }
            hasParred = false;
            asciende = true;
            animator.SetTrigger("Jump Trigger");
            if (jumpCount == 2) { soundManager.PlayDoubleJump(); }
        }

    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        jumpInputPressed = false;
        if (jumping)
        {
            jumping = false;
            asciende = false;
            bufferTimer = -1;
            if (myRigidbody.velocity.y < 0) return;

            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
        }

    }

    private void Update()
    {
        if (myRigidbody.velocity.y < fallSpeedYDumpingChangeThreshold && !CameraManager.Instance.IsLerpingYDamping && !CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpYDamping(true);
            ShakeOnImpact = true;
            Debug.Log("shake");
        }

        if (ShakeOnImpact && myRigidbody.velocity.y < -35f && !SpartaOnImpact)
        {
            SpartaOnImpact = true;
            Debug.Log("SPARTAAA");
        }

        else if (myRigidbody.velocity.y >= 0 && !CameraManager.Instance.IsLerpingYDamping && CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpedFromPlayerFalling = false;
            CameraManager.Instance.LerpYDamping(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (DISABLED) return;

        bool auxGrounded = isGrounded;
        isGrounded = Physics2D.Raycast(myCollider.bounds.center, Vector2.down,
            myCollider.bounds.extents.y + raycastFeetLength, groundLayer);

        if (auxGrounded == false && isGrounded == true)
        {
            if (ShakeOnImpact && !SpartaOnImpact)
            {
                CameraShakeManager.instance.CameraShake(impulseSource, new Vector3(0, 0.25f, 0));
            } else if (SpartaOnImpact)
            {
                CameraShakeManager.instance.CameraShake(impulseSource, new Vector3(0, 0.5f, 1.25f));
            }
            ShakeOnImpact = false;
            SpartaOnImpact = false;
            soundManager.PlayLanding();
        }

        if (GetComponent<Dash>().IsDashing) return;

        animator.SetBool("isJumping", jumping);

        if (!isGrounded)
        {
            animator.SetBool("Is Airborne", true);
        }
        else
        {
            animator.SetBool("Is Airborne", false);
        }


        if ((isGrounded || grabWallComponent.IsGrabbingWall) && !asciende)
        {
            asciende = false;
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
        {
            bufferTimer -= Time.deltaTime;
            if (bufferTimer < 0) asciende = false; jumping = false;
        }



        if (jumpInputPressed)
        {
            JumpMethod();
        }

    }

    void JumpMethod()
    {
        if ((bufferTimer > 0.0f && asciende))
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
