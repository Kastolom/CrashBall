using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool enable;
    private float timer;


    void Start()
    {
        enable = false;
    }

    void Update()
    {
        if (enable)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
                enable = false;
            }
        }
    }

    public void StartTimer(float _timer)
    {
        timer = _timer;
        enable = true;
    }
}
