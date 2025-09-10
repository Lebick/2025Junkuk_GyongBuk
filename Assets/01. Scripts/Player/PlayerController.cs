using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    public Transform model;

    public Animator anim;

    public float moveSpeed = 3f;

    public Material test;

    public ToolInfo currentTool;
    public SeedInfo currentSeed;

    public bool isCantMove;

    private void Awake()
    {
        GamePlayManager gamePlayManager = GamePlayManager.Instance;

        if (gamePlayManager.playerController != this)
        {
            Transform targetPos = GameObject.Find(gamePlayManager.teleportName).transform;

            gamePlayManager.playerController.transform.position = targetPos.position;
            gamePlayManager.playerController.transform.rotation = targetPos.rotation;

            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        
        if(!isCantMove)
            Move(dir);
        else
            anim.SetBool("isMove", false);

        test.SetVector("_Pos", transform.position);
    }

    private void Move(Vector3 dir)
    {
        Vector3 current = transform.eulerAngles;
        float targetY = Camera.main.transform.eulerAngles.y;

        float rotateAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        if (dir != Vector3.zero)
            model.rotation = Quaternion.Lerp(model.rotation, Quaternion.Euler(current.x, targetY + rotateAngle, current.z), Time.deltaTime * 10f);

        bool isRun = Input.GetKey(KeyCode.LeftShift);

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, isRun ? 70f : 60f, Time.deltaTime * 10f);

        Vector3 moveDir = Camera.main.transform.TransformDirection(dir);
        moveDir.y = 0;
        moveDir.Normalize();

        rigidBody.MovePosition(rigidBody.position + moveDir * Time.fixedDeltaTime * (isRun ? 2.5f : 1f) * moveSpeed);

        anim.SetBool("isMove", dir.magnitude != 0);
        anim.SetBool("isRun", isRun);
    }
}
