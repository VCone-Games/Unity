using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemy : MonoBehaviour, IHookable
{
    private int weight = 0;
    private Rigidbody2D myRigidbody;
   
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public int getWeight()
    {
        return weight;
    }

    public void Hooked()
    {
        
    }

    public void Unhook()
    {
        myRigidbody.velocity = Vector2.zero;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
