using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class DataManager : MonoBehaviour
{
    [SerializeField] GameObject Environment;
    [SerializeField] Text TextMoney;
    [SerializeField] Text TextDiamond;
    [SerializeField] Text[] TextValueGold;
    [SerializeField] Text[] LevelGold;
    [SerializeField] Text[] TextChanceGold;
    [SerializeField] Text[] TextValues;
    [SerializeField] Text[] TextTimes;
    [SerializeField] Text[] LeveTextValues;
    [SerializeField] Text[] levelTextTimes;
    [SerializeField] Text[] levelValueCount;
    [SerializeField] Text[] levelTimeCount;
    [SerializeField] Ball[] balls;

    [SerializeField] string[] keyPlayerPrefs;
    [SerializeField] float coefValue;
    [SerializeField] float coefTime;
    [SerializeField] float levelcoefValue;
    [SerializeField] float levelcoefTime;
    [SerializeField] float coefChanceGold;
    //[SerializeField] float coefCostGold;

    private float money;
    private int diamond;

    [SerializeField] private int[] costDiamond;
    [SerializeField] private int[] levelDiamond;
    [SerializeField] private int increment;

    private List<Ball> sceneBalls = new List<Ball>();

    private void Start()
    {
        diamond = 5;
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i] = balls[i];
        }
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        GetSaveValues(true);
        //SpawnBall(0);
        SpawnBalls();
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
        TextDiamond.text = diamond.ToString();


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
            levelValueCount[i].text = "level" + balls[i].features[(int)Features.levelValue].ToString();
            levelTimeCount[i].text = "level" + balls[i].features[(int)Features.levelTime].ToString();

            balls[i].features[(int)Features.curentValue] = curentValue;
            balls[i].features[(int)Features.curentTime] = curentTime;
            balls[i].features[(int)Features.buyValue] = buyValue;
            balls[i].features[(int)Features.buyTime] = buyTime;
        }

        for (int i = 0; i < costDiamond.Length; i++)
        {
            TextValueGold[i].text = costDiamond[i].ToString();
            LevelGold[i].text = levelDiamond[i].ToString();
            TextChanceGold[i].text = Math.Round(balls[i].features[(int)Features.chanceGold], 2).ToString();
        }
    }

    private void CrashEvent(float _curentValue)
    {
        money += _curentValue;
        ShowData();
    }

    private void PlayerPrefsfFloat(bool _get, string key, ref float value)
    {
        if (_get)
        {
            if (PlayerPrefs.HasKey(key))
            {
                value = PlayerPrefs.GetFloat(key);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }
    }

    private void PlayerPrefsfInt(bool _get, string key, ref int value)
    {
        if (_get)
        {
            if (PlayerPrefs.HasKey(key))
            {
                value = PlayerPrefs.GetInt(key);
            }
        }
        else
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
    }

    private void GetSaveValues(bool get)
    {
        for (int i = 0; i < costDiamond.Length; i++)
        {
            string diamondKey = "diamondCost" + i.ToString();
            PlayerPrefsfInt(get, diamondKey, ref costDiamond[i]);
        }

        PlayerPrefsfFloat(get, "money", ref money);
        PlayerPrefsfInt(get, "diamond", ref diamond);

        for (int i = 0; i < balls.Length; i++)
        {
            string stateKey = "state" + i.ToString();
            int curentState = (int)balls[i].state;
            PlayerPrefsfInt(get, stateKey, ref curentState);
            balls[i].state = (State)curentState;

            for (int j = 0; j < keyPlayerPrefs.Length; j++)
            {
                string key = keyPlayerPrefs[j] + i.ToString() + j.ToString();
                PlayerPrefsfFloat(get, key, ref balls[i].features[j]);             
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
        if (money >= balls[i].features[(int)Features.buyValue])
        {
            money -= balls[i].features[(int)Features.buyValue];
            balls[i].features[(int)Features.curentValue] = balls[i].features[(int)Features.curentValue] * coefValue;
            balls[i].features[(int)Features.buyValue] = balls[i].features[(int)Features.buyValue] * levelcoefValue;
            balls[i].features[(int)Features.levelValue] += 1f;
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
        if (money >= balls[i].features[(int)Features.buyTime])
        {
            money -= balls[i].features[(int)Features.buyTime];
            float timeOffset = balls[i].features[(int)Features.curentTime] * coefTime;
            balls[i].features[(int)Features.curentTime] -= timeOffset;
            balls[i].features[(int)Features.buyTime] = balls[i].features[(int)Features.buyTime] * levelcoefTime;
            balls[i].features[(int)Features.levelTime] += 1f;
            ShowData();
        }        
    }

    public void ByChanceGold(int index)
    {
        if (diamond >= costDiamond[index])
        {
            diamond -= costDiamond[index];
            levelDiamond[index] += 1;
            costDiamond[index] += increment;
            increment += 1;

            int length = ((index + 1) * 3) - 1;
            for (int i = length-3; i < length; i++)
            {
                balls[i].features[(int)Features.chanceGold] *= coefChanceGold;
            }
        }
    }
}
