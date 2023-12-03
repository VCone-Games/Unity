using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O_Brigth : MonoBehaviour
{
	[Header("UI elements")]
	[SerializeField] private Slider brightSlider;
	[SerializeField] private Image brightPanel;

	public float sliderValue;
	private float maxSliderValue;

	// Start is called before the first frame update
	void Start()
	{
		maxSliderValue = brightSlider.maxValue;
		sliderValue = PlayerPrefs.GetFloat("Bright", 0f);
		brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, sliderValue);
	}

	public void ChangeSlider()
	{
		float value = maxSliderValue - brightSlider.value;

		PlayerPrefs.SetFloat("Bright", value);
		brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, value);
		//Debug.Log("Transparencia: " + brightPanel.color);
	}
}
