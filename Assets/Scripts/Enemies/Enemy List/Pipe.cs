using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : EnemyBaseBehaviour
{
	private Transform currentPatrolPoint;
	private int currentPatrolIndex = 0;
	private bool isPlayerInSight = false;

	private void Start()
	{
		// Inicializa el primer punto de patrulla
		currentPatrolPoint = patrolPoints[currentPatrolIndex];
	}

	protected override void Patrol()
	{
		// Mueve al enemigo hacia el punto de patrulla actual
		float step = moveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, currentPatrolPoint.position, step);

		// Si llega al punto de patrulla, avanza al siguiente
		if (Vector3.Distance(transform.position, currentPatrolPoint.position) < patrolDistanceToChangePoint)
		{
			currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
			currentPatrolPoint = patrolPoints[currentPatrolIndex];
		}

		Vector3 destiny = currentPatrolPoint.position - gameObject.transform.position;
		myRigidbody2D.velocity = (destiny.x >= 0)? new Vector2(moveSpeed, myRigidbody2D.velocity.y) : new Vector2(-moveSpeed, myRigidbody2D.velocity.y);

		Debug.Log("Patrol state");
	}

	protected override void Attack()
	{
		// Realiza el comportamiento de ataque al jugador
		// Puedes implementar la lógica de ataque aquí

		Debug.Log("Attack state");
	}

	protected override void Chase()
	{
		// Persigue al jugador
		Vector3 destiny = playerObject.transform.position - gameObject.transform.position;
		myRigidbody2D.velocity = (destiny.x >= 0) ? new Vector2(moveSpeed, myRigidbody2D.velocity.y) : new Vector2(-moveSpeed, myRigidbody2D.velocity.y);

		Debug.Log("Chase state");
	}

	protected override void TakeDamage(float damage)
	{
		// Implementa cómo el enemigo maneja el daño
		health -= damage;
		if (health <= 0)
		{
			ChangeState(TState.DIE);
		}
	}

	protected override void Die()
	{
		// Implementa la lógica de muerte del enemigo
		Destroy(gameObject);
	}

	private void FixedUpdate()
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

	protected override void ChangeState(TState newState)
	{
		tState = newState;
	}

	private void CheckChangeState()
	{
		isPlayerInSight = (Vector3.Distance(transform.position, playerObject.transform.position) <= visionRange);

		if (isPlayerInSight)
		{
			ChangeState(TState.CHASE);
		} else	// Si el jugador está dentro del rango de ataque, cambia a estado de ataque
		if (Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
		{
			ChangeState(TState.ATTACK);
		} else
		{
			ChangeState(TState.PATROL);
		}

	}
}
