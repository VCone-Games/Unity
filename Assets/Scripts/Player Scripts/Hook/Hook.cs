using EZCameraShake;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hook : MonoBehaviour
{
    //INPUT ACTIONS VARIABLES
    [Header("Input system")]
    [SerializeField] private InputActionReference hookShootMouseReference;
    [SerializeField] private InputActionReference hookAimMouseReference;

    [SerializeField] private InputActionReference hookShootGamepadReference;
    [SerializeField] private InputActionReference hookAimGamepadReference;

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


    //HOOK PARAMETERS
    [Header("Hook Parameters")]
    [SerializeField] public float hookingRange;
    [SerializeField] private float hookProjectileSpeed;
    [SerializeField] private float hookingSpeed;
    [SerializeField] private float hookingMaxDuration;
    [SerializeField] private float hookCooldDown;

    [SerializeField] private float unhookDistance;


    //HOOK LOGIC VARIABLES
    [Header("Hook Control Variables")]
    [SerializeField] private float hookingUnstuckTimer;
    [SerializeField] private float hookCoolDownTimer;
    [SerializeField] private bool shootingHook;
    [SerializeField] private bool hookLanded;
    [SerializeField] private bool hookFailed;
    [SerializeField] private bool disableAim;

    //GAMEOBJECTS
    [Header("Hook Projectile Prefab and GameObject")]
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Transform hookGunPosition;
    private GameObject hookProjectile;

    [Header("Hooked GameObject")]
    [SerializeField] private GameObject hookedObject;

    //OTHER VARIABLES
    [Header("Other Variables")]
    [SerializeField] private Vector3 shootDirection;

    [Header("Animation Variables")]
    [SerializeField] private Animator animator;




    void Start()
    {
        hookAimMouseReference.action.performed += OnMouseMovement;
        hookShootMouseReference.action.performed += OnMouseShoot;

        hookAimGamepadReference.action.performed += OnControllerAim;
        hookShootGamepadReference.action.performed += OnControllerShoot;

        //hookShootControllerReference.action.Disable();
        //hookAimControllerReference.action.Disable();
    }


    void FixedUpdate()
    {
        if (hookingUnstuckTimer > 0)
        {
            hookingUnstuckTimer -= Time.fixedDeltaTime;
            if (hookedObject.GetComponent<IHookable>().IsParried())
            {
                hookingUnstuckTimer = parryComponent.parryKnockbackTime;
            }
            if (hookingUnstuckTimer < 0)
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
        if (disableAim) return;
        controllerAim = context.ReadValue<Vector2>();
        //controllerAim = new Vector2((float)Math.Round(controllerAim.x, 2), (float)Math.Round(controllerAim.y, 2));

        if (controllerAim.magnitude > 0.5f)
        {
            shootDirection = controllerAim;
            aimRepresentation.GetComponent<Transform>().localPosition = shootDirection.normalized * hookingRange;
        }


    }

    private void OnControllerShoot(InputAction.CallbackContext context)
    {
        if (shootingHook || hookCoolDownTimer > 0) return;

        hookCoolDownTimer = hookCooldDown;
        shootingHook = true;

        Shoot(shootDirection);
    }

    private void OnMouseMovement(InputAction.CallbackContext context)
    {
        if (disableAim) return;
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
        if (shootDirection.x < 0)
        {
            horizontalMovementComponent.IsFacingRight = false;
        }
        else if (shootDirection.x > 0)
        {
            horizontalMovementComponent.IsFacingRight = true;
        }

        hookProjectile = Instantiate(hookPrefab, hookGunPosition.position, Quaternion.identity);
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

        animator.SetBool("Hooking", false);

        horizontalMovementComponent.EnableMovementInput();
        dashComponent.EnableDashInput();
        wallGrabComponent.EnableWallGrabInput();
        jumpComponent.EnableJumpInput();

        hookShootGamepadReference.action.Enable();
        hookShootMouseReference.action.Enable();
        disableAim = false;
        
        parryComponent.DisableParry();

        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SomethingHooked(GameObject hookedObject)
    {
        CameraShaker.Instance.ShakeOnce(2f, 4f, .1f, 1f);
        TimeStop.instance.StopTime(0.05f, 15f, 1f);

        animator.SetBool("Hooking", true);

        hookingUnstuckTimer = -1;
        this.hookedObject = hookedObject;
        hookProjectile.GetComponent<Collider2D>().enabled = false;

        hookedObject.GetComponent<IHookable>().Hooked(hookProjectile, hookingSpeed);

        hookShootGamepadReference.action.Disable();
        hookShootMouseReference.action.Disable();
        disableAim = true;

        parryComponent.EnableParry();

        parryComponent.SetHookedObject(hookedObject);
        hookingUnstuckTimer = hookingMaxDuration;

    }
}
