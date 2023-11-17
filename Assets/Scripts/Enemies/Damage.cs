using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Damage params")]
    [SerializeField] protected int damage;

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);

        HealthManager healthManager;
        if (collision.CompareTag("Player")) healthManager = collision.GetComponent<HealthPlayerManager>();
        else healthManager = collision.GetComponent<HealthManager>();

        if (healthManager == null) return;

        Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            Vector2 contactPoint = rigidbody2D.ClosestPoint(transform.position);
            contactPoint = contactPoint - new Vector2(collision.transform.position.x, collision.transform.position.y);
            Vector3 damageContactPoint = new Vector3(damage, contactPoint.x, contactPoint.y);
            //if (!collision.gameObject.CompareTag("Player"))
            //    collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 7);
            healthManager.EventDamageTaken(this, damageContactPoint);
        }
    }
}
