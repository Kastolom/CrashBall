using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    const int countballs = 1;
    [SerializeField] Text TextMoney;
    [SerializeField] Text[] TextValues;
    [SerializeField] Text[] TextTimes;
    [SerializeField] Text[] LeveTextValues;
    [SerializeField] Text[] levelTextTimes;
    [SerializeField] Ball[] balls;
    //[SerializeField] Timer timer;
    private float money;
    private float[] values = new float[countballs];
    private float[] times = new float[countballs];
    private float[] levelvalues = new float[countballs];
    private float[] leveltimes = new float[countballs];

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        GetDefoltValue();
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
                if(PlayerPrefs.HasKey(textValue + value.ToString()))
                    _values[value] = PlayerPrefs.GetFloat(textValue + value.ToString());
                item.text = _values[value].ToString();
                money = PlayerPrefs.GetFloat("money");
                TextMoney.text = money.ToString();
            }
            else //Сохраняем значение
            {
                PlayerPrefs.SetFloat(textValue + value.ToString(), _values[value]);
                PlayerPrefs.SetFloat("money", money);
            }
            value++;
        }
        PlayerPrefs.Save();
    }

    private void GetDefoltValue()
    {
        int i = 0;
        foreach (var item in balls)
        {
            values[i] = item.curentValue;
            times[i] = item.curentTime;
            levelvalues[i] = item.buyValue;
            leveltimes[i] = item.buyTime;
            i++;
        }
    }

    private void CrashInvoke(float curentValue)
    {
        money += curentValue;
    }

    private void OnApplicationQuit()
    {
        GetSetValues(TextValues, values, "ball", false);
        GetSetValues(TextTimes, times, "time", false);
        GetSetValues(LeveTextValues, levelvalues, "levelball", false);
        GetSetValues(levelTextTimes, leveltimes, "leveltime", false);
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
    }


}
