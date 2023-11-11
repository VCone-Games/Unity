using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedChaseAndPatrol : IAGroundChaseAndPatrol
{
	protected override void Attack()
	{
		Debug.Log("Attack mode");
	}
}
