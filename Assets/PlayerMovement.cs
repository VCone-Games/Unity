using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputAction jumpAction;
    private InputAction moveAction;

    [Space][SerializeField] private InputActionAsset myActionsAsset;

    private float horizontalMovement;

    [SerializeField] private float horizontalSpeed = 7;
    [SerializeField] private float jumpForce = 15;

    private Rigidbody2D myRigidbody;

    [SerializeField] private bool isGrounded;
    private bool jumping;


    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimer;

    [SerializeField] private float bonusAirTimeInterval = 1;




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

    // Update is called once per frame
    void FixedUpdate()
    {
        coyoteTimer = isGrounded ? coyoteTime : coyoteTimer - Time.deltaTime;

        myRigidbody.velocity = new Vector2(horizontalMovement * horizontalSpeed, myRigidbody.velocity.y);
        if (!isGrounded && myRigidbody.velocity.y <= bonusAirTimeInterval && -bonusAirTimeInterval <= myRigidbody.velocity.y)
        {
            myRigidbody.gravityScale = 1.5f;
        }
        else
        {
            myRigidbody.gravityScale = 3;
        }
    }


    void OnJump(InputAction.CallbackContext context)
    {
        if (coyoteTimer > 0f)
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
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        horizontalMovement = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isGrounded = true;
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
