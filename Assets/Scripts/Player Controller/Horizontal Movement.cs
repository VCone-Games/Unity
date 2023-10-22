using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Input system")]
    [SerializeField] private InputActionReference movementActionReference;


    [Header("Movement params")]
    [SerializeField] private float movementSpeed;

    private bool movementInputPressed;
    private bool DISABLED;

    private float playerDirection;
    private bool facingRight = true;
    private Rigidbody2D myRigidbody;

    private Jump jumpComponent;
    private AirDash dashComponent;
    private Hook hookComponent;

    public bool IsFacingRight
    {
        get { return facingRight; }
        set { facingRight = value; }
    }

 
    void Start()
    {
        movementActionReference.action.performed += OnPressed;
        movementActionReference.action.canceled += OnRelease;

        myRigidbody = GetComponent<Rigidbody2D>();

        jumpComponent = GetComponent<Jump>();
        dashComponent = GetComponent<AirDash>();
        hookComponent = GetComponent<Hook>();

    }

    private void OnPressed(InputAction.CallbackContext context)
    {
        movementInputPressed = true;
        playerDirection = movementActionReference.action.ReadValue<float>();

        SpriteDirectionManager();
    }

    private void SpriteDirectionManager()
    {
        if (playerDirection < 0) //SI MUEVE HACIA IZQUIERDA
        {
            if (facingRight) //ME GIRO SI MIRABA A LA DERECHA
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            facingRight = false;
        }

        else //SI MUEVE HACIA DERECHA   
        {
            if (!facingRight) //ME GIRO SI MIRABA A LA IZQUIERDA
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            facingRight = true;
        }
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        movementInputPressed = false;
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
        if (DISABLED) return;
        if (movementInputPressed)
        {
            myRigidbody.velocity = new Vector2(movementSpeed * playerDirection, myRigidbody.velocity.y);
        }
        else
        {
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        }
        //else if (isHooking && !keyPressed)
        //{
        //    myRigidbody.velocity = new Vector2(Vector2.right.x * movementSpeed, myRigidbody.velocity.y);
        //
        //}

    }
}
