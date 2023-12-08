using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AHookable : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected Rigidbody2D playerRigidbody;
    [SerializeField] protected Parry parryComponent;
    [SerializeField] protected Collider2D playerCollider;
    [SerializeField] protected GameObject playerGO;
    [SerializeField] protected Transform hookGun;

    [Header("My Components")]
    [SerializeField] protected Rigidbody2D myRigidbody;
    [SerializeField] protected Collider2D myCollider;
    [SerializeField] protected Enemy enemyComponent;

    [Header("Parry variables")]
    [SerializeField] public bool isParried;
    [SerializeField] protected bool parrying;
    [SerializeField] protected float parryKnockbackTime;
    [SerializeField] public float parryKnockbackTimer;

    [Header("Parry Time Stop")]
    [SerializeField] private float stopTimeDistance;
    [SerializeField] private float timeScale;
    [SerializeField] private float timeScaleRecoveryRatio;

    [Header("Other Variables")]
    [SerializeField] public bool isHooked;
    [SerializeField] protected Vector3 vectorToHookGun;
    [SerializeField] protected GameObject hookProjectile;
    [SerializeField] protected float hookingSpeed;
    [SerializeField] protected Vector3 parryDirection;
    [SerializeField] protected bool timeStopped;

    [SerializeField] public float failedParryTimer;

    [Header("Layers")]
    [SerializeField] protected int normalLayer;
    [SerializeField] protected int noPlayerLayer;

    protected virtual void Start()
    {
        playerGO = GameObject.FindWithTag("Player");
        playerTransform = playerGO.transform;
        playerRigidbody = playerGO.GetComponent<Rigidbody2D>();
        hookingSpeed = playerGO.GetComponent<Hook>().hookingSpeed;
        parryComponent = playerGO.GetComponent<Parry>();
        playerCollider = playerGO.GetComponent<Collider2D>();
        enemyComponent = GetComponent<Enemy>();

        hookGun = playerGO.transform.GetChild(0).transform;

        stopTimeDistance = parryComponent.stopTimeDistance;
        timeScale = parryComponent.timeScale;
        timeScaleRecoveryRatio = parryComponent.timeScaleRecoveryRatio;
    }


    protected virtual void FixedUpdate()
    {
        if(failedParryTimer > 0)
        {
            parryKnockbackTimer -= Time.fixedDeltaTime;
        }

        if (parryKnockbackTimer > 0)
        {
            parryKnockbackTimer -= Time.fixedDeltaTime;
            if (parryKnockbackTimer < 0)
            {
                Unhook();
            }
        }
        else if (isHooked && !parrying)
        {
            HookingInteraction();
        }
    }


    protected virtual void ParryingAction()
    {
        playerGO.GetComponent<Parry>().parryEffects(parryDirection.x > 0);

        parrying = false;
        parryKnockbackTimer = parryKnockbackTime;
        gameObject.layer = noPlayerLayer;
        Destroy(hookProjectile);
        playerGO.GetComponent<Interact>().enabled = false;
    }

    protected virtual void HookingInteraction()
    {
        vectorToHookGun = hookGun.position - transform.position;
        if (Physics2D.Distance(myCollider, playerCollider).distance < stopTimeDistance && !timeStopped)
        {
            TimeStop.instance.StopTime(timeScale, timeScaleRecoveryRatio, stopTimeDistance / hookingSpeed + 0.2f);
            timeStopped = true;
        }
        vectorToHookGun.Normalize();
    }

    public virtual void Hooked(GameObject hookProjectile, float hoookingSpeed)
    {
        isHooked = true;
        if (enemyComponent != null)
        {
            enemyComponent.isBeingHooked = true;
            enemyComponent.StopAttack();
        }

        this.hookProjectile = hookProjectile;
        this.hookingSpeed = hoookingSpeed;
    }

    public virtual void Unhook()
    {
        if (!isHooked) return;


        //if (isParried)
        gameObject.layer = normalLayer;
        isParried = false;

        if (hookProjectile != null)
        {
            hookProjectile.GetComponent<HookProjectile>().DestroyProjectile();
        }
        else
        {
            playerGO.GetComponent<Hook>().HookDestroyed();
        }

        myRigidbody.velocity *= new Vector3(0.75f, 1, 1);
        hookProjectile = null;
        hookingSpeed = 0;
        isHooked = false;
        if (enemyComponent != null) enemyComponent.SetUnhookTimer();

        timeStopped = false;
        playerGO.GetComponent<Interact>().enabled = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHooked) return;

        if (collision.gameObject.tag == "Player")
        {
            //closeParryTimer = closeParryTime;
            if (!isParried)
            {
                Unhook();
            }
            else if (isParried)
            {
                parrying = true;
                ParryingAction();
                Debug.Log("parreao");
            }
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

    public bool IsParried()
    {
        return isParried;
    }
}
