using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perejil_RightHand : MonoBehaviour
{
	public EventHandler<int> EventAttack;
	[Header("Own components")]
	[SerializeField] private Animator myAnimator;

	enum TStateAttack { SLAM, STOMP }
	[Header("Attack state")]
	[SerializeField] private TStateAttack tStateAttack;

	public int TStateAttackLenght { get { return System.Enum.GetNames(typeof(TStateAttack)).Length; } }
	
	// Start is called before the first frame update
	void Start()
	{
		myAnimator = GetComponent<Animator>();
		EventAttack += Attack;
	}

	void Attack(object sender, int selAttack)
	{
		tStateAttack = (TStateAttack)selAttack;
		Debug.Log($"{gameObject}: Atacando con: {tStateAttack}");
		switch (tStateAttack)
		{
			case TStateAttack.SLAM:
				myAnimator.SetBool("isSlam", true);
				break;
			case TStateAttack.STOMP:
				myAnimator.SetBool("isStomp", true);
				break;
		}
	}

	void InAnimation_EndSlam()
	{
		myAnimator.SetBool("isSlam", false);
	}

	void InAnimation_EndStomp()
	{
		myAnimator.SetBool("isStomp", false);
	}
}
