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
    bool forceAplied;
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
        forceAplied = true;
	}

	private void OnPressed(InputAction.CallbackContext context)
	{
		playerDirection = movementReference.action.ReadValue<float>();
        keyPressed = true;
	}

	private void OnRelease(InputAction.CallbackContext context)
	{
		playerDirection = 0.0f;
        keyPressed = false;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if (jump.IsGrounded) //PLACE HOLDER A ESPERA DE QUE EL GANCHO ESTÉ LISTO
        {
            forceAplied = false;
        }

        if (!forceAplied || keyPressed)
        {
            myRigidbody.velocity = new Vector2(playerDirection * movementSpeed, myRigidbody.velocity.y);
            forceAplied = false;
        }
        else if (forceAplied && !keyPressed)
        {
            myRigidbody.velocity = new Vector2(Vector2.right.x * movementSpeed, myRigidbody.velocity.y);

        }

    }


}
