using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
	[Header("Audio Sources")]
	[SerializeField] private AudioSource musicSource;

	[Header("Sonidos")]
	[SerializeField] private AudioClip first;
	[SerializeField] private AudioClip interlude;
	[SerializeField] private AudioClip second;

	private bool interludeHasBegin = false;

	public void Update()
	{
		if (interludeHasBegin && !musicSource.isPlaying)
		{
			PlaySecond();
		}
	}
	public void PlayFirst()
	{
		musicSource.clip = first;
		musicSource.Play();
	}

	public void PlayInterlude()
	{
		Debug.Log("Interlude");
		musicSource.Stop();
		musicSource.PlayOneShot(interlude);
		interludeHasBegin = true;
	}
	public void PlaySecond()
	{
		musicSource.clip = second;
		musicSource.Play();
	}
}
