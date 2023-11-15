using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarInteractable : MonoBehaviour
{
    [SerializeField] private Interact playerInteract;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            playerInteract.OnInteractableEnter(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            playerInteract.OnInteractableExit(interactable);
        }
    }
}
