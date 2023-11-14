using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStone : MonoBehaviour
{
	public Transform jugador;
	public float vel = 20f;
	Vector3 direccion;

	public float gravityScale;
	public float alturaMaxima = 1f;

	private void Start()
	{
		// Lanzar el objeto al jugador cuando se inicia el juego
		jugador = GameObject.FindGameObjectWithTag("Player").transform;
		gravityScale = GetComponent<Rigidbody2D>().gravityScale;

		direccion = (jugador.position - transform.position).normalized;
	}

	private void FixedUpdate()
	{
		GetComponent<Rigidbody2D> ().velocity = (direccion.x >= 0)?
			new Vector2(vel, vel) * direccion :
			new Vector2(-vel, vel) * direccion;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("damage");
		}
		Destroy(gameObject);
	}
}
