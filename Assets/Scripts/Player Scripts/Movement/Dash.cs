
using Cinemachine;
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
    [SerializeField] float coolDownDuration;

    [Header("Control variables")]
    [SerializeField] bool isDashing;
    [SerializeField] bool hasDashed;
    [SerializeField] bool hasParred;
    [SerializeField] float dashTimer;
    [SerializeField] float coolDownTimer;
    private float normalGravityScale;

    [Header("Animator Variables")]
    [SerializeField] private Animator animator;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;


    public bool HasParred { set { hasParred = value; } }
    



    public bool IsDashing
    {
        get { return isDashing; }
    }

    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        dashReference.action.performed += OnDashing;
        normalGravityScale = myRigidbody.gravityScale;
        horizontalMovementComponent.enabled = false;
        horizontalMovementComponent.enabled = true;
    }

    private void OnDashing(InputAction.CallbackContext context)
    {
        if ((!hasDashed || hasParred) && coolDownTimer <= 0)
        {
            animator.SetBool("Is Dashing", true);
            hasParred = false;

            TimeStop.instance.StopTime(0.05f, 20f, 0.3f);
            CameraShakeManager.instance.CameraShake(impulseSource, new Vector3(0.5f,0,0));

            isDashing = true;
            hasDashed = true;
            dashTimer = dashDuration;
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.gravityScale = 0;
            myRigidbody.velocity = Vector3.zero;
            coolDownTimer = coolDownDuration;
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (DISABLED) return;

        if(coolDownTimer > 0)
        {
            coolDownTimer -= Time.fixedDeltaTime;
        }

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
            animator.SetBool("Is Dashing", false);
        }

        if (dashTimer <= 0.0f && (jumpReference.IsGrounded || wallGrabComponent.IsGrabbingWall))
        {
            isDashing = false;
            hasDashed = false;
 

            horizontalMovementComponent.EnableMovementInput();
            wallGrabComponent.EnableWallGrabInput();
        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDashing) return;
        if (collision.gameObject.layer == 7)
        {
            Debug.Log("AAAAAAAA ME VENGOOO");
            myRigidbody.velocity = Vector2.zero;
            dashTimer = -1;
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
