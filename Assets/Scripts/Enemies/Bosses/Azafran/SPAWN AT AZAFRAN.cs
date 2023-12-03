using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWNATAZAFRAN : MonoBehaviour
{
    [SerializeField] private GameObject DENSE_FOG_1;
    [SerializeField] private GameObject DENSE_FOG_2;
    [SerializeField] private GameObject DENSE_FOG_3;

    private float timer;
    void Start()
    {
        timer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 2)
            {
                DENSE_FOG_1.SetActive(false);
            }
            if(timer < 1)
            {
                DENSE_FOG_2.SetActive(false);
            }
            if(timer < 0)
            {
                DENSE_FOG_3.SetActive(false);
            }
        }
    }
}
