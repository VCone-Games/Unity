using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengibreUtilitySystemCalculator
{
	public enum CharacterAction
	{
		DROP_STONES,
		THROW_STONES,
		PROTECT,
		ENERGY_BALL,
		PHYSICAL_ATTACK, 
		IDLE
	}
	public CharacterAction action;

	private int stonesInGame;
	private float distanceToPlayer;
	private float playerHealthPercentage;
	private float ownHealthPercentage;
	private bool playerIsParrying;

	public int StonesInGame
	{
		get { return stonesInGame; }
		set { stonesInGame = value; }
	}
	public float DistanceToPlayer
	{
		get { return distanceToPlayer; }
		set { distanceToPlayer = value; }
	}
//	public int PlayerHealth
//	{
//		get { return playerHealth; }
//		set { playerHealth = value; }
//	}
//	public int OwnHealth
//	{
//		get { return ownHealth; }
//		set { ownHealth = value; }
//	}
	public bool PlayerIsParrying
	{
		get { return playerIsParrying; }
		set { playerIsParrying = value; }
	}

	public void SetChances(int stonesInGame, float distanceToPlayer,
		float playerHealthPercentage, float ownHealthPercentage, bool playerIsParrying)
	{
		this.stonesInGame = stonesInGame;
		this.distanceToPlayer = distanceToPlayer;

		this.playerHealthPercentage = playerHealthPercentage;
		this.ownHealthPercentage = ownHealthPercentage;
		this.playerIsParrying = playerIsParrying;
	}

	private float Calculate_DROP_STONES()
	{
		float chance = 0;

		return chance;
	}
	private float Calculate_THROW_STONES()
	{
		float chance = 0;

		return chance;
	}
	private float Calculate_PROTECT()
	{
		float chance = 0;

		return chance;
	}
	private float Calculate_ENERGY_BALL()
	{
		float chance = 0;

		return chance;
	}
	private float Calculate_PHYSICAL_ATTACK()
	{
		float chance = 0;

		return chance;
	}

	public CharacterAction ChooseBestAction()
	{

		return action;
	}
}
