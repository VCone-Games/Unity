using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengibreHealthManager : HealthManager
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.F)) EventDamageTaken?.Invoke(this, new Vector3(1,0,0));
    }
}
