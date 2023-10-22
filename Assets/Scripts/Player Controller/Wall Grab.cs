using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wallgrab : MonoBehaviour
{

	[Header("Input system")]
	[SerializeField] InputActionReference grabWallReference;

	[Header("Params")]
	[SerializeField] float distanceMax;

	[Header("Wall Layermask")]
	[SerializeField] LayerMask wallLayer;

	[Header("Control variables")]
	[SerializeField][ReadOnly] bool isGrabbingWall;
	[SerializeField][ReadOnly] bool wantsToGrabWall;
	[SerializeField][ReadOnly] bool isPressingWall;

	Rigidbody2D myRigidbody;
	Collider2D myCollider;
	Jump jumpScript;

	// Start is called before the first frame update
	void Start()
    {
        grabWallReference.action.performed += OnPressed;
        grabWallReference.action.canceled += OnReleased;

		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		jumpScript = GetComponent<Jump>();
    }

	private void OnPressed(InputAction.CallbackContext context)
	{
		wantsToGrabWall = true;
	}

	private void OnReleased(InputAction.CallbackContext context)
	{
		wantsToGrabWall = false;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		isPressingWall = Physics2D.Raycast(myCollider.bounds.center, Vector2.right, distanceMax + myCollider.bounds.extents.x, wallLayer)
			|| Physics2D.Raycast(myCollider.bounds.center, Vector2.left, distanceMax + myCollider.bounds.extents.x, wallLayer);

		if (!jumpScript.IsGrounded && wantsToGrabWall && isPressingWall)
		{
			myRigidbody.velocity = Vector2.zero;
			isGrabbingWall = true;
			GetComponent<Jump>().IsWalled = true;
		}
		else
		{
			isPressingWall = false;
			GetComponent<Jump>().IsWalled = false;

        }
    }


}
