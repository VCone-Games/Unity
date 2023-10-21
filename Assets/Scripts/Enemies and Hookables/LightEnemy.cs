using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemy : MonoBehaviour, IHookable
{
    [SerializeField] private int weight = 0;
    private Rigidbody2D myRigidbody;
    private bool beingHooked;
   
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
        beingHooked = true;
    }

    public void Unhook()
    {
        myRigidbody.velocity = Vector3.zero;
        beingHooked = false;
        gameObject.layer = 0;
    }
}
