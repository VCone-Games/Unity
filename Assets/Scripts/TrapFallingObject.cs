using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFallingObject : MonoBehaviour
{
	[Header("Player")]
	[SerializeField] private LayerMask playerMask;

	[Header("Sprites")]
    [SerializeField] private List<Sprite> spritesBase;
    [SerializeField] private List<Sprite> spritesFalling;

	[Header("Components")]
    [SerializeField] private SpriteRenderer mySpriteRenderer;
    [SerializeField] private Rigidbody2D myRigidbody2D;

	[Header("Falling params")]
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float distanceFall;
    [SerializeField] private bool playerFirstDetected;
    [SerializeField] private bool skinChanged;
    [SerializeField] private float damage;

	// Start is called before the first frame update
	void Start()
    {
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myRigidbody2D.isKinematic = true;
		int selSprite = Random.Range(0, spritesBase.Count);
		mySpriteRenderer.sprite = spritesBase[selSprite];
    }

	/*private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			HealthManager healthManager = collision.gameObject.GetComponent<HealthPlayerManager>();
			if (healthManager == null) return;

			Vector3 collisionPoint = collision.GetContact(0).point;
			collisionPoint = collisionPoint - collision.transform.position;
			Vector3 damageContactPoint = new Vector3(damage, collisionPoint.x, collisionPoint.y);

			healthManager.EventDamageTaken(this, damageContactPoint);
		}
		Destroy(gameObject);
	}*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthPlayerManager>();
            if (healthManager == null) return;

            Vector2 contactPoint = myRigidbody2D.ClosestPoint(transform.position);
            contactPoint = contactPoint - new Vector2(collision.transform.position.x, collision.transform.position.y);
            Vector3 damageContactPoint = new Vector3(damage, contactPoint.x, contactPoint.y);

            healthManager.EventDamageTaken(this, damageContactPoint);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		playerFirstDetected = Physics2D.Raycast(transform.position, Vector2.down, distanceFall, playerMask);
		if (playerFirstDetected)
		{
			playerFirstDetected = false;
			myRigidbody2D.velocity = new Vector2(0.0f, -fallingSpeed);
			if (!skinChanged)
			{
				skinChanged = true;
				int selSprite = Random.Range(0, spritesFalling.Count);
				mySpriteRenderer.sprite = spritesFalling[selSprite];
			}
		}
	}
}
