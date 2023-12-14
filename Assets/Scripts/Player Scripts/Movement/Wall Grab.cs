using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallGrab : MonoBehaviour
{
    private bool wallGrabUnlocked = false;
    public bool WallGrabUnlocked { set { wallGrabUnlocked = value; } }

    [Header("Is Disabled")]
    [SerializeField] private bool DISABLED;

    [Header("Input system")]
    [SerializeField] InputActionReference wallJumpMobileReference;
    [SerializeField] InputActionReference wallJumpPCReference;
    [SerializeField] private bool MOBILE;

    [Header("Player Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private Jump jumpComponent;
    [SerializeField] private HorizontalMovement horizontalMovementReference;
    [SerializeField] private Dash airDashMovementReference;

    [Header("Parameters")]
    [SerializeField] private float distanceMax;
    [SerializeField] private float jumpWallForce;
    [SerializeField] private float ForceAddX;
    [SerializeField] private float jumpWallTime;
    [SerializeField] private float wallGravity;
    [SerializeField] private float normalGravityScale;

    [Header("Wall Layermask")]
    [SerializeField] LayerMask wallLayer;

    [Header("Control variables")]
    [SerializeField] private bool jumpWall;
    [SerializeField] private bool isGrabbingWall;
    [SerializeField] private bool leftWall;
    [SerializeField] private bool rightWall;
    [SerializeField] private bool isJumpingLeft;
    [SerializeField] private bool isJumpingRight;
    [SerializeField] private float jumpWallTimer;

    [Header("Animator Variables")]
    [SerializeField] private Animator animator;

    public bool IsGrabbingWall
    {
        get { return isGrabbingWall; }
    }

    public bool JumpWall
    {
        get { return jumpWall; }
        set { jumpWall = true; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (MOBILE) wallJumpMobileReference.action.performed += WallJump;
        else wallJumpPCReference.action.performed += WallJump;
        normalGravityScale = myRigidbody.gravityScale;
    }

    private void WallJump(InputAction.CallbackContext context)
    {
        if (!wallGrabUnlocked) return;

        if (isGrabbingWall)
        {
            if (rightWall)
            {
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                horizontalMovementReference.SpriteFlipManager(false);
            }
            if (leftWall)
            {
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                horizontalMovementReference.SpriteFlipManager(true);
            }

            jumpWallTimer = jumpWallTime;
            jumpWall = true;

            horizontalMovementReference.DisableMovementInput();
            airDashMovementReference.DisableDashInput();

            animator.SetTrigger("Jump Trigger");
            jumpComponent.IsJumping = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!wallGrabUnlocked) return;
        if (DISABLED) return;

        leftWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.left, distanceMax + myCollider.bounds.extents.x, wallLayer);
        rightWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.right, distanceMax + myCollider.bounds.extents.x, wallLayer);



        if (!jumpComponent.IsGrounded && (leftWall || rightWall))
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.gravityScale = wallGravity;
            jumpComponent.DisableBonusAirTime();
            isGrabbingWall = true;
            animator.SetBool("isGrabbingWall", isGrabbingWall);

            if (!jumpComponent.jumpInputPressed || !jumpComponent.IsJumping)
            {
                if (leftWall) horizontalMovementReference.SpriteFlipManager(true);
                if (rightWall) horizontalMovementReference.SpriteFlipManager(false);
            }
        }
        else if(isGrabbingWall) 
        {
            myRigidbody.gravityScale = normalGravityScale;
            leftWall = false;
            rightWall = false;
            isGrabbingWall = false;
            animator.SetBool("isGrabbingWall", isGrabbingWall);
            jumpComponent.EnableBonusAirTime();
           if (!jumpWall)
           {
               horizontalMovementReference.CheckDirection();
           }

        }

        if (jumpWall && jumpComponent.IsJumping)
        {

            if (leftWall || isJumpingLeft)
            {
                myRigidbody.velocity = new Vector2(jumpWallForce + ForceAddX, jumpWallForce);
                isJumpingLeft = true;
            }
            else if (rightWall || isJumpingRight)
            {
                myRigidbody.velocity = new Vector2(-jumpWallForce + ForceAddX, jumpWallForce);
                isJumpingRight = true;
            }
            jumpWallTimer -= Time.deltaTime;
        }

        if ((jumpWallTimer <= 0.0f || !jumpComponent.IsJumping) && jumpWall)
        {
            jumpWall = false;
            isJumpingLeft = false;
            isJumpingRight = false;
            horizontalMovementReference.EnableMovementInput();
            airDashMovementReference.EnableDashInput();
            horizontalMovementReference.CheckDirection();
        }
    }


    public void DisableWallGrabInput()
    {
        wallJumpMobileReference.action.Disable();
        wallJumpPCReference.action.Disable();
        DISABLED = true;
    }
    public void EnableWallGrabInput()
    {
        wallJumpMobileReference.action.Enable();
        wallJumpPCReference.action.Enable();
        DISABLED = false;
    }

    private void OnDestroy()
    {
        wallJumpMobileReference.action.performed -= WallJump;
        wallJumpPCReference.action.performed -= WallJump;
    }
}
