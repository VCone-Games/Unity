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

	// Start is called before the first frame update
	void Start()
	{
		sliderValue = PlayerPrefs.GetFloat("Bright", 0.5f);
		brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, brightSlider.value);
	}

	public void ChangeSlider(float valor)
	{
		sliderValue = valor;
		PlayerPrefs.SetFloat("Bright", sliderValue);
		brightPanel.color = new Color(brightPanel.color.r, brightPanel.color.g, brightPanel.color.b, brightSlider.value);
		//Debug.Log("Transparencia: " + brightPanel.color);
	}
}
