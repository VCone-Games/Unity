using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    // Referencia al enemigo eliminado para poder respawnearlo
    public GameObject enemyToRespawn;
    /* Referencia al jugador. Necesaria puesto que los enemigos respawneables
     van a suscribirse al metodo ReachCheckpoint para hacer el respawn */
    private GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // // Suscripcion al evento

        enemyToRespawn = transform.GetChild(0).gameObject;
    }

    // Que ocurre si el jugador alcanza el respawn
    public void OnReachCheckpoint()
    {
        // Script de respawn
        enemyToRespawn.SetActive(true);
       //Instantiate(enemyToRespawn);

        //REINICIAR VIDA enemyToRespawn.GetComponent<HealthManager>(). = enemyToRespawn.GetComponent<Enemy>().maxHealth;
        //ASGINAR BOOL ENEIMGOS enemyToRespawn.GetComponent<Enemy>().isDefeated = false;
        // Guardamos si llegamos a un punto de guardado
        DataPersistenceManager.instance.SaveGame();

        print("Vuelvo a respawnear");
    }
}
