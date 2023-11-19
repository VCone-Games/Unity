using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Facilita encontrar los objetos que necesitan ser guardados
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    // NO OLVIDAR DARLE NOMBRE AL FICHERO QUE VA A CONTENER LA INFO. POR EJ. data.game
    // Se guarda en AppData  
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    // Importante que esta clase sea Singleton

    // Referencia privada a un GameData
    private GameData gameData;

    // Lista de los objetos que implementan la interfaz IDataPersistance que interesa guardar
    private List<IDataPersistance> dataPersistenceObjects;

    // File Data Handler que gestiona los flujos de informacion y la serialización y deserializacion
    private FileDataHandler dataHandler;

    // Se puede acceder de manera privada y se puede modificar de manera pública
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager");
        }

        instance = this;
    }

    private void Start()
    {
        // Inicialización del dataHandler.
        // El Application.persistenceDataPath da al SO el directorio estándar de informacion persistente de un proyecto de Unity
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        Debug.Log(Application.persistentDataPath);

        this.dataPersistenceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing new game");
            NewGame();
        }
        // Recorremos todos los objetos guardables
        foreach (IDataPersistance dataPersistenceObj in dataPersistenceObjects)
        {
            
            // Cargamos según su metodo de cargado
            dataPersistenceObj.LoadData(gameData);

        }
        Debug.Log("Data loaded");

    }

    public void SaveGame()
    {
        // Recorremos todos los objetos guardables
        foreach (IDataPersistance dataPersistanceObj in dataPersistenceObjects)
        {
            // Guardamos segun su metodo de guardado
            dataPersistanceObj.SaveData(ref gameData);

        }
        Debug.Log("Guardar monedas recogidas = " + gameData.numberOfCollectionables);
        Debug.Log("Guardar posicion del jugador " + gameData.playerPosition);

        // Guardar en el fichero
        dataHandler.Save(gameData);
    }

    // Si realmente deseamos guardar partida al cerrar la aplicación
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        /* Uso de System.Linq. Necesario que los objetos a guardar hereden de MonoBehaviour e
        implementen la interfaz IDataPersistance*/

        IEnumerable<IDataPersistance> dataPersistenceObjects = FindObjectsOfType<Enemy>()
            .OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistenceObjects);
    }
}
