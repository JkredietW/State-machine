using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0, 24)]
    public float time;
    public float timeSpeed;

    int dayState;
    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        TimeScale();
    }
    void TimeScale()
    {
        time += timeSpeed * Time.deltaTime;
        CheckTime();
    }
    void CheckTime()
    {
        if (time > 2 && time < 16)
        {
            if(dayState == 0)
            {
                dayState = 1;
                gm.GiveTimeToHumans();
            }
        }
        else if (time > 20 && time < 24)
        {
            if (dayState == 1)
            {
                dayState = 2;
                gm.GiveTimeToHumans();
            }
        }
        else if (time >= 24)
        {
            if (dayState == 2)
            {
                dayState = 0;
                time = 0;
            }
        }
    }
}
