using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStone : MonoBehaviour
{
	public float vel = 20f;

	// Start is called before the first frame update
	void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -vel);
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
