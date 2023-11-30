using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O_Volumen : MonoBehaviour
{
	[Header("UI elements")]
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private Toggle toggleMuteAudio;

	[Header("Global volumen")]
	[SerializeField] private AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
	{
		volumeSlider.value = PlayerPrefs.GetFloat("volumeAudio");
		audioSource.volume = volumeSlider.value;
	}

	public void ChangeSlider()
	{
		PlayerPrefs.SetFloat("volumeAudio", volumeSlider.value);
		audioSource.volume = volumeSlider.value;
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
			audioSource.volume = volumeSlider.value;
			
		}

	}
}
