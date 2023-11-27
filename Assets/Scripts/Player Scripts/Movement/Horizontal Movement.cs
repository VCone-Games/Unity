using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    public EventHandler<int> EventConfused;
    [Header("Is Disabled")]
    [SerializeField] private bool DISABLED;

    [Header("Input system")]
    [SerializeField] private InputActionReference movementActionReference;


    [Header("Movement params")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool moving;
    [SerializeField] private float movementDirection;
    [SerializeField] private bool facingRight = true;

    [Header("State debuff")]
    [SerializeField] private bool isConfused;
    [SerializeField] private float confuseTime;


    [Header("Player Component")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private Jump jumpComponent;
    [SerializeField] private Dash dashComponent;
    [SerializeField] private Hook hookComponent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject hookGun;
    [SerializeField] Vector3 initialGunPosition;
    [SerializeField] Vector3 invertedGunPosition;

    [Header("Animator Variables")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animationTimer;
    [SerializeField] private float animationCancelTime;

    [Header("Audio Management")]
    [SerializeField] private PlayerSoundManager soundManager;

    [Header("Camera Management")]
    [SerializeField] public CameraFollowObject cameraFollow;

    public bool IsFacingRight
    {
        get { return facingRight; }
        set { facingRight = value; }
    }

    void Start()
    {
        movementActionReference.action.performed += OnPressed;
        movementActionReference.action.canceled += OnRelease;

        initialGunPosition = hookGun.transform.localPosition;
        invertedGunPosition = initialGunPosition;
        invertedGunPosition.x = -invertedGunPosition.x;

        EventConfused += HittedWithConfused;

        cameraFollow = GameObject.FindWithTag("CameraFollow").GetComponent<CameraFollowObject>();

        // myRigidbody = GetComponent<Rigidbody2D>();
        //
        // jumpComponent = GetComponent<Jump>();
        // dashComponent = GetComponent<AirDash>();
        // hookComponent = GetComponent<Hook>();

    }

	private void HittedWithConfused(object sender, int timer)
	{
        confuseTime = timer;
        isConfused = true;
	}

	private void OnPressed(InputAction.CallbackContext context)
    {
        moving = true;
        movementDirection = movementActionReference.action.ReadValue<float>();
        movementDirection = (isConfused) ? -movementDirection : movementDirection;
        
        if (movementDirection < 0)
        {
            SpriteFlipManager(false);
        }
        if (movementDirection > 0)
        {
            SpriteFlipManager(true);
        }
    }

    public void SpriteFlipManager(bool isFacingRight)
    {
        facingRight = isFacingRight;
        cameraFollow.CallTurn(facingRight);
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        moving = false;
    }

    public void DisableMovementInput()
    {
        soundManager.StoptFootsteps();
        movementActionReference.action.Disable();
        DISABLED = true;
    }
    public void EnableMovementInput()
    {
        movementActionReference.action.Enable();
        DISABLED = false;
    }

    void ConfuseLogic()
    {
		if (isConfused)
		{
			confuseTime -= Time.deltaTime;
		}
		if (confuseTime < 0.0f)
		{
			isConfused = false;
		}
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        ConfuseLogic();

        if (facingRight)
        {
            Vector3 rotator = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(rotator);

        }
        else
        {
            Vector3 rotator = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotator);
            //spriteRenderer.flipX = true;
            //hookGun.transform.localPosition = invertedGunPosition;
        }

        if (DISABLED) return;
        if (moving)
        {
            if(jumpComponent.IsGrounded)
            {
                animator.SetBool("Running", true);
                animationTimer = animationCancelTime;
                soundManager.PlayFootsteps();
            }else
            {
                soundManager.StoptFootsteps();
            }
            myRigidbody.velocity = new Vector2(movementSpeed * movementDirection, myRigidbody.velocity.y);
            animator.SetBool("MovingAir", true);
            animationTimer = animationCancelTime;
        }
        else
        {
            soundManager.StoptFootsteps();
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);

        }
    }

    private void Update()
    {
        if (animationTimer > 0)
        {
            animationTimer -= Time.deltaTime;
            if (animationTimer < 0)
            {
                animator.SetBool("MovingAir", false);
                animator.SetBool("Running", false);

            }
        }
    }

    private void OnDestroy()
    {
        movementActionReference.action.performed -= OnPressed;
        movementActionReference.action.canceled -= OnRelease;
    }
}
