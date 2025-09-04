using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text alertMessage;

    public void SetAlert(string message)
    {
        alertMessage.text = message;
    }
}
