using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject currentInteraction;

    public float interactionRange;

    private void Update()
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, interactionRange, LayerMask.GetMask("Interactable"));

        if(objs.Length > 0)
        {
            float distance = Mathf.Infinity;

            GameObject nearInteraction = null;

            foreach (Collider obj in objs)
            {
                float currentDistance = Vector3.Distance(transform.position, obj.transform.position);

                if (currentDistance < distance)
                {
                    nearInteraction = obj.gameObject;
                    distance = currentDistance;
                }
            }

            currentInteraction = nearInteraction.transform.gameObject;

            if (nearInteraction.transform.TryGetComponent(out IMouseOver mouseOver))
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
