using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyTetera : IAFlyPatrol
{
    [SerializeField] Transform spawnAttack;
    [SerializeField] GameObject prefabThunder;
    protected override void CheckState()
    {
        if(Physics2D.Raycast(myCollider2D.bounds.center, Vector2.down,
			myCollider2D.bounds.extents.y + attackRange, playerLayer))
        {
			tState = TState.ATTACK;
		}
    }

    private void SummonLighting()
    {
        Instantiate(prefabThunder, spawnAttack);
        Debug.Log("Spawn lighting");
    }
}
