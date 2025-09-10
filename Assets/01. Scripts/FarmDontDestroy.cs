using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmDontDestroy : MonoBehaviour
{
    private static GameObject farm;

    private void Awake()
    {
        if (farm == null)
            farm = gameObject;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        farm = null;
    }
}
