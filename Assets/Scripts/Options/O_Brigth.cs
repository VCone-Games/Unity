using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O_Brigth : MonoBehaviour
{
	[Header("UI elements")]
	[SerializeField] private Slider brightSlider;
	[SerializeField] private Image brightPanel;

	[Header("Control variables")]
	[SerializeField] private float sliderValue;
	[SerializeField] private float maxValue;

	// Start is called before the first frame update
	void Start()
	{
		maxValue = brightSlider.maxValue;
		brightSlider.value = PlayerPrefs.GetFloat("Bright");
		brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, brightSlider.value);
	}

	public void ChangeSlider(float value)
	{
		sliderValue = (maxValue - value);
		PlayerPrefs.SetFloat("Bright", sliderValue);
		brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, sliderValue);
	}
}
