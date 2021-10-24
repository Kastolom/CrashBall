using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

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
    [SerializeField] float coefChanceGold;

    private float money;
    private List<Ball> sceneBalls = new List<Ball>();
    private Ball[] curentballs = new Ball[9];

    private void Start()
    {
        for (int i = 0; i < curentballs.Length; i++)
        {
            curentballs[i] = balls[i];
        }
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        GetSaveValues(true);
        SpawnBalls();
        //SpawnBall(0);
        ShowData();
    }   

    private void SpawnBalls()
    {
        foreach (var item in balls)
        {
            if (item.state == State.timer)
            {
                Ball ball = Instantiate(item);
                ball.CrashBall.AddListener(CrashEvent);
                ball.SpawnBall.AddListener(SpawnBall);
                ball.gameObject.transform.parent = Environment.transform;
            }         
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
        TextMoney.text = FormatWrite.FormatNumber(money, 10) + " $";
        for (int i = 0; i < balls.Length; i++)
        {
            float curentValue = (float)Math.Round(balls[i].features[(int)Features.curentValue], 1);
            float curentTime = (float)Math.Round(balls[i].features[(int)Features.curentTime], 2);
            float buyValue = (float)Math.Round(balls[i].features[(int)Features.buyValue], 1);
            float buyTime = (float)Math.Round(balls[i].features[(int)Features.buyTime], 1);

            string block;
            if (balls[i].state == State.wait)
            {
                block = "$ locked";
            }
            else
            {
                block = "$";
            }

            TextValues[i].text = FormatWrite.FormatNumber(curentValue ,10) + " $";
            TextTimes[i].text = FormatWrite.FormatNumber(curentTime, 100) + "sec.";
            LeveTextValues[i].text = FormatWrite.FormatNumber(buyValue, 10) + block;
            levelTextTimes[i].text = FormatWrite.FormatNumber(buyTime, 10) + block;

            balls[i].features[(int)Features.curentValue] = curentValue;
            balls[i].features[(int)Features.curentTime] = curentTime;
            balls[i].features[(int)Features.buyValue] = buyValue;
            balls[i].features[(int)Features.buyTime] = buyTime;
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
            money = PlayerPrefs.GetFloat("money", 0f);
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
        if (money> balls[i].features[(int)Features.buyValue])
        {
            money -= balls[i].features[(int)Features.buyValue];
            balls[i].features[(int)Features.curentValue] = balls[i].features[(int)Features.curentValue] * coefValue;
            balls[i].features[(int)Features.buyValue] = balls[i].features[(int)Features.buyValue] * levelcoefValue;
            if (balls[i].state == State.wait)
            {
                balls[i].state = State.timer;
                SpawnBall(i);
            }
            ShowData();
        }      
    }

    public void ByTime(int i)
    {
        if (money > balls[i].features[(int)Features.buyTime])
        {
            money -= balls[i].features[(int)Features.buyTime];
            float timeOffset = balls[i].features[(int)Features.curentTime] * coefTime;
            balls[i].features[(int)Features.curentTime] -= timeOffset;
            balls[i].features[(int)Features.buyTime] = balls[i].features[(int)Features.buyTime] * levelcoefTime;           
            ShowData();
        }        
    }
}
