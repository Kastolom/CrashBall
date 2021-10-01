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
        money = (float)Math.Round(money, 1);
        TextMoney.text = money.ToString() + "$";
        for (int i = 0; i < balls.Length; i++)
        {
            float curentValue = (float)Math.Round(balls[i].features[(int)Ball.Features.curentValue], 1);
            float curentTime = (float)Math.Round(balls[i].features[(int)Ball.Features.curentTime], 2);
            float buyValue = (float)Math.Round(balls[i].features[(int)Ball.Features.buyValue], 1);
            float buyTime = (float)Math.Round(balls[i].features[(int)Ball.Features.buyTime], 1);

            TextValues[i].text = curentValue.ToString() + "$";
            TextTimes[i].text = curentTime.ToString() + "sec.";
            LeveTextValues[i].text = buyValue.ToString() + "$";
            levelTextTimes[i].text = buyTime.ToString() + "$";

            balls[i].features[(int)Ball.Features.curentValue] = curentValue;
            balls[i].features[(int)Ball.Features.curentTime] = curentTime;
            balls[i].features[(int)Ball.Features.buyValue] = buyValue;
            balls[i].features[(int)Ball.Features.buyTime] = buyTime;
        }
    }

    private void CrashEvent(float _curentValue)
    {
        money += _curentValue;
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
            PlayerPrefs.Save();
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
                    PlayerPrefs.Save();
                }               
            }          
        }       
    }

    //private void OnApplicationQuit()
    //{
    //    GetSaveValues(false);
    //}

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        GetSaveValues(false);
    }
    public void BuyValue(int i)
    {
        if (money> balls[i].features[(int)Ball.Features.buyValue])
        {
            money -= balls[i].features[(int)Ball.Features.buyValue];
            balls[i].features[(int)Ball.Features.curentValue] = balls[i].features[(int)Ball.Features.curentValue] * coefValue;
            balls[i].features[(int)Ball.Features.buyValue] = balls[i].features[(int)Ball.Features.buyValue] * levelcoefValue;          
            ShowData();
        }      
    }

    public void ByTime(int i)
    {
        if (money > balls[i].features[(int)Ball.Features.buyTime])
        {
            money -= balls[i].features[(int)Ball.Features.buyTime];
            float timeOffset = balls[i].features[(int)Ball.Features.curentTime] * coefTime;
            balls[i].features[(int)Ball.Features.curentTime] -= timeOffset;
            balls[i].features[(int)Ball.Features.buyTime] = balls[i].features[(int)Ball.Features.buyTime] * levelcoefTime;           
            ShowData();
        }        
    }
}
