using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class O_FullScreen : MonoBehaviour
{

	[Header("UI elements")]
	[SerializeField] private Toggle toggleFullScreen;
	[SerializeField] private TMP_Dropdown dropdownResolutions;

	[Header("Control variables")]
	[SerializeField] private List<Resolution> resolutions;
	[SerializeField] private List<string> options;


	// Start is called before the first frame update
	void Start()
	{
		int value = PlayerPrefs.GetInt("FullScreen");

		toggleFullScreen.isOn = (value == 0) ? false : true;

		ResolutionsCheck();
	}

	public void OnTogglePressed(bool pressed)
	{
		if (pressed)
		{
			PlayerPrefs.SetInt("FullScreen", 1);
			Screen.fullScreen = true;
		}
		else
		{
			PlayerPrefs.SetInt("FullScreen", 0);
			Screen.fullScreen = false;
		}

	}

	public void ResolutionsCheck()
	{
		resolutions = Screen.resolutions.ToList();
		resolutions.Reverse();

		dropdownResolutions.ClearOptions();

		int currentResolution = 0;

		for (int i = 0; i < resolutions.Count - 1; i++)
		{
			int width = resolutions[i].width;
			int height = resolutions[i].height;

			if (width == resolutions[i + 1].width && height == resolutions[i + 1].height)
			{
				resolutions.RemoveAt(i + 1);
				i--;
			}
		}

		for (int i = 0; i < resolutions.Count; i++)
		{
			string opcion = resolutions[i].width + "x" + resolutions[i].height;
			options.Add(opcion);

			if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width
				&& resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolution = i;
			}
		}

		//options = options.Distinct().ToList();

		dropdownResolutions.AddOptions(options);
		dropdownResolutions.value = currentResolution;
		dropdownResolutions.RefreshShownValue();

		dropdownResolutions.value = PlayerPrefs.GetInt("Resolution", 0);

	}

	public void ChangeResolution(int index)
	{
		PlayerPrefs.SetInt("Resolution", index);

		Resolution resolution = resolutions[index];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}
}
