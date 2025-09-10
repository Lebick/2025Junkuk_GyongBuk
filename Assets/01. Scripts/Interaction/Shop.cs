using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable, IMouseOver
{
    private GamePlayManager gamePlayManager;
    private FadeManager fadeManager;

    private bool isInteraction;

    public GameObject shopUI;

    private void Start()
    {
        gamePlayManager = GamePlayManager.Instance;
        fadeManager = FadeManager.Instance;
    }

    public bool Interaction()
    {
        if (isInteraction)
            return false;

        isInteraction = true;

        shopUI.gameObject.SetActive(true);
        Time.timeScale = 0;

        return true;
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
