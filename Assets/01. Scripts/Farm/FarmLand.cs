using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmLand : MonoBehaviour
{
    public SeedInfo currentCrop;
    public FarmTile parentTile;

    private Transform cropTransform;

    private float growTimer = 0;

    public GameObject alertGameObject;
    public Text alertTitle;
    public Text alertStopCause;
    public Text alertGrowthGauge;

    public bool isPlowing;

    public void PlantCrop(SeedInfo crop)
    {
        growTimer = 0;
        currentCrop = crop;
        cropTransform = Instantiate(crop.myPrefab, transform).transform;
    }

    private void Update()
    {
        alertGameObject.SetActive(currentCrop != null);

        if (currentCrop == null)
            return;

        alertStopCause.gameObject.SetActive(!IsGrowAble());
        alertTitle.text = $"{currentCrop.myName} 작물 성장중... {(IsGrowAble() ? string.Empty : "<color=#aa0000>(멈춤)</color>")}";

        if (IsGrowAble())
        {
            growTimer += Time.deltaTime * GamePlayManager.Instance.growthGauge;
        }

        growTimer = Mathf.Clamp(growTimer, 0, currentCrop.requireTime);
        cropTransform.localScale = new Vector3(1, growTimer / currentCrop.requireTime, 1);

        alertGrowthGauge.text = $"성장도 : {(int)(growTimer / currentCrop.requireTime * 100)}%";
    }

    public bool IsCanHarvast()
    {
        if (currentCrop == null)
            return false;

        return growTimer >= currentCrop.requireTime;
    }

    public void Harvast()
    {
        currentCrop = null;
        Destroy(cropTransform.gameObject);
        isPlowing = false;
    }

    private bool IsGrowAble()
    {
        bool isGrowAble = true;
        string causeText = "사유 : ";

        if (!currentCrop.allowWeathers.Contains(GamePlayManager.Instance.currentWeather))
        {
            causeText += "날씨  ";
        }

        if (parentTile.humidity < 30 || parentTile.humidity > 80)
        {
            causeText += $"<color=red>습도 부족/과다({parentTile.humidity}%)</color>  ";
            growTimer -= Time.deltaTime;
            isGrowAble = false;
        }
        else if (Mathf.Clamp(parentTile.humidity, currentCrop.minHumidity, currentCrop.maxHumidity) != parentTile.humidity)
        {
            causeText += $"습도({parentTile.humidity}%)  ";
            isGrowAble = false;
        }

        int minTime = currentCrop.minTime;
        int maxTime = currentCrop.maxTime;

        int currentTime = GamePlayManager.Instance.GetTime()[1];

        if (minTime < maxTime)
        {
            if (currentTime < minTime || currentTime > maxTime)
            {
                causeText += "시간  ";
                isGrowAble = false;
            }
        }
        else
        {
            if (currentTime < minTime && currentTime > maxTime)
            {
                causeText += "시간  ";
                isGrowAble = false;
            }
        }

        alertStopCause.text = causeText;

        return isGrowAble;
    }
}