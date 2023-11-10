using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public abstract class EnemyFlyingBehaviour : EnemyBaseBehaviour
{
	[Header("Flying patrol params")]
	[SerializeField] private List<Transform> patrolPoints;
	[SerializeField] private int currentPatrolPoint;
	[SerializeField] private float patrolDistance;

	[Header("AI flying params")]
	[SerializeField] protected float nextWaypointDistance = 1.0f;
	[SerializeField] protected int currentWayPoint;
	[SerializeField] protected Path path;
	[SerializeField] protected Seeker seeker;
	[SerializeField] protected bool reachedEndOfPath;
	protected override void CheckChangeState()
	{

		isPlayerInSight = Vector3.Distance(transform.position, playerObject.transform.position) < visionRange;

		if (Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
		{
			ChangeState(TState.ATTACK);
		}
		else if (isPlayerInSight)
		{
			ChangeState(TState.CHASE);
		}
		else
		{
			ChangeState(TState.PATROL);
		}
	}

	void UpdatePath()
	{
		if (seeker.IsDone())
			switch (tState)
			{
				case TState.CHASE:
					Chase();
					break;

				case TState.PATROL:
					Patrol();
					break;
			}
	}

	protected override void Patrol()
	{
		seeker.StartPath(myRigidbody2D.position, patrolPoints[currentPatrolPoint].position, OnPathComplete);
	}
	protected override void Chase()
	{
		seeker.StartPath(myRigidbody2D.position, playerObject.transform.position, OnPathComplete);
	}

	// Start is called before the first frame update

	protected void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWayPoint = 0;
		}
	}

	protected virtual void FixedUpdate()
	{
		base.FixedUpdate();

		if (tState == TState.PATROL)
		{
			if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < patrolDistance)
			{
				int previousCurrentPatrolPoint = currentPatrolPoint;
				do
				{
					currentPatrolPoint = Random.Range(0, patrolPoints.Count);
				} while (previousCurrentPatrolPoint == currentPatrolPoint);
			}
		}

		if (tState == TState.ATTACK) return;

		IAPath();
		Flip();

	}

	void IAPath()
	{
		if (path == null) return;

		if (currentWayPoint >= path.vectorPath.Count)
		{
			reachedEndOfPath = true;
			return;
		}
		else
		{
			reachedEndOfPath = false;
		}

		Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - myRigidbody2D.position).normalized;

		myRigidbody2D.velocity = direction * moveSpeed;

		float distance = Vector2.Distance(myRigidbody2D.position, path.vectorPath[currentWayPoint]);
		if (distance <= nextWaypointDistance)
		{
			currentWayPoint++;
		}
	}

	void Flip()
	{
		if (myRigidbody2D.velocity.x >= 0.0f)
		{
			facingRight = true;
			mySpriteRenderer.flipX = false;
		}
		else
		{
			facingRight = false;
			mySpriteRenderer.flipX = true;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		seeker = GetComponent<Seeker>();

		InvokeRepeating("UpdatePath", 0f, .5f);
	}

}
