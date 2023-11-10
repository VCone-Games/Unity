using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFly : SimpleFlyingBehaviour
{
	protected override void Attack()
	{
		Debug.Log("Attack state");
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player")) Attack();
	}

}
