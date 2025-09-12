using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IInteractable, IMouseOver
{
    private bool isInteraction;

    public int maxInventoryCount;

    public bool Interaction()
    {
        if (isInteraction)
            return false;

        isInteraction = true;

        CursorManager.Instance.CursorState = CursorState.Free;
        UIManager.Instance.shopAnim.SetTrigger("Open");
        Time.timeScale = 0;

        return true;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
            isInteraction = false;
    }

    public void MouseOverEvent()
    {
        UIManager.Instance.SetAlert("창고 이용하기");
    }
    public void MouseOutEvent()
    {
        UIManager.Instance.SetAlert(string.Empty);
    }
}
