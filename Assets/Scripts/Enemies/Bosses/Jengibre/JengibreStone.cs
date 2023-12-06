using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengibreStone : MonoBehaviour
{
	[SerializeField] private int damage = 1;
	[SerializeField] private float stoneVelocity = 3.0f;
	[SerializeField] private Transform playerTransform;
	[SerializeField] private Transform telekinesisTransform;
	

	Rigidbody2D myRigidbody2D;
	bool Initialized;
	bool CanDamage;
	bool IsOffensive;

	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!CanDamage) return;
		if (collision.CompareTag("Player"))
		{
			HealthManager healthManager = null;
			if (collision.CompareTag("Player")) healthManager = collision.GetComponent<HealthPlayerManager>();

			if (healthManager == null) return;

			Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
			if (rigidbody2D != null)
			{
				Vector2 contactPoint = rigidbody2D.ClosestPoint(transform.position);
				contactPoint = contactPoint - new Vector2(collision.transform.position.x, collision.transform.position.y);
				Vector3 damageContactPoint = new Vector3(damage, contactPoint.x, contactPoint.y);
				//if (!collision.gameObject.CompareTag("Player"))
				//    collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 7);
				healthManager.EventDamageTaken(this, damageContactPoint);
			}
		}
	}

	public void InitStoneMovement(Transform playerPos, Transform telekinesisPoint, bool attack)
	{
		Initialized = true;
		playerTransform = playerPos;
		telekinesisTransform = telekinesisPoint;
		IsOffensive = attack;
	}

	void MoveToTelekinesisPoint()
	{
		Vector3 direction = (telekinesisTransform.position - gameObject.transform.position).normalized;

		myRigidbody2D.velocity = direction * stoneVelocity;
	}

	void MoveToPlayer()
	{
		CanDamage = true;

		Vector3 direction = (playerTransform.position - gameObject.transform.position).normalized;

		myRigidbody2D.velocity = direction * stoneVelocity;
	}


	// Update is called once per frame
	void FixedUpdate()
	{
		if (!Initialized) return;
		if (IsOffensive)
		{
			float Distance = Vector3.Distance(telekinesisTransform.position, gameObject.transform.position);
			if (Distance < 0.5f)
			{

			} else
			{
				MoveToTelekinesisPoint();
			}

		} else
		{

		}
		

	}
}
