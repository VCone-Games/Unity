using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    public static TimeStop instance;

    private float speed;
    private bool restoreTime;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (restoreTime)
        {
            if(Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }

    }

    public void StopTime(float changeTime, float restoreSpeed, float delay)
    {
        speed = restoreSpeed;

        if(delay > 0f)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay)); 
        }
        else
        {
            restoreTime = true;
        }
        Time.timeScale = changeTime;
    }

    IEnumerator StartTimeAgain(float amount)
    {
        restoreTime = true;
        yield return new WaitForSeconds(amount);
    }
}
