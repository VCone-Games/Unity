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
    Vector3 myLocalScale;

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
        myLocalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
        if (jumpReference.IsGrounded) //PLACE HOLDER A ESPERA DE QUE EL GANCHO ESTÉ LISTO
        {
            forceAplied = false;
        }

        if (dashReference.IsDashing) return;
            
        if (!forceAplied || keyPressed)
        {
            myRigidbody.velocity = new Vector2(playerDirection * movementSpeed, myRigidbody.velocity.y);
            if (playerDirection != 0)
            {
                transform.localScale = new Vector3(playerDirection * myLocalScale.x, myLocalScale.y, myLocalScale.z);
            }

            forceAplied = false;
        }
        else if (forceAplied && !keyPressed)
        {
            myRigidbody.velocity = new Vector2(Vector2.right.x * movementSpeed, myRigidbody.velocity.y);

        }

    }


}
