using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase Singleton;

    private Dictionary<string, bool> dataBoss = new Dictionary<string, bool>();
    private List<string> bossNames = new List<string>
    { "Curcuma", "Azafran", "Perejil", "Jengibre" };
    private float timerGame;
    private float startedTimerGame;
    private string username = "Username";
    private int deathCount = 0;

    public Dictionary<string, bool> DataBoss {  get { return dataBoss; } }
    public float TimerGame {  get { return timerGame; } set {  timerGame = value; } }
    public string Username { get { return username;} set { username = value; } }
    public int DeathCount { get { return deathCount; } set {  deathCount = value; } }

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

		startedTimerGame = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        timerGame = Time.time - startedTimerGame;

		int minutos = Mathf.FloorToInt(timerGame / 60);
		int segundos = Mathf.FloorToInt(timerGame % 60);

		// Muestra el tiempo transcurrido en la consola
		//Debug.Log("Tiempo transcurrido: " + minutos.ToString("00") + ":" + segundos.ToString("00"));
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
}
