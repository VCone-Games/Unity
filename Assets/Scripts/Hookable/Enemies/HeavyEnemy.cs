using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : MonoBehaviour, IHookable
{
    [Header("Player Components")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private GameObject playerGO;

    [Header("My Components")]
    [SerializeField] private Rigidbody2D myRigidbody;

    [Header("Parry variables")]
    [SerializeField] private bool isParried;
    [SerializeField] private bool parrying;
    [SerializeField] private float parryKnockbackTime;
    [SerializeField] public float parryKnockbackTimer;

    [Header("Other Variables")]
    [SerializeField] private bool isHooked;
    [SerializeField] private Vector3 vectorToPlayer;
    [SerializeField] private GameObject hookProjectile;
    [SerializeField] private float hookingSpeed;
    [SerializeField] private Vector3 parryDirection;


    void Start()
    {
        playerGO = GameObject.FindWithTag("Player");
        playerTransform = playerGO.transform;
        playerRigidbody = playerGO.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (parryKnockbackTimer > 0)
        {
            parryKnockbackTimer -= Time.fixedDeltaTime;
            if (parryKnockbackTimer < 0)
            {
                Debug.Log("UNHOOKING POR PARRY");
                gameObject.layer = 0;
                Unhook();
            }
        }
        else if (isHooked && !parrying)
        {
            HookingInteraction();
        }
    }

    private void ParryingAction()
    {
        playerGO.GetComponent<Parry>().parryEffects();

        playerRigidbody.velocity = parryDirection;
        Debug.Log("PARRIED: " + parryDirection);
        isParried = false;
        parrying = false;
        parryKnockbackTimer = parryKnockbackTime;
        gameObject.layer = 9;
        Destroy(hookProjectile);
    }

    private void HookingInteraction()
    {
        vectorToPlayer = playerTransform.position - transform.position;
        vectorToPlayer.Normalize();
        playerRigidbody.velocity = hookingSpeed * -vectorToPlayer;
    }

    public void Hooked(GameObject hookProjectile, float hoookingSpeed)
    {
        isHooked = true;
        this.hookProjectile = hookProjectile;
        this.hookingSpeed = hoookingSpeed;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Unhook()
    {
        if (!isHooked) return;
        Debug.Log("UNHOOKING EN METODO UNHOOK TU PUTA MADRE");
        if(hookProjectile != null)
        {
            hookProjectile.GetComponent<HookProjectile>().DestroyProjectile();
        }
        else
        {
            playerGO.GetComponent<Hook>().HookDestroyed();
        }
        hookProjectile = null;
        hookingSpeed = 0;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        isHooked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHooked) return;

        if (collision.gameObject.tag == "Player" && !isParried)
        {
            Unhook();
        }
        else if (collision.gameObject.tag == "Player" && isParried)
        {
            parrying = true;
            Debug.Log(parrying);
            ParryingAction();
        }
    }

    public void Parried(Vector3 direction, float knockbackTime)
    {
        parryDirection = direction;
        parryKnockbackTime = knockbackTime;
    }

    public void SetParried(bool parried)
    {
        isParried = parried;
    }
}
