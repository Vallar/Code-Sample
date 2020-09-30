using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float currentTime;
    private float timer;

    public Timer(float _time)
    {
        currentTime = _time;
        timer = _time;
    }

    public bool IsTimerUp()
    {
        if(currentTime <= 0)
        {
            currentTime = timer;
            return true;
        }
        else
        {
            currentTime -= Time.deltaTime;
            return false;
        }
    }
}
