using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;

    [SerializeField] private GameObject player;


    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(SceneObject scene, int EnterPoint)
    {

        player = GameObject.FindWithTag("Player");
        player.transform.position = scene.EnterPositions[EnterPoint];
        player.GetComponent<HorizontalMovement>().IsFacingRight = scene.isFacingRight[EnterPoint];
        //PANTALLA DE CARGA
        SceneManager.LoadScene(scene.sceneName);

    }




}
