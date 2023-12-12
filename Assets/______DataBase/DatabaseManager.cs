using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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
        StartCoroutine(SendPostRequest());
    }

    string CreateJSON(string tabla, string name, int edad, string gender)
    {
        //Construye JSON para la petición REST         
        string json = $@"{{
            ""username"":""{username}"",
            ""password"":""{password}"",
            ""table"":""{tabla}"",
            ""data"": {{
                ""Nombre"": ""{name}"",
                ""Edad"": ""{edad}"",
                ""Genero"": ""{gender}""
            }}
        }}";

        return json;
    }
    IEnumerator SendPostRequest()
    {
        string data = CreateJSON("prueba", "Pepe", 10, "Hombre");

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
