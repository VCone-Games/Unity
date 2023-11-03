using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class O_ShowOptions : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string mainSceneName;

    [Header("UI Component")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject mainUI;

    [Header("Input action")]
    [SerializeField] InputActionReference pauseReference;

    // Start is called before the first frame update
    void Start()
    {
		mainSceneName = SceneManager.GetSceneByName("MainScene").name;

		pauseReference.action.performed += OnPressed;
    }

	private void OnPressed(InputAction.CallbackContext context)
	{
        if (mainSceneName == SceneManager.GetActiveScene().name) return;

		if (pauseUI.activeSelf)
        {
            Time.timeScale = 1.0f;
            pauseUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            pauseUI.SetActive(true);
        }
	}

    public void OnButtonPressed()
    {
        if (mainSceneName == SceneManager.GetActiveScene().name)
        {
            mainUI.SetActive(true);
            pauseUI.SetActive(false);
        } else
        {
			Time.timeScale = 1.0f;
			pauseUI.SetActive(false);
        }
    }

}
