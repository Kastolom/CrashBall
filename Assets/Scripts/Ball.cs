using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    private bool enable;
    private float timer;

    public float curentValue;
    public float curentTime;
    public float buyValue;
    public float buyTime;

    public UnityEvent CrashBall;

    //public bool locked;

    void Start()
    {
        enable = true;
    }

    void Update()
    {
        if (enable)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = curentTime;
                //enable = false;
                GetComponent<CircleCollider2D>().enabled = true;
                GetComponent<Rigidbody2D>().simulated = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CrashBall.Invoke();
    }
}
