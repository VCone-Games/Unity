using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Usings importantes
using System;
using System.IO;

public class FileDataHandler
{
    
    // Path del directorio donde se va ha alojar la informacion
    private string dataDirPath = "";
    
    // Nombre del fichero que va a contener la información
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;

    }

    public GameData Load()
    {
        // Usar Path.Combine garantiza independencia sobre los separadores que usan los diferentes SO's
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // Cargamos la información serializada del archivo
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                // Deserializar la info de Json a info de un objeto de C#
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.Log("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        // Usar Path.Combine garantiza independencia sobre los separadores que usan los diferentes SO's
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try    
        {
            // Creacion del directorio de guardado/cargado
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
            // Serializacion la información del juego a Json
            string dataToStore = JsonUtility.ToJson(data, true);
            
            // Escritura de la información serializada
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                
                }
                
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
