using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAGroundChaseAndPatrol : MovementGroundedIA
{

	[SerializeField] protected float chaseTime = 3.0f;
	[SerializeField] protected float lostTime = 0.5f;
	protected enum TState { PATROL, CHASE, ATTACK }
	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected float visionRange;
	[SerializeField] protected float attackRange;

	[Header("Control ground IA chase")]
    
    [SerializeField] protected bool isPlayerInSight;
	[SerializeField] protected float chaseTimer;
	[SerializeField] protected float lostTimer;
	protected void Chase()
	{
		// Persigue al jugador
		chaseTimer -= Time.deltaTime;
		Vector3 destiny = playerObject.transform.position - gameObject.transform.position;

		if (destiny.x > 0)
		{
			myRigidbody2D.velocity = new Vector2(moveSpeed, myRigidbody2D.velocity.y);
			facingRight = true;
		}
		else
		{
			myRigidbody2D.velocity = new Vector2(-moveSpeed, myRigidbody2D.velocity.y);
			facingRight = false;
		}

		//Debug.Log("Chase state");
	}

	protected override void CheckState()
	{
		if (lostTimer > 0.0f)
		{
			lostTimer -= Time.deltaTime;
			return;
		}
		// Cacheo de variables
		bool isAttacking = myAnimator.GetBool("isAttacking");
		Vector3 playerPosition = playerObject.transform.position;

		// Determinar la dirección del raycast basado en la posición relativa del jugador
		Vector2 raycastDirection = (playerPosition.x < transform.position.x) ? Vector2.left : Vector2.right;

		// Raycast para detectar al jugador
		isPlayerInSight = Physics2D.Raycast(myCollider2D.bounds.center, raycastDirection,
							myCollider2D.bounds.extents.y + visionRange, playerLayer);


		// Si está atacando, salir temprano del método
		if (isAttacking || !canAttack) return;

		if (isPlayerInSight) chaseTimer = chaseTime;
		// Calcula la distancia al cuadrado para optimizar
		float distanceToPlayerSquared = (playerPosition - transform.position).magnitude;

		if (distanceToPlayerSquared <= attackRange)
		{
			// Si el jugador está dentro del rango de ataque, cambiar al estado de ataque
			tState = TState.ATTACK;
		}
		else if (isPlayerInSight || chaseTimer > 0.0f)
		{
			// Si el jugador está en la vista o se está persiguiendo, cambiar al estado de persecución
			tState = TState.CHASE;
		}
		else
		{
			// Si no, volver al estado de patrulla
			tState = TState.PATROL;
		}
		
		// Evitar cambiar a PATROL si no hay edgeDetector y está en estado CHASE
		if (!edgeDetector && tState == TState.CHASE)
		{
			tState = TState.PATROL;
			lostTimer = lostTime;
			chaseTimer = 0.0f;
		}
	}
	public override void StopAttack()
	{
		base.StopAttack();
		tState = TState.CHASE;
		Debug.Log("Cambiando a patrulla");
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
        if (isBeingHooked || isDead) return;

		
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
		}
	}
}
