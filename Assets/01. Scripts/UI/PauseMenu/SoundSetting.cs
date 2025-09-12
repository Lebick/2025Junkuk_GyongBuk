using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider weatherBGMSlider;
    public Slider weatherSFXSlider;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        bgmSlider.value = soundManager.bgmSource.volume;
        sfxSlider.value = soundManager.sfxSource.volume;
        weatherBGMSlider.value = soundManager.weatherBGMSource.volume;
        weatherSFXSlider.value = soundManager.weatherSFXSource.volume;
    }

    public void SetBGMVolume(float value)
    {
        soundManager.bgmSource.volume = bgmSlider.value;
    }

    public void SetSFXVolume(float value)
    {
        soundManager.sfxSource.volume = sfxSlider.value;
    }

    public void SetWeatherBGMVolume(float value)
    {
        soundManager.weatherBGMSource.volume = weatherBGMSlider.value;
    }

    public void SetWeatherSFXVolume(float value)
    {
        soundManager.weatherSFXSource.volume = weatherSFXSlider.value;
    }
}
