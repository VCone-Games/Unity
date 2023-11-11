using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedPatrol : IAGroundPatrol
{
	protected override void Attack()
	{
		Debug.Log("Attack mode");
	}
}
