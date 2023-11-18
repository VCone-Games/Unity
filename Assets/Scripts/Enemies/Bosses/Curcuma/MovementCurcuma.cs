using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementCurcuma : IAFlyPatrol
{
	private enum TStateAttack { CROW_A, CROW_B, CROW_C, ENERGY_CROW }
	[Header("State params")]
	[SerializeField] private TStateAttack tStateAttack;
	[SerializeField] private List<float> probAttacks;
	[SerializeField] private List<float> probAttacksSecondPhase;
	[SerializeField] private Dictionary<TStateAttack, float> stateDictionary;
	[SerializeField] private Dictionary<TStateAttack, float> stateDictionarySecondPhase;
	[SerializeField] private float attackTimer;
	[SerializeField] private float attackTimerSecondPhase;

	[Header("Spawn params")]
	[SerializeField] private List<Transform> spawnList;
	[SerializeField] private List<GameObject> spawnEnemiesPrefabs;
	[SerializeField] private Transform spawnPointCurcumaSkeleton;
	[SerializeField] private GameObject curcumaSkeletonPrefab;

	[Header("Control variables")]
	[SerializeField] private float attackTime;
	[SerializeField] private bool secondPhase = false;

	private GameObject curcumaSkeleton;
	protected override void Start()
	{
		base.Start();
		GetComponent<HealthManagerCurcuma>().EventSecondPhase += SecondPhaseActivation;

		stateDictionary = new Dictionary<TStateAttack, float>();
		stateDictionarySecondPhase = new Dictionary<TStateAttack, float>();

		InitializeStateDictionary(probAttacks, stateDictionary);
		InitializeStateDictionary(probAttacksSecondPhase, stateDictionarySecondPhase);

		seeker = GetComponent<Seeker>();
		InvokeRepeating("UpdatePath", 0f, 0.5f);
		InvokeRepeating("Attack", attackTimer, attackTimer);
	}
	private void InitializeStateDictionary(List<float> probabilities, Dictionary<TStateAttack, float> dictionary)
	{
		float cumulativeProbability = 0f;

		for (int i = 0; i < probabilities.Count; i++)
		{
			cumulativeProbability += probabilities[i];
			dictionary[(TStateAttack)i] = cumulativeProbability;
		}
	}

	protected override void Attack()
	{
		float accumulate = 0;
		List<float> list = (secondPhase) ? probAttacksSecondPhase : probAttacks;
		foreach (var probability in list)
		{
			accumulate += probability;
		}

		float selectAttack = (float)UnityEngine.Random.Range(0, accumulate*100) /100;
		Debug.Log(selectAttack);

		Dictionary<TStateAttack, float> dictonary = (secondPhase) ? stateDictionarySecondPhase : stateDictionary;
		foreach (var state in dictonary)
		{
			if (selectAttack < state.Value)
			{
				tStateAttack = state.Key;
				Attack(tStateAttack);
				break;
			}
		}
	}

	private void Attack(TStateAttack tState)
	{
		int selectedCrow = UnityEngine.Random.Range(0, spawnList.Count);
		Transform spawnPoint = spawnList[selectedCrow];
		switch (tState)
		{
			case TStateAttack.CROW_A:
				Debug.Log("Invocar crow A");
				SpawnCuervo(spawnPoint, spawnEnemiesPrefabs[0]);
				break;
			case TStateAttack.CROW_B:
				Debug.Log("Invocar crow B");
				SpawnCuervo(spawnPoint, spawnEnemiesPrefabs[1]);
				break;
			case TStateAttack.CROW_C:
				Debug.Log("Invocar crow C");
				SpawnCuervo(spawnPoint, spawnEnemiesPrefabs[2]);
				break;
			case TStateAttack.ENERGY_CROW:
				Debug.Log("Invocar energy crow");
				SpawnCuervo(spawnPoint, spawnEnemiesPrefabs[3]);
				break;
		}
	}

	private void SpawnCuervo(Transform posicion, GameObject prefab)
	{
		GameObject crow = Instantiate(prefab, posicion);
		crow.AddComponent<DispawnTemporalEnemies>();
	}

	private void SecondPhaseActivation(object sender, EventArgs e)
	{
		moveSpeed += 2;
		secondPhase = true;
		CancelInvoke("Attack");
		InvokeRepeating("Attack", 0f, attackTimerSecondPhase);

		curcumaSkeleton = Instantiate(curcumaSkeletonPrefab, spawnPointCurcumaSkeleton.position, Quaternion.identity);
	}

	protected override void Die(object sender, EventArgs e)
	{
		base.Die(sender, e);
		curcumaSkeleton.GetComponent<HealthManager>().EventDie?.Invoke(this, null); ;
	}

}
