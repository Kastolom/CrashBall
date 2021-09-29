using UnityEngine;

public class ObjectMoving : MonoBehaviour
{
    [SerializeField] private Vector3 speedTurn; //Количество градусов в секунду

    void Update()
    {
        transform.Rotate(speedTurn * Time.deltaTime, Space.Self);
    }
}
