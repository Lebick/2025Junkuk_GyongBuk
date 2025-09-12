using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EctCheat : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) //일시정지화면없이일시정지
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

        if (Input.GetKeyDown(KeyCode.F2)) //돈 내놔
        {
            GamePlayManager.Instance.money += 10000;
        }

        if (Time.timeScale == 0) return;

        if (Input.GetKey(KeyCode.F3)) //2배속
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
