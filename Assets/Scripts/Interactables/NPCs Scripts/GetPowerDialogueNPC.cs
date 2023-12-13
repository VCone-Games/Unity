using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPowerDialogueNPC : BaseDialogueNPC
{
    [Header("Power obtained")]
    [Tooltip("0. Wall grab.\n1. Dash.\n2. Double Jump.")]
    [SerializeField] private int power;
    [SerializeField] private int textObtained;

	public override void EndInteraction()
	{
        if (currentText == textObtained)
        {
			UpdatePower();
        }
		base.EndInteraction();
	}

	private void UpdatePower()
	{
		switch (power)
		{
			case 0:
				PlayerInfo.Instance.CanWallGrab = true;
				break;
			case 1:
				PlayerInfo.Instance.CanDash = true;
				break;
			case 2:
				PlayerInfo.Instance.CanDoubleJump = true;
				break;

		}

		GameObject.FindObjectOfType<OnSceneLoad>().SetPowers();
	}
}
