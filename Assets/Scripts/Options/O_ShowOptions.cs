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
    [Header("Scenes")]
    [SerializeField] private SceneAsset mainScene;

    [Header("UI Component")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject mainUI;

    [Header("Input action")]
    [SerializeField] InputActionReference pauseReference;

    // Start is called before the first frame update
    void Start()
    {
		pauseReference.action.performed += OnPressed;
    }

	private void OnPressed(InputAction.CallbackContext context)
	{
        if (mainScene.name == SceneManager.GetActiveScene().name) return;

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

    public void OnButtonPressed()
    {
        if (mainScene.name == SceneManager.GetActiveScene().name)
        {
            //Estamos en la escena del menú, la primera, antes de empezar partida
            mainUI.SetActive(true);
            pauseUI.SetActive(false);
        } else
        {
            //Estamos jugando (no en la escena principal)
			Time.timeScale = 1.0f;
			pauseUI.SetActive(false);
        }
    }

}
