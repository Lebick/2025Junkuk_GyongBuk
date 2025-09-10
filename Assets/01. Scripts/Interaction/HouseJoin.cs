using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseJoin : MonoBehaviour, IInteractable, IMouseOver
{
    private GamePlayManager gamePlayManager;
    private FadeManager fadeManager;
    private UIManager uiManager;
    private CameraManager cameraManager;

    public Animator enterAnim;
    public Transform enterTransform;

   private void Start()
    {
        gamePlayManager = GamePlayManager.Instance;
        fadeManager = FadeManager.Instance;
        uiManager = UIManager.Instance;
        cameraManager = CameraManager.Instance;
    }

    public bool Interaction()
    {
        StartCoroutine(HouseEnter());

        return true;
    }

    private IEnumerator HouseEnter()
    {
        enterAnim.SetTrigger("Show");

        Transform lastTr = cameraManager.targetTransform;

        cameraManager.CameraAttach(enterTransform);

        yield return new WaitForSeconds(0.5f);

        Color start = Color.black;
        Color end = Color.black;
        start.a = 0f;

        fadeManager.SetFade(start, end, 0.5f, () =>
        {
            cameraManager.CameraAttach(lastTr);
            gamePlayManager.isHouse = true;
            gamePlayManager.teleportName = "HouseTeleport";
            SceneManager.LoadScene("House");
            fadeManager.SetFade(end, start, 1f);
        });

    }

    public void MouseOverEvent()
    {
        uiManager.SetAlert("집에 들어가기");
    }
    public void MouseOutEvent()
    {
        uiManager.SetAlert(string.Empty);
    }
}
