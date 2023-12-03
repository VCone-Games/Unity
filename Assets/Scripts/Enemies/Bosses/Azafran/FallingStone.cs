using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class FallingStone : MonoBehaviour
{
	[Header("Sprites")]
	[SerializeField] private List<Sprite> spritesFalling;

	[Header("Components")]
	[SerializeField] private SpriteRenderer mySpriteRenderer;
	[SerializeField] private Rigidbody2D myRigidbody2D;

	public float fallingSpeed = 20f;
	public bool CanDamageBoss;
    public bool hooked;
    
    [SerializeField] private bool skinChanged;


	// Start is called before the first frame update
	void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myRigidbody2D.velocity = new Vector2(0, -fallingSpeed);
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

        if (!(collision.gameObject.CompareTag("Projectile") && GetComponent<LightHookable>().isHooked))
        {
            GetComponent<LightHookable>().Unhook();
            Destroy(gameObject);
        }
    }

	private void Update()
	{
		if (!skinChanged)
		{
			skinChanged = true;
			int selSprite = Random.Range(0, spritesFalling.Count);
			mySpriteRenderer.sprite = spritesFalling[selSprite];
		}
	}
}
