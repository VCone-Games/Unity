using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToSafeGround : MonoBehaviour, IDataPersistance
{

    [SerializeField] private LayerMask whatIsCheckPoint;

    public event Action evento;

    public Vector2 SafeGroundLocation {  get; private set; } = Vector2.zero;

    private Collider2D coll;
    private float safeSpotYOffset;

    private void Start()
    {
        SafeGroundLocation = transform.position;
        coll = GetComponent<Collider2D>();
        safeSpotYOffset = (coll.bounds.size.y) / 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatIsCheckPoint.value & (1 << collision.gameObject.layer)) > 0)
        {
            //LLAMAR AL EVENTO DE CHECKPOINT
            evento.Invoke();

            GetComponent<RespawnScript>().OnReachCheckpoint();
            SafeGroundLocation = new Vector2(collision.bounds.center.x, collision.bounds.min.y + safeSpotYOffset);
            Debug.Log(SafeGroundLocation);
        }
    }

    public void WarpPlayerToSafeGround()
    {
        transform.position = SafeGroundLocation;
    }

    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }

}
