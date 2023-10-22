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

    bool keyPressed;
    public bool isHooking;
    float playerDirection;
    bool facingRight = true;
    Rigidbody2D myRigidbody;

    Jump jumpReference;
    AirDash dashReference;


    // Start is called before the first frame update
    void Start()
    {
        movementReference.action.performed += OnPressed;
        movementReference.action.canceled += OnRelease;
        myRigidbody = GetComponent<Rigidbody2D>();

        jumpReference = GetComponent<Jump>();
        dashReference = GetComponent<AirDash>();

        //PRUEBA DE FUERZAS
        shootReference.action.performed += Test;
    }

    private void Test(InputAction.CallbackContext context)
    {
        isHooking = true;
    }

    private void OnPressed(InputAction.CallbackContext context)
    {
        if (!isHooking)
        {
            playerDirection = movementReference.action.ReadValue<float>();

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
            keyPressed = true;
        }
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        if (!isHooking)
        {
            playerDirection = 0.0f;
            keyPressed = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (jumpReference.IsGrounded) //PLACE HOLDER A ESPERA DE QUE EL GANCHO EST� LISTO
        {
            isHooking = false;
        }

        if (dashReference.IsDashing) return;

        if (!isHooking || keyPressed)
        {
            myRigidbody.velocity = new Vector2(playerDirection * movementSpeed, myRigidbody.velocity.y);

            isHooking = false;
        }
        //else if (isHooking && !keyPressed)
        //{
        //    myRigidbody.velocity = new Vector2(Vector2.right.x * movementSpeed, myRigidbody.velocity.y);
        //
        //}

    }
}
