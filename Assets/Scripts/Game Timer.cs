using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
	private float startTime;
	private bool isGamePaused = false;

	private void Start()
	{
		startTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		if (!isGamePaused)
		{
			float elapsedTime = Time.realtimeSinceStartup - startTime;
			UpdateTimerText(elapsedTime);
		}

		if (Input.GetKey(KeyCode.G)) Time.timeScale = 0.0f;
		if (Input.GetKey(KeyCode.H)) Time.timeScale = 1.0f;
	}

	// Método para actualizar el texto del temporizador
	private void UpdateTimerText(float time)
	{
		int minutes = Mathf.FloorToInt(time / 60f);
		int seconds = Mathf.FloorToInt(time % 60f);

		string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
		Debug.Log(timerString);
	}

	// Método para pausar/reanudar el temporizador
	public void TogglePause()
	{
		isGamePaused = !isGamePaused;
	}
}
