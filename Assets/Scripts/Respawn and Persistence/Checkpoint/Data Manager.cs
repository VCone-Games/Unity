using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string playerScene;
    [SerializeField] private int checkPointId;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SaveData( string sceneName, int checkPoint)
    {
        playerScene = sceneName;
        checkPointId = checkPoint;
    }

    public string GetSceneToLoad()
    {
        return playerScene;
    }

    public int GetCheckPointId()
    {
        return checkPointId;
    }


    public void DeathManager()
    {
       // Destroy(GameObject.FindWithTag("Player"));
        SceneManager.LoadScene(playerScene);
    }

}
