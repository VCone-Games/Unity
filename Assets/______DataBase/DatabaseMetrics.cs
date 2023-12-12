using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DatabaseMetrics : MonoBehaviour
{
	public static DatabaseMetrics Singleton;

	private Dictionary<int, SceneMetrics> dictionarySceneMetrics = new Dictionary<int, SceneMetrics>();

	private Dictionary<string, bool> dataBoss = new Dictionary<string, bool>();
	private enum GenderState { MASCULINO, FEMENINO, OTRO }

	private List<string> bossNames = new List<string>
	{ "Curcuma", "Azafran", "Perejil", "Jengibre" };

	[Header("Global metrics")]
	[SerializeField] private float timerGame;
	private float startedTimerGame;
	[SerializeField] private string username;
	[SerializeField] private int age;
	[SerializeField] private GenderState gender;
	public float TimerGame { get { return Time.time - startedTimerGame; } set { timerGame = value; } }
	public string TimerGameString
	{
		get
		{
			int minutos = Mathf.FloorToInt(timerGame / 60);
			int segundos = Mathf.FloorToInt(timerGame % 60);

			return minutos.ToString("00") + ":" + segundos.ToString("00");
		}
	}
	public string Username { get { return username; } set { username = value; } }
	public int Age { get { return age; } set { age = value; } }
	public string Gender
	{
		get
		{
			switch (gender)
			{
				case GenderState.MASCULINO:
					return "Masculino";
				case GenderState.FEMENINO:
					return "Femenino";
				default:
					return "Otro";
			}
		}

		set
		{
			switch (value)
			{
				case "Masculino":
					this.gender = GenderState.MASCULINO; break;
				case "Femenino":
					this.gender = GenderState.FEMENINO; break;
				default:
					this.gender = GenderState.OTRO; break;
			}
		}
	}

	//METRICAS DE ESCENA
	private class SceneMetrics
	{
		public SceneMetrics()
		{
			timeInScene = 0.0f;
			timesEntered = 0;
			deathCount = 0;
			objectsHookeds = 0;
			objectsParried = 0;
			lifesLosts = 0;
			collectionablePickeds = 0;
			totalCollecionables = 0;
			defeatedEnemies = 0;
		}
		public float timeInScene;
		public int timesEntered;
		public int deathCount;
		public int objectsHookeds;
		public int objectsParried;
		public int lifesLosts;
		public int collectionablePickeds;
		public int totalCollecionables;
		public int defeatedEnemies;
	}
	public float TimeInScene
	{
		set
		{
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.timeInScene = value;
		}
	}
	public int TimesEntered
	{
		set
		{
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.timesEntered = value;
		}
	}
	public int DeathCount {
		set {
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.deathCount = value;
		}
	}
	public int ObjectsHookeds
	{
		set
		{
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.objectsHookeds = value;
		}
	}
	public int objectsParried
	{
		set
		{
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.objectsParried = value;
		}
	}
	public int LifesLosts
	{
		set
		{
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.lifesLosts = value;
		}
	}
	public int CollectionablePickeds
	{
		set
		{
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.collectionablePickeds = value;
		}
	}
	public int TotalCollecionables
	{
		set
		{
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.totalCollecionables = value;
		}
	}
	public int DefeatedEnemies
	{
		set
		{
			int sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneMetrics currentScene = dictionarySceneMetrics[sceneID];
			currentScene.defeatedEnemies = value;
		}
	}

	public Dictionary<string, bool> DataBoss { get { return dataBoss; } }

	private void Awake()
	{
		if (Singleton == null)
			Singleton = this;
	}
	// Start is called before the first frame update
	void Start()
	{
		foreach (var name in bossNames)
		{
			dataBoss.Add(name, false);
		}


		for (int i = 1; i < SceneManager.sceneCountInBuildSettings - 1; i++)
		{
			dictionarySceneMetrics.Add(i, new SceneMetrics());
			//Debug.Log($"Escena: {i}, nombre:  { SceneManager.GetSceneByBuildIndex(i).name }");
		}

		startedTimerGame = Time.time;
	}


	// Update is called once per frame
	void Update()
	{
		timerGame = Time.time - startedTimerGame; //SE PUEDE BORRAR MAS TARDE PARA OPTIMIZAR
	}

	public void OnDeathBoss(string nameBoss)
	{
		Debug.Log("Boss ha muerto");

		if (dataBoss.ContainsKey(nameBoss))
		{
			Debug.Log("El nombre del boss esta en el diccionario. Se muere.");
			dataBoss[nameBoss] = true;
		}
		else
		{
			Debug.Log("El nombre del boss no se encuentra en el diccionario.");
		}
	}

	public bool IsBossDead(string nameBoss)
	{
		return dataBoss[nameBoss];
	}
}
