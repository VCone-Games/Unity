using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet params")]
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private HealthManager healthManager;
    private bool facingRightFather;
    public bool FacingRight { get { return facingRightFather; } set { facingRightFather = value; } }

    private Rigidbody2D myRigidbody2D;

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
        {
            healthManager = collision.gameObject.GetComponent<HealthPlayerManager>();
            if (healthManager == null) return;

            Vector3 collisionPoint = collision.GetContact(0).point;
            
            healthManager.EventDamageTaken(this, damage);
        }
        Destroy(gameObject);
	}

	private void FixedUpdate()
	{
		if (facingRightFather)
        {
            myRigidbody2D.velocity = new Vector2(speed, 0);
        } else
        {
			myRigidbody2D.velocity = new Vector2(-speed, 0);
		}
	}

}
