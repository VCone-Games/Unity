using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataMetricBase : MonoBehaviour
{

    [SerializeField] GameObject nameCanvas;
	[SerializeField] InputField nameField;
	public string Username;
    public float TimeGame;
    public int DeadTimes;

    public bool finishTimer = false;
    public bool startTimer = false;

    // Update is called once per frame
    void Update()
    {
        if (!finishTimer && startTimer)
        {
			TimeGame = Time.realtimeSinceStartup;
		}   
    }

    public void OnButtonPressed()
    {
        nameCanvas.SetActive(true);
	}

    public void StartGameWithName()
    {
		Username = nameField.text;
		nameCanvas.SetActive(false);
        SceneManager.LoadScene("0. Tutorial");
        startTimer = true;
	}
}
