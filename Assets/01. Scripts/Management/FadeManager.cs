using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    private IEnumerator fadeCoroutine;

    public Image fadeImage;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void SetFade(Color start, Color end, float duration, Action endAction = null, bool isUnscaledDeltaTime = false)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = FadeCoroutine(start, end, duration, endAction, isUnscaledDeltaTime);
        StartCoroutine(fadeCoroutine);
    }

    private IEnumerator FadeCoroutine(Color start, Color end, float duration, Action endAction, bool isUnscaledDeltaTime)
    {
        float p = 0f;

        while(p < 1f)
        {
            if(isUnscaledDeltaTime)
                p += Time.unscaledDeltaTime / duration;
            else
                p += Time.deltaTime / duration;

            fadeImage.color = Color.Lerp(start, end, p);

            yield return null;
        }

        endAction?.Invoke();
    }
}
