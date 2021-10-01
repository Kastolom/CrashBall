using System;
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

    private bool enable;
    private bool active;
    private bool locked;
    private float timer;
    private TextMesh textBall;

    public float[] features = new float[counFeature];

    public CrashBallEvent CrashBall = new CrashBallEvent();
    public UnityEvent SpavnEvent;

    void Start()
    {
        textBall = gameObject.GetComponentInChildren<TextMesh>();
        locked = false;
        active = false;
        timer = features[(int)Features.curentTime];
        enable = true;
    }

    void Update()
    {
        if (enable)
        {
            textBall.text = Math.Round(timer).ToString();
            timer -= Time.deltaTime;         

            if (timer <= 0)
            {
                textBall.text = features[(int)Features.curentValue].ToString() + "$";
                timer = features[(int)Features.curentTime];
                GetComponent<CircleCollider2D>().enabled = true;
                GetComponent<Rigidbody2D>().simulated = true;
                active = true;
                enable = false;
                SpavnEvent.Invoke();
            }
        }
    }

    private void OnMouseDown()
    {
        if (active)
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
    }

    public class CrashBallEvent : UnityEvent<float> { }
}
