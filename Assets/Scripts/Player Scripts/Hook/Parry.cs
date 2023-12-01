
using Cinemachine;
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

    [SerializeField] private InputActionReference parryMOBILEReference;
    [SerializeField] private InputActionReference hookAimMOBILEReference;
    [SerializeField]private bool MOBILE;


    [Header("Player Components")]
    [SerializeField] private Dash dashComponent;
    [SerializeField] private Jump jumpComponent;
    [SerializeField] private HorizontalMovement horizontalMovementComponent;

    //AIM REPRESENTATION
    [Header("AIM REPRESENTATION")]
    [SerializeField] public GameObject aimRepresentation;
    [SerializeField] private Vector3 shootDirection;
    [SerializeField] private GameObject parryAim1;
    [SerializeField] private GameObject parryAim2;
    [SerializeField] private SpriteRenderer parryAimSprite1;
    [SerializeField] private SpriteRenderer parryAimSprite2;

    private int hookableWeight;

    //MOUSE COORDINATES
    [Header("Mouse Coordinates")]
    [SerializeField] private Vector2 mousePositionInScreen;
    [SerializeField] private Vector2 mousePositionInWorld;

    //JOYSTICK AIM COORDINATES
    [Header("Controller Aim Variables")]
    [SerializeField] private Vector2 controllerAim;

    //PARRY PARAMETERS
    [Header("Parry Parameters")]
    [SerializeField] private float parryForce;
    [SerializeField] private float parryTime;
    [SerializeField] public float parryKnockbackTime;

    [Header("Parry Time Stop")]
    [SerializeField] public float stopTimeDistance;
    [SerializeField] public float timeScale;
    [SerializeField] public float timeScaleRecoveryRatio;


    //PARRY LOGIC VARIABLES
    [Header("Parry Logic Variables")]
    [SerializeField] public float parryTimer;
    [SerializeField] private bool Disabled;
    [SerializeField] private GameObject hookedObject;
    [SerializeField] private AHookable hookableComponent;
    [SerializeField] private bool parryReady;


    [Header("Animator Variables")]
    [SerializeField] private Animator animator;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Audio Management")]
    [SerializeField] private PlayerSoundManager soundManager;

    private float hookingRange;

    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (MOBILE)
        {
            parryMOBILEReference.action.performed += OnParry;
            hookAimMOBILEReference.action.performed += OnControllerAim;
        }else
        {
            parryReference.action.performed += OnParry;
            hookAimMouseReference.action.performed += OnMouseMovement;
            hookAimGamepadReference.action.performed += OnControllerAim;
        }



        hookingRange = GetComponent<Hook>().hookingRange;

        aimRepresentation = GameObject.FindWithTag("AimRepresentation");
        DisableParry();
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
        if(hookedObject != null)
        {
            if(hookableWeight == 0)
            {
                parryAim1.transform.position = hookedObject.transform.position + new Vector3(shootDirection.x, shootDirection.y, 0).normalized * 3;
                parryAim1.transform.right = shootDirection.normalized;
            }
            else if(hookableWeight == 1)
            {
                parryAim1.transform.position = hookedObject.transform.position + new Vector3(shootDirection.x, shootDirection.y, 0).normalized * 3;
                parryAim1.transform.right = shootDirection.normalized;
                parryAim2.transform.position = transform.position - new Vector3(shootDirection.x, shootDirection.y, 0).normalized * 3;
                parryAim2.transform.right = -shootDirection.normalized;
            }
            else if(hookableWeight == 2)
            {
                parryAim1.transform.position = transform.position + new Vector3(shootDirection.x, shootDirection.y, 0).normalized * 3;
                parryAim1.transform.right = shootDirection.normalized;
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
    }

    public void SetHookedObject(GameObject hooked)
    {
        hookedObject = hooked;
        hookableComponent = hookedObject.GetComponent<AHookable>();
        if (hookedObject.GetComponent<LightHookable>() != null)
        {
            hookableWeight = 0;
        }
        if (hookedObject.GetComponent<MediumHookable>() != null)
        {
            parryAimSprite2.enabled = true;
            hookableWeight = 1;
        }
        if (hookedObject.GetComponent<HeavyHookable>() != null)
        {
            hookableWeight = 2;
        }
        parryAimSprite1.enabled = true;
        aimRepresentation.GetComponent<SpriteRenderer>().enabled = false;

    }

    public void DisableParry()
    {
        parryReference.action.Disable();
        hookedObject = null;
        Disabled = true;
        parryReady = false;
        parryAimSprite1.enabled = false;
        parryAimSprite2.enabled = false;
        aimRepresentation.GetComponent<SpriteRenderer>().enabled = true;
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

        //aimRepresentation.GetComponent<Transform>().localPosition = shootDirection.normalized * hookingRange;
        shootDirection = controllerAim;



    }

    private void OnMouseMovement(InputAction.CallbackContext context)
    {
        if (Disabled) return;
        mousePositionInScreen = context.ReadValue<Vector2>();
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);
        // aimRepresentation.GetComponent<Transform>().position = mousePositionInWorld;


        switch (hookableWeight)
        {
            case 0:
                shootDirection = mousePositionInWorld - new Vector2(hookedObject.transform.position.x, hookedObject.transform.position.y);
                break;
            case 1:
                shootDirection = mousePositionInWorld - new Vector2(hookedObject.transform.position.x, hookedObject.transform.position.y);   
                break;
            case 2:
                shootDirection = mousePositionInWorld - new Vector2(transform.position.x, transform.position.y);
                break;

        }
    }


    public void parryEffects(bool facingRight)
    {
        animator.SetTrigger("Parry");
        soundManager.PlayParry();

        horizontalMovementComponent.SpriteFlipManager(facingRight);
        CameraShakeManager.instance.CameraShake(impulseSource, new Vector3(1, 0.2f, 0));
        TimeStop.instance.StopTime(0.05f, 10f, 0.5f);
        dashComponent.HasParred = true;
        jumpComponent.HasParred = true;

    }

    private void OnDestroy()
    {
        parryReference.action.performed -= OnParry;
    }

}
