using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispawnTemporalEnemies : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float DeathTimer = 5.0f;

    [Header("Components")]
    [SerializeField] private HealthManager healthManager;

    [Header("Control params")]
    [SerializeField] private float DeathTime;

	private void Start()
	{
		healthManager = GetComponent<HealthManager>();
		DeathTime = DeathTimer;
	}
	private void FixedUpdate()
	{
		DeathTime -= Time.deltaTime;

		if (DeathTime < 0)
		{
			healthManager.EventDamageTaken(this, 9999);
		}
	}
}
