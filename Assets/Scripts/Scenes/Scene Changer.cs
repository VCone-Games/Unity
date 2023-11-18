using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    private bool SceneChanged;
    private SceneObject auxScene;
    private int auxEnterPoint;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(SceneObject scene, int EnterPoint)
    {
        auxScene = scene;
        auxEnterPoint = EnterPoint;
        SceneManager.LoadScene(scene.sceneName);
        SceneChanged = true;
       
    }

    private void Update()
    {
        if (SceneChanged)
        {
            player = GameObject.FindWithTag("Player");
            if(player != null)
            {
                player.transform.position = auxScene.EnterPositions[auxEnterPoint];
                Debug.Log(auxScene.EnterPositions[auxEnterPoint]);
                player.GetComponent<HorizontalMovement>().IsFacingRight = auxScene.isFacingRight[auxEnterPoint];
                Debug.Log(player.transform.position);
                SceneChanged = false;
            }
           
        }
    }


}
