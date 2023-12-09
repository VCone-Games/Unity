using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet params")]
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private HealthManager healthManager;
    private GameObject playerObject;
    private Vector3 direction;

    private bool startedMoving = false;
    private bool parried;
    public GameObject PlayerObject { get { return playerObject; } set { playerObject = value; } }

    private Rigidbody2D myRigidbody2D;

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            healthManager = collision.gameObject.GetComponent<HealthPlayerManager>();
            if (healthManager == null) return;

            Vector3 collisionPoint = collision.GetContact(0).point;
            collisionPoint = collisionPoint - collision.transform.position;
            Vector3 damageContactPoint = new Vector3(damage, collisionPoint.x, collisionPoint.y);

            healthManager.EventDamageTaken(this, damageContactPoint);
        }
        if (!collision.gameObject.CompareTag("Projectile") && !GetComponent<LightHookable>().isHooked)
        {
            GetComponent<LightHookable>().Unhook();
            Destroy(gameObject);
        }
        if (GetComponent<LightHookable>().parryKnockbackTimer > 0f)
        {
            GetComponent<LightHookable>().Unhook();
            Destroy(gameObject);
        }


    }

    private void FixedUpdate()
    {
        if (GetComponent<LightHookable>().isHooked && !parried)
        {
            parried = true;
            return;
        }
        if (!startedMoving)
        {
            direction = playerObject.transform.position - transform.position;
            startedMoving = true;
        }

        if (!parried)
            myRigidbody2D.velocity = direction.normalized * speed;
    }

}
