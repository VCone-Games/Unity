using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    private bool optionsIsShowing;

    private void Start()
    {
        // Si volvemos de la pantalla de gameOver que pausa el tiempo, lo volvemos a reanudar
        //Time.timeScale = 1;
        Debug.Log(Application.persistentDataPath);
    }

    private void Update()
    {
        HideOptions();
    }

    public void StartGame()
    {
        // Si no hay información guardada, empecemos nueva partida
        if (DataPersistenceManager.instance.GetGameData() == null)
        {
            DataPersistenceManager.instance.NewGame();

        }
        // Por el contrario, cargamos partida
        else
        {
            DataPersistenceManager.instance.LoadGame();
        }
        // Cargamos escena
        SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
        print("Game closed");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        DataPersistenceManager.instance.SaveGame();
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
        optionsIsShowing = true;
    }
    
    public void HideOptions()
    {
        if (optionsIsShowing && Input.GetKeyDown(KeyCode.Escape))
        {
            optionsPanel.SetActive(false);
            optionsIsShowing = false;
        }
    }
}
