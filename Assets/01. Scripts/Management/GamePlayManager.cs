using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool isCheckSellPrice;

    public List<CropInfo> cropInfos = new();

    public List<FarmTile> activeFarmTiles = new();

    public GameObject hailDestroyParticle;

    private bool isEnd;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);

        InvokeRepeating(nameof(HailEvent), 5f, 5f);
    }

    private void HailEvent()
    {
        if (currentWeather != Weather.Hail) return;

        int randomEvent = Random.Range(0, 100);

        if (randomEvent >= 2) return;

        int randomTile = Random.Range(0, activeFarmTiles.Count);

        FarmLand[] lands = activeFarmTiles[randomTile].childLands.Where(a => a.currentCrop != null).OrderBy(a => Random.value).ToArray();

        for(int i=0; i<Mathf.Min(lands.Length, 2); i++)
        {
            lands[i].Harvast();
            Instantiate(hailDestroyParticle, lands[i].transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        int[] times = GetTime();

        skyBox.SetColor("_Tint", skyBoxGradient.Evaluate((times[1] + (times[2] / 60f)) / 24f));

        inGameTime += Time.deltaTime;

        if(lastWeatherChangeDay != times[0])
        {
            lastWeatherChangeDay = times[0];
            ChangeWeather(times[0]);
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


        if(money >= 100000 && !isEnd)
        {
            print("縛註 內內內內");

            isEnd = true;

            Color start = Color.black;
            Color end = Color.black;
            start.a = 0;

            FadeManager.Instance.SetFade(start, end, 1f, () =>
            {
                SceneManager.LoadScene("Ranking");
            });
        }
    }

    public void ChangeWeather(int time)
    {
        isCheckSellPrice = false;

        foreach(var crop in cropInfos)
            crop.SetRandomPrice(time == 0);

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

    public void SetTime(int[] time)
    {
        int totalSeconds = 0;
        totalSeconds += time[0] * 86400;
        totalSeconds += time[1] * 3600;
        totalSeconds += time[2] * 60;
        totalSeconds += time[3];

        inGameTime = (float)totalSeconds / 720f;
    }
}
