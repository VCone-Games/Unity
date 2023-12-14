using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
	[Header("UI Components")]
	[SerializeField] private InputField nameField;
	[SerializeField] private TMP_Dropdown ageDrowpdown;
	[SerializeField] private Toggle maleToggle;
	[SerializeField] private Toggle femToggle;
	[SerializeField] private Toggle otherToggle;

	[Header("Error texts")]
	[SerializeField] private GameObject errorName;
	[SerializeField] private GameObject errorGender;

	string gender;

	public void OnButtonPressed_StartGame()
	{

		bool validToggle = maleToggle.isOn || femToggle.isOn || otherToggle.isOn;
		if (!validToggle)
		{
			if (!errorGender.activeSelf) errorGender.SetActive(true);
			return;
		}
		if (nameField.text.Length == 0)
		{
			if (!errorName.activeSelf) errorName.SetActive(true);
			return;
		}

		int age = ageDrowpdown.value;
		LoadGame(age, gender);
		

	}

	public void INPUTFIELD_checkName()
	{
		if (nameField.text.Length > 0)
			if (errorName.activeSelf) errorName.SetActive(false);
	}

	public void INPUTFIELD_checkAge()
	{

	}

	public void OnTogglePressed_MaleToggle()
	{
		if (!maleToggle.isOn) return;
		femToggle.isOn = false; otherToggle.isOn = false;
		gender = "Masculino";
		if (errorGender.activeSelf) errorGender.SetActive(false);
	}
	public void OnTogglePressed_FemToggle()
	{
		if (!femToggle.isOn) return;
		maleToggle.isOn = false; otherToggle.isOn = false;
		gender = "Femenino";
		if (errorGender.activeSelf) errorGender.SetActive(false);
	}
	public void OnTogglePressed_OtherToggle()
	{
		if (!otherToggle.isOn) return;
		maleToggle.isOn = false; femToggle.isOn = false;

		if (errorGender.activeSelf) errorGender.SetActive(false);
	}


	void LoadGame(int age, string gender)
	{
		SceneManager.LoadScene("0. Tutorial");
		DatabaseMetrics dataBase = gameObject.AddComponent<DatabaseMetrics>();

		dataBase.Username = nameField.text;
		dataBase.Age = age;
		dataBase.Gender = gender;

		Destroy(this);
	}
}
