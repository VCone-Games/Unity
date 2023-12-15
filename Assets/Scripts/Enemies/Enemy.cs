using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
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

    protected virtual void Die(object sender, GameObject gameObject)
    {
        gameObject.layer = 9;
        isDead = true;
        myAnimator.SetBool("isDead", true);
        // myRigidbody2D.velocity = Vector2.zero;
        myRigidbody2D.isKinematic = false;
        DatabaseMetrics.Singleton.DefeatedEnemies++;

	}

    protected virtual void Disappear()
    {
		//DataBase.Singleton.DeathEnemies++;
        GameObject.FindWithTag("Player").GetComponent<HealthPlayerManager>().EventHealing.Invoke(this, 1);
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
                GetComponent<SpriteRenderer>().color = Color.white ;
                gameObject.layer = 11;
            }
        }
    }

    public void SetUnhookTimer()
    {
        unhookTimer = unhookTime;
        Color color = Color.white;
        color.a = 0.65f;
        GetComponent<SpriteRenderer>().color = color;
    }
}
