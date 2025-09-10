using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorState
{
    Free,
    PlayerView
}

public class CursorManager : Singleton<CursorManager>
{
    private CursorState _cursorState;

    public CursorState CursorState
    {
        get => _cursorState;
        set
        {
            switch (value)
            {
                case CursorState.Free:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;

                case CursorState.PlayerView:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
            }

            _cursorState = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }
}
