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
    [SerializeField] private float parryKnockbackTime;

    //PARRY LOGIC VARIABLES
    [Header("Parry Logic Variables")]
    [SerializeField] private float parryTimer;
    [SerializeField] private bool Disabled;
    [SerializeField] private GameObject hookedObject;
    [SerializeField] private IHookable hookableComponent;


    private float hookingRange;

    // Start is called before the first frame update
    void Start()
    {
        parryReference.action.performed += OnParry;
        parryReference.action.Disable();

        hookAimMouseReference.action.performed += OnMouseMovement;
        hookAimGamepadReference.action.performed += OnControllerAim;

        hookingRange = GetComponent<Hook>().hookingRange;
    }

    private void FixedUpdate()
    {
        if (parryTimer > 0)
        {
            parryTimer -= Time.fixedDeltaTime;
            if (parryTimer < 0)
            {
                hookableComponent.SetParried(false);
            }
        }
    }

    private void OnParry(InputAction.CallbackContext context)
    {
        // Vector3 distance = transform.position - hookedObject.transform.position;
        // if (distance.magnitude < parryDistance)
        // {
        //
        //     shootDirection.Normalize();
        //     Debug.Log("parried");
        //     hookableComponent.Unhook();
        //     hookableComponent.Parried(shootDirection * parryForce);
        //
        // }
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
    }

    public void EnableParry()
    {
        parryReference.action.Enable();
        Disabled = false;
    }

    private void OnControllerAim(InputAction.CallbackContext context)
    {
        if (Disabled) return;
        controllerAim = context.ReadValue<Vector2>();
        //controllerAim = new Vector2((float)Math.Round(controllerAim.x, 2), (float)Math.Round(controllerAim.y, 2));

        shootDirection = controllerAim;
        aimRepresentation.GetComponent<Transform>().localPosition = shootDirection.normalized * hookingRange;


    }

    private void OnMouseMovement(InputAction.CallbackContext context)
    {
        if (Disabled) return;
        mousePositionInScreen = context.ReadValue<Vector2>();
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);
        shootDirection = mousePositionInWorld - new Vector2(transform.position.x, transform.position.y);
        aimRepresentation.GetComponent<Transform>().position = mousePositionInWorld;
    }


    public void parryEffects()
    {
        CameraShaker.Instance.ShakeOnce(2f, 20f, .1f, 0.7f);
        TimeStop.instance.StopTime(0.05f, 13f, 0.15f);
        dashComponent.HasParred = true;
        jumpComponent.HasParred = true;
    }

}
