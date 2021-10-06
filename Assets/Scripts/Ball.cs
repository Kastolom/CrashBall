﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    const int counFeature = 4;

    public enum Features
    {
        curentValue,
        curentTime,
        buyValue,
        buyTime
    }

    //private enum State
    //{
    //    standart,
    //    wait,
    //    fly,
    //    timer
    //}

    public Obstruction.State state;

    //public bool locked;
    //private bool enable;
    //private bool active;
    private float timer;

    private TextMesh textBall;
    [SerializeField] private int ballNumber;

    public float[] features = new float[counFeature];

    public CrashBallEvent CrashBall = new CrashBallEvent();
    public SpawnBallEvent SpawnBall = new SpawnBallEvent();

    void Start()
    {
        textBall = gameObject.GetComponentInChildren<TextMesh>();
        //active = false;
        timer = features[(int)Features.curentTime];
        textBall.text = Math.Round(timer).ToString();
        //enable = true;
    }

    void Update()
    {
        if (state == Obstruction.State.timer)
        {
            textBall.text = Math.Round(timer).ToString();
            timer -= Time.deltaTime;         

            if (timer <= 0)
            {
                textBall.text = features[(int)Features.curentValue].ToString() + "$";
                timer = features[(int)Features.curentTime];
                GetComponent<CircleCollider2D>().enabled = true;
                GetComponent<Rigidbody2D>().simulated = true;
                //active = true;
                //enable = false;
                state = Obstruction.State.fly;
                SpawnBall.Invoke(ballNumber);
            }
        }
    }

    private void OnMouseDown()
    {
        if (state == Obstruction.State.fly)
        {
            CrashBall.Invoke(features[(int)Features.curentValue] / 2);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Gates gate))
        {
            gate.GatePowerChange(-features[(int)Features.curentValue]);
            CrashBall.Invoke(features[(int)Features.curentValue]);
            Destroy(gameObject);
        }

        if(collision.gameObject.TryGetComponent(out Obstruction obstruction))
        {
            obstruction.ChangeNumber(-features[(int)Features.curentValue]);
        }
    }

    public class CrashBallEvent : UnityEvent<float> { }
    public class SpawnBallEvent : UnityEvent<int> { }
}
