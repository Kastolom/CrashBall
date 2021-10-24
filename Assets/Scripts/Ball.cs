using System;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts;

public class Ball : MonoBehaviour
{
    public enum PrefabType
    {
        circle,
        cube,
        poly
    }

    public PrefabType prefabType;

    public State state;

    private float timer;

    private TextMesh textBall;
    [SerializeField] private int ballNumber;
    [SerializeField] private Sprite spriteYellow;

    public float[] features;

    public CrashBallEvent CrashBall = new CrashBallEvent();
    public SpawnBallEvent SpawnBall = new SpawnBallEvent();

    void Start()
    {
        textBall = gameObject.GetComponentInChildren<TextMesh>();
        timer = features[(int)Features.curentTime];
        textBall.text = Math.Round(timer).ToString();
    }

    void Update()
    {
        if (state == State.timer)
        {
            textBall.text = Math.Round(timer).ToString();
            timer -= Time.deltaTime;         

            if (timer <= 0)
            {
                textBall.text = FormatWrite.FormatNumber(features[(int)Features.curentValue] ,10) + " $";
                timer = features[(int)Features.curentTime];
                if (gameObject.TryGetComponent(out BoxCollider2D collider2D))
                {
                    collider2D.enabled = true;
                }
                if (gameObject.TryGetComponent(out CircleCollider2D CircleCollider2D))
                {
                    CircleCollider2D.enabled = true;
                }
                if (gameObject.TryGetComponent(out PolygonCollider2D polygonCollider2D))
                {
                    polygonCollider2D.enabled = true;
                }
                GetComponent<Rigidbody2D>().simulated = true;
                state = State.fly;
                float chance = UnityEngine.Random.Range(0f, 100f);
                if (chance <= features[(int)Features.chanceGold])
                {
                    features[(int)Features.curentValue] *= 10;
                    textBall.text = FormatWrite.FormatNumber(features[(int)Features.curentValue], 10) +"$";
                    gameObject.GetComponent<SpriteRenderer>().sprite = spriteYellow;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 50, 255);
                }

                SpawnBall.Invoke(ballNumber);
            }
        }
    }

    private void OnMouseDown()
    {
        if (state == State.fly)
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
    public class SpawnBallEvent : UnityEvent<int> { }
}
