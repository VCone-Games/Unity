using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AHookable : MonoBehaviour, IHookable
{
    [SerializeField] protected float hookedSpeed;
    [SerializeField] protected float hookedTime;
    [SerializeField] protected AnimationCurve animationCurve;

    [SerializeField] protected GameObject playerGameObject;

    [SerializeField] protected Vector2 hookDirection;
    [SerializeField] protected float minimumDistance;

    protected float hookingElapsedTime;
    protected float hookingPercentageComplete;
    abstract public void Hooked();
}
