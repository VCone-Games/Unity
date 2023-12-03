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
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent<Jump>().DisableJumpInput();
		player.GetComponent<HorizontalMovement>().DisableMovementInput();
		player.GetComponent<Hook>().DisableHookInput();
		player.GetComponent<WallGrab>().DisableWallGrabInput();
		player.GetComponent<Dash>().DisableDashInput();
		ShowMenu();
	}

    public void ShowMenu()
    {
		if (pauseUI.activeSelf)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			player.GetComponent<Jump>().EnableJumpInput();
			player.GetComponent<HorizontalMovement>().EnableMovementInput();
			player.GetComponent<Hook>().EnableHookInput();
			player.GetComponent<WallGrab>().EnableWallGrabInput();
			player.GetComponent<Dash>().EnableDashInput();

			//Si esta activada, la desactiva
			Time.timeScale = 1.0f;
			pauseUI.SetActive(false);
		}
		else
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			player.GetComponent<Jump>().DisableJumpInput();
			player.GetComponent<HorizontalMovement>().DisableMovementInput();
			player.GetComponent<Hook>().DisableHookInput();
			player.GetComponent<WallGrab>().DisableWallGrabInput();
			player.GetComponent<Dash>().DisableDashInput();

			//Si esta desactivada, la activa
			Time.timeScale = 0.0f;
			pauseUI.SetActive(true);
		}
	}

}
