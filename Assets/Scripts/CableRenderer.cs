using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableRenderer : MonoBehaviour
{
    
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private Transform hookGunTransform;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        hookGunTransform = GameObject.FindWithTag("HookGun").transform;
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0,hookGunTransform.position);
        lineRenderer.SetPosition(1,projectileTransform.position);
    }
}
