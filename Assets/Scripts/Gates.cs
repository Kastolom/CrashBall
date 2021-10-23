using UnityEngine;
using Assets.Scripts;
public class Gates : MonoBehaviour
{
    private TextMesh textGate;
    [SerializeField] private float powerGate;

    void Start()
    {
        textGate = gameObject.GetComponentInChildren<TextMesh>();
        textGate.text = FormatWrite.FormatNumber(powerGate, 10) + " $";
    }

    public void GatePowerChange(float value)
    {
        powerGate += value;
        powerGate = (float)System.Math.Round(powerGate, 1);
        textGate.text = FormatWrite.FormatNumber(powerGate, 10) + " $";
        if (powerGate<0)
        {
            Destroy(gameObject);
        }
    }
}
