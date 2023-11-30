using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class O_ShowOptions : MonoBehaviour
{
    [Header("UI Component")]
    [SerializeField] GameObject pauseUI;

    [Header("Input action")]
    [SerializeField] InputActionReference pauseReference;

    // Start is called before the first frame update
    void Start()
    {
		pauseReference.action.performed += OnPressed;
    }

	private void OnPressed(InputAction.CallbackContext context)
	{
		ShowMenu();
	}

    public void ShowMenu()
    {
		if (pauseUI.activeSelf)
		{
			//Si esta activada, la desactiva
			Time.timeScale = 1.0f;
			pauseUI.SetActive(false);
		}
		else
		{
			//Si esta desactivada, la activa
			Time.timeScale = 0.0f;
			pauseUI.SetActive(true);
		}
	}

}
