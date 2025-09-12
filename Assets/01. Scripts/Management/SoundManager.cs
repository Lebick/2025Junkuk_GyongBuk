using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource weatherBGMSource;
    public AudioSource weatherSFXSource;

    public AudioClip testClip;
    public AudioClip testWeather;
    public AudioClip testWeather2;

    public float testPitch;
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    public void SetBGM(AudioClip clip)
    {
        bgmSource.clip = clip;

    }

    public void SetSFX(AudioClip clip, float pitch = 1f)
    {
        if(pitch == 1)
        {
            sfxSource.PlayOneShot(clip, 1f);
        }
        else
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.pitch = pitch;
            newSource.clip = clip;
            newSource.Play();
            //print(clip.length + "\n" + clip.length / pitch);
            Destroy(newSource, clip.length / pitch);
        }
    }

    public void SetWeatherBGM(AudioClip clip)
    {
        if (weatherCoroutine != null)
            StopCoroutine(weatherCoroutine);

        if(clip == null)
        {
            weatherBGMSource.Stop();
            return;
        }

        weatherCoroutine = LoopWeatherBGM(clip);
        StartCoroutine(weatherCoroutine);
    }

    public void SetWeatherSFX(AudioClip clip)
    {
        weatherSFXSource.PlayOneShot(clip, 2f);
    }

    private WaitForSecondsRealtime waitTime;
    private IEnumerator weatherCoroutine;

    private IEnumerator LoopWeatherBGM(AudioClip clip)
    {
        waitTime = new WaitForSecondsRealtime(clip.length);
        weatherBGMSource.Stop();

        while (true)
        {
            weatherBGMSource.PlayOneShot(clip, 5f);
            yield return waitTime;
        }
    }
}
