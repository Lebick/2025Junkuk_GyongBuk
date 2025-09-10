using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelector : MonoBehaviour
{
    [System.Serializable]
    public struct AngleData
    {
        public float minAngle;
        public float maxAngle;
        public GameObject image;
        public ToolInfo toolInfo;
    }

    private GamePlayManager gamePlayManager;

    private bool isOpen = false;

    public List<AngleData> angleDatas = new();

    public ToolInfo currentSelectTool;

    public Animator toolSelectorAnim;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        gamePlayManager = GamePlayManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftControl))
        {
            isOpen = true;
            CursorManager.Instance.CursorState = CursorState.Free;
        }

        if (isOpen && Input.GetKeyUp(KeyCode.Mouse0))
        {
            isOpen = false;
            CursorManager.Instance.CursorState = CursorState.PlayerView;

            if(currentSelectTool != null)
                Select();
        }

        toolSelectorAnim.SetBool("isOpen", isOpen);

        if (isOpen)
        {
            CheckSelected();
        }
    }

    private void CheckSelected()
    {
        Vector2 dir = (rect.anchoredPosition + new Vector2(960, 540) - (Vector2)Input.mousePosition).normalized;
        float currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        foreach (AngleData angleData in angleDatas)
        {
            if (gamePlayManager.inventory.IsExistItem(angleData.toolInfo))
                angleData.image.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            else
            {
                angleData.image.transform.GetChild(0).GetComponent<Image>().color = Color.black;
                continue;
            }

            if (Mathf.Clamp(currentAngle, angleData.minAngle, angleData.maxAngle) == currentAngle)
            {
                currentSelectTool = angleData.toolInfo;
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
        gamePlayManager.playerController.currentSeed = null;
        gamePlayManager.playerController.currentTool = currentSelectTool;
    }
}
