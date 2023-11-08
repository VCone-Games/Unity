using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseBehaviour : MonoBehaviour
{

	[Header("Player params")]
	[SerializeField] protected GameObject playerObject;
	[SerializeField] protected LayerMask playerLayer;

	[Header("Enemy params")]
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected bool facingRight;
	[SerializeField] protected SpriteRenderer mySpriteRenderer;
	[SerializeField] protected float health;
	[SerializeField] protected float damage;

	protected enum TState { PATROL, CHASE, ATTACK, DIE }
	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected float visionRange;
	[SerializeField] protected float attackRange;

	[Header("Own Components")]
	[SerializeField] protected Rigidbody2D myRigidbody2D;
	[SerializeField] protected Collider2D myCollider2D;

	[Header("Control variables")]
	[SerializeField] protected bool isPlayerInSight = false;

	protected abstract void Patrol();
	protected abstract void Attack();

	protected abstract void Chase();

	protected void ChangeState(TState newState)
	{
		tState = newState;
	}

	protected abstract void CheckChangeState();

	protected void TakeDamage(float damage)
	{
		// Implementa cómo el enemigo maneja el daño
		health -= damage;
		if (health <= 0)
		{
			ChangeState(TState.DIE);
		}
	}

	protected void Die()
	{
		// Implementa la lógica de muerte del enemigo
		Destroy(gameObject);
	}

	protected virtual void FixedUpdate()
	{
		CheckChangeState();

		switch (tState)
		{
			case TState.PATROL:
				Patrol();
				break;
			case TState.CHASE:
				Chase();
				break;
			case TState.ATTACK:
				Attack();
				break;
			case TState.DIE:
				Die();
				break;
		}
	}

	protected virtual void Awake()
	{
		playerObject = GameObject.FindGameObjectWithTag("Player");
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider2D = GetComponent<Collider2D>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}

}
