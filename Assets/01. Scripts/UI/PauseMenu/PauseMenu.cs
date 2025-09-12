using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isPause;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            Time.timeScale = isPause ? 0 : 1;
            CursorManager.Instance.CursorState = isPause ? CursorState.Free : CursorState.PlayerView;
            pauseMenu.SetActive(isPause);
        }

    }

    public void OnClickBackButton()
    {
        isPause = false;
        Time.timeScale = 1;
        CursorManager.Instance.CursorState = CursorState.PlayerView;
        pauseMenu.SetActive(isPause);
    }
}
