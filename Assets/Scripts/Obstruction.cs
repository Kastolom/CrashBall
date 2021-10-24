using System;
using UnityEngine;
using Assets.Scripts;

public class Obstruction : MonoBehaviour
{
    [SerializeField] private Sprite whiteSprite;
    [SerializeField] private Sprite blackSprite;
    [SerializeField] private Color waitColor;
    [SerializeField] private Color flyColor;
    [SerializeField] private Color standartColor;
    [SerializeField] private Color timerColor;

    [SerializeField] private float priceObstruction;
    [SerializeField] private float timerObstruction;
    private float curentNumber;
    private TextMesh textObstruction;

    private GameObject environment;
    private float curentTimer;

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
                gameObject.GetComponent<SpriteRenderer>().color = standartColor;

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
                gameObject.GetComponent<SpriteRenderer>().sprite = whiteSprite;
                gameObject.GetComponent<SpriteRenderer>().color = waitColor;
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
            ChangeNumber(-ball.features[(int)Features.curentValue]);
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
            newObstruction.GetComponent<SpriteRenderer>().sprite = blackSprite;
            newObstruction.GetComponent<SpriteRenderer>().color = timerColor;
            if(newObstruction.TryGetComponent(out BoxCollider2D collider2D)) 
            {
                collider2D.enabled = false;
            }
            if (newObstruction.TryGetComponent(out CircleCollider2D CircleCollider2D))
            {
                CircleCollider2D.enabled = false;
            }
            if (newObstruction.TryGetComponent(out PolygonCollider2D polygonCollider2D))
            {
                polygonCollider2D.enabled = false;
            }
            newObstruction.GetComponent<Obstruction>().state = State.timer;

            state = State.fly;
            gameObject.GetComponent<SpriteRenderer>().color = flyColor;
            gameObject.AddComponent<Rigidbody2D>();            
        }
    }
}
