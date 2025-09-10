using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TV : MonoBehaviour, IInteractable, IMouseOver
{
    public CanvasGroupSetting canvasGroupSetting;
    public Text newsText;

    private bool isPrinting;

    private readonly Dictionary<Weather, string> weatherText = new()
        {
            {Weather.Clear, "맑을"},
            {Weather.Cloudy, "흐릴"},
            {Weather.Rain, "비가 내릴"},
            {Weather.Stormy, "폭풍이 올"},
            {Weather.Hail, "우박이 내릴"}
        };

    public bool Interaction()
    {
        if (isPrinting) return false;
        isPrinting = true;

        StartCoroutine(ShowTVText());
        return false;
    }

    private WaitForSeconds textWait = new(0.1f);

    private IEnumerator ShowTVText()
    {
        newsText.text = string.Empty;
        canvasGroupSetting.SetAlpha(1f, 1f);

        yield return new WaitForSeconds(0.5f);

        Weather nowWeather = GamePlayManager.Instance.currentWeather;
        Weather nextWeather = GamePlayManager.Instance.nextWeather;

        string content = $"오늘의 날씨는 {weatherText[nowWeather]} 예정이며...\n" +
            $"...내일{(nowWeather == nextWeather ? "도" : "은")} {weatherText[nextWeather]} 예정입니다.";

        for(int i=1; i<=content.Length; i++)
        {
            newsText.text = content[..i];
            yield return textWait;
        }

        GamePlayManager.Instance.isKnowCurrentWeather = true;
        GamePlayManager.Instance.isKnowNextWeather = true;

        yield return new WaitForSeconds(3f);

        canvasGroupSetting.SetAlpha(0f, 5f);

        isPrinting = false;
    }

    public void MouseOverEvent()
    {
        UIManager.Instance.SetAlert("날씨 확인하기");
    }
    public void MouseOutEvent()
    {
        UIManager.Instance.SetAlert(string.Empty);
    }
}
