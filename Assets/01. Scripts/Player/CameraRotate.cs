using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform targetPos;
    public Transform cameraTrasnform;

    public float rotateSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Rotate();
        SetPosition();
    }

    private float rotationX = 0f; // X축 누적 회전 값
    private float rotationY = 0f; // Y축 누적 회전 값

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX -= mouseY * rotateSpeed; // 마우스 Y 이동은 X축 회전
        rotationY += mouseX * rotateSpeed; // 마우스 X 이동은 Y축 회전

        // X축 회전 제한 (-80 ~ 80)
        rotationX = Mathf.Clamp(rotationX, -80f, 30f);

        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
    }

    private void SetPosition()
    {
        Vector3 dir = (targetPos.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPos.position);

        Debug.DrawRay(transform.position, dir, Color.red, 5f);
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, distance, LayerMask.GetMask("Wall")))
        {
            cameraTrasnform.position = hit.point;
        }
        else
        {
            cameraTrasnform.position = targetPos.position;
        }
    }
}
