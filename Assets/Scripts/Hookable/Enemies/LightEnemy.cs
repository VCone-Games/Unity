using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemy : MonoBehaviour, IHookable
{
    [Header("Player Components")]
    [SerializeField] private Transform playerTransform;

    [Header("My Components")]
    [SerializeField] private Rigidbody2D myRigidbody;

    [Header("Other Variables")]
    [SerializeField] private bool isHooked;
    [SerializeField] private Vector3 vectorToPlayer;
    [SerializeField] private float distanceToUnhook;
    [SerializeField] private bool unhooking;
    [SerializeField] private GameObject hookProjectile;
    [SerializeField] private float hookingSpeed;


    void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        playerTransform = playerGO.transform;
    }


    void FixedUpdate()
    {
        if (isHooked)
        {
            if (unhooking)
            {
                Unhook();
            }
            else
            {
                vectorToPlayer = playerTransform.position - transform.position;
                vectorToPlayer.Normalize();
                myRigidbody.velocity = hookingSpeed * vectorToPlayer;
            }

        }
    }

    public void Hooked(float distanceToUnhook, GameObject hookProjectile, float hoookingSpeed)
    {
        isHooked = true;
        this.distanceToUnhook = distanceToUnhook;
        this.hookProjectile = hookProjectile;
        this.hookingSpeed = hoookingSpeed;
    }

    public void Unhook()
    {
        if (!isHooked) return;
        unhooking = false;
        hookProjectile.GetComponent<HookProjectile>().DestroyProjectile();
        distanceToUnhook = 0;
        hookProjectile = null;
        hookingSpeed = 0;
        isHooked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHooked) return;

        if (collision.gameObject.tag == "Player")
        {
            unhooking = true;
        }
    }
}
