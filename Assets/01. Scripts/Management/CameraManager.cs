using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Transform targetTransform;

    public void CameraAttach(Transform tr)
    {
        targetTransform = tr;
    }

    private void FixedUpdate()
    {
        if (targetTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * 10f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, Time.deltaTime * 10f);
    }
}
