using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispawnTemporalEnemies : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float DeathTimer;

    [Header("Control params")]
    [SerializeField] private float DeathTime;

	private bool initialized;
	private void Start()
	{
		DeathTime = DeathTimer;
	}

	public void InitializeDeathTimer(float timer)
	{
		DeathTimer = timer;
		initialized = true;
	}
	private void FixedUpdate()
	{
		if (!initialized) return;

		DeathTime -= Time.deltaTime;

		if (DeathTime < 0)
		{
			Debug.Log("SOY: " + gameObject);
			GetComponent<Animator>().SetBool("isDeadTimer", true);
		}
	}
}
