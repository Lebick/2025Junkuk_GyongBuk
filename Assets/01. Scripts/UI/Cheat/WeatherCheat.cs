using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WeatherCheat : MonoBehaviour
{
    [System.Serializable]
    public struct AngleData
    {
        public float minAngle;
        public float maxAngle;
        public GameObject image;
        public Weather weather;
    }

    private GamePlayManager gamePlayManager;

    public List<AngleData> angleDatas = new();

    public Weather currentSelectWeather;

    public Animator cheatSelector;
    private RectTransform rect;

    private bool isOpen;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        gamePlayManager = GamePlayManager.Instance;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.F5) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            isOpen = true;
            CursorManager.Instance.CursorState = CursorState.Free;
        }

        if (isOpen && Input.GetKeyUp(KeyCode.Mouse0))
        {
            isOpen = false;
            CursorManager.Instance.CursorState = CursorState.PlayerView;

            Select();
        }

        cheatSelector.SetBool("isOpen", isOpen);

        if (isOpen)
        {
            CheckSelected();
        }
    }

    private void CheckSelected()
    {
        Vector2 dir = (rect.anchoredPosition + new Vector2(960, 540) - (Vector2)Input.mousePosition).normalized;
        float currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        currentAngle = Mathf.Repeat(currentAngle + 180, 360f) - 180f;

        foreach (AngleData angleData in angleDatas)
        {

            if (Mathf.Clamp(currentAngle, angleData.minAngle, angleData.maxAngle) == currentAngle)
            {
                currentSelectWeather = angleData.weather;
                angleData.image.GetComponent<Animator>().SetBool("isSelect", true);
            }
            else
            {
                angleData.image.GetComponent<Animator>().SetBool("isSelect", false);
            }
        }
    }

    private void Select()
    {
        gamePlayManager.currentWeather = currentSelectWeather;
    }
}
