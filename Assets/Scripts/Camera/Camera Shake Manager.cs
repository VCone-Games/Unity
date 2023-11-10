using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    [SerializeField] private float globalShakeForce = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        instance = this;
    }

    public void CameraShake(CinemachineImpulseSource impulseSource, Vector3 impulse)
    {
        impulseSource.m_DefaultVelocity = impulse;
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }

    public void CameraShake(CinemachineImpulseSource impulseSource, Vector3 impulse, float force)
    {
        impulseSource.m_DefaultVelocity = impulse;
        impulseSource.GenerateImpulseWithForce(force);
    }
}
