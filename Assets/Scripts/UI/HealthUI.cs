using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	[Header("UI gameobjects")]
	[SerializeField] private GameObject healthList;

	[Header("Health manager")]
	[SerializeField] private HealthPlayerManager healthManagerComponent;

	[Header("Control variables")]
	[SerializeField] private int currentHearth;
	[SerializeField] private int totalHearths;

	[Header("Control UI")]
	[SerializeField] private List<GameObject> AllHearthsGameObject;

	// Start is called before the first frame update
	void Start()
    {
		totalHearths = 0;

		for (int i = 0; i < healthList.transform.childCount; i++)
		{
			GameObject currentHearth = healthList.transform.GetChild(i).gameObject;
			AllHearthsGameObject.Add(currentHearth);
			totalHearths++;
		}

		for (int i = 0; i < healthManagerComponent.MaxHealth; i++)
		{
			AllHearthsGameObject[i].SetActive(true);
			currentHearth++;
            RawImage hearthColor = AllHearthsGameObject[i].GetComponent<RawImage>();
			hearthColor.color = Color.red;
        }

		healthManagerComponent.EventUpdateHealthUI += UpdateHealUI;
	}

	void UpdateHealUI(object sender, int currentHealth)
	{
		for (int i = 0; i < AllHearthsGameObject.Count; i++)
		{
			RawImage hearthColor = AllHearthsGameObject[i].GetComponent<RawImage>();

			if (i < currentHealth) hearthColor.color = Color.red;
			else hearthColor.color = Color.gray;
		}
	}

	void AddHearth()
	{
		if (currentHearth == totalHearths) return;
		currentHearth++;
		healthManagerComponent.MaxHealth++;

		AllHearthsGameObject[currentHearth].SetActive(true);
	}

	void RemoveHearth()
	{
		if (currentHearth <= 0) return;

		currentHearth--;
		healthManagerComponent.MaxHealth--;

		AllHearthsGameObject[currentHearth].SetActive(false);
	}


}
