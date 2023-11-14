using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Damage params")]
    [SerializeField] private int damage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
        Debug.Log(collision.gameObject);

        HealthManager healthManager;
        if (collision.CompareTag("Player")) healthManager = collision.GetComponent<HealthPlayerManager>();
        else healthManager = collision.GetComponent<HealthManager>();

        if (healthManager == null) return;

        Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            Vector2 contactPoint = rigidbody2D.ClosestPoint(transform.position);
            healthManager.EventDamageTaken(this, damage);
		}
	}
}
