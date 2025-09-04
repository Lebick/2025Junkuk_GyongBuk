using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject currentInteraction;

    private void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5f, LayerMask.GetMask("Interactable")))
        {
            currentInteraction = hit.transform.gameObject;

            if (hit.transform.TryGetComponent(out IMouseOver mouseOver))
                mouseOver.MouseOverEvent();
        }
        else
        {
            FailedFindInteraction();
            return;
        }

        if (Input.GetKeyDown(KeyCode.F) && currentInteraction != null)
        {
            currentInteraction.GetComponent<IInteractable>().Interaction();
        }
    }

    private void FailedFindInteraction()
    {
        if(currentInteraction != null)
        {
            if(currentInteraction.TryGetComponent(out IMouseOver mouseOver))
                mouseOver.MouseOutEvent();

            currentInteraction = null;
        }
    }
}
