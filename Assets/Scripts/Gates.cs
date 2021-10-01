using UnityEngine;

public class Gates : MonoBehaviour
{
    private TextMesh textGate;
    [SerializeField] private float powerGate;

    void Start()
    {
        textGate = gameObject.GetComponentInChildren<TextMesh>();
        textGate.text = powerGate.ToString();
    }

    public void GatePowerChange(float value)
    {
        powerGate += value;
        powerGate = (float)System.Math.Round(powerGate, 1);
        textGate.text = powerGate.ToString() + "$";
        if (powerGate<0)
        {
            Destroy(gameObject);
        }
    }
}
