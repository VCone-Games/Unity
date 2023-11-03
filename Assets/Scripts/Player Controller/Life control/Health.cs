using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    [Header("Health params")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [Header("UI gameobjects")]
    [SerializeField] private GameObject healthList;

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

        for (int i = 0; i < maxHealth; i++)
        {
            AllHearthsGameObject[i].SetActive(true);
		}

        currentHearth = maxHealth;
	}

    void AddHearth()
    {
        if (currentHearth == totalHearths) return;
        currentHearth++;
        AllHearthsGameObject[currentHearth].SetActive(true);

    }

    void HealOneHearth()
    {
        if (currentHealth == maxHealth) return;

        AllHearthsGameObject[currentHearth].GetComponent<RawImage>().color = Color.red;
		currentHealth++;

        Debug.Log("Healing...");
    }

    void DealDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            Debug.Log("Entrando...");
			if (currentHealth <= 0) return;

			AllHearthsGameObject[currentHealth - 1].GetComponent<RawImage>().color = Color.gray;
			currentHealth--;
            Debug.Log("Current health: " + currentHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            HealOneHearth();
		} 

        if (Input.GetKey(KeyCode.DownArrow))
        {
            DealDamage(1);
        }

        if (Input.GetKey(KeyCode.P))
        {
            AddHearth();
        }
    }
}
