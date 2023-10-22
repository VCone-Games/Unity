using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookableEnemy : MonoBehaviour, IHookable
{
    [SerializeField] private int weight;
    [SerializeField] private Rigidbody2D myRigidbody;
    private bool beingHooked;
    private bool isBeingParried;
    private bool beenParried;
    [SerializeField] private float afterParriedTime;
    [SerializeField] private float afterParriedTimer;
    [SerializeField] private int layerNoPlayer;
    [SerializeField] private int defaultLayer;

    void Start()
    {

    }

    void Update()
    {
        if (afterParriedTimer > 0)
        {
            afterParriedTimer -= Time.deltaTime;
            if (afterParriedTimer < 0)
            {
                gameObject.layer = defaultLayer;
            }
        }
    }

    public int GetWeight()
    {
        return weight;
    }

    public void Hooked()
    {
        beingHooked = true;
    }

    public void Unhook()
    {
        beingHooked = false;

        if (!beenParried)
        {
            myRigidbody.velocity = Vector3.zero;
        }
        if (isBeingParried)
        {
            isBeingParried = false;
        }
        beenParried = false;
    }

    public void ParryThis()
    {
        isBeingParried = true;
        gameObject.layer = layerNoPlayer;
    }

    public void AfterParry()
    {
        isBeingParried = false;
        beenParried = true;
        afterParriedTimer = afterParriedTime;
    }
    public bool HasBeenParried()
    {
        return beenParried;
    }

    public bool IsBeingParried()
    {
        return isBeingParried;
    }
}
