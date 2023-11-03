using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHookable 
{
    public void Hooked(GameObject hookProjectile, float hookingSpeed);

    public void Unhook();

    public void Parried(Vector3 direction, float knockbackTime);

    public void SetParried(bool parried);

    public bool IsParried();

}
