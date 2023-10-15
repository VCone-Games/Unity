using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputAction jumpAction;
    private InputAction moveAction;
    private Rigidbody2D myRigidbody;
    private bool jumping;
    private float coyoteTimer;
    private float horizontalMovement;

    [Header("Input action")]
	[SerializeField] private InputActionAsset myActionsAsset;

	[Header("Player params")]
    [SerializeField] float horizontalSpeed = 7;
    [SerializeField] float jumpForce = 15;
    [SerializeField] bool isGrounded;
    [SerializeField] bool hasGrapped;
    [SerializeField] bool hasPressedMoveKey;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float bonusAirTimeInterval = 1;




    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();


        jumpAction = myActionsAsset.FindAction("Player/Jump");
        moveAction = myActionsAsset.FindAction("Player/Move");

        jumpAction.performed += OnJump;
        moveAction.performed += OnMove;
        moveAction.canceled += OnMoveCanceled;
        jumpAction.canceled += OnJumpCanceled;
    }

    private void Update()
    {

    }


    void FixedUpdate()
    {
        coyoteTimer = isGrounded ? coyoteTime : coyoteTimer - Time.deltaTime;

        bool isGrappling = GetComponent<PlayerGrapling>().isGrappling;
		float previousMovement = myRigidbody.velocity.x;
		if (isGrappling)
        {
            hasGrapped = true;
            if (previousMovement < 0.0f)
                horizontalMovement = -1.0f;
            else if (previousMovement > 0.0f)
                horizontalMovement = 1.0f;
        }

        myRigidbody.velocity = new Vector2(horizontalMovement * horizontalSpeed, myRigidbody.velocity.y);

		if (!isGrounded)
        {
            if (myRigidbody.velocity.y <= bonusAirTimeInterval && -bonusAirTimeInterval <= myRigidbody.velocity.y)
            {
                myRigidbody.gravityScale = 1.5f;
            }
            else
            {
                myRigidbody.gravityScale = 3;
            }
        }
    }



	void OnJump(InputAction.CallbackContext context)
    {
        if (coyoteTimer > 0.0f)
        {
            jumping = true;
            myRigidbody.velocity += new Vector2(0, jumpForce);
            coyoteTimer = 0;
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

    void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<float>();
        hasPressedMoveKey = true;
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        horizontalMovement = 0;
		hasPressedMoveKey = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isGrounded = true;
            if (hasGrapped)
            {
                hasGrapped = false;
                if (!hasPressedMoveKey)
                    horizontalMovement = 0.0f;;
            }
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
