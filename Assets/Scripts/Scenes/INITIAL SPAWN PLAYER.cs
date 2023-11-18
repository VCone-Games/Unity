using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INITIALSPAWNPLAYER : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 position;
    // Start is called before the first frame update
    private void Awake()
    {
        if (GameObject.FindWithTag("Player") != null) return;
        Instantiate(player, position, player.transform.rotation);
    }
}
