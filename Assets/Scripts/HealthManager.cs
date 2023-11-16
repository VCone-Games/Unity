using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public EventHandler EventDie;
    public EventHandler<Vector3> EventDamageTaken;

    [Header("Health params")]
    [SerializeField] protected int max_health;
    [SerializeField] protected int current_health;
    [SerializeField] protected bool canTakeDamage = true;

    [Header("Components")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Rigidbody2D myRigidbody;

    protected bool OnlyTakeDmgOnce;

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
            EventDie?.Invoke(this, null);
        }

        myAnimator.SetBool("isDamaging", true);

        //COSAS DURANTE LA ANIMACION



    }

    public virtual void EndDamaging()
    {
        myAnimator.SetBool("isDamaging", false);
        canTakeDamage = true;
        OnlyTakeDmgOnce = false;
    }

}
