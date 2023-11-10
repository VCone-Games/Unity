using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedChase : IAGroundChase
{
	protected override void Attack()
	{
		Debug.Log("Attack mode");
	}
}
