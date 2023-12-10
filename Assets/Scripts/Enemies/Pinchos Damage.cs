using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PinchosDamage : Damage
{
     private float warpTime = 0.4f;
    private float warpTimer;

    private GameObject player;

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        
    }
    protected  void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ME PINCHO");
        base.OnTriggerStay2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            warpTimer = warpTime;
        }else if(collision.gameObject.GetComponent<HealthManager>() != null)
        {
            collision.gameObject.GetComponent<HealthManager>().EventDie.Invoke(this, collision.gameObject);
        }
    }

    private void Update()
    {
        if (warpTimer > 0)
        {
            warpTimer -= Time.deltaTime;
            if(warpTimer < 0)
            {
                player.GetComponent<TeleportToSafeGround>().WarpPlayerToSafeGround();
            }
        }
    }
}
