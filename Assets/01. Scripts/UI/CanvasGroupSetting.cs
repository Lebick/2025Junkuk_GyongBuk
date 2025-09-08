using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupSetting : MonoBehaviour
{
    private IEnumerator alphaCoroutine;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetAlpha(float value, float duration)
    {
        if (alphaCoroutine != null)
            StopCoroutine(alphaCoroutine);

        alphaCoroutine = AlphaCoroutine(value, duration);
        StartCoroutine(alphaCoroutine);
    }

    private IEnumerator AlphaCoroutine(float value, float duration)
    {
        float p = 0f;

        float start = canvasGroup.alpha;

        while (p < 1f)
        {
            p += Time.deltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(start, value, p);
            yield return null;
        }

        yield break;
    }
}
