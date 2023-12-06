using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public EventHandler<GameObject> EventDie;

    public EventHandler<Vector3> EventDamageTaken;

    [Header("Health params")]
    [SerializeField] protected int max_health;
    [SerializeField] protected int current_health;
    [SerializeField] protected bool canTakeDamage = true;

    [Header("Components")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Rigidbody2D myRigidbody;

    protected bool OnlyTakeDmgOnce;

    [SerializeField] protected float InvulnerabilityTime;
    protected float InvulnerabilityTimer;

    [SerializeField] protected Vector2 DamageKnockbackVector;
    [SerializeField] protected float DamageKnockbackForce;

    protected Vector2 auxKnockback;

    public int MaxHealth { get { return max_health; } set { max_health = value; } }
    public int CurrentHealth { get { return current_health; } set { current_health = value; } }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        current_health = max_health;
        EventDamageTaken += TakeDamage;
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void TakeDamage(object sender, Vector3 damageContactPoint)
    {
        if (!canTakeDamage || myAnimator.GetBool("isDamaging") || OnlyTakeDmgOnce)
        {
            Debug.Log("FUS ESTOY RECIBIENDO DAÑO");
            return;
        }
        canTakeDamage = false;
        myAnimator.SetBool("isAttacking", false);
        Debug.Log(gameObject + ": Me han dañado");

        myAnimator.SetTrigger("Damaged");

        // Implementa cómo el enemigo maneja el daño
        int objetiveHealth = current_health - (int)damageContactPoint.x;
        current_health = (objetiveHealth < 0) ? 0 : objetiveHealth;

        if (current_health <= 0)
        {
            EventDie?.Invoke(this, gameObject);
        }

        myAnimator.SetBool("isDamaging", true);

        if (!gameObject.CompareTag("Player"))
        {
            myRigidbody.GetComponent<Rigidbody2D>().velocity += new Vector2(3, 7);
            GetComponent<Enemy>().StopAttack();
        }

        myRigidbody.velocity = Vector2.zero;

        if (damageContactPoint.y <= 0)
        {
            auxKnockback = DamageKnockbackVector;
        }
        if (damageContactPoint.y > 0)
        {
            auxKnockback = new Vector2(-DamageKnockbackVector.x, DamageKnockbackVector.y);
        }
        myRigidbody.velocity = auxKnockback.normalized * DamageKnockbackForce;

        //COSAS DURANTE LA ANIMACION



    }

    public virtual void EndDamaging()
    {
        Debug.Log("Fin del daño. Soy: " + gameObject);
        myAnimator.SetBool("isDamaging", false);

        InvulnerabilityTimer = InvulnerabilityTime;

    }

    protected virtual void Update()
    {
        if (InvulnerabilityTimer > 0)
        {
            InvulnerabilityTimer -= Time.deltaTime;
            if (InvulnerabilityTimer < 0)
            {
                canTakeDamage = true;
                OnlyTakeDmgOnce = false;
            }
        }
    }

}
