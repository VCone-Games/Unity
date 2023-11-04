using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O_Volumen : MonoBehaviour
{
	[Header("UI elements")]
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private GameObject imagenMute;

	[Header("Control variables")]
	[SerializeField] private float sliderValue;


	// Start is called before the first frame update
	void Start()
	{
		volumeSlider.value = PlayerPrefs.GetFloat("volumeAudio");
		AudioListener.volume = volumeSlider.value;
	}

	public void ChangeSlider(float value)
	{
		sliderValue = value;
		PlayerPrefs.SetFloat("volumeAudio", sliderValue);
		AudioListener.volume = volumeSlider.value;

		if (sliderValue != 0f) { imagenMute.SetActive(false); } else { imagenMute.SetActive(true); }
	}
}
