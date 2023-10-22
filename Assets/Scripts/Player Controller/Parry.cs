using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Parry : MonoBehaviour
{
    [Header("Input system")]
    [SerializeField] InputActionReference parryReference;
    [SerializeField] InputActionReference aimReference;

    private GameObject hookedObject;

    [Header("Parry Parameters")]
    [SerializeField] private float parryDistance;
    [SerializeField] private float parryForce;

    //MOUSE COORDINATES
    [Header("Mouse Coordinates")]
    [SerializeField][ReadOnly] private Vector2 mousePositionInScreen;
    [SerializeField][ReadOnly] private Vector2 mousePositionInWorld;

    private bool parrySuccess;

    [SerializeField] private float parryKnockBackTimer;
    [SerializeField] private float parryKnockbackTime;

    [SerializeField] private Rigidbody2D myRigidbody;

    private HorizontalMovement horizontalMovementComponent;
    private AirDash dashComponent;
    private Wallgrab wallGrabComponent;

    // Start is called before the first frame update
    void Start()
    {
        parryReference.action.performed += OnParry;
        parryReference.action.canceled += OffParry;
        aimReference.action.performed += OnMouseMovement;

        horizontalMovementComponent = GetComponent<HorizontalMovement>();
        dashComponent = GetComponent<AirDash>();
        wallGrabComponent = GetComponent<Wallgrab>();
    }

    // Update is called once per frame
    void Update()
    {
        if(parryKnockBackTimer > 0)
        {
            parryKnockBackTimer -= Time.deltaTime;
            if(parryKnockBackTimer < 0)
            {
                horizontalMovementComponent.EnableMovementInput();
                dashComponent.EnableDashInput();
                wallGrabComponent.EnableWallGrabInput();
            }

        }
    }

    private void OnMouseMovement(InputAction.CallbackContext context)
    {
        mousePositionInScreen = context.ReadValue<Vector2>();
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);
    }

    private void OnParry(InputAction.CallbackContext context)
    {
        if (!GetComponent<Hook>().HookLanded()) return;



        hookedObject = GetComponent<Hook>().getHookedObject();

        Vector3 distance = transform.position - hookedObject.transform.position;

        if (distance.magnitude <= parryDistance)
        {
            Debug.Log("PARREO");
            hookedObject.GetComponent<IHookable>().ParryThis();
            parrySuccess = true;
            Time.timeScale = 0.001f;
        }

    }

    private void OffParry(InputAction.CallbackContext context)
    {
        if (!parrySuccess) return;
        parrySuccess = false;

        Rigidbody2D parriedRigidBody = hookedObject.GetComponent<Rigidbody2D>();

        Time.timeScale = 1;

        horizontalMovementComponent.DisableMovementInput();
        dashComponent.DisableDashInput();
        wallGrabComponent.DisableWallGrabInput();

        parryKnockBackTimer = parryKnockbackTime;

        hookedObject.GetComponent<IHookable>().AfterParry();
        switch (hookedObject.GetComponent<IHookable>().GetWeight())
        {
            case 0:
                Vector2 direction = mousePositionInWorld - new Vector2(hookedObject.transform.position.x, transform.position.y);
                direction.Normalize();

                parriedRigidBody.velocity = new Vector3(direction.x, direction.y) * parryForce;
                break;
            case 1:
                direction = mousePositionInWorld - new Vector2(transform.position.x, transform.position.y);
                direction.Normalize();

                myRigidbody.velocity = new Vector2(direction.x, direction.y) * parryForce;
                parriedRigidBody.velocity = new Vector3(-direction.x, direction.y) * parryForce;

                break;
            case 2:
                direction = mousePositionInWorld - new Vector2(transform.position.x, transform.position.y);
                direction.Normalize();

                myRigidbody.velocity = new Vector2(direction.x, direction.y) * parryForce;

                break;

        }
        hookedObject = null;
    }


}
