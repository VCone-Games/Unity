using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyChase : IAFlyChase
{
	[Header("Enemy chase params")]
	[SerializeField] private int damage;
	private HealthManager healthManager;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			healthManager = collision.gameObject.GetComponent<HealthPlayerManager>();
			if (healthManager == null) return;

			Vector3 collisionPoint = collision.GetContact(0).point;

			healthManager.EventDamageTaken(this, damage);
		}
	}
}
