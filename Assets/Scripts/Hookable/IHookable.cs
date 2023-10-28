using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHookable 
{
    public void Hooked(float distanceToUnhook, GameObject hookProjectile, float hookingSpeed);

    public void Unhook();

}
