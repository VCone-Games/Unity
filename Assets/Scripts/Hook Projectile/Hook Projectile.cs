using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookProjectile : MonoBehaviour
{
    [Header("Projectile Parameters")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float hookingRange;
    [SerializeField] private float unhookDistance;

    [Header("Components")]
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private Transform myTransform;
    [SerializeField] private GameObject sprite;

    [Header("Other Variables")]
    [SerializeField] private Vector3 shootDirection;
    [SerializeField] private Vector3 distanceToHookGun;
    [SerializeField] private bool triggered;
    [SerializeField] private bool failed;

    [Header("Player Components")]
    [SerializeField] Transform hookGunTransform;
    [SerializeField] Hook hookComponent;

    void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        hookGunTransform = playerGO.transform.GetChild(0).transform;
        hookComponent = playerGO.GetComponent<Hook>();
    }


    void FixedUpdate()
    {
        if (!triggered) return;

        distanceToHookGun = hookGunTransform.position - myTransform.position;

        if (!failed)
        {       
            ShootingMovement();
        }
        else
        {
            RetractingMovement();
        }


    }

    private void ShootingMovement()
    {
        if (distanceToHookGun.magnitude > hookingRange)
        {
            HookFailed();
            return;
        }
        myRigidbody.velocity = projectileSpeed * shootDirection;
    }

    private void RetractingMovement()
    {
        if (distanceToHookGun.magnitude < unhookDistance)
        {
            DestroyProjectile();
        }
        else
        {
            distanceToHookGun.Normalize();
            myRigidbody.velocity = projectileSpeed * distanceToHookGun;
        }
    }

    private void HookFailed()
    {
        failed = true;
        GetComponent<Collider2D>().enabled = false;
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
        hookComponent.HookDestroyed();
    }

    public void Shoot(Vector3 direction, float hookingRange, float unhookDistance, float speed)
    {
        shootDirection = direction;
        transform.right = shootDirection;
        triggered = true;
        this.hookingRange = hookingRange;
        this.unhookDistance = unhookDistance;
        this.projectileSpeed = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!triggered) return;

        GameObject hookedObject = collision.gameObject;

        if (hookedObject.GetComponent<AHookable>() == null)
        {
            HookFailed();
            return;
        }
        else
        {
            triggered = false;
            hookedObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            hookComponent.SomethingHooked(hookedObject);
            gameObject.AddComponent<FixedJoint2D>();
            GetComponent<FixedJoint2D>().connectedBody = hookedObject.GetComponent<Rigidbody2D>();

        }
    }
}
