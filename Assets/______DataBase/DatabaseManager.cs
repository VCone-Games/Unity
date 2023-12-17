using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using static DatabaseMetrics;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Singleton;
    [ContextMenuItem("Load credentilas", "ChargeMetricsOnCloud")]
    public string a;
	string username;
    string password;
    string uri;
    string contentType = "application/json";

    public bool charged;


    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;

        LoadCredentials();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.N))
        {
            if (charged) return;
            charged = true;
			ChargeMetricsOnCloud();
		}
	}

	private void OnApplicationQuit()
	{
        ChargeMetricsOnCloud();
    }

    [ContextMenu("Load credentilas - Context Menu")]
	public void ChargeMetricsOnCloud()
    {
		StartCoroutine(SendPostRequest());
	}

	string CreateJSON(string tabla, string name, string edad, string gender, string date, string timePlayed, SceneMetrics sceneMetrics)
	//string CreateJSON(string tabla, string name, string edad, string gender, string date, SceneMetrics sceneMetrics)
    {

		string coleccionables = sceneMetrics.collectionablePickeds + "/" + sceneMetrics.totalCollecionables;
		//Construye JSON para la petición REST         
		string json = $@"{{
            ""username"":""{username}"",
            ""password"":""{password}"",
            ""table"":""{tabla}"",
            ""data"": {{
                ""Nombre"": ""{name}"",
                ""Edad"": ""{edad}"",
                ""Genero"": ""{gender}"",
                ""Fecha_de_inicio"": ""{date}"",
                ""Tiempo_jugado"": ""{timePlayed}"",
                ""ID_Zona"": ""{sceneMetrics.zoneID}"",
                ""Tiempo_en_escena"": ""{sceneMetrics.TimeInSceneString}"",
                ""Numero_de_entradas_a_la_escena"": ""{sceneMetrics.timesEntered}"",
                ""Muertes_del_jugador"": ""{sceneMetrics.deathCount}"",
                ""Objetos_enganchados"": ""{sceneMetrics.objectsHookeds}"",
                ""Contraataques_realizados"": ""{sceneMetrics.objectsParried}"",
                ""Vidas_perdidas"": ""{sceneMetrics.lifesLosts}"",
                ""Coleccionables"": ""{coleccionables}"",
                ""Enemigos_derrotados"": ""{sceneMetrics.defeatedEnemies}""
            }}
        }}";
		/*string json = $@"{{
            ""username"":""{username}"",
            ""password"":""{password}"",
            ""table"":""{tabla}"",
            ""data"": {{
                ""Nombre"": ""{name}"",
                ""Edad"": ""{edad}"",
                ""Genero"": ""{gender}"",
                ""Fecha_de_inicio"": ""{date}""
            }}
        }}";*/

		Debug.Log(json);

        return json;
    }
    IEnumerator SendPostRequest()
    {
        DatabaseMetrics metrics = DatabaseMetrics.Singleton;
        string namePlayer = metrics.Username;
        string agePlayer = metrics.Age;
        string gender = metrics.Gender;
        string date = metrics.Date;
        string timePlayed = metrics.TimerGameString;

		foreach (var sceneMetric in metrics.DictionarySceneMetrics)
        {
            SceneMetrics currentScene = sceneMetric.Value;

			string data = CreateJSON("TURr_02", namePlayer, agePlayer, gender, date, timePlayed, currentScene);
			using (UnityWebRequest www = UnityWebRequest.Post(uri, data, contentType))
			{
				yield return www.SendWebRequest();

				if (www.result != UnityWebRequest.Result.Success)
				{
					print("Error: " + www.error);
				}
				else
				{
					print("Respuesta: " + www.downloadHandler.text);
				}
			}
        }

        Destroy(this);

        /*DatabaseMetrics dataBase = DatabaseMetrics.Singleton;
        SceneMetrics scene = dataBase.DictionarySceneMetrics[1];

		//string data = CreateJSON("tur", namePlayer, agePlayer, gender, date, timePlayed, scene);
		string data = CreateJSON("prueba2", namePlayer, agePlayer, gender, date, scene);
		using (UnityWebRequest www = UnityWebRequest.Post(uri, data, contentType))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				print("Error: " + www.error);
			}
			else
			{
				print("Respuesta: " + www.downloadHandler.text);
			}
		}*/
	}

    void LoadCredentials()
    {
        string configPath = "Assets/______DataBase/config.js";

        if (File.Exists(configPath))
        {
            string configJson = File.ReadAllText(configPath);
            var config = JsonUtility.FromJson<Credentials>(configJson);

            username = config.username;
            password = config.password;
            uri = config.uri;
        }
        else
        {
            Debug.LogError("Config file not found!");
        }
    }

    [System.Serializable]
    private class Credentials
    {
        public string username;
        public string password;
        public string uri;
    }
}
