using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlySimple : SimpleFlyingBehaviour
{
	[Header("Simple fly enemy params")]
    [SerializeField] private int damage;
	private HealthManager healthManager;

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			healthManager = collision.gameObject.GetComponent<HealthPlayerManager>();
			if (healthManager == null) return;

			Vector3 collisionPoint = collision.GetContact(0).point;
            collisionPoint = collisionPoint - collision.transform.position;
            Vector3 damageContactPoint = new Vector3(damage, collisionPoint.x, collisionPoint.y);

            healthManager.EventDamageTaken(this, damageContactPoint);
        }
	}

}
