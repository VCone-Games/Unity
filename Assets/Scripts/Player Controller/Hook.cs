using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hook : MonoBehaviour
{
    //INPUT ACTIONS VARIABLES
    [Header("Input system")]
    [SerializeField] private InputActionReference graplingShootReference;
    [SerializeField] private InputActionReference graplingAimReference;
    [SerializeField] private InputActionReference graplingShootAimReference;


    //MOUSE COORDINATES
    [Header("Mouse Coordinates")]
    [SerializeField][ReadOnly] private Vector2 mousePositionInScreen;
    [SerializeField][ReadOnly] private Vector2 mousePositionInWorld;

    //HOOK PARAMETERS
    [Header("Hook Parameters")]
    [SerializeField] private GameObject hookRangeRepresentation;
    [SerializeField] private float hookRange;
    [SerializeField] private float hookProjectileSpeed;
    [SerializeField] private float hookingSpeed;
    [SerializeField] private float hookingDuration;
    [SerializeField] AnimationCurve hookingAnimationCurve;
    [SerializeField] private float hookCoolDownTime;
    [SerializeField] private float distanceToUnhook;
    Vector3 shootDirection;


    //HOOK LOGIC VARIABLES
    [Header("Hook Control Variables")]
    [SerializeField][ReadOnly] private bool isHooking;
    [SerializeField][ReadOnly] private bool hookLanded;
    [SerializeField][ReadOnly] private bool hookFailed;
    [SerializeField][ReadOnly] private float hookCoolDownTimer;
    private float hookElapsedTime;
    private float hookPercentageComplete;
    private Vector3 collisionPoint;

    //GAMEOBJECTS
    [Header("Hook Projectile Prefab Selector")]
    [SerializeField] private GameObject hookPrefab;
    private GameObject hookProjectile;

    [Header("Hooked GameObject")]
    [SerializeField][ReadOnly] private GameObject hookedGameObject;

    //RIGIDBODIES
    private Rigidbody2D myRigidbody;
    private Rigidbody2D hookedRigidBody;

    void Start()
    {
        //ASIGN ACTIONS TO CODE
        graplingAimReference.action.performed += OnMouseMovement;
        graplingShootReference.action.performed += OnMouseShoot;

        hookRangeRepresentation.transform.localScale = new Vector3(hookRange * 2, hookRange * 2, hookRange * 2);
        hookRangeRepresentation.transform.SetParent(transform, false);
        //hookRange = transform.GetChild(0).transform.lossyScale.x * 0.5f;

        myRigidbody = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (isHooking && !hookLanded && !hookFailed)
        {
            HookProjectileMovementAndCollision();

        }
        else if (isHooking && hookLanded && !hookFailed)
        {
            HookingInteraction();
        }
        else if (hookFailed)
        {
            RetractHook();
        }
    }

    void HookProjectileMovementAndCollision()
    {
        Vector3 distance = transform.position - hookProjectile.transform.position;
        if (distance.magnitude > hookRange)
        {
            hookFailed = true;
            hookProjectile.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            //Debug.Log("MOVIENDO hacia " + shootDirection);
            hookProjectile.transform.position += shootDirection * hookProjectileSpeed * Time.fixedDeltaTime;

            RaycastHit2D hit = Physics2D.Raycast(hookProjectile.transform.position, shootDirection, 0.1f, ~LayerMask.GetMask("Player"));
            if (hit.collider != null && hit.collider.gameObject.GetComponent<IHookable>() == null)
            {
                hookFailed = true;
                hookProjectile.GetComponent<Collider2D>().enabled = false;
            }

            if (hit.collider != null && hit.collider.gameObject.GetComponent<IHookable>() != null)
            {
                //Debug.Log("ME CHOCO");
                hookLanded = true;
                hookedGameObject = hit.collider.gameObject;
                hookedRigidBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                hookedGameObject.GetComponent<IHookable>().Hooked();
                hookProjectile.AddComponent<FixedJoint2D>();
                hookProjectile.GetComponent<FixedJoint2D>().connectedBody = hookedRigidBody;
                hookedGameObject.layer = 9;

            }
        }
    }

    void RetractHook()
    {
        Vector2 direction = new Vector2(transform.position.x - hookProjectile.transform.position.x, transform.position.y - hookProjectile.transform.position.y);

        if (direction.magnitude < 1)
        {
            Destroy(hookProjectile);
            isHooking = false;
            shootDirection = Vector3.zero;
            hookFailed = false;
        }
        direction.Normalize();
        hookProjectile.GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y, 0) * hookProjectileSpeed;
    }

    void HookingInteraction()
    {
        Vector2 direction = new Vector2(transform.position.x - hookedGameObject.transform.position.x, transform.position.y - hookedGameObject.transform.position.y);

        if (direction.magnitude < distanceToUnhook)
        {
            isHooking = false;

            Time.timeScale = 1;

            hookedGameObject.GetComponent<IHookable>().Unhook();

            ResetAfterHooking();

        }
        else
        {
            direction.Normalize();
            hookElapsedTime += Time.deltaTime;
            hookPercentageComplete = hookElapsedTime / hookingDuration;

            Time.timeScale = hookingAnimationCurve.Evaluate(hookPercentageComplete);
            switch (hookedGameObject.GetComponent<IHookable>().getWeight())
            {
                case 0:
                    //myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                    hookedRigidBody.velocity = new Vector3(direction.x, direction.y, 0) * hookingSpeed;

                    break;
                case 1:
                    hookedRigidBody.velocity = new Vector3(direction.x, direction.y, 0) * hookingSpeed * 0.5f;
                    myRigidbody.velocity = new Vector3(-direction.x, -direction.y, 0) * hookingSpeed * 0.5f;
                    break;
                case 2:
                    myRigidbody.velocity = new Vector3(-direction.x, -direction.y, 0) * hookingSpeed;
                    break;
            }
        }
    }

    void ResetAfterHooking()
    {
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        hookedGameObject = null;
        hookedRigidBody = null;
        hookElapsedTime = 0;
        hookPercentageComplete = 0;
        hookLanded = false;

        Destroy(hookProjectile);
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


    public bool HookLanded()
    {
        return hookLanded;
    }

    public GameObject getHookedObject()
    {
        return hookedGameObject;
    }


}
