using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O_Volumen : MonoBehaviour
{
	[Header("UI elements")]
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private Toggle toggleMuteAudio;


	// Start is called before the first frame update
	void Start()
	{
		volumeSlider.value = PlayerPrefs.GetFloat("volumeAudio");
		AudioListener.volume = volumeSlider.value;
	}

	public void ChangeSlider(float value)
	{
		PlayerPrefs.SetFloat("volumeAudio", value);
		AudioListener.volume = value;
	}
	
	public void OnTogglePressed(bool pressed)
	{
		if (pressed)
		{
			// Se silencia el sonido en el juego
			AudioListener.volume = 0;
			
		}
		else if (!pressed)
		{
			// Se vuelve ha activar el sonido
			AudioListener.volume = volumeSlider.value;
			
		}

	}
}
