using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHearthUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] HealthUI healthUI;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		healthUI.EventAddHeartUI?.Invoke();
	}
}
