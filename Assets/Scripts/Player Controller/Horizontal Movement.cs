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
    Rigidbody2D myRigidbody;

    Jump jump;


    // Start is called before the first frame update
    void Start()
    {
        movementReference.action.performed += OnPressed;
        movementReference.action.canceled += OnRelease;
        myRigidbody = GetComponent<Rigidbody2D>();

        jump = GetComponent<Jump>();
        //PRUEBA DE FUERZAS
        shootReference.action.performed += Test;
       
    }

	private void Test(InputAction.CallbackContext context)
	{
        isHooking = true;
	}

	private void OnPressed(InputAction.CallbackContext context)
	{
        if(!isHooking)
        {
            playerDirection = movementReference.action.ReadValue<float>();
            keyPressed = true;
        }
	}

	private void OnRelease(InputAction.CallbackContext context)
	{
        if(!isHooking)
        {
            playerDirection = 0.0f;
            keyPressed = false;
        }
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if (jump.IsGrounded) //PLACE HOLDER A ESPERA DE QUE EL GANCHO ESTÉ LISTO
        {
            isHooking = false;
        }

       // if (!isHooking || keyPressed)
       // {
            myRigidbody.velocity = new Vector2(playerDirection * movementSpeed, myRigidbody.velocity.y + 0.001f);
            isHooking = false;
       // }
       // else if (isHooking && !keyPressed)
       // {
       //     myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y);
       //
       // }

    }


}
