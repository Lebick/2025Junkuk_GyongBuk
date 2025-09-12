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
            {Weather.Clear, "����"},
            {Weather.Cloudy, "�帱"},
            {Weather.Rain, "�� ����"},
            {Weather.Stormy, "��ǳ�� ��"},
            {Weather.Hail, "����� ����"}
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

        string content = $"������ ������ {weatherText[nowWeather]} �����̸�...\n" +
            $"...����{(nowWeather == nextWeather ? "��" : "��")} {weatherText[nextWeather]} �����Դϴ�.";

        for(int i=1; i<=content.Length; i++)
        {
            newsText.text = content[..i];
            yield return textWait;
        }

        GamePlayManager.Instance.isKnowCurrentWeather = true;
        GamePlayManager.Instance.isKnowNextWeather = true;

        yield return new WaitForSeconds(0.5f);

        canvasGroupSetting.SetAlpha(0f, 1f);
        yield return new WaitForSeconds(1f);
        newsText.text = string.Empty;
        canvasGroupSetting.SetAlpha(1f, 1f);

        yield return new WaitForSeconds(0.5f);

        content = $"���� ���� ���� �۹����� �ü��� �����Ͽ��ٴ� �ҽ��� �ֽ��ϴ�. ���� ������ ...";

        for (int i = 1; i <= content.Length; i++)
        {
            newsText.text = content[..i];
            yield return textWait;
        }

        GamePlayManager.Instance.isCheckSellPrice = true;

        yield return new WaitForSeconds(0.5f);

        canvasGroupSetting.SetAlpha(0f, 1f);

        isPrinting = false;
    }

    public void MouseOverEvent()
    {
        UIManager.Instance.SetAlert("���� Ȯ���ϱ�");
    }
    public void MouseOutEvent()
    {
        UIManager.Instance.SetAlert(string.Empty);
    }
}
