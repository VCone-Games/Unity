using System;
using System.Collections;
using System.Collections.Generic;
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
        if(movementDirection > 0)
        {
            SpriteFlipManager(true);
        }
 
    }

    public void SpriteFlipManager(bool isFacingRight)
    {
        //if (movementDirection < 0 || direction == 1) //SI MUEVE HACIA IZQUIERDA
        //{
        //    if (facingRight) //ME GIRO SI MIRABA A LA DERECHA
        //    {
        //        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //    }
        //    facingRight = false;
        //}
        //
        //else if(movementDirection > 0 || direction == 2)//SI MUEVE HACIA DERECHA   
        //{
        //    if (!facingRight) //ME GIRO SI MIRABA A LA IZQUIERDA
        //    {
        //        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //    }
        //    facingRight = true;
        //}
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
            myRigidbody.velocity = new Vector2(movementSpeed * movementDirection, myRigidbody.velocity.y);
        }
        else
        {
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        }

    }
}
