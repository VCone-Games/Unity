using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyPatrolAndChase : IAFlyChaseAndPatrol
{
	protected override void Attack()
	{
		Debug.Log("Attack mode");
	}
}
