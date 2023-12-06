using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengibreStone : MonoBehaviour
{

	public EventHandler<GameObject> OnDestroy;
	[SerializeField] private int damage = 1;
	[SerializeField] private float fallingSpeedAtSpawn = 10;
	[SerializeField] private float speedRot = 10;

	float speed;
	Rigidbody2D myRigidbody2D;
	Collider2D myCollider2D;
	bool FirstSpawn = true;
	bool Attack;
	bool CanMove;
	[SerializeField] bool CanDamage;
	Vector3 Direction;


	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider2D = GetComponent<Collider2D>();
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!CanDamage) return;
		if (collision.gameObject.CompareTag("Player"))
		{
			HealthManager healthManager = collision.gameObject.GetComponent<HealthPlayerManager>();

			if (healthManager == null) return;

			Rigidbody2D rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
			if (rigidbody2D != null)
			{
				Vector2 contactPoint = rigidbody2D.ClosestPoint(transform.position);
				contactPoint = contactPoint - new Vector2(collision.transform.position.x, collision.transform.position.y);
				Vector3 damageContactPoint = new Vector3(damage, contactPoint.x, contactPoint.y);
				//if (!collision.gameObject.CompareTag("Player"))
				//    collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 7);
				healthManager.EventDamageTaken(this, damageContactPoint);
			}
			Debug.Log("STONEJENGIBRE:\tDestruido por tocar jugador");
			StoneDestroy();
		}

		if (FirstSpawn)
		{
			FirstSpawn = false;
			ResetState();
		} else
		{
			if (!collision.gameObject.CompareTag("Ground")) return;
            Debug.Log("STONEJENGIBRE:\tDestruido por colision");
			StoneDestroy();
		}
	}

	public void GoJengibre(Transform telekinesisTransform, float speed)
	{
        Debug.Log("STONEJENGIBRE:\tIr a jengibre");
		Attack = true;
		CanMove = true;
		myCollider2D.isTrigger = true;
		Direction = (telekinesisTransform.position - transform.position).normalized;
		this.speed = speed;
	}

	public void AttackPlayer(Transform player, float speed)
	{
        Debug.Log("STONEJENGIBRE:\tAtacar a jugador");
		myCollider2D.isTrigger = false;

		Attack = true;
		CanMove = true;
		CanDamage = true;
		Direction = (player.position - transform.position).normalized;
		this.speed = speed;
	}

	public void Protect()
	{
        Debug.Log("STONEJENGIBRE:\tProteger");
		myCollider2D.isTrigger = true;

		Attack = false;
		CanMove = true;
		CanDamage = true;

	}

	public void ResetState()
	{
		CanMove = false;
		CanDamage = false;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!CanMove) return;
		if(Attack) myRigidbody2D.velocity = Direction * speed;
		else transform.Rotate(Vector3.up, speedRot * Time.deltaTime);
	}
}
