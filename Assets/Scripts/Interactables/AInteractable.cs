using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject interactionSprite;

    private GameObject playerGameObject;

    private HorizontalMovement horizontalMovementComponent;
    private Dash dashComponent;
    private Jump jumpComponent;
    private Hook hook;


    // Start is called before the first frame update
    void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        horizontalMovementComponent = playerGameObject.GetComponent<HorizontalMovement>();
        dashComponent = playerGameObject.GetComponent<Dash>();
        jumpComponent = playerGameObject.GetComponent<Jump>();
        hook = playerGameObject.GetComponent<Hook>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public virtual void Interact()
    {
        Debug.Log("INTERACTUANDO");
        horizontalMovementComponent.DisableMovementInput();
        dashComponent.DisableDashInput();
        jumpComponent.DisableJumpInput();
        hook.DisableHookInput();
    }

    public virtual void EndInteraction()
    {
        Debug.Log(" AAAAAA QUE PASASSA");
        horizontalMovementComponent.EnableMovementInput();
        dashComponent.EnableDashInput();
        jumpComponent.EnableJumpInput();
        hook.EnableHookInput();
    }

    public virtual void InInteractionRange(bool isTrue)
    {
        if (isTrue)
        {
            interactionSprite.SetActive(true);
        }
        else
        {
            interactionSprite.SetActive(false);
        }
    }

}
