using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWater : MonoBehaviour, IInteractable, IMouseOver
{
    private GamePlayManager gamePlayManager;
    private FadeManager fadeManager;

    private bool isInteraction;

    private void Start()
    {
        gamePlayManager = GamePlayManager.Instance;
        fadeManager = FadeManager.Instance;
    }

    public bool Interaction()
    {
        if(isInteraction)
            return false;

        isInteraction = true;

        Color start = Color.black;
        Color end = Color.black;
        start.a = 0f;

        fadeManager.SetFade(start, end, 1f, () =>
        {
            PlayerTool tool = gamePlayManager.playerController.GetComponent<PlayerTool>();
            tool.waterUseCount = 0;
            fadeManager.SetFade(end, start, 1f, () =>
            {
                isInteraction = false;
            });
        });

        return true;
    }

    public void MouseOverEvent()
    {
        UIManager.Instance.SetAlert("¹° ±å±â");
    }
    public void MouseOutEvent()
    {
        UIManager.Instance.SetAlert(string.Empty);
    }
}
