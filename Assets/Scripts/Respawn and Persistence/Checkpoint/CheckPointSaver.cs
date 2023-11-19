using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointSaver : AInteractable
{
    [SerializeField] private int checkpointId;
    [SerializeField] private Transform respawnPosition;


    protected override void Start()
    {
        base.Start();
        if(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<DataManager>().GetCheckPointId() == checkpointId)
        {
            playerGameObject.transform.position = respawnPosition.position;
        }
    }

    public override void Interact()
    {
        base.Interact();

        if(GameObject.FindGameObjectWithTag("Game Manager") == null)
        {
            Debug.Log("no hay data manager");
        }else
        {
            Debug.Log("hay data manager");
        }
        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<DataManager>().SaveData(SceneManager.GetActiveScene().name, checkpointId);
        Debug.Log("GUARDANDO");
        EndInteraction();
    }
}
