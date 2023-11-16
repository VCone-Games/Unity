using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStone : MonoBehaviour
{
	public Transform jugador;
	public float vel = 20f;
	Vector3 direccion;

	private void Start()
	{
		// Lanzar el objeto al jugador cuando se inicia el juego
		jugador = GameObject.FindGameObjectWithTag("Player").transform;

		direccion = (jugador.position - transform.position).normalized;
		Debug.Log(direccion);
	}

	private void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(vel, vel) * direccion;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Resultado de la colisión: " + collision.gameObject);
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("damage");
		}
		Destroy(gameObject);
	}
}
