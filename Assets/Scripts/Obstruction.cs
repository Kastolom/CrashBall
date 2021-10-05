using System;
using UnityEngine;

public class Obstruction : MonoBehaviour
{
    [SerializeField] private float priceObstruction;
    private float curentNumber;
    private TextMesh textObstruction;
    private bool locked;


    void Start()
    {
        locked = true;
        curentNumber = priceObstruction;
        textObstruction = gameObject.GetComponentInChildren<TextMesh>();
        textObstruction.text = curentNumber.ToString() + "$";
    }

    public void ChangeNumber(float number)
    {
        if (locked)
        {
            curentNumber += number;
            textObstruction.text = Math.Round(curentNumber, 1).ToString() + "$";
            if (curentNumber <= 0)
            {
                locked = false;
                gameObject.GetComponent<ObjectMoving>().moving = false;
                curentNumber = priceObstruction;
                textObstruction.color = Color.black;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                textObstruction.text = Math.Round(curentNumber, 1).ToString() + "$";
            }
        }     
    }
    private void OnMouseDown()
    {
        if (!locked)
        {
            Obstruction newObstruction = Instantiate(this);
            newObstruction.textObstruction.color = Color.white;
            newObstruction.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            newObstruction.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            gameObject.AddComponent<Rigidbody2D>();            
        }
    }
    //public void ChangeLock(bool key)
    //{
    //    locked = key;
    //}
}
