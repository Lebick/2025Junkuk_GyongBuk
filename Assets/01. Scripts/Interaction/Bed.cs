using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour, IInteractable, IMouseOver
{
    public PlayerController player;

    public GameObject bedUI;

    public CanvasGroupSetting canvasGroupSetting;

    public int sleepTime;

    public RectTransform hourLine;
    public Text timeText;
    public Image timeCircle;

    private float timeCircleValue;

    private Transform lastCameraTransform;
    public Transform bedCamera;

    private bool isInteraction = false;

    private void Start()
    {
        canvasGroupSetting.GetComponent<CanvasGroup>().alpha = 0;
    }

    public bool Interaction()
    {
        if (isInteraction)
            return false;

        isInteraction = true;

        bedUI.SetActive(true);

        lastCameraTransform = CameraManager.Instance.targetTransform;
        CameraManager.Instance.CameraAttach(bedCamera);
        CursorManager.Instance.CursorState = CursorState.Free;
        canvasGroupSetting.SetAlpha(1f, 0.5f);

        return true;
    }

    public void SetSleepTime(int value)
    {
        sleepTime += value;
        sleepTime = Mathf.Clamp(sleepTime, 1, 12);
    }

    public void Accept()
    {
        StartCoroutine(SleepCoroutine());
    }

    private IEnumerator SleepCoroutine()
    {
        Color start = Color.black;
        Color end = Color.black;
        start.a = 0f;

        FadeManager.Instance.SetFade(start, end, 1f);
        yield return new WaitForSeconds(1.5f);

        GamePlayManager gameManager = GamePlayManager.Instance;

        gameManager.fatigue -= Mathf.Min(10, sleepTime) * 10;
        gameManager.fatigue = Mathf.Max(0, gameManager.fatigue);

        gameManager.fatigue -= gameManager.isClearBuff ? 30 : 0;

        Cancel();

        float p = 0f;
        float startTime = gameManager.inGameTime;

        while (p < 1f)
        {
            p += Time.deltaTime / 2f;
            float value = 1 - Mathf.Pow(1 - p, 3f);
            float addTime = sleepTime * 5 * Mathf.Clamp01(value);
            gameManager.inGameTime = startTime + addTime;

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        FadeManager.Instance.SetFade(end, start, 1f);
    }

    public void Cancel()
    {
        CameraManager.Instance.CameraAttach(lastCameraTransform);
        CursorManager.Instance.CursorState = CursorState.PlayerView;
        canvasGroupSetting.SetAlpha(0f, 0.5f);

        isInteraction = false;
    }

    public void Sleep()
    {
        Color start = Color.black;
        Color end = Color.black;
        start.a = 0;

        FadeManager.Instance.SetFade(start, end, 1f, () =>
        {
            FadeManager.Instance.SetFade(end, start, 1f);
        });
    }

    private void Update()
    {
        timeText.text = $"{sleepTime} : 00";
        timeCircleValue = Mathf.Lerp(timeCircleValue, sleepTime / 12f, Time.deltaTime * 5f);

        timeCircle.fillAmount = timeCircleValue;
        hourLine.localRotation = Quaternion.Euler(0, 0, timeCircleValue * -360f + 90);
    }

    public void MouseOverEvent()
    {
        UIManager.Instance.SetAlert("¿·¿⁄±‚");
    }
    public void MouseOutEvent()
    {
        UIManager.Instance.SetAlert(string.Empty);
    }
}
