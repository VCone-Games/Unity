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
	[SerializeField] private float nextWaypointDistance = 3.0f;
	[SerializeField] private int currentWayPoint;
	[SerializeField] private Path path;
	[SerializeField] private Seeker seeker;
	[SerializeField] private bool reachedEndOfPath;

	protected override void Chase()
	{
		seeker.StartPath(myRigidbody2D.position, playerObject.transform.position, OnPathComplete);
	}

	void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWayPoint = 0;
		}
	}

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

	protected override void Patrol()
	{
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

		seeker.StartPath(myRigidbody2D.position, patrolPoints[currentPatrolPoint].position, OnPathComplete);
	}

	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();
		seeker = GetComponent<Seeker>();

		InvokeRepeating("UpdatePath", 0f, .5f);
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

	// Update is called once per frame
	protected override void FixedUpdate()
	{
		if (path == null) return;
		CheckChangeState();
		if (tState == TState.ATTACK) return;

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



}
