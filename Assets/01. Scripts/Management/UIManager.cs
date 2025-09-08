using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text alertMessage;

    public Text timeText;

    public Image fatigueFill;

    public Image weatherIcon;
    public Image nextWeatherIcon;

    public Sprite clearSprite;
    public Sprite cloudySprite;
    public Sprite rainSprite;
    public Sprite stormySprite;
    public Sprite hailSprite;
    public Sprite untifinedSprite;

    public Transform timeCircle;

    private Dictionary<Weather, Sprite> weatherSprite = new();

    protected override void Awake()
    {
        base.Awake();

        weatherSprite = new()
        {
            {Weather.Clear, clearSprite},
            {Weather.Cloudy, cloudySprite},
            {Weather.Rain, rainSprite},
            {Weather.Stormy, stormySprite},
            {Weather.Hail, hailSprite}
        };
    }

    private void Update()
    {
        GamePlayManager gamePlayManager = GamePlayManager.Instance;

        int[] times = gamePlayManager.GetTime();

        timeText.text = $"{times[0]}d {times[1]}h {times[2]}m {times[3]}s";

        timeCircle.transform.localEulerAngles = new Vector3(0, 0, (times[1] + (times[2] / 60f)) / 24f * 360 - 90);

        weatherIcon.sprite = gamePlayManager.isKnowCurrentWeather ? weatherSprite[gamePlayManager.currentWeather] : untifinedSprite;
        nextWeatherIcon.sprite = gamePlayManager.isKnowNextWeather ? weatherSprite[gamePlayManager.nextWeather] : untifinedSprite;

        fatigueFill.fillAmount = Mathf.Lerp(fatigueFill.fillAmount, gamePlayManager.fatigue / 100, Time.deltaTime * 5f);
    }

    public void SetAlert(string message)
    {
        alertMessage.text = message;
    }
}
