using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWater : MonoBehaviour, IInteractable, IMouseOver
{
    public bool Interaction()
    {
        return false;
    }

    public void MouseOverEvent()
    {
        UIManager.Instance.SetAlert("�� �⸣��");
    }
    public void MouseOutEvent()
    {
        UIManager.Instance.SetAlert(string.Empty);
    }
}
