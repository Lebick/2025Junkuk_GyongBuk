using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weather
{
    Clear = 0,
    Cloudy = 1,
    Rain = 2,
    Stormy = 3,
    Hail = 4
}

public class GamePlayManager : Singleton<GamePlayManager>
{
    public Weather currentWeather = Weather.Clear;
    public Weather nextWeather = Weather.Clear;

    public bool isKnowCurrentWeather = false;
    public bool isKnowNextWeather = false;

    private int lastWeatherChangeDay = -1;

    public float inGameTime = 0;

    public bool isFarm;
    public bool isHouse;

    public int growthGauge = 1;
    public int humidity = -1;

    public float fatigue;

    public Material skyBox;

    public Gradient skyBoxGradient;

    public bool isClearBuff;

    public string teleportName = string.Empty;

    private PlayerController _playerController;
    public PlayerController playerController
    {
        get
        {
            if(_playerController == null)
                _playerController = FindAnyObjectByType<PlayerController>();

            return _playerController;
        }
        private set { }
    }

    public Inventory inventory;

    public int money = 1000;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        int[] times = GetTime();

        skyBox.SetColor("_Tint", skyBoxGradient.Evaluate((times[1] + (times[2] / 60f)) / 24f));

        inGameTime += Time.deltaTime;

        if(lastWeatherChangeDay != times[0])
        {
            lastWeatherChangeDay = times[0];
            ChangeWeather();
        }

        switch (currentWeather)
        {
            case Weather.Clear:
                growthGauge = 2;
                humidity = -1;
                break;

            case Weather.Cloudy:
                growthGauge = 1;
                humidity = -1;
                break;

            case Weather.Rain:
                growthGauge = 2;
                humidity = +1;
                break;

            case Weather.Stormy:
                growthGauge = -1;
                humidity = -10;
                break;

            case Weather.Hail:
                growthGauge = 1;
                humidity = -15;
                break;
        }
    }

    public void ChangeWeather()
    {
        isClearBuff = currentWeather == Weather.Clear && nextWeather == Weather.Clear;

        currentWeather = nextWeather;
        isKnowCurrentWeather = isKnowNextWeather;

        nextWeather = (Weather)Random.Range(0, 5);
        isKnowNextWeather = false;
    }

    public int[] GetTime()
    {
        float realTime = inGameTime * 720f;

        int dd = (int)(realTime / 86400);
        int hh = (int)(realTime % 86400 / 3600);
        int mm = (int)(realTime % 3600 / 60);
        int ss = (int)(realTime % 60);

        return new int[] { dd, hh, mm, ss };
    }
}
