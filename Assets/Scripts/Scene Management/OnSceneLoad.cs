using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoad : MonoBehaviour
{
    private GameObject gameManager;
    private PlayerInfo playerInfo;
    private GameObject playerObject;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("Game Manager");
        playerInfo = gameManager.GetComponent<PlayerInfo>();
        playerObject = GameObject.FindWithTag("Player");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (!playerInfo.Firstspawn)
        {
            playerInfo.Firstspawn = true;
            foreach (CheckpointSaver go in GameObject.FindObjectsOfType<CheckpointSaver>())
            {
                if (go.StartSpawnPoint)
                {
                    playerInfo.SetCheckpoint(SceneManager.GetActiveScene().name, go.spawnPoint.position, go.ID);
                    playerObject.transform.position = playerInfo.CheckPointPosition;
                    go.Initialize();
                    go.SpawnPlayer();
                }
            }

        }
        else if (playerInfo.IsDead)
        {
            playerInfo.IsDead = false;
            playerObject.GetComponent<HealthPlayerManager>().Restore();
            playerObject.transform.position = playerInfo.CheckPointPosition;
            foreach (CheckpointSaver go in GameObject.FindObjectsOfType<CheckpointSaver>())
            {
                if (go.ID == playerInfo.CheckpointID)
                {

                    go.Initialize();
                    go.SpawnPlayer();
                }
            }
        }
        else
        {
            playerObject.GetComponent<HealthPlayerManager>().CurrentHealth = playerInfo.CurrentHealth;
            playerObject.transform.position = playerInfo.SpawnPosition;
        }

        SetPowers();
        SetFootstepsAudio();
    }

    private void SetPowers()
    {
        playerObject.GetComponent<Jump>().MaxJumps = playerInfo.CanDoubleJump ? 2 : 1;
        playerObject.GetComponent<Dash>().DashUnlocked = playerInfo.CanDash;
        playerObject.GetComponent<WallGrab>().WallGrabUnlocked = playerInfo.CanWallGrab;
    }

    private void SetFootstepsAudio()
    {
        playerObject.GetComponent<PlayerSoundManager>().SetGroundMaterial(playerInfo.footstepsId);
    }
}
