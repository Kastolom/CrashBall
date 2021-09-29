using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [SerializeField] Text TextMoney;
    [SerializeField] Text[] TextValues;
    [SerializeField] Text[] TextTimes;
    [SerializeField] Text[] LeveTextValues;
    [SerializeField] Text[] levelTextTimes;
    private float money;
    private float[] values = new float[1];
    private float[] times = new float[1];
    private float[] levelvalues = new float[1];
    private float[] leveltimes = new float[1];

    private void Start()
    {
        GetSetValues(TextValues, values, "ball" , true);
        GetSetValues(TextTimes, times, "time", true);
        GetSetValues(LeveTextValues, levelvalues, "levelball", true);
        GetSetValues(levelTextTimes, leveltimes, "leveltime", true);
    }

    private void GetSetValues(Text[] textItems, float[] _values, string textValue, bool state)
    {
        int value = 0;
        foreach (var item in textItems)
        {
            if (state) //Берем значение
            {
                _values[value] = PlayerPrefs.GetFloat(textValue + value.ToString(), 0);
                item.text = values[value].ToString();
                money = PlayerPrefs.GetFloat("money", 0);
                TextMoney.text = PlayerPrefs.GetFloat("money", 0).ToString();
            }
            else //Сохраняем значение
            {
                PlayerPrefs.SetFloat(textValue + value.ToString(), _values[value]);               
                PlayerPrefs.SetFloat("money", money);
            }
            value++;
        }
    }

    private void OnApplicationQuit()
    {
        GetSetValues(TextValues, values, "ball", false);
        GetSetValues(TextTimes, times, "time", false);
        GetSetValues(LeveTextValues, levelvalues, "levelball", false);
        GetSetValues(levelTextTimes, leveltimes, "leveltime", false);
    }


}
