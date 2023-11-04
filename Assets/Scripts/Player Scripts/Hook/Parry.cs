using EZCameraShake;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Parry : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference parryReference;
    [SerializeField] private InputActionReference hookAimMouseReference;
    [SerializeField] private InputActionReference hookAimGamepadReference;


    [Header("Player Components")]
    [SerializeField] private Dash dashComponent;
    [SerializeField] private Jump jumpComponent;
    [SerializeField] private HorizontalMovement horizontalMovementComponent;

    //AIM REPRESENTATION
    [Header("AIM REPRESENTATION")]
    [SerializeField] private GameObject aimRepresentation;
    [SerializeField] private Vector3 shootDirection;

    //MOUSE COORDINATES
    [Header("Mouse Coordinates")]
    [SerializeField] private Vector2 mousePositionInScreen;
    [SerializeField] private Vector2 mousePositionInWorld;

    //JOYSTICK AIM COORDINATES
    [Header("Controller Aim Variables")]
    [SerializeField] private Vector2 controllerAim;

    //PARRY PARAMETERS
    [Header("Parry Parameters")]
    [SerializeField] private float parryDistance;
    [SerializeField] private float parryForce;
    [SerializeField] private float parryTime;
    [SerializeField] public float parryKnockbackTime;


    //PARRY LOGIC VARIABLES
    [Header("Parry Logic Variables")]
    [SerializeField] private float parryTimer;
    [SerializeField] private bool Disabled;
    [SerializeField] private GameObject hookedObject;
    [SerializeField] private IHookable hookableComponent;
    [SerializeField] private bool parryReady;


    [SerializeField] private float hookingRange;

    // Start is called before the first frame update
    void Start()
    {
        parryReference.action.performed += OnParry;
        parryReference.action.Disable();

        hookAimMouseReference.action.performed += OnMouseMovement;
        hookAimGamepadReference.action.performed += OnControllerAim;

        hookingRange = GetComponent<Hook>().hookingRange;
    }

    private void Update()
    {
        if (parryTimer > 0)
        {
            parryTimer -= Time.deltaTime;
            if (parryTimer < 0)
            {
                hookableComponent.SetParried(false);
            }
        }
    }

    private void OnParry(InputAction.CallbackContext context)
    {
        if (!parryReady) return;

        parryReady = false;
        parryTimer = parryTime;
        shootDirection.Normalize();
        hookableComponent.Parried(shootDirection * parryForce, parryKnockbackTime);
        hookableComponent.SetParried(true);
        Debug.Log("PARRY INPUT");
    }

    public void SetHookedObject(GameObject hooked)
    {
        hookedObject = hooked;
        hookableComponent = hookedObject.GetComponent<IHookable>();
    }

    public void DisableParry()
    {
        parryReference.action.Disable();
        Disabled = true;
        parryReady = false;
    }

    public void EnableParry()
    {
        parryReference.action.Enable();
        Disabled = false;
        parryReady = true;
    }

    private void OnControllerAim(InputAction.CallbackContext context)
    {
        if (Disabled) return;
        controllerAim = context.ReadValue<Vector2>();
        //controllerAim = new Vector2((float)Math.Round(controllerAim.x, 2), (float)Math.Round(controllerAim.y, 2));
        aimRepresentation.GetComponent<Transform>().localPosition = shootDirection.normalized * hookingRange;
        shootDirection = controllerAim;



    }

    private void OnMouseMovement(InputAction.CallbackContext context)
    {
        if (Disabled) return;
        mousePositionInScreen = context.ReadValue<Vector2>();
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);
        aimRepresentation.GetComponent<Transform>().position = mousePositionInWorld;
        shootDirection = mousePositionInWorld - new Vector2(transform.position.x, transform.position.y);
    }


    public void parryEffects(bool facingRight)
    {
        horizontalMovementComponent.IsFacingRight = facingRight;
        CameraShaker.Instance.ShakeOnce(15f, 1f, .1f, 0.7f);
        TimeStop.instance.StopTime(0.05f, 10f, 0.5f);
        dashComponent.HasParred = true;
        jumpComponent.HasParred = true;

    }

}
