
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class Hook : MonoBehaviour
{
    //INPUT ACTIONS VARIABLES
    [Header("Input system")]
    [SerializeField] private InputActionReference hookShootMouseReference;
    [SerializeField] private InputActionReference hookAimMouseReference;

    [SerializeField] private InputActionReference hookShootGamepadReference;
    [SerializeField] private InputActionReference hookAimGamepadReference;

    [SerializeField] private InputActionReference hookShootMOBILEReference;
    [SerializeField] private InputActionReference hookAimMOBILEReference;
    [SerializeField] private bool MOBILE;

    //PLAYER COMPONENTS
    [Header("Player Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private HorizontalMovement horizontalMovementComponent;
    [SerializeField] private Dash dashComponent;
    [SerializeField] private WallGrab wallGrabComponent;
    [SerializeField] private Jump jumpComponent;
    [SerializeField] private Parry parryComponent;
    [SerializeField] private Interact interactComponent;
    [SerializeField] private Rigidbody2D hookedRigidBody;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask projectileLayer;

    //AIM REPRESENTATION
    [Header("AIM REPRESENTATION")]
    [SerializeField] public GameObject aimRepresentation;
    private bool mouseOrGamepad;

    //MOUSE COORDINATES
    [Header("Mouse Coordinates")]
    [SerializeField] private Vector2 mousePositionInScreen;
    [SerializeField] private Vector2 mousePositionInWorld;

    //JOYSTICK AIM COORDINATES
    [Header("Controller Aim Variables")]
    [SerializeField] private Vector2 controllerAim;
    private Vector2 positionController;


    //HOOK PARAMETERS
    [Header("Hook Parameters")]
    [SerializeField] public float hookingRange;
    [SerializeField] private float hookProjectileSpeed;
    [SerializeField] public float hookingSpeed;
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
    public GameObject hookProjectile;

    [Header("Hooked GameObject")]
    [SerializeField] private GameObject hookedObject;

    public GameObject HookedObject { get { return hookedObject; } }

    //OTHER VARIABLES
    [Header("Other Variables")]
    [SerializeField] private Vector2 shootDirection;

    [Header("Animation Variables")]
    [SerializeField] private Animator animator;

    [Header("Audio Management")]
    [SerializeField] private PlayerSoundManager soundManager;

    public bool ShootingHook { get { return shootingHook; } } 
    void Start()
    {
        if (MOBILE)
        {
            hookShootMOBILEReference.action.performed += OnControllerShoot;
            hookAimMOBILEReference.action.performed += OnControllerAim;
        }else
        {
            hookAimMouseReference.action.performed += OnMouseMovement;
            hookShootMouseReference.action.performed += OnMouseShoot;

            hookAimGamepadReference.action.performed += OnControllerAim;
            hookShootGamepadReference.action.performed += OnControllerShoot;
        }

        
        interactComponent = GetComponent<Interact>();

        aimRepresentation = GameObject.FindWithTag("AimRepresentation");


        //hookShootControllerReference.action.Disable();
        //hookAimControllerReference.action.Disable();
    }

    void Update()
    {
        if (!mouseOrGamepad && !MOBILE)
        {
            mousePositionInScreen = Input.mousePosition;
            mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);
            aimRepresentation.GetComponent<Transform>().position = new Vector3(mousePositionInWorld.x, mousePositionInWorld.y, aimRepresentation.GetComponent<Transform>().position.z);
        }
        else
        {
           positionController= shootDirection.normalized * hookingRange;
            aimRepresentation.GetComponent<Transform>().localPosition = new Vector3(positionController.x, positionController.y, 10);
        }
    }
    void FixedUpdate()
    {

        if (hookingUnstuckTimer > 0)
        {
            if(hookedObject == null)
            {
                HookDestroyed();
            }

            hookingUnstuckTimer -= Time.fixedDeltaTime;
            if (hookedObject!= null && hookedObject.GetComponent<AHookable>().IsParried())
            {
                hookingUnstuckTimer = parryComponent.parryKnockbackTime;
            }
            if (hookingUnstuckTimer < 0 && hookedObject != null)
            {
                hookedObject.GetComponent<AHookable>().Unhook();
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

        mouseOrGamepad = true;
        shootDirection = controllerAim;
       



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
        if (!disableAim)
        {
            mouseOrGamepad = false;
            
        }
    }


    private void OnMouseShoot(InputAction.CallbackContext context)
    {
        if (shootingHook || hookCoolDownTimer > 0) return;

        hookCoolDownTimer = hookCooldDown;

        shootingHook = true;

        shootDirection = mousePositionInWorld - new Vector2(hookGunPosition.position.x, hookGunPosition.position.y);

        
        Shoot(shootDirection);
        
    }

    private void Shoot(Vector2 shootDirection)
    {
        animator.SetBool("Running", false);
        Time.timeScale = 0.75f;
        soundManager.PlayHook();
        interactComponent.enabled = false;

        if (shootDirection.x < 0)
        {
            horizontalMovementComponent.SpriteFlipManager( false);
            
        }
        else if (shootDirection.x > 0)
        {
            horizontalMovementComponent.SpriteFlipManager(true);

        }


        shootDirection.Normalize();

        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;


        horizontalMovementComponent.DisableMovementInput();
        dashComponent.DisableDashInput();
        wallGrabComponent.DisableWallGrabInput();
        jumpComponent.DisableJumpInput();

        hookProjectile = Instantiate(hookPrefab, hookGunPosition.position, Quaternion.identity);
        hookedRigidBody = hookProjectile.GetComponent<Rigidbody2D>();
        hookProjectile.GetComponent<HookProjectile>().Shoot(shootDirection, hookingRange, unhookDistance, hookProjectileSpeed);
    }

    public void HookDestroyed()
    {
        Time.timeScale = 1;

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

        interactComponent.enabled = true;
    }

    public void SomethingHooked(GameObject hookedObject)
    {
        //CameraShaker.Instance.ShakeOnce(2f, 4f, .1f, 1f);
        //TimeStop.instance.StopTime(0.05f, 15f, 1f);

        animator.SetBool("Hooking", true);

        hookingUnstuckTimer = -1;
        this.hookedObject = hookedObject;
        hookProjectile.GetComponent<Collider2D>().enabled = false;


        hookedObject.GetComponent<AHookable>().Hooked(hookProjectile, hookingSpeed);

        hookShootGamepadReference.action.Disable();
        hookShootMouseReference.action.Disable();
        disableAim = true;

        parryComponent.EnableParry();

        parryComponent.SetHookedObject(hookedObject);
        hookingUnstuckTimer = hookingMaxDuration;

    }


    public void DisableHookInput()
    {
        hookShootGamepadReference.action.Disable();
        hookShootMouseReference.action.Disable();

    }
    public void EnableHookInput()
    {
        hookShootGamepadReference.action.Enable();
        hookShootMouseReference.action.Enable();

    }

    private void OnDestroy()
    {
        hookAimMouseReference.action.performed -= OnMouseMovement;
        hookShootMouseReference.action.performed -= OnMouseShoot;

        hookAimGamepadReference.action.performed -= OnControllerAim;
        hookShootGamepadReference.action.performed -= OnControllerShoot;

        hookAimMOBILEReference.action.performed -= OnControllerAim;
        hookShootMOBILEReference.action.performed -= OnControllerShoot;
    }
}
