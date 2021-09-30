using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [SerializeField] GameObject Environment;
    [SerializeField] Text TextMoney;
    [SerializeField] Text[] TextValues;
    [SerializeField] Text[] TextTimes;
    [SerializeField] Text[] LeveTextValues;
    [SerializeField] Text[] levelTextTimes;
    [SerializeField] Ball[] balls;

    [SerializeField] string[] keyPlayerPrefs;
    [SerializeField] float coefValue;
    [SerializeField] float coefTime;
    [SerializeField] float levelcoefValue;
    [SerializeField] float levelcoefTime;

    private float money;
    private List<Ball> sceneBalls = new List<Ball>();

    private void Start()
    {
        GetSaveValues(true);
        SpawnBalls();
        ShowData();
    }   

    private void SpawnBalls()
    {
        foreach (var item in balls)
        {
            Ball ball = Instantiate(item);
            ball.CrashBall.AddListener(CrashEvent);
            ball.SpavnEvent.AddListener(SpawnBalls);
            ball.gameObject.transform.parent = Environment.transform;
        }
    }

    private void ShowData()
    {
        TextMoney.text = money.ToString();
        for (int i = 0; i < balls.Length; i++)
        {
            TextValues[i].text = balls[i].features[(int)Ball.Features.curentValue].ToString() + "$";
            TextTimes[i].text = balls[i].features[(int)Ball.Features.curentTime].ToString() + "sec.";
            LeveTextValues[i].text = balls[i].features[(int)Ball.Features.buyValue].ToString() + "$";
            levelTextTimes[i].text = balls[i].features[(int)Ball.Features.buyTime].ToString() + "$";
        }
    }

    private void CrashEvent(float _curentValue)
    {
        money += _curentValue;
        money = (float)Math.Round(money, 1);
        ShowData();
    }

    private void GetSaveValues(bool get)
    {
        if (get)
        {
            money = PlayerPrefs.GetFloat("money");
        }
        else
        {
            PlayerPrefs.SetFloat("money", money);
        }

        for (int i = 0; i < balls.Length; i++)
        {
            for (int j = 0; j < keyPlayerPrefs.Length; j++)
            {
                string key = keyPlayerPrefs[j] + i.ToString() + j.ToString();
                if (get)
                {
                    if (PlayerPrefs.HasKey(key))
                    {
                        balls[i].features[j] = PlayerPrefs.GetFloat(key);
                    }
                }
                else
                {
                    PlayerPrefs.SetFloat(key, balls[i].features[j]);
                }               
            }          
        }
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        GetSaveValues(false);
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
    }

    public void BuyValue(int i)
    {
        if (money> balls[i].features[(int)Ball.Features.buyValue])
        {
            balls[i].features[(int)Ball.Features.curentValue] = (float)Math.Round(balls[i].features[(int)Ball.Features.curentValue] * coefValue, 2);
            balls[i].features[(int)Ball.Features.buyValue] = (float)Math.Round(balls[i].features[(int)Ball.Features.buyValue] * levelcoefValue, 2);
            money -= balls[i].features[(int)Ball.Features.buyValue];
            ShowData();
        }      
    }

    public void ByTime(int i)
    {
        if (money > balls[i].features[(int)Ball.Features.buyTime])
        {          
            float timeOffset = (float)Math.Round(balls[i].features[(int)Ball.Features.curentTime] * coefTime, 2);
            balls[i].features[(int)Ball.Features.curentTime] -= timeOffset;
            balls[i].features[(int)Ball.Features.buyTime] = (float)Math.Round(balls[i].features[(int)Ball.Features.buyTime] * levelcoefTime, 2);
            money -= balls[i].features[(int)Ball.Features.buyTime];
            ShowData();
        }        
    }
}
