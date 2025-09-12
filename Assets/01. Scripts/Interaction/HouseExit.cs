using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseExit : MonoBehaviour, IInteractable, IMouseOver
{
    private GamePlayManager gamePlayManager;
    private FadeManager fadeManager;
    private UIManager uiManager;

    public Animator enterAnim;
    public Transform enterTransform;

    private bool isInteraction;

    private void Start()
    {
        gamePlayManager = GamePlayManager.Instance;
        fadeManager = FadeManager.Instance;
        uiManager = UIManager.Instance;
    }

    public bool Interaction()
    {
        if (isInteraction) return false;
        isInteraction = true;

        Color start = Color.black;
        Color end = Color.black;
        start.a = 0f;

        fadeManager.SetFade(start, end, 2f, () =>
        {
            gamePlayManager.isHouse = false;
            gamePlayManager.teleportName = "HouseTeleport";
            SceneManager.LoadScene("GameScene");
            fadeManager.SetFade(end, end, 0.5f, () =>
            {
                fadeManager.SetFade(end, start, 1f);
            });
        });

        return true;
    }

    public void MouseOverEvent()
    {
        uiManager.SetAlert("³ª°¡±â");
    }
    public void MouseOutEvent()
    {
        uiManager.SetAlert(string.Empty);
    }
}
