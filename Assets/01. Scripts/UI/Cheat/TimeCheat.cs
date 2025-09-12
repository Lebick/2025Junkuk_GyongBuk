using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TimeCheat : MonoBehaviour
{
    [System.Serializable]
    public struct AngleData
    {
        public float minAngle;
        public float maxAngle;
        public GameObject image;
        public int targetTime;
    }

    private GamePlayManager gamePlayManager;

    public List<AngleData> angleDatas = new();

    public int currentSelectTime;

    public Animator cheatSelector;
    private RectTransform rect;

    private bool isOpen;

    private bool isAfter;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        gamePlayManager = GamePlayManager.Instance;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.F4) && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            if (Input.GetMouseButton(1))
                isAfter = true;

            isOpen = true;
            CursorManager.Instance.CursorState = CursorState.Free;
        }

        if (isOpen && ((!isAfter && Input.GetMouseButtonUp(0)) || (isAfter && Input.GetMouseButtonUp(1))))
        {
            isOpen = false;
            
            CursorManager.Instance.CursorState = CursorState.PlayerView;

            Select(isAfter);
            isAfter = false;
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
        float currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 15;
        currentAngle = Mathf.Repeat(currentAngle + 180, 360f) - 180f;

        foreach (AngleData angleData in angleDatas)
        {

            if (Mathf.Clamp(currentAngle, angleData.minAngle, angleData.maxAngle) == currentAngle)
            {
                currentSelectTime = angleData.targetTime;
                angleData.image.GetComponent<Animator>().SetBool("isSelect", true);
            }
            else
            {
                angleData.image.GetComponent<Animator>().SetBool("isSelect", false);
            }
        }
    }

    private void Select(bool isAfter)
    {
        int[] times = gamePlayManager.GetTime();
        times[1] = currentSelectTime + (isAfter ? 12 : 0);
        gamePlayManager.SetTime(times);
    }
}
