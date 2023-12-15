using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyGoldHologram : IAFlyChase
{
	[SerializeField] private GameObject ownCoin;

	protected override void UpdatePath()
	{
		if (seeker.IsDone())
		{
			switch (tState)
			{
				case TState.CHASE:
					Chase();
					break;
				case TState.ATTACK:
					Scape();
					break;
			}
		}
	}

	void Chase()
	{
		seeker.StartPath(myRigidbody2D.position, playerObject.transform.position, OnPathComplete);
	}

	void Scape()
	{
		Vector3 myPosition = transform.position;
		Vector3 playerPosition = playerObject.transform.position;

		Vector3 direction = (myPosition - playerPosition).normalized + myPosition;
		seeker.StartPath(myRigidbody2D.position, direction, OnPathComplete);

	}

	private void CheckState()
	{
		if (Vector3.Distance(transform.position, playerObject.transform.position) < attackRange)
		{
			tState = TState.ATTACK;
		} else
		{
			tState = TState.CHASE;
		}
	}

	// Update is called once per frame
	protected override void FixedUpdate()
	{
		CheckState();
		IAWorking();
	}

	protected override void Die(object sender, GameObject gameObject)
	{
		isDead = true;
		myAnimator.SetBool("isDeadTimer", true);
	}

	public void DieNoCoin()
	{
        isDead = true;
        myAnimator.SetBool("isDead", true);
    }

	public void DisapearWithCoin()
	{
		gameObject.SetActive(false);
		ownCoin.SetActive(false);
	}

	public void DesactivateCoin()
	{
		ownCoin.SetActive(false);
	}
}
