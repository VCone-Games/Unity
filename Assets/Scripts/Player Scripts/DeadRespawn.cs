using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawnPoint.position;
        player.GetComponent<HealthPlayerManager>().Restore();

    }
}
