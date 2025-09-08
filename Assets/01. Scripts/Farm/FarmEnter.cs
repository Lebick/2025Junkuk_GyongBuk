using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GamePlayManager.Instance.isFarm = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GamePlayManager.Instance.isFarm = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GamePlayManager.Instance.isFarm = false;
        }
    }
}
