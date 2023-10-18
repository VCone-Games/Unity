using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AHookable : MonoBehaviour, IHookable
{
    [SerializeField] private float hookedSpeed;
    [SerializeField] private float hookedTime;
    [SerializeField] private AnimationCurve animationCurve;
    abstract public void Hooked();
}
