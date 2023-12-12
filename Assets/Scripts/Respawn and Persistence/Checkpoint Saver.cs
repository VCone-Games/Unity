using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheckpointSaver : AInteractable
{
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public bool StartSpawnPoint;
    [SerializeField] public int ID;
    public GameObject playerObject;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        Initialize();

    }
    public void Initialize()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        if (PlayerInfo.Instance.CheckpointID == ID && PlayerInfo.Instance.CheckpointSceneName == SceneManager.GetActiveScene().name && !StartSpawnPoint)
        {
            animator.SetBool("Active", true);
        }

    }
    public override void Interact()
    {
        base.Interact();
        PlayerInfo.Instance.SetCheckpoint(SceneManager.GetActiveScene().name, spawnPoint.position, ID);
        PlayerInfo.Instance.sceneMusicId = SceneChanger.Instance.zoneId;
        foreach (CheckpointSaver go in GameObject.FindObjectsOfType<CheckpointSaver>())
        {
            if(go != this)
            {
                go.animator.SetBool("Active", false);
            }
        }

        animator.SetBool("Active", true);
        EndInteraction();
    }

    public void SpawnPlayer()
    {
        playerObject.GetComponent<PlayerInput>().enabled = false;
        playerObject.GetComponent<SpriteRenderer>().enabled = false;
        playerObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("Spawn");
    }

    public void EndSpawn()
    {
        playerObject.GetComponent<PlayerInput>().enabled = true;
        playerObject.GetComponent<SpriteRenderer>().enabled = true;
        playerObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.SetBool("Active", true);
        animator.SetTrigger("EndSpawn");
    }

}
