using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public EventHandler EventDie;

	[Header("Enemy params")]
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected bool facingRight = true;
	[SerializeField] protected SpriteRenderer mySpriteRenderer;
	[SerializeField] protected float damage;

	[Header("Own Components")]
	[SerializeField] protected Rigidbody2D myRigidbody2D;
	[SerializeField] protected Collider2D myCollider2D;

	protected virtual void Die(object sender, EventArgs e)
	{
		Debug.Log("Die");
		Dissapear();
	}

	protected void Dissapear()
	{
		Destroy(gameObject);
	}

	protected abstract void Attack();

	protected virtual void Awake()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider2D = GetComponent<Collider2D>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
}
