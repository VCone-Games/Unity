using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	public static HealthUI HealthUISingleton;

	public Action EventAddHeartUI;
	public Action EventRemoveHeartUI;
	public EventHandler<HealthPlayerManager> EventInitialiteUI;

	[Header("External GameObjects")]
	[SerializeField] private Transform hearthList;
	[SerializeField] private GameObject playerGameObject;

	[Header("Prefab hearts")]
	[SerializeField] private GameObject prefab_hearth_full;

	[Header("Sprites")]
	[SerializeField] private Sprite hearth_sprite_full;
	[SerializeField] private Sprite hearth_sprite_empty;

	[Header("Health manager")]
	[SerializeField] private HealthPlayerManager healthManagerComponent;

	[Header("Control variables")]
	[SerializeField] private int currentHearth;
	[SerializeField] private int totalHearths;

	[Header("Control UI")]
	[SerializeField] private List<GameObject> AllHearthsGameObject;
	[SerializeField] private float offSetX = 90;
	[SerializeField] private float offSetY = 90;
	[SerializeField] private int maxHearthsPerRow = 10;

	[Header("Control variables")]
	[SerializeField] private int currentRow;
	[SerializeField] private int currentColumn;

	void Awake()
	{
		if (HealthUISingleton == null)
			HealthUISingleton = this;

		EventAddHeartUI += AddHearth;
		EventRemoveHeartUI += RemoveHearth;
		EventInitialiteUI += LateStart;
	}

	private void LateStart(object sender, HealthPlayerManager healthPlayerManager)
	{
		if (AllHearthsGameObject.Count > 0) ResetUI();
		healthPlayerManager.EventUpdateHealthUI += UpdateHealUI;
		int max_player_health = healthPlayerManager.MaxHealth;
		Debug.Log(max_player_health);
		for (int i = 0; i < max_player_health; i++)
		{
			Debug.Log("Añadiendo corazón...");
			GameObject currentHearth = Instantiate(prefab_hearth_full, hearthList);
			AllHearthsGameObject.Add(currentHearth);
			RectTransform currentRectTransform = currentHearth.GetComponent<RectTransform>();

			// Ajustar la posición en el eje X y eje Y basado en filas y columnas
			currentRectTransform.localPosition += new Vector3(offSetX * currentColumn, -offSetY * currentRow, 0);

			this.currentHearth++;

			currentColumn++;

			// Verificar si alcanzamos el límite de corazones por fila
			if (currentColumn >= maxHearthsPerRow)
			{
				currentRow++;
				currentColumn = 0;
			}
		}
		UpdateHealUI(this, healthPlayerManager.CurrentHealth);
	}

	private void ResetUI()
	{
		foreach(var hearth in AllHearthsGameObject)
		{
			Debug.Log("Destruyendo corazon en reset...");
			Destroy(hearth);
		}
		AllHearthsGameObject.Clear();
		currentRow = 0;
		currentColumn = 0;
	}

	void UpdateHealUI(object sender, int currentHealth)
	{
		Debug.Log("Updating heal...");
		for (int i = 0; i < AllHearthsGameObject.Count; i++)
		{
			Debug.Log("Updateo una vez");
			RawImage hearthImage = AllHearthsGameObject[i].GetComponent<RawImage>();

			if (i < currentHealth) hearthImage.texture = hearth_sprite_full.texture;
			else hearthImage.texture = hearth_sprite_empty.texture;
		}
	}

	void AddHearth()
	{
		if (currentHearth == totalHearths) return;

		if (currentColumn >= maxHearthsPerRow)
		{
			currentRow++;
			currentColumn = 0;
		}

		float xPos = offSetX * currentColumn;
		float yPos = -offSetY * currentRow;

		GameObject currentHearthObject = Instantiate(prefab_hearth_full, hearthList.transform);
		AllHearthsGameObject.Add(currentHearthObject);

		RectTransform currentRectTransform = currentHearthObject.GetComponent<RectTransform>();
		currentRectTransform.localPosition += new Vector3(xPos, yPos, 0);

		currentHearth++;
		currentColumn++;

		// Incrementar la vida máxima y actual del jugador
		healthManagerComponent.MaxHealth++;
		healthManagerComponent.CurrentHealth++;

		UpdateHealUI(this, healthManagerComponent.CurrentHealth);
	}

	void RemoveHearth()
	{
		if (currentHearth <= 0) return;

		currentHearth--;

		Destroy(AllHearthsGameObject[currentHearth]);
		AllHearthsGameObject.RemoveAt(currentHearth);

		// Decrementar la vida máxima y actual del jugador
		healthManagerComponent.MaxHealth--;
		if (healthManagerComponent.MaxHealth < healthManagerComponent.CurrentHealth)
			healthManagerComponent.CurrentHealth = healthManagerComponent.MaxHealth;

		if (currentColumn == 0)
		{
			currentRow--;
			currentColumn = maxHearthsPerRow - 1;
		}
		else
		{
			currentColumn--;
		}
		UpdateHealUI(this, healthManagerComponent.CurrentHealth);
	}
}
