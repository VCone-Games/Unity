using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnFallingStones : MonoBehaviour
{
    [Header("Spawn params")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float offSetX = 2.5f;
    [SerializeField] private float offSetY = 2.5f;

    [Header("PREFABS")]
    [SerializeField] private GameObject prefabStone;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PhaseManagerAzafran>().summonFallingStone += SummonStones;    
    }

	private void SummonStones(object sender, int stonesPerPoint)
	{
        for (int i = 0;  i < stonesPerPoint; i++)
        {
            for (int j = 0; j < spawnPoints.Count; j++)
            {
                float valOffSetX = (float) Random.Range(-offSetX, offSetX);
                float valOffSetY = (float) Random.Range(-offSetY, offSetY);

                Vector3 position = new Vector3(spawnPoints[j].position.x + valOffSetX, spawnPoints[j].position.y + valOffSetY);
                Instantiate(prefabStone, position, Quaternion.identity);
            }
        }
	}

}
