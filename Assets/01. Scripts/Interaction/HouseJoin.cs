using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseJoin : MonoBehaviour, IInteractable, IMouseOver
{
    public Animator enterAnim;
    public Transform enterTransform;

    public bool Interaction()
    {
        StartCoroutine(HouseEnter());

        return true;
    }

    private IEnumerator HouseEnter()
    {
        enterAnim.SetTrigger("Show");
        CameraManager.Instance.CameraAttach(enterTransform);

        yield return new WaitForSeconds(0.5f);

        Color start = Color.black;
        Color end = Color.black;
        start.a = 0f;

        FadeManager.Instance.SetFade(start, end, 0.5f, () =>
        {
            SceneManager.LoadScene("House");
            FadeManager.Instance.SetFade(end, start, 1f);
        });
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
