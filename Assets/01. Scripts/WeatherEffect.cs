using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class WeatherEffect : MonoBehaviour
{
    //¸¼À½, Èå¸², ºñ, ÆøÇ³, ¿ì¹Ú

    public ParticleSystem rainParticle;
    public ParticleSystem hailParticle;

    private GamePlayManager gamePlayManager;

    private Weather lastWeather;

    private bool isCloudy;
    private IEnumerator cloudyCoroutine;

    private void Start()
    {
        gamePlayManager = GamePlayManager.Instance;
    }

    private void Update()
    {
        Weather currentWeather = gamePlayManager.currentWeather;

        Vector3 pos = transform.position;
        pos.x = gamePlayManager.playerController.transform.position.x;
        pos.z = gamePlayManager.playerController.transform.position.z;
        transform.position = pos;

        if (currentWeather != lastWeather)
        {
            lastWeather = currentWeather;

            SetWeatherState(currentWeather);
        }
    }

    private void SetWeatherState(Weather weather)
    {
        switch (weather)
        {
            case Weather.Clear:
                rainParticle.Stop();
                hailParticle.Stop();
                break;

            case Weather.Cloudy:
                rainParticle.Stop();
                hailParticle.Stop();
                break;

            case Weather.Rain:
                rainParticle.Play();
                hailParticle.Stop();
                break;

            case Weather.Stormy:
                print("²¦ ÆøÇ³ÀÌ¾ß!!!!!!!");
                rainParticle.Play();
                hailParticle.Stop();
                break;

            case Weather.Hail:
                rainParticle.Stop();
                hailParticle.Play();
                break;
        }

        if (isCloudy && weather != Weather.Cloudy)
        {
            if (cloudyCoroutine != null)
                StopCoroutine(cloudyCoroutine);

            isCloudy = false;
            cloudyCoroutine = SetCloudy(80);
            StartCoroutine(cloudyCoroutine);
        }

        if (!isCloudy && weather == Weather.Cloudy)
        {
            if (cloudyCoroutine != null)
                StopCoroutine(cloudyCoroutine);

            isCloudy = true;
            cloudyCoroutine = SetCloudy(0);
            StartCoroutine(cloudyCoroutine);
        }
    }

    private IEnumerator SetCloudy(float targetValue)
    {
        float start = RenderSettings.fogStartDistance;

        float p = 0f;

        while(p < 1f)
        {
            p += Time.deltaTime / 3f;
            float value = 1 - Mathf.Pow(1 - p, 3f);
            RenderSettings.fogStartDistance = Mathf.Lerp(start, targetValue, value);

            yield return null;
        }

        yield break;
    }
}
