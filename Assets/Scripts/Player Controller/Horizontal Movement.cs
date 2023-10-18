using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Input system")]
    [SerializeField] InputActionReference movementReference;

    [Header("Movement params")]
    [SerializeField] float movementSpeed;

    float playerDirection;
    Rigidbody2D myRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        movementReference.action.performed += OnPressed;
        movementReference.action.canceled += OnRelease;
        myRigidbody = GetComponent<Rigidbody2D>();
    }

	private void OnPressed(InputAction.CallbackContext context)
	{
		playerDirection = movementReference.action.ReadValue<float>();
	}

	private void OnRelease(InputAction.CallbackContext context)
	{
		playerDirection = 0.0f;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
       myRigidbody.velocity = new Vector2 (playerDirection * movementSpeed, myRigidbody.velocity.y);

    }

}
