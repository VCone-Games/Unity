using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class O_ReturnMainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private InputActionReference pauseReference;
    [SerializeField] private GameObject principalGameObject;
    [Header("UI")]
    [SerializeField] private GameObject ConfirmPanel;
    [SerializeField] private GameObject PausePanel;

    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;

	private void Start()
	{
        pauseReference.action.performed += OnEscPressed;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		/*// L�gica que se activa cuando el rat�n pasa por encima del bot�n
		Debug.Log("El rat�n est� encima del bot�n");
		Debug.Log(eventData);

		// Aqu� puedes activar eventos, cambiar propiedades, etc.
		// Ejemplo: tuBoton.onClick.Invoke(); // Activa el evento onClick del bot�n*/
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		//throw new NotImplementedException();
	}

	private void OnEscPressed(InputAction.CallbackContext context)
	{
		if (ConfirmPanel.activeSelf) ConfirmPanel.SetActive(false);
	}

	public void OnPressedMainMenuButton()
    {
        ConfirmPanel.SetActive(true);
    }

	public void OnCancelButton()
    {
        ConfirmPanel.SetActive(false);
    }

	public void OnReturnMenuButton()
    {
        SceneManager.LoadScene("-1. Main Menu");
        Time.timeScale = 1.0f;
        Destroy(principalGameObject);
    }

	
}
