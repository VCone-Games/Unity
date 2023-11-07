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
	[SerializeField] protected float chaseTime;
	[SerializeField] protected float attackRange;

	[Header("Own Components")]
	[SerializeField] protected Rigidbody2D myRigidbody2D;
	[SerializeField] protected Collider2D myCollider2D;

	[Header("Control variables")]
	[SerializeField] protected bool isPlayerInSight = false;
	[SerializeField] protected float chaseTimer;

	protected abstract void Patrol();
	protected abstract void Attack();

	protected virtual void Chase()
	{
		// Persigue al jugador
		chaseTimer -= Time.deltaTime;
		Vector3 destiny = playerObject.transform.position - gameObject.transform.position;

		if (destiny.x > 0)
		{
			myRigidbody2D.velocity = new Vector2(moveSpeed, myRigidbody2D.velocity.y);
			facingRight = true;
		} else
		{
			myRigidbody2D.velocity = new Vector2(-moveSpeed, myRigidbody2D.velocity.y);
			facingRight = false;
		}

		Debug.Log("Chase state");
	}
	protected void ChangeState(TState newState)
	{
		tState = newState;
	}

	protected abstract void CheckChangeState();

	protected void TakeDamage(float damage)
	{
		// Implementa c�mo el enemigo maneja el da�o
		health -= damage;
		if (health <= 0)
		{
			ChangeState(TState.DIE);
		}
	}

	protected void Die()
	{
		// Implementa la l�gica de muerte del enemigo
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
