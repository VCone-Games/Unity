using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perejil_Head : Enemy
{
	[Header("Perejil's body")]
	[SerializeField] private Perejil_LeftHand _leftHand;
	[SerializeField] private Perejil_RightHand _rightHand;

	enum TStateBody { LEFT_HAND, RIGHT_HAND }
	enum TStateSummon { HOLOGRAM, GOLD_HOLOGRAM }
	[Header("State params")]
	[SerializeField] private TStateBody tStateBody;
	[SerializeField] private TStateSummon tStateSummon;

	[Header("Summons prefab")]
	[SerializeField] private GameObject normalHologram;
	[SerializeField] private GameObject goldHologram;

	[Header("Perejil params")]
	[SerializeField] private float attackTime;
	[SerializeField] private float summonTime;
	[SerializeField] private List<Transform> summonPlaces;

	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();
		InvokeRepeating("Attack", attackTime, attackTime);
		InvokeRepeating("Summon", summonTime, summonTime);
	}

	protected override void Attack()
	{
		int numStatesBody = System.Enum.GetNames(typeof(TStateBody)).Length;
		tStateBody = (TStateBody)Random.Range(0, numStatesBody);
		Debug.Log("Invocando ataque: " + tStateBody);
		int selAttack;

		switch (tStateBody)
		{
			case TStateBody.LEFT_HAND:
				selAttack = Random.Range(0, _leftHand.TStateAttackLenght);
				_leftHand.EventAttack.Invoke(this, selAttack);
				break;
			case TStateBody.RIGHT_HAND:
				selAttack = Random.Range(0, _rightHand.TStateAttackLenght);
				_rightHand.EventAttack.Invoke(this, selAttack);
				break;
		}
	}

	private void Summon()
	{
		int numStatesSummon = System.Enum.GetNames(typeof(TStateSummon)).Length;
		tStateSummon = (TStateSummon)Random.Range(0, numStatesSummon);

		switch(tStateSummon)
		{
			case TStateSummon.HOLOGRAM:
				Debug.Log("Invocando holograma normal");
				break;
			case TStateSummon.GOLD_HOLOGRAM:
				Debug.Log("Invocando holograma dorado");
				break;
		}
	}
}
