using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHookable 
{
    public void Hooked();

    public void Unhook();
    public int getWeight();
}
