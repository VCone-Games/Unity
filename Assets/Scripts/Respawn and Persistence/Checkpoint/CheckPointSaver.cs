using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointSaver : AInteractable
{
    [SerializeField] private int checkpointId;
    [SerializeField] private Transform respawnPosition;
    [SerializeField] public Sprite OnSprite;
    [SerializeField] public Sprite OffSprite;


    protected override void Start()
    {
        base.Start();
        if(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<DataManager>().GetCheckPointId() == checkpointId)
        {
            playerGameObject.transform.position = respawnPosition.position;
            Interact();
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

      
        foreach(GameObject a in GameObject.FindGameObjectsWithTag("spawn"))
        {
                a.GetComponent<SpriteRenderer>().sprite = OffSprite;
      
        }

        GetComponent<SpriteRenderer>().sprite = OnSprite;

        EndInteraction();
    }
}
