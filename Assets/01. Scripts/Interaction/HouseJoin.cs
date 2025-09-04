using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseJoin : MonoBehaviour, IInteractable, IMouseOver
{
    public bool Interaction()
    {
        Color start = Color.black;
        Color end = Color.black;
        start.a = 0f;

        FadeManager.Instance.SetFade(start, end, 1f, () =>
        {
            SceneManager.LoadScene("House");
            FadeManager.Instance.SetFade(end, start, 1f);
        });

        return true;
    }

    public void MouseOverEvent()
    {
        UIManager.Instance.SetAlert("집에 들어가기");
    }
    public void MouseOutEvent()
    {
        UIManager.Instance.SetAlert(string.Empty);
    }
}
