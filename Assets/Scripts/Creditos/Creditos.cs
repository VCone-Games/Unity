using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DatabaseManager.Singleton.ChargeMetricsOnCloud();
        Invoke("WaitForEnd", 30);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void WaitToEnd()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
