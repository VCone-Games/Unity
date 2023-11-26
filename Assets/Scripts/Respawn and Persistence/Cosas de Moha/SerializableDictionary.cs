using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
{
    
    // JsonUtility.ToJson no funciona bien con diccionaros. Esta clase adapta los diccionarios para que puedan ser serializables

    [SerializeField] private List<Tkey> keys = new List<Tkey>();

    [SerializeField] private List<TValue> values = new List<TValue>();

    // Metodo que adquiere por implementeas ISerializationCallbacKReceiver
    
    // Guarda el diccionario en una lista
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<Tkey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }

    }

    // Metodo que adquiere por implementeas ISerializationCallbacKReceiver
    
    // Carga el diccionario en una lista
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("No hay el mismo numero de claves que valores");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }


    }



}
