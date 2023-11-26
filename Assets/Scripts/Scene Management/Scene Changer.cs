using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;


    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeSceneByMoving(SceneObject scene, int EnterPoint)
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerInfo.Instance.CanDoubleJump = true;
        PlayerInfo.Instance.CanDash = true;

        PlayerInfo.Instance.OnSceneChange(player.GetComponent<HealthPlayerManager>().CurrentHealth, scene.sceneName, scene.EnterPositions[EnterPoint]);

        //PANTALLA DE CARGA
        SceneManager.LoadScene(scene.sceneName);

    }

    public void ChangeSceneByDeath()
    {
        SceneManager.LoadScene(PlayerInfo.Instance.CheckpointSceneName);
    }




}
