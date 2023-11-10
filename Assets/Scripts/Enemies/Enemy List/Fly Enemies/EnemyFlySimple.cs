using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlySimple : SimpleFlyingBehaviour
{
	protected override void Attack()
	{
		Debug.Log("Attacked");
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player")) Attack();
	}
}
