using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : MonoBehaviour, IHookable
{
    [Header("Player Components")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D playerRigidbody;

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
        playerRigidbody = playerGO.GetComponent<Rigidbody2D>();
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
                playerRigidbody.velocity = hookingSpeed * -vectorToPlayer;
            }

        }
    }

    public void Hooked(float distanceToUnhook, GameObject hookProjectile, float hoookingSpeed)
    {
        isHooked = true;
        this.distanceToUnhook = distanceToUnhook;
        this.hookProjectile = hookProjectile;
        this.hookingSpeed = hoookingSpeed;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Unhook()
    {
        if (!isHooked) return;
        unhooking = false;
        hookProjectile.GetComponent<HookProjectile>().DestroyProjectile();
        distanceToUnhook = 0;
        hookProjectile = null;
        hookingSpeed = 0;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
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
