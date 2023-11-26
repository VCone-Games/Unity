using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedCanon : IAGroundChaseAndPatrol
{
    [Header("Grounded canon params")]
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private Transform spawnAttackPoint;
    public void SpawnBullet()
    {
        GameObject bullet = Instantiate(prefabBullet, spawnAttackPoint.position, Quaternion.identity);
        bullet.GetComponent<BulletController>().PlayerObject = playerObject;
    }

	protected override void Attack()
	{
		base.Attack();
		Vector3 direction = (playerObject.transform.position - transform.position);
		facingRight = (direction.x > 0) ? true : false;
	}
}
