using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    private GamePlayManager gamePlayManager;
    private CameraManager cameraManager;
    private CursorManager cursorManager;

    private Transform targetPos;

    public Transform normalTargetPos;
    public Transform houseTargetPos;
    public Transform freeTargetPos;

    public Transform realPosition;

    public float rotateSpeed;

    private void Start()
    {
        gamePlayManager = GamePlayManager.Instance;
        cameraManager = CameraManager.Instance;
        cursorManager = CursorManager.Instance;

        cursorManager.CursorState = CursorState.PlayerView;
        cameraManager.CameraAttach(realPosition);
    }

    private void FixedUpdate()
    {
        if (cursorManager.CursorState == CursorState.PlayerView && gamePlayManager.isFarm)
        {
            targetPos = freeTargetPos;
            Rotate();
        }
        else
        {
            if (gamePlayManager.isHouse)
                targetPos = houseTargetPos;
            else
                targetPos = normalTargetPos;
            rotationX = 0;
            rotationY = 0;
            transform.localEulerAngles = new Vector3(30f, 0, 0f);
        }
        SetPosition();
    }

    private float rotationY = 0f;
    private float rotationX = 0f;

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * rotateSpeed;
        rotationX += -mouseY * rotateSpeed;

        rotationX = Mathf.Clamp(rotationX, -30, 30);

        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
    }

    private void SetPosition()
    {
        Vector3 dir = (targetPos.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPos.position);

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, distance, LayerMask.GetMask("Wall")))
        {
            realPosition.position = hit.point - dir;
        }
        else
        {
            realPosition.position = targetPos.position;
        }
    }
}
