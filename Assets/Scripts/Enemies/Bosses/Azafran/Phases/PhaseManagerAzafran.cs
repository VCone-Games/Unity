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
	
    [Header("Azafran Components")]
    [SerializeField] private HealthManagerAzafran healthManager;

	private Rigidbody2D myRigidbody2D;
	[SerializeField] private bool firstPhaseEnded = false;
	[SerializeField] private bool secondPhaseBegin = false;

    // Start is called before the first frame update
    void Start()
    {
		healthManager.EventSecondPhase += ActivateSecondPhase;
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void ActivateSecondPhase(object sender, EventArgs e)
	{
		Destroy(firstPhaseComponent);

		myRigidbody2D.isKinematic = false;
		firstPhaseEnded = true;
		Debug.Log("Cambio de fase");
	}

	private void FixedUpdate()
	{
		if (firstPhaseEnded && !secondPhaseBegin)
		{
			Debug.Log("Transicionando...");
			Vector3 direction = (secondPhaseSpawn.position - transform.position);
			myRigidbody2D.velocity = direction.normalized * speed;
		}

		if (firstPhaseEnded && !secondPhaseBegin && Vector3.Distance(transform.position, secondPhaseSpawn.position) < 0.5f)
		{
			gameObject.AddComponent<SecondPhaseAzafran>();
			secondPhaseBegin = true;
		}
	}
}
