using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyEnergyCrow : IAFlyChase
{
	[Header("Crow energy params")]
	[SerializeField] private int confusedTime;
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			HorizontalMovement horizontalMovement = collision.gameObject.GetComponent<HorizontalMovement>();
			if (horizontalMovement == null) return;

			Vector3 collisionPoint = collision.GetContact(0).point;
			horizontalMovement.EventConfused(this, confusedTime);
		}
	}
}
