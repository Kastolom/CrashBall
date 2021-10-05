using UnityEngine;

public class ObjectMoving : MonoBehaviour
{
    [SerializeField] private Vector3 speedTurn; //Количество градусов в секунду
    public bool moving { get; set; }

    private void Start()
    {
        moving = true;
    }
    void Update()
    {
        if (moving)
        {
            transform.Rotate(speedTurn * Time.deltaTime, Space.Self);
        }       
    }
}
