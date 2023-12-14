using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramadeOro : LightHookable
{
    public float forceMultiplicator;
    public GameObject hologramaDeOro;
    public bool Stop;
    public override void Unhook()
    {
        if (!isHooked) return;

        //if (isParried)
        gameObject.layer = normalLayer;
        isParried = false;

        if (hookProjectile != null)
        {
            hookProjectile.GetComponent<HookProjectile>().DestroyProjectile();
        }
        else
        {
            playerGO.GetComponent<Hook>().HookDestroyed();
        }

        hookProjectile = null;
        hookingSpeed = 0;
        isHooked = false;
        if (enemyComponent != null) enemyComponent.SetUnhookTimer();

        timeStopped = false;
        playerGO.GetComponent<Interact>().enabled = true;
    }

    public override void Hooked(GameObject hookProjectile, float hookingSpeed)
    {
        hologramaDeOro.GetComponent<EnemyFlyGoldHologram>().DieNoCOin();
        GetComponent<SpriteRenderer>().enabled = true;
        base.Hooked(hookProjectile, hookingSpeed);
        Stop = true;
    }

    private void Update()
    {
        if (!isHooked && !Stop)
        {
            transform.position = hologramaDeOro.transform.position;
        }
    }

    protected  void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.GetComponent<Perejil_Head>() != null)
            Destroy(gameObject);
    }
}
