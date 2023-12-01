using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference interactReference;
    [SerializeField] private InputActionReference interactMOBILEReference;

    [SerializeField] private InputActionReference nextLineReference;
    [SerializeField] private InputActionReference nextLineMOBILEReference;


    [SerializeField] private bool MOBILE;

    private Jump jumpComponent;

    private IInteractable interactable;

    private void Update()
    {
        if (jumpComponent.IsGrounded && !interactReference.action.enabled)
        {
            interactReference.action.Enable();
        }
        else if (!jumpComponent.IsGrounded && interactReference.action.enabled)
        {
            interactReference.action.Disable();
        }

    }
    private void Start()
    {
        if (MOBILE)
        {
            interactMOBILEReference.action.performed += OnInteract;
            interactMOBILEReference.action.Disable();

            nextLineMOBILEReference.action.performed += OnInteract;
            nextLineMOBILEReference.action.Disable();
        }
        else
        {
            interactReference.action.performed += OnInteract;
            interactReference.action.Disable();

            nextLineReference.action.performed += OnInteract;
            nextLineReference.action.Disable();
        }

        jumpComponent = GetComponent<Jump>();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (interactable == null)
        {
            Debug.Log("No hay interactable cerca");
            return;
        }
        nextLineMOBILEReference.action.Enable();
        nextLineReference.action.Enable();

        interactable.Interact();
        GetComponent<Animator>().SetBool("Running", false);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void EndInteraction()
    {
        nextLineReference.action.Disable();
        nextLineMOBILEReference.action.Disable();
    }


    public void OnInteractableEnter(IInteractable collision)
    {
        Debug.Log("\t\tInteractuable cerca");
        interactable = collision;
        interactReference.action.Enable();
        interactable.InInteractionRange(true);

    }

    public void OnInteractableExit(IInteractable collision)
    {
        Debug.Log("\t\tInteractuable cerca");
        interactable = collision;
        interactReference.action.Disable();
        interactable.InInteractionRange(false);
        interactable = null;
    }
    private void OnDestroy()
    {
        interactReference.action.performed -= OnInteract;
        interactMOBILEReference.action.performed -= OnInteract;
    }
}
