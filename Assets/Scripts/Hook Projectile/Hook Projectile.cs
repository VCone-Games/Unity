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

    [Header("Other Variables")]
    [SerializeField] private Vector3 shootDirection;
    [SerializeField] private Vector3 distanceToPlayer;
    [SerializeField] private bool triggered;
    [SerializeField] private bool failed;

    [Header("Player Components")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Hook hookComponent;

    void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        playerTransform = playerGO.transform;
        hookComponent = playerGO.GetComponent<Hook>();
    }


    void FixedUpdate()
    {
        if (!triggered) return;

        distanceToPlayer = playerTransform.position - myTransform.position;

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
        if (distanceToPlayer.magnitude > hookingRange)
        {
            HookFailed();
            return;
        }
        myRigidbody.velocity = projectileSpeed * shootDirection;
    }

    private void RetractingMovement()
    {
        if (distanceToPlayer.magnitude < unhookDistance)
        {
            DestroyProjectile();
        }
        else
        {
            distanceToPlayer.Normalize();
            myRigidbody.velocity = projectileSpeed * distanceToPlayer;
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
        triggered = true;
        this.hookingRange = hookingRange;
        this.unhookDistance = unhookDistance;
        this.projectileSpeed = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!triggered) return;

        GameObject hookedObject = collision.gameObject;

        if (hookedObject.GetComponent<IHookable>() == null)
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
