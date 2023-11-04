using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
	private void Awake()
	{
		var noDestroy = FindObjectsOfType<SceneManagerScript>();
		if (noDestroy.Length > 1)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
	}

	public void SceneStartGame()
	{
		SceneManager.LoadScene("SampleScene");
	}
}
