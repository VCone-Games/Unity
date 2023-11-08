using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallGrab : MonoBehaviour
{
    [Header("Is Disabled")]
    [SerializeField] private bool DISABLED;

    [Header("Input system")]
    [SerializeField] InputActionReference wallJumpReference;

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
        wallJumpReference.action.performed += WallJump;
        normalGravityScale = myRigidbody.gravityScale;
    }

    private void WallJump(InputAction.CallbackContext context)
    {
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


            jumpComponent.IsJumping = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (DISABLED) return;

        leftWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.left, distanceMax + myCollider.bounds.extents.x, wallLayer);
        rightWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.right, distanceMax + myCollider.bounds.extents.x, wallLayer);


        if (!jumpComponent.IsGrounded && (leftWall || rightWall))
        {
            if (leftWall) horizontalMovementReference.IsFacingRight = true;
            if (rightWall) horizontalMovementReference.IsFacingRight = false;
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.gravityScale = wallGravity;
            jumpComponent.DisableBonusAirTime();
            isGrabbingWall = true;
            animator.SetBool("isGrabbingWall", true);
        }
        else
        {
            myRigidbody.gravityScale = normalGravityScale;
            leftWall = false;
            rightWall = false;
            isGrabbingWall = false;
            animator.SetBool("isGrabbingWall", false);
            jumpComponent.EnableBonusAirTime();
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

        if (jumpWallTimer <= 0.0f || !jumpComponent.IsJumping)
        {
            jumpWall = false;
            isJumpingLeft = false;
            isJumpingRight = false;
            horizontalMovementReference.EnableMovementInput();
            airDashMovementReference.EnableDashInput();
        }
    }


    public void DisableWallGrabInput()
    {
        wallJumpReference.action.Disable();
        DISABLED = true;
    }
    public void EnableWallGrabInput()
    {
        wallJumpReference.action.Enable();
        DISABLED = false;
    }
}
