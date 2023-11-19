using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject introduceYourNamePanel;
    public InputField nameField;

    private void Start()
    {
        // Si volvemos de la pantalla de gameOver que pausa el tiempo, lo volvemos a reanudar
        //Time.timeScale = 1;
        Debug.Log(Application.persistentDataPath);
    }

    

    public void StartGame()
    {
        Debug.Log(Application.persistentDataPath);
        // Si no hay información guardada, empecemos nueva partida
        if (DataPersistenceManager.instance.GetGameData() == null)
        {
            DataPersistenceManager.instance.NewGame();
            // Mostrar el panel de "Introduce tu nombre"
            introduceYourNamePanel.SetActive(true);
        }
        // Por el contrario, cargamos partida
        else
        {
            DataPersistenceManager.instance.LoadGame();
            // Iniciamos partida
           // GetComponent<SceneManagerScript>().SceneStartGame();
        }
    }

    public void StartGameWithName()
    {
        // Actualizamos el nombre del jugador
        DataPersistenceManager.instance.GetGameData().username = nameField.text;
        introduceYourNamePanel.SetActive(false);
        // Cargamos escena
        //GetComponent<SceneManagerScript>().SceneStartGame();
        

    }

    public void QuitGame()
    {
        Application.Quit();
        print("Game closed");
    }


}
