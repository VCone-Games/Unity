using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyGroundedBehaviour : EnemyBaseBehaviour
{
	protected override void Patrol()
	{
		// Si llega al punto de patrulla, avanza al siguiente
		if (Vector3.Distance(transform.position, currentPatrolPoint.position) < patrolDistanceToChangePoint)
		{
			currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
			currentPatrolPoint = patrolPoints[currentPatrolIndex];
			Debug.Log("Cambiando patrol point");
		}

		Vector3 destiny = currentPatrolPoint.position - gameObject.transform.position;
		myRigidbody2D.velocity = (destiny.x >= 0) ? new Vector2(moveSpeed, myRigidbody2D.velocity.y) : new Vector2(-moveSpeed, myRigidbody2D.velocity.y);

		Debug.Log("Patrol state");
	}

	protected override void CheckChangeState()
	{
		isPlayerInSight = Physics2D.Raycast(myCollider2D.bounds.center, Vector2.left, myCollider2D.bounds.extents.y + visionRange, playerLayer) ||
			Physics2D.Raycast(myCollider2D.bounds.center, Vector2.right, myCollider2D.bounds.extents.y + visionRange, playerLayer);
		
		if (isPlayerInSight)
		{
			chaseTimer = chaseTime;
		}
		if (Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
		{
			ChangeState(TState.ATTACK);
		}
		else    // Si el jugador está dentro del rango de ataque, cambia a estado de ataque
		if (isPlayerInSight || chaseTimer > 0.0f)
		{
			ChangeState(TState.CHASE);
		}
		else
		{
			ChangeState(TState.PATROL);
		}
	}
}
