using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;
	[SerializeField] private InputActionReference showMetricsReference;

	private bool firstSpawn = false;
    [SerializeField] private int InitialHealthPlayer = 5;

	//CHECKPOINT MANAGEMENT
	private string sceneNameCheckpoint = "0. Tutorial";
    public int sceneMusicId;
    private Vector3 checkPointPosition = Vector3.zero;
    private bool isDead;
    private int checkpointID = -1;

    //PLAYER POWERS
    private bool canDoubleJump = false;
    private bool canDash = false;
    private bool canWallGrab = false;

    //BETWEEN SCENES VARIABLES
    private int maxHealth;
    private int currentHealth;
    private string sceneNameObjective;
    private Vector3 spawnPosition;
    public int footstepsId;
	public bool FirstSpawn { get { return firstSpawn; } set { firstSpawn = value; } }
	public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
	public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }

    public Vector3 CheckPointPosition { get { return checkPointPosition; } set { checkPointPosition = value; } }
    public bool CanDash { get { return canDash; } set { canDash = value; } }
    public bool CanWallGrab { get { return canWallGrab; } set { canWallGrab = value; } }
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
        maxHealth = InitialHealthPlayer;
        currentHealth = maxHealth;
		showMetricsReference.action.performed += OnMetricsPressed;

	}
	private void OnMetricsPressed(InputAction.CallbackContext context)
	{
		SceneManager.LoadScene("Creditos");
	}
	public void SetCheckpoint(string sceneName, Vector3 checkPointPosition, int checkpointID)
    {
        sceneNameCheckpoint = sceneName;
        this.checkPointPosition = checkPointPosition;
        this.checkpointID = checkpointID;
    }

    public void OnSceneChange(int currentHealth, int maxHealth,
        string sceneName, Vector3 spawnPosition, int footstepsId)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.sceneNameObjective = sceneName;
        this.spawnPosition = spawnPosition;
        this.footstepsId = footstepsId;
    }

}
