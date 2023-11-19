using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingScenes : MonoBehaviour
{
    [SerializeField] SceneAsset SceneA;
    [SerializeField] SceneAsset SceneB;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SceneA.name);    
        Debug.Log(SceneB.name);
        Scene SceneC = SceneManager.GetActiveScene();

        if (SceneA.name == SceneC.name) Debug.Log("Son iguales");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
