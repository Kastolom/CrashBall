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
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        GetSaveValues(true);
        //SpawnBalls();
        SpawnBall(0);
        ShowData();
    }   

    private void SpawnBalls()
    {
        foreach (var item in balls)
        {
            Ball ball = Instantiate(item);
            ball.CrashBall.AddListener(CrashEvent);
            ball.SpawnBall.AddListener(SpawnBall);
            ball.gameObject.transform.parent = Environment.transform;
        }
    }

    private void SpawnBall(int ballNumber)
    {
        Ball ball = Instantiate(balls[ballNumber]);
        ball.CrashBall.AddListener(CrashEvent);
        ball.SpawnBall.AddListener(SpawnBall);
        ball.gameObject.transform.parent = Environment.transform;
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

            string block;
            if (balls[i].state == Obstruction.State.wait)
            {
                block = "$ locked";
            }
            else
            {
                block = "$";
            }

            TextValues[i].text = curentValue.ToString() + "$";
            TextTimes[i].text = curentTime.ToString() + "sec.";
            LeveTextValues[i].text = buyValue.ToString() + block;
            levelTextTimes[i].text = buyTime.ToString() + block;

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

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            GetSaveValues(false);
        }
        else
        {
            GetSaveValues(true);
        }            
    }

    public void BuyValue(int i)
    {
        if (money> balls[i].features[(int)Ball.Features.buyValue])
        {
            money -= balls[i].features[(int)Ball.Features.buyValue];
            balls[i].features[(int)Ball.Features.curentValue] = balls[i].features[(int)Ball.Features.curentValue] * coefValue;
            balls[i].features[(int)Ball.Features.buyValue] = balls[i].features[(int)Ball.Features.buyValue] * levelcoefValue;
            if (balls[i].state == Obstruction.State.wait)
            {
                balls[i].state = Obstruction.State.timer;
                SpawnBall(i);
            }
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
