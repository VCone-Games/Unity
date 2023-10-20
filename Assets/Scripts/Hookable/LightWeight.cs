using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightWeight : AHookable
{
    private bool hooked;
    private Rigidbody2D myRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hooked)
        {
            hookDirection = new Vector2 (playerGameObject.transform.position.x - transform.position.x, playerGameObject.transform.position.y - transform.position.y); 

            if(hookDirection.magnitude < minimumDistance)
            {
                hooked = false;
            } else
            {
                myRigidbody2D.velocity = new Vector3(hookDirection.x, hookDirection.y, 0) * animationCurve.Evaluate(hookingPercentageComplete) * hookedSpeed;
            }
        }
    }

    override public void Hooked()
    {
        hooked = true;
        playerGameObject.GetComponent<HorizontalMovement>().isHooking = true;
    }
}
