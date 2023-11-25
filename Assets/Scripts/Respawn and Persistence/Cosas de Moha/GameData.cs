using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    // Variables que interesan guardar
    
    // 1. Conteo de monedas/coleccionables
    public int numberOfCollectionables;
    
    // 2. Monedas recogidas
    public SerializableDictionary<string, bool> collectablesCollected;

    // 3. Bosses derrotados. Usar un diccionario
    public SerializableDictionary<string, bool> defeatedBosses;
    
    // 4. Posicion del jugador
    public Vector3 playerPosition;
    
    // 5. Enemigos respawneables eliminados
    public SerializableDictionary<string, bool> defeatedEnemies;
    
    // 6. Configuraciones de sonido/resolución/opciones
    public float audioValue;

    // Constructor. Valores con los que se empieza una nueva partida
    public GameData()
    {
        numberOfCollectionables = 0;
        defeatedBosses = new SerializableDictionary<string, bool>();
        collectablesCollected = new SerializableDictionary<string, bool>();
        defeatedEnemies = new SerializableDictionary<string, bool>();
        playerPosition = Vector3.zero;
        audioValue = 0;
    }
}
