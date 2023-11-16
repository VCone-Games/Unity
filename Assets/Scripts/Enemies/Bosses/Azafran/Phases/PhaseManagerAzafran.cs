using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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

	private Rigidbody2D myRigidbody2D;
	[SerializeField] private bool firstPhaseEnded = false;
	[SerializeField] private bool secondPhaseBegin = false;

    // Start is called before the first frame update
    void Start()
    {
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}

	void ActivateSecondPhase()
	{
		Destroy(firstPhaseComponent);

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
			gameObject.AddComponent<SecondPhaseAzafran>();
			secondPhaseBegin = true;
		}
	}
}
