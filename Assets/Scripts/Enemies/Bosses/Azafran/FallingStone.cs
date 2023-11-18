using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class FallingStone : MonoBehaviour
{
	public float vel = 20f;
	public bool CanDamageBoss;
    public bool hooked;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -vel);
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!CanDamageBoss)
		{ 
            HealthPlayerManager playerHealth  = collision.gameObject.GetComponent<HealthPlayerManager>();
            if (playerHealth != null)
            {
                Rigidbody2D rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
                Vector2 contactPoint = rigidbody2D.ClosestPoint(transform.position);
                contactPoint = contactPoint - new Vector2(collision.transform.position.x, collision.transform.position.y);
                Vector3 damageContactPoint = new Vector3(1, contactPoint.x, contactPoint.y);
                playerHealth.EventDamageTaken(this, damageContactPoint);
                Destroy(gameObject);
            }
        }else
        {
            HealthManagerAzafran azafranHealth = collision.gameObject.GetComponent<HealthManagerAzafran>();
            if (azafranHealth != null)
            {
                azafranHealth.EventDamageTaken(this, new Vector3(1,0,0));
                Destroy(gameObject);
            }
        }

        if (!collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
		

	}
}
