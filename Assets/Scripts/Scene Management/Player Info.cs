using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;

    public bool Firstspawn;

    //CHECKPOINT MANAGEMENT
    private string sceneNameCheckpoint = "0. Tutorial";
    private Vector3 checkPointPosition = Vector3.zero;
    private bool isDead;
    private int checkpointID;

    //PLAYER POWERS
    private bool canDoubleJump = true;
    private bool canDash = true;
    private bool canWallGrab = true;

    //BETWEEN SCENES VARIABLES
    private int currentHealth;
    private string sceneNameObjective;
    private Vector3 spawnPosition;

    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }

    public Vector3 CheckPointPosition { get { return checkPointPosition; } set { checkPointPosition = value; } }
    public bool CanDash { get { return canDash; } set { canDash = value; } }
    public bool CanWallGrab { get { return canWallGrab; } set { CanWallGrab = value; } }
    public bool CanDoubleJump { get { return canDoubleJump; } set { canDoubleJump = value; } }

    public Vector3 SpawnPosition { get { return spawnPosition; } set { spawnPosition = value; } }
    public string CheckpointSceneName { get { return sceneNameCheckpoint; } }

    public int CheckpointID { get { return checkpointID; } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(string sceneName, Vector3 checkPointPosition, int checkpointID)
    {
        sceneNameCheckpoint = sceneName;
        this.checkPointPosition = checkPointPosition;
        this.checkpointID = checkpointID;
    }

    public void OnSceneChange(int currentHealth, string sceneName, Vector3 spawnPosition)
    {
        this.currentHealth = currentHealth;
        sceneNameObjective = sceneName;
        this.spawnPosition = spawnPosition;
    }

}
