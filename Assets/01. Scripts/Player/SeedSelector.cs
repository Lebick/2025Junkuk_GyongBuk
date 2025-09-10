using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedSelector : MonoBehaviour
{
    [System.Serializable]
    public struct AngleData
    {
        public float minAngle;
        public float maxAngle;
        public GameObject image;
        public SeedInfo cropInfo;
    }

    private GamePlayManager gamePlayManager;

    private bool isOpen = false;
    public List<AngleData> angleDatas = new();

    public SeedInfo currentSelectCrop;

    public Animator cropSelectorAnim;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        gamePlayManager = GamePlayManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftAlt))
        {
            isOpen = true;
            CursorManager.Instance.CursorState = CursorState.Free;
        }

        if (isOpen && Input.GetKeyUp(KeyCode.Mouse0))
        {
            isOpen = false;
            CursorManager.Instance.CursorState = CursorState.PlayerView;

            if(currentSelectCrop != null)
                Select();
        }

        cropSelectorAnim.SetBool("isOpen", isOpen);

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
            if (gamePlayManager.inventory.IsExistItem(angleData.cropInfo))
                angleData.image.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            else
            {
                angleData.image.transform.GetChild(0).GetComponent<Image>().color = Color.black;
                continue;
            }

            if (Mathf.Clamp(currentAngle, angleData.minAngle, angleData.maxAngle) == currentAngle)
            {
                currentSelectCrop = angleData.cropInfo;
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
        gamePlayManager.playerController.currentTool = null;
        gamePlayManager.playerController.currentSeed = currentSelectCrop;
    }
}
