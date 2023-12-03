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
	bool muted;

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
	
	public void OnTogglePressed()
	{
		muted = !muted;
		if (muted)
		{
			// Se silencia el sonido en el juego
			audioSource.volume = 0;
		}
		else if (!muted)
		{
			// Se vuelve ha activar el sonido
			audioSource.volume = volumeSlider.value;
		}
	}
}
