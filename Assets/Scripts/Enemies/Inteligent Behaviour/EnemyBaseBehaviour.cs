using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseBehaviour : Enemy
{

	[Header("Player params")]
	[SerializeField] protected GameObject playerObject;
	[SerializeField] protected LayerMask playerLayer;

	protected enum TState { PATROL, CHASE, ATTACK }
	[Header("State params")]
	[SerializeField] protected TState tState;
	[SerializeField] protected float visionRange;
	[SerializeField] protected float attackRange;


	[Header("Control variables")]
	[SerializeField] protected bool isPlayerInSight = false;

	protected abstract void Patrol();

	protected abstract void Chase();

	protected void ChangeState(TState newState)
	{
		tState = newState;
	}

	protected abstract void CheckChangeState();

	protected virtual void FixedUpdate()
	{
		CheckChangeState();
	}

	protected virtual void Awake()
	{
		base.Awake();
		playerObject = GameObject.FindGameObjectWithTag("Player");
	}
}
