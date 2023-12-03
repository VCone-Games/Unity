using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //GameData gameData = DataPersistenceManager.instance.GetGameData();

    [Header("Enemy params")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected bool facingRight = true;

    [Header("Own Components")]
    [SerializeField] protected Rigidbody2D myRigidbody2D;
    [SerializeField] protected Collider2D myCollider2D;
    [SerializeField] protected Animator myAnimator;

    [SerializeField] protected AHookable hookableComponent;

    [SerializeField] public bool isBeingHooked;

    [SerializeField] public float unhookTime;
    [SerializeField] public float unhookTimer;
    [SerializeField] public bool isDead;

    protected virtual void Die()
    {
        isDead = true;
        myAnimator.SetBool("isDead", true);
        // myRigidbody2D.velocity = Vector2.zero;
        myRigidbody2D.isKinematic = false;
        DataBase.Singleton.DeathCount++;
    }

    protected virtual void Disappear()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    protected virtual void Attack()
    {
        myAnimator.SetBool("isAttacking", true);
        myRigidbody2D.velocity = Vector3.zero;
        myRigidbody2D.isKinematic = true;
        //Debug.Log("Attack mode");
    }

    public virtual void StopAttack()
    {
        myAnimator.SetBool("isAttacking", false);
        myRigidbody2D.isKinematic = false;
        //Debug.Log("Fin del ataque");
    }

    protected virtual void Start()
    {
        GetComponent<HealthManager>().EventDie += Die;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
        hookableComponent = GetComponent<AHookable>();
    }

    protected virtual void FixedUpdate()
    {
        if(unhookTimer > 0)
        {
            unhookTimer -= Time.fixedDeltaTime;
            if (unhookTimer < 0)
            { 
                isBeingHooked = false;
                gameObject.layer = 11;
            }
        }
    }

    public void SetUnhookTimer()
    {
        unhookTimer = unhookTime;
    }
}
