using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementFlyIA : Enemy
{
	[Header("Player params")]
	[SerializeField] protected GameObject playerObject;
	[SerializeField] protected LayerMask playerLayer;

	[Header("AI flying params")]
	[SerializeField] protected float nextWaypointDistance = 1.0f;
	[SerializeField] protected int currentWayPoint;
	[SerializeField] protected Path path;
	[SerializeField] protected Seeker seeker;
	[SerializeField] protected bool reachedEndOfPath;


	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();
		seeker = GetComponent<Seeker>();
		playerObject = GameObject.FindGameObjectWithTag("Player");

		InvokeRepeating("UpdatePath", 0f, .5f);
	}

	// Update is called once per frame
	protected abstract void FixedUpdate();

	protected abstract void UpdatePath();
	protected void IAWorking()
	{
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
			Vector3 rotator = new Vector3(0, 0, 0);
			transform.rotation = Quaternion.Euler(rotator);
			facingRight = true;
		}
		else
		{
			Vector3 rotator = new Vector3(0, 180, 0);
			transform.rotation = Quaternion.Euler(rotator);
			facingRight = false;
		}
	}

	protected void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWayPoint = 0;
		}
	}
}
