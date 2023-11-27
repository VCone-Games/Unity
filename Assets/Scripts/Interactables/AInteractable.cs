using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject interactionSprite;
    [SerializeField] protected Animator myAnimator;

	protected GameObject playerGameObject;

	protected HorizontalMovement horizontalMovementComponent;
	protected Dash dashComponent;
	protected Jump jumpComponent;
	protected Hook hook;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        myAnimator = GetComponent<Animator>();
        horizontalMovementComponent = playerGameObject.GetComponent<HorizontalMovement>();
        dashComponent = playerGameObject.GetComponent<Dash>();
        jumpComponent = playerGameObject.GetComponent<Jump>();
        hook = playerGameObject.GetComponent<Hook>();
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
		myAnimator.SetBool("isTalking", false);

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
