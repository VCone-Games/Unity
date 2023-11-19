using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PhaseManagerAzafran : MonoBehaviour
{
	public EventHandler<int> summonFallingStone;

	[Header("Phase Components")]
    [SerializeField] private FirstPhaseAzafran firstPhaseComponent;
	[SerializeField] private Transform secondPhaseSpawn;
	[SerializeField] private float speed = 1f;
	[SerializeField] private float distanceToStart = 1f;
	
    [Header("Azafran Components")]
    [SerializeField] private HealthManagerAzafran healthManager;
    [SerializeField] private Animator myAnimator;

    [Header("Audio Management")]
    [SerializeField] private MusicManager musicManager;

    private Rigidbody2D myRigidbody2D;
	[SerializeField] private bool firstPhaseEnded = false;
	[SerializeField] private bool secondPhaseBegin = false;

	[SerializeField] private Transform endCombat;
	void SpawnDeadPoint()
	{
		transform.position = endCombat.position;
	}
	// Start is called before the first frame update
	void Start()
    {
		
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
		musicManager.PlayFirst();
    }

	void ActivateSecondPhase()
	{
		firstPhaseComponent.IsDisabled = true;
        
        myAnimator.SetBool("isTransicion", false);
		myAnimator.SetBool("isWalking", true);
		firstPhaseEnded = true;
		Debug.Log("Cambio de fase");


    }

	private void FixedUpdate()
	{
		bool condition = firstPhaseEnded && !secondPhaseBegin;
		if (condition)
		{

			Vector3 direction = (secondPhaseSpawn.position - transform.position);
			myRigidbody2D.velocity = direction.normalized * speed;
		}

		if (condition && Vector3.Distance(transform.position, secondPhaseSpawn.position) < distanceToStart)
		{
			Debug.Log("Activando segunda fase");
			Vector3 rotator = new Vector3(0, 0, 0);
			transform.rotation = Quaternion.Euler(rotator);

			myAnimator.SetBool("isWalking", false);
			myAnimator.SetBool("beginDigging", true);
			myRigidbody2D.gravityScale = 0.0f;
			gameObject.AddComponent<SecondPhaseAzafran>();
			secondPhaseBegin = true;
		}
	}

	void StartDiggingPhaseManager()
	{
		if (!secondPhaseBegin) return;
		myAnimator.SetBool("beginDigging", false);
		myAnimator.SetBool("isDigging", true);
	}
}
