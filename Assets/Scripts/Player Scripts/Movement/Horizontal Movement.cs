using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Is Disabled")]
    [SerializeField] private bool DISABLED;

    [Header("Input system")]
    [SerializeField] private InputActionReference movementActionReference;


    [Header("Movement params")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool moving;
    [SerializeField] private float movementDirection;
    [SerializeField] private bool facingRight = true;


    [Header("Player Component")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private Jump jumpComponent;
    [SerializeField] private Dash dashComponent;
    [SerializeField] private Hook hookComponent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject hookGun;
    [SerializeField] Vector3 initialGunPosition;
    [SerializeField] Vector3 invertedGunPosition;

    [Header("Animator Variables")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animationTimer;
    [SerializeField] private float animationCancelTime;

    public bool IsFacingRight
    {
        get { return facingRight; }
        set { facingRight = value; }
    }


    void Start()
    {
        movementActionReference.action.performed += OnPressed;
        movementActionReference.action.canceled += OnRelease;

        initialGunPosition = hookGun.transform.localPosition;
        invertedGunPosition = initialGunPosition;
        invertedGunPosition.x = -invertedGunPosition.x;

        // myRigidbody = GetComponent<Rigidbody2D>();
        //
        // jumpComponent = GetComponent<Jump>();
        // dashComponent = GetComponent<AirDash>();
        // hookComponent = GetComponent<Hook>();

    }

    private void OnPressed(InputAction.CallbackContext context)
    {
        moving = true;
        movementDirection = movementActionReference.action.ReadValue<float>();
        if (movementDirection < 0)
        {
            SpriteFlipManager(false);
        }
        if (movementDirection > 0)
        {
            SpriteFlipManager(true);
        }
    }

    public void SpriteFlipManager(bool isFacingRight)
    {
        facingRight = isFacingRight;
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        moving = false;
    }

    public void DisableMovementInput()
    {
        movementActionReference.action.Disable();
        DISABLED = true;
    }
    public void EnableMovementInput()
    {
        movementActionReference.action.Enable();
        DISABLED = false;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (facingRight)
        {
            spriteRenderer.flipX = false;
            hookGun.transform.localPosition = initialGunPosition;
        }
        else
        {
            spriteRenderer.flipX = true;
            hookGun.transform.localPosition = invertedGunPosition;
        }

        if (DISABLED) return;
        if (moving)
        {
            if(jumpComponent.IsGrounded)
            {
                animator.SetBool("Running", true);
                animationTimer = animationCancelTime;
            }
            myRigidbody.velocity = new Vector2(movementSpeed * movementDirection, myRigidbody.velocity.y);
            animator.SetBool("MovingAir", true);
            animationTimer = animationCancelTime;
        }
        else
        {

            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);

        }
    }

    private void Update()
    {
        if (animationTimer > 0)
        {
            animationTimer -= Time.deltaTime;
            if (animationTimer < 0)
            {
                animator.SetBool("MovingAir", false);
                animator.SetBool("Running", false);
            }
        }
    }
}
