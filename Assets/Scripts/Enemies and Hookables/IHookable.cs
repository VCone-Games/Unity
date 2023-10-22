using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHookable 
{
    public void Hooked();

    public void Unhook();
    public int GetWeight();

    public void ParryThis();

    public bool IsBeingParried();

    public bool HasBeenParried();

    public void AfterParry();
}
