using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference interactReference;

    [Header("InteractionCollider")]
    [SerializeField] private Collider2D interactionCollider;

    private Jump jumpComponent;

    private IInteractable interactable;

    private void Update()
    {
        if (jumpComponent.IsGrounded)
        {
            interactionCollider.enabled = true;
        }
        else
            interactionCollider.enabled = false;
    }
    private void Start()
    {
        interactReference.action.performed += OnInteract;
        interactReference.action.Disable();
        jumpComponent = GetComponent<Jump>();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactable.Interact();
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactReference.action.Enable();
            interactable.InInteractionRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactReference.action.Disable();
            interactable.InInteractionRange(false);
            interactable = null;
        }
    }
}
