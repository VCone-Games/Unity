using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

	[Header("Enemy params")]
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected bool facingRight = true;

	[Header("Own Components")]
	[SerializeField] protected Rigidbody2D myRigidbody2D;
	[SerializeField] protected Collider2D myCollider2D;
	[SerializeField] protected Animator myAnimator;

	protected virtual void Die(object sender, EventArgs e)
	{
		myAnimator.SetBool("isDead", true);
		myRigidbody2D.velocity = Vector2.zero;
		myRigidbody2D.isKinematic = false;
	}

	protected void Disappear()
	{
		Destroy(gameObject);
	}

	protected virtual void Attack()
	{
		myAnimator.SetBool("isAttacking", true);
		myRigidbody2D.velocity = Vector3.zero;
		myRigidbody2D.isKinematic = true;
		Debug.Log("Attack mode");
	}

	protected virtual void StopAttack()
	{
		myAnimator.SetBool("isAttacking", false);
		myRigidbody2D.isKinematic = false;
		Debug.Log("Fin del ataque");
	}

	protected virtual void Awake()
	{
		GetComponent<HealthManager>().EventDie += Die;
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider2D = GetComponent<Collider2D>();
		myAnimator = GetComponent<Animator>();
	}
}
