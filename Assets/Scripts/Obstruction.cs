using System;
using UnityEngine;

public class Obstruction : MonoBehaviour
{
    [SerializeField] private float priceObstruction;
    [SerializeField] private float timerObstruction;
    private float curentNumber;
    private TextMesh textObstruction;

    private GameObject environment;
    private float curentTimer;
    public enum State
    {
        standart,
        wait,
        fly,
        timer
    }

    public State state { get; set; }

    private void Start()
    {
        environment = FindObjectOfType<ScrollEnvironment>().gameObject;
        curentTimer = timerObstruction;
        curentNumber = priceObstruction;
        textObstruction = gameObject.GetComponentInChildren<TextMesh>();
        textObstruction.text = curentNumber.ToString() + "$";
    }

    private void Update()
    {
        if (state == State.timer)
        {
            curentTimer -= Time.deltaTime;
            textObstruction.text = Math.Round(curentTimer).ToString();
            if (curentTimer <= 0)
            {
                state = State.standart;
                curentNumber = priceObstruction;
                textObstruction.text = Math.Round(curentNumber, 1).ToString() + "$";
                gameObject.GetComponent<SpriteRenderer>().color = Color.black;

                if (gameObject.TryGetComponent(out BoxCollider2D collider2D))
                {
                    collider2D.enabled = true;
                }
                if (gameObject.TryGetComponent(out CircleCollider2D CircleCollider2D))
                {
                    CircleCollider2D.enabled = true;
                }
            }
        }
    }

    public void ChangeNumber(float number)
    {
        if (state == State.standart)
        {
            curentNumber += number;
            textObstruction.text = Math.Round(curentNumber, 1).ToString() + "$";
            if (curentNumber <= 0)
            {
                state = State.wait;
                if (gameObject.TryGetComponent(out ObjectMoving objectMoving))
                {
                    objectMoving.moving = false;
                }            
                curentNumber = priceObstruction;
                textObstruction.color = Color.black;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                textObstruction.text = Math.Round(curentNumber, 1).ToString() + "$";
            }
        }     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Gates gate))
        {
            gate.GatePowerChange(-priceObstruction);
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Obstruction obstruction))
        {
            obstruction.ChangeNumber(- priceObstruction);
        }

        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            ChangeNumber(-ball.features[(int)Ball.Features.curentValue]);
            //if (ball.prefabType == Ball.PrefabType.cube)
            //{
            //    OnMouseDown();
            //}
        }
    }

    private void OnMouseDown()
    {
        if (state == State.wait )
        {
            GameObject newObstruction = Instantiate(gameObject , environment.transform);
            newObstruction.GetComponent<Obstruction>().textObstruction = newObstruction.GetComponentInChildren<TextMesh>();
            newObstruction.GetComponent<Obstruction>().textObstruction.color = Color.white;
            newObstruction.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
            if(newObstruction.TryGetComponent(out BoxCollider2D collider2D)) 
            {
                collider2D.enabled = false;
            }
            if (newObstruction.TryGetComponent(out CircleCollider2D CircleCollider2D))
            {
                CircleCollider2D.enabled = false;
            }
            newObstruction.GetComponent<Obstruction>().state = State.timer;

            state = State.fly;
            gameObject.AddComponent<Rigidbody2D>();            
        }
    }
}
