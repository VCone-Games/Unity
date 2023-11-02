using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class VelocityClamper : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Dash dashComponent;


    private void FixedUpdate()
    {
        if(dashComponent != null)
        {
            if (dashComponent.IsDashing) return;
        }

        if (myRigidbody.velocity.magnitude > maxSpeed)
        {
            myRigidbody.velocity = myRigidbody.velocity.normalized * maxSpeed;

        }
    }
}
