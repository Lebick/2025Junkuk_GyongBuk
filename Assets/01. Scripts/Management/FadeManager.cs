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

    public void SetFade(Color start, Color end, float duration, Action endAction = null)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = FadeCoroutine(start, end, duration, endAction);
        StartCoroutine(fadeCoroutine);
    }

    private IEnumerator FadeCoroutine(Color start, Color end, float duration, Action endAction)
    {
        float p = 0f;

        while(p < 1f)
        {
            p += Time.deltaTime / duration;
            fadeImage.color = Color.Lerp(start, end, p);

            yield return null;
        }

        endAction?.Invoke();
    }
}
