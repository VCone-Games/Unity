using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManagerAzafran : MonoBehaviour
{
    [Header("Phase Components")]
    [SerializeField] private FirstPhaseAzafran firstPhaseComponent;
    [SerializeField] private SecondPhaseAzafran secondPhaseComponent;

    [Header("Azafran Components")]
    [SerializeField] private HealthManagerAzafran healthManager;

    // Start is called before the first frame update
    void Start()
    {
		healthManager.EventSecondPhase += ActivateSecondPhase;

		firstPhaseComponent.enabled = true;
		secondPhaseComponent.enabled = false;
	}

	private void ActivateSecondPhase(object sender, EventArgs e)
	{
        firstPhaseComponent.enabled = false;
        secondPhaseComponent.enabled = true;
	}
}
