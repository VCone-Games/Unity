using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWalkableObject : MonoBehaviour, IHookable
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

    [Header("Layers")]
    [SerializeField] private int normalLayer;
    [SerializeField] private int noPlayerLayer;


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
                gameObject.layer = normalLayer;
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
        playerGO.GetComponent<Parry>().parryEffects(parryDirection.x > 0);

        myRigidbody.velocity = parryDirection;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRigidbody.velocity = new Vector3(-parryDirection.x, parryDirection.y, parryDirection.z) * 0.25f;

        playerRigidbody.velocity += new Vector2(0, 8);
        
        isParried = false;
        parrying = false;
        parryKnockbackTimer = parryKnockbackTime;
        gameObject.layer = noPlayerLayer;
        Destroy(hookProjectile);
    }

    private void HookingInteraction()
    {
        vectorToPlayer = playerTransform.position - transform.position;
        vectorToPlayer.Normalize();
        myRigidbody.velocity = hookingSpeed * vectorToPlayer;
    }

    public void Hooked(GameObject hookProjectile, float hoookingSpeed)
    {
        isHooked = true;
        this.hookProjectile = hookProjectile;
        this.hookingSpeed = hoookingSpeed;

        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Unhook()
    {
        if (!isHooked) return;
  
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
    public bool IsParried()
    {
        return isParried;
    }
}
