using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hook : MonoBehaviour
{
    //INPUT ACTIONS VARIABLES
    [Header("Input system")]
    [SerializeField] private InputActionReference hookShootReference;
    [SerializeField] private InputActionReference hookAimReference;
    [SerializeField] private InputActionReference hookShootAimReference;

    //PLAYER COMPONENTS
    [Header("Player Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private HorizontalMovement horizontalMovementComponent;
    [SerializeField] private Dash dashComponent;
    [SerializeField] private WallGrab wallGrabComponent;
    [SerializeField] private Jump jumpComponent;
    [SerializeField] private Parry parryComponent;
    [SerializeField] private Rigidbody2D hookedRigidBody;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask projectileLayer;

    //AIM REPRESENTATION
    [Header("AIM REPRESENTATION")]
    [SerializeField] private GameObject aimRepresentation;

    //MOUSE COORDINATES
    [Header("Mouse Coordinates")]
    [SerializeField] private Vector2 mousePositionInScreen;
    [SerializeField] private Vector2 mousePositionInWorld;

    //JOYSTICK AIM COORDINATES
    [Header("Controller Aim Variables")]
    [SerializeField] private Vector2 controllerAim;
    [SerializeField] private bool readyToShoot;

    //HOOK RANGE REPRESENTATION
    [Header("Hook Range Representation")]
    [SerializeField] private GameObject hookRangeRepresentation;

    //HOOK PARAMETERS
    [Header("Hook Parameters")]
    [SerializeField] private float hookingRange;
    [SerializeField] private float hookProjectileSpeed;
    [SerializeField] private float hookingSpeed;
    [SerializeField] private float hookingMaxDuration;
    [SerializeField] private float hookCooldDown;
    [SerializeField] private float hookingDuration;
    [SerializeField] AnimationCurve hookingAnimationCurve;
    [SerializeField] private float unhookDistance;


    //HOOK LOGIC VARIABLES
    [Header("Hook Control Variables")]
    [SerializeField] private float hookingUnstuckTimer;
    [SerializeField] private float hookCoolDownTimer;
    [SerializeField] private bool shootingHook;
    [SerializeField] private bool hookLanded;
    [SerializeField] private bool hookFailed;

    //GAMEOBJECTS
    [Header("Hook Projectile Prefab and GameObject")]
    [SerializeField] private GameObject hookPrefab;
    private GameObject hookProjectile;

    [Header("Hooked GameObject")]
    [SerializeField] private GameObject hookedObject;

    //OTHER VARIABLES
    [Header("Other Variables")]
    [SerializeField] private Vector3 shootDirection;
    [SerializeField] private float hookElapsedTime;
    [SerializeField] private float hookPercentageComplete;



    void Start()
    {
        hookAimReference.action.performed += OnMouseMovement;
        hookShootReference.action.performed += OnMouseShoot;
        hookShootAimReference.action.performed += OnControllerAim;
        hookShootAimReference.action.canceled += OnControllerShoot;

        hookRangeRepresentation.transform.localScale = new Vector3(hookingRange * 2, hookingRange * 2, hookingRange * 2);
        hookRangeRepresentation.transform.SetParent(transform, false);
    }


    void FixedUpdate()
    {
        if(hookingUnstuckTimer > 0)
        {
            hookingUnstuckTimer -= Time.fixedDeltaTime;
            if(hookingUnstuckTimer < 0)
            {
                hookedObject.GetComponent<IHookable>().Unhook();
            }
        }
        if (hookCoolDownTimer > 0)
        {
            hookCoolDownTimer -= Time.fixedDeltaTime;
        }
    }

    private void OnControllerAim(InputAction.CallbackContext context)
    {
        controllerAim = context.ReadValue<Vector2>();
        controllerAim = new Vector2((float)Math.Round(controllerAim.x, 2), (float)Math.Round(controllerAim.y, 2));

        if (controllerAim.magnitude > 0.75f)
        {
            shootDirection = controllerAim;
            aimRepresentation.GetComponent<Transform>().localPosition = shootDirection.normalized * hookingRange;
            readyToShoot = true;
        } 
    }

    private void OnControllerShoot(InputAction.CallbackContext context)
    {
        if (shootingHook || !readyToShoot || hookCoolDownTimer > 0) return;

        hookCoolDownTimer = hookCooldDown;
        shootingHook = true;
        readyToShoot = false;
        Shoot(shootDirection);
    }

    private void OnMouseMovement(InputAction.CallbackContext context)
    {
        mousePositionInScreen = context.ReadValue<Vector2>();
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);
        aimRepresentation.GetComponent<Transform>().position = mousePositionInWorld;
    }


    private void OnMouseShoot(InputAction.CallbackContext context)
    {
        if (shootingHook || hookCoolDownTimer > 0) return;

        hookCoolDownTimer = hookCooldDown;

        shootingHook = true;

        shootDirection = mousePositionInWorld - new Vector2(transform.position.x, transform.position.y);

        Shoot(shootDirection);

    }

    private void Shoot(Vector2 shootDirection)
    {
        shootDirection.Normalize();
        hookProjectile = Instantiate(hookPrefab, transform.position, Quaternion.identity);
        hookedRigidBody = hookProjectile.GetComponent<Rigidbody2D>();
        hookProjectile.GetComponent<HookProjectile>().Shoot(shootDirection, hookingRange, unhookDistance, hookProjectileSpeed);

        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        horizontalMovementComponent.DisableMovementInput();
        dashComponent.DisableDashInput();
        wallGrabComponent.DisableWallGrabInput();
        jumpComponent.DisableJumpInput();
    }

    public void HookDestroyed()
    {
        shootingHook = false;
        horizontalMovementComponent.EnableMovementInput();
        dashComponent.EnableDashInput();
        wallGrabComponent.EnableWallGrabInput();
        jumpComponent.EnableJumpInput();
        parryComponent.DisableParry();
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SomethingHooked(GameObject hookedObject)
    {
        this.hookedObject = hookedObject;
        hookProjectile.GetComponent<Collider2D>().enabled = false;

        hookedObject.GetComponent<IHookable>().Hooked(unhookDistance, hookProjectile, hookingSpeed);

        parryComponent.EnableParry();
        parryComponent.SetHookedObject(hookedObject);
        hookingUnstuckTimer = hookingMaxDuration;

    }
}
