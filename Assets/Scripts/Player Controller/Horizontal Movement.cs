using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Input system")]
    [SerializeField] InputActionReference movementReference;
    [SerializeField] InputActionReference shootReference;

    [Header("Movement params")]
    [SerializeField] float movementSpeed;

    bool movementInputPressed;
    bool DISABLED;

    float playerDirection;
    bool facingRight = true;
    Rigidbody2D myRigidbody;

    Jump jumpReference;
    AirDash dashReference;
    Hook hookReference;

    // Start is called before the first frame update
    void Start()
    {
        movementReference.action.performed += OnPressed;
        movementReference.action.canceled += OnRelease;

        myRigidbody = GetComponent<Rigidbody2D>();

        jumpReference = GetComponent<Jump>();
        dashReference = GetComponent<AirDash>();
        hookReference = GetComponent<Hook>();

        // //PRUEBA DE FUERZAS
        // shootReference.action.performed += Test;
    }

    private void OnPressed(InputAction.CallbackContext context)
    {
        movementInputPressed = true;
        playerDirection = movementReference.action.ReadValue<float>();

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
        movementReference.action.Disable();
        DISABLED = true;    
    }
    public void EnableMovementInput()
    {
        movementReference.action.Enable();
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
