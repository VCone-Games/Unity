using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengibreStone : MonoBehaviour
{

	public EventHandler<GameObject> OnDestroy;
	[SerializeField] private int damage = 1;
	[SerializeField] private float fallingSpeedAtSpawn = 5;
	[SerializeField] private float speedRot = 20;
	[SerializeField] private float normalGravity;
	[SerializeField] private float telekinesisGravity = 0;

	float speed;
	Rigidbody2D myRigidbody2D;

	[SerializeField] Collider2D myCollider2D;
	[SerializeField] Collider2D myTriggerCollider2D;

	bool FirstSpawn = true;
	bool GiveOrdered = false;
	bool Attack;
	bool CanMove;
	[SerializeField] bool CanDamage;
	Vector3 Direction;


	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		normalGravity = myRigidbody2D.gravityScale;

		Direction = Vector3.down;
		CanMove = true;
		CanDamage = true;
		Attack = true;
		speed = fallingSpeedAtSpawn;
	}


	void StoneDestroy()
	{
		OnDestroy?.Invoke(this, gameObject);
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!CanDamage || !other.gameObject.CompareTag("Player")) return;

		HealthPlayerManager healthManager = other.GetComponent<HealthPlayerManager>();
		if (healthManager == null) return;

		Rigidbody2D rigidbody2D = other.GetComponent<Rigidbody2D>();
		if (rigidbody2D != null)
		{
			Vector2 contactPoint = rigidbody2D.ClosestPoint(transform.position) - (Vector2)other.transform.position;
			Vector3 damageContactPoint = new Vector3(damage, contactPoint.x, contactPoint.y);
			healthManager.EventDamageTaken(this, damageContactPoint);
		}
		Debug.Log("STONEJENGIBRE:\t (triggerPlayer) HE TOCADO: {" + other.gameObject + "}");
		StoneDestroy();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (FirstSpawn)
		{
			FirstSpawn = false;
			ResetState();
		} else
		{
			if (!CanMove || (!Attack && collision.gameObject.CompareTag("Ground"))) return;
            Debug.Log("STONEJENGIBRE:\t (collision) HE TOCADO: {" + collision.gameObject + "}");
			StoneDestroy();
		}
	}

	public void GoJengibre(Transform telekinesisTransform, float speed)
	{
        Debug.Log("STONEJENGIBRE:\tIr a jengibre");
		Attack = true;
		CanMove = true;

		myCollider2D.enabled = false;
		myRigidbody2D.isKinematic = false;

		Direction = (telekinesisTransform.position - transform.position).normalized;
		this.speed = speed;
	}

	public void AttackPlayer(Transform player, float speed)
	{
		if (GiveOrdered) return;
		GiveOrdered = true;
        Debug.Log("STONEJENGIBRE:\tAtacar a jugador");
		myRigidbody2D.gravityScale = normalGravity;

		Attack = true;
		CanMove = true;

		myCollider2D.enabled = true;

		CanDamage = true;
		Direction = (player.position - transform.position).normalized;
		this.speed = speed;
	}

	public void Protect()
	{
        Debug.Log("STONEJENGIBRE:\tProteger");

		myCollider2D.enabled = true;
		myRigidbody2D.gravityScale = telekinesisGravity;

		Attack = false;
		CanMove = true;
		CanDamage = true;
	}

	public void ResetState()
	{
		myCollider2D.enabled = false;

		CanMove = false;
		CanDamage = false;
		myRigidbody2D.isKinematic = true;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!CanMove) return;
		if (Attack) myRigidbody2D.velocity = Direction * speed;
		else
		{
			transform.Rotate(Vector3.forward, speedRot);
			myRigidbody2D.velocity = Vector2.zero;
		}
	}
}
