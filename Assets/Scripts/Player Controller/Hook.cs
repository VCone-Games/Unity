using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hook : MonoBehaviour
{
    //INPUT ACTIONS VARIABLES
    [Space][SerializeField] private InputActionAsset myActionsAsset;
    private InputAction graplingShootAction;
    private InputAction graplingAimAction;
    private InputAction graplingShootAimAction;

    //MOUSE COORDINATES
    private Vector2 mousePositionInScreen;
    private Vector2 mousePositionInWorld;

    //HOOK PARAMETERS
    private float hookRange;
    [SerializeField] private float projectileSpeed;
    Vector3 shootDirection;
    [SerializeField] private float hookingSpeed = 10;
    [SerializeField] AnimationCurve hookingAnimationCurve;
    [SerializeField] private float hookingDuration = 2f;
    private float hookElapsedTime;
    private float hookPercentageComplete;

    //HOOK LOGIC VARIABLES
    private bool isHooking;
    [SerializeField] private bool hookLanded;
    private float hookCoolDownTime;
    private float hookCoolDownTimer;

    [SerializeField] private GameObject hookPrefab;
    private GameObject hookProjectile;

    [SerializeField] private GameObject hookedGameObject;

    private Rigidbody2D myRigidbody2D;
    private Rigidbody2D hookedRigidBody2D;

    void Start()
    {
        //LOCATE INPUT ACTIONS
        graplingAimAction = myActionsAsset.FindAction("Player/Hook Aim ");
        graplingShootAction = myActionsAsset.FindAction("Player/Hook Shoot");
        graplingShootAimAction = myActionsAsset.FindAction("Player/Hook Aim + Shoot ");

        //ASIGN ACTIONS TO CODE
        graplingAimAction.performed += OnMouseMovement;
        graplingShootAction.performed += OnMouseShoot;


        hookRange = transform.GetChild(0).transform.lossyScale.x * 0.5f;

        myRigidbody2D = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (isHooking && !hookLanded)
        {
            HookProjectileMovementAndCollision();

        }
        else if (isHooking && hookLanded)
        {
            HookingInteraction();
        }
    }

    void HookProjectileMovementAndCollision()
    {
        Vector3 distance = transform.position - hookProjectile.transform.position;
        if (distance.magnitude > hookRange)
        {
            Destroy(hookProjectile);
            isHooking = false;
            shootDirection = Vector3.zero;
        }
        else
        {
            hookProjectile.transform.position += shootDirection * projectileSpeed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(hookProjectile.transform.position, shootDirection, 0.01f, ~LayerMask.GetMask("Player"));
            if (hit.collider != null && hit.collider.gameObject.GetComponent<IHookable>() != null)
            {
                hookLanded = true;
                hookedGameObject = hit.collider.gameObject;
                hookedRigidBody2D = GetComponent<Rigidbody2D>();
                hookedGameObject.GetComponent<IHookable>().Hooked();
            }
        }
    }

    void HookingInteraction()
    {
        Vector2 direction = new Vector2(transform.position.x - hookedGameObject.transform.position.x, transform.position.y - hookedGameObject.transform.position.y);
        Debug.Log(direction);
        if (direction.magnitude < 1)
        {
            isHooking = false;

            hookedGameObject.GetComponent<IHookable>().Unhook();
            

            myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            hookedGameObject = null;
            hookedRigidBody2D = null;
            hookElapsedTime = 0;
            hookPercentageComplete = 0;
        }
        else
        {
            direction.Normalize();
            hookElapsedTime += Time.deltaTime;
            hookPercentageComplete = hookElapsedTime / hookingDuration;
            switch (hookedGameObject.GetComponent<IHookable>().getWeight())
            {
                case 0:
                    myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                    hookedRigidBody2D.velocity = new Vector3(direction.x, direction.y, 0) * hookingAnimationCurve.Evaluate(hookPercentageComplete) * hookingSpeed;
                    break;
                case 1:
                    hookedRigidBody2D.velocity = new Vector3(direction.x, direction.y, 0) * hookingAnimationCurve.Evaluate(hookPercentageComplete) * hookingSpeed;
                    myRigidbody2D.velocity = new Vector3(-direction.x, -direction.y, 0) * hookingAnimationCurve.Evaluate(hookPercentageComplete) * hookingSpeed;
                    break;
                case 2:
                    myRigidbody2D.velocity = new Vector3(-direction.x, -direction.y, 0) * hookingAnimationCurve.Evaluate(hookPercentageComplete) * hookingSpeed;
                    break;
            }
        }
    }

    private void OnMouseMovement(InputAction.CallbackContext context)
    {
        mousePositionInScreen = context.ReadValue<Vector2>();
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);

    }

    private void OnMouseShoot(InputAction.CallbackContext context)
    {
        if (isHooking) return;

        isHooking = true;

        shootDirection = mousePositionInWorld - new Vector2(transform.position.x, transform.position.y);
        shootDirection.Normalize();


        hookProjectile = Instantiate(hookPrefab, transform.position, Quaternion.identity);
    }





}
