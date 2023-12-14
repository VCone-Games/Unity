using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perejil_LeftHand : MonoBehaviour
{
	public EventHandler<int> EventAttack;
	[Header("Own components")]
	[SerializeField] private Animator myAnimator;

	enum TStateAttack { SWEEP }
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
			case TStateAttack.SWEEP:
				myAnimator.SetBool("isSweep", true);
				break;
		}
	}

	void InAnimation_EndSweep()
	{
		myAnimator.SetBool("isSweep", false);
	}

	public void Die()
	{
		myAnimator.SetBool("isDead", true);
	}

	public void Dissapear()
	{
		gameObject.SetActive(false);
	}
}
