using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using static DatabaseMetrics;

public class DatabaseManager : MonoBehaviour
{
    string username;
    string password;
    string uri;
    string contentType = "application/json";

    private void Awake()
    {
        LoadCredentials();
    }
    void Start()
    {
    }

    public void ChargeMetricsOnCloud()
    {
		StartCoroutine(SendPostRequest());
	}

	string CreateJSON(string tabla, string name, int edad, string gender, string date, string timePlayed, SceneMetrics sceneMetrics)
    {
        //Construye JSON para la petición REST         
        string json = $@"{{
            ""username"":""{username}"",
            ""password"":""{password}"",
            ""table"":""{tabla}"",
            ""data"": {{
                ""Nombre"": ""{name}"",
                ""Edad"": ""{edad}"",
                ""Genero"": ""{gender}"",
                ""Fecha de inicio"": ""{date}"",
                ""Tiempo jugado"": ""{timePlayed}"",
                ""ID Zona"": ""{sceneMetrics.zoneID}"",
                ""Tiempo en escena"": ""{sceneMetrics.timeInScene}"",
                ""Numero de entradas a la escena"": ""{sceneMetrics.timesEntered}"",
                ""Muertes del jugador"": ""{sceneMetrics.deathCount}"",
                ""Objetos enganchados"": ""{sceneMetrics.objectsHookeds}"",
                ""Contraataques realizados"": ""{sceneMetrics.objectsParried}"",
                ""Vidas perdidas"": ""{sceneMetrics.lifesLosts}"",
                ""Coleccionables"": ""{sceneMetrics.collectionablePickeds}/{sceneMetrics.totalCollecionables}"",
                ""Enemigos derrotados"": ""{sceneMetrics.defeatedEnemies}""
            }}
        }}";

        return json;
    }
    IEnumerator SendPostRequest()
    {
        DatabaseMetrics metrics = DatabaseMetrics.Singleton;
        string namePlayer = metrics.Username;
        int agePlayer = metrics.Age;
        string gender = metrics.Gender;
        string date = metrics.Date;
        string timePlayed = metrics.TimerGameString;

        foreach (var sceneMetric in metrics.DictionarySceneMetrics)
        {
            SceneMetrics currentScene = sceneMetric.Value;

			string data = CreateJSON("TURr_01", namePlayer, agePlayer, gender, date, timePlayed, currentScene);
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
