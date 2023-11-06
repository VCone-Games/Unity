using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseBehaviour : MonoBehaviour
{

    [Header("Player object")]
    [SerializeField] protected GameObject playerObject;

    [Header("Enemy params")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float health;
    [SerializeField] protected float damage;

	protected enum TState { PATROL, CHASE, ATTACK, DIE }
	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected List<Transform> patrolPoints;
	[SerializeField] protected float patrolDistanceToChangePoint;
	[SerializeField] protected float visionRange;
	[SerializeField] protected float attackRange;

	[Header("Own Components")]
	[SerializeField] protected Rigidbody2D myRigidbody2D;
	[SerializeField] protected Collider2D myCollider2D;

	protected abstract void Patrol();
    protected abstract void Attack();
	protected abstract void Chase();
	protected abstract void ChangeState(TState tState);
	protected abstract void TakeDamage(float damage);
	protected abstract void Die();

	public void Awake()
	{
		playerObject = GameObject.FindGameObjectWithTag("Player");
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider2D = GetComponent<Collider2D>();
	}

}
