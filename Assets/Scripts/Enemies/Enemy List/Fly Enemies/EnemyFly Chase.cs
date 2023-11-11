using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyChase : IAFlyChase
{
	protected override void Attack()
	{
		Debug.Log("Attack mode");
	}
}
