using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INITIALSPAWNPLAYER : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPosition;

    private GameObject player;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Instantiate(playerPrefab, spawnPosition.position, playerPrefab.transform.rotation);
        }
        player = GameObject.FindWithTag("Player");
        player.GetComponent<HorizontalMovement>().cameraFollow = GameObject.FindGameObjectWithTag("CameraFollow").GetComponent<CameraFollowObject>();
        player.GetComponent<Hook>().aimRepresentation = GameObject.FindGameObjectWithTag("AimRepresentation");
        player.GetComponent<Parry>().aimRepresentation = GameObject.FindGameObjectWithTag("AimRepresentation");
    }
}
