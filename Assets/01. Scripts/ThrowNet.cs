using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowNet : MonoBehaviour
{
    public Vector3 startPos;
    public Quaternion startRot;
    public Vector3 offset;
    private Animator anim;
    private Transform parent;

    private void Start()
    {
        parent = transform.parent;
        anim = GetComponent<Animator>();
        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }   

    public void Throw(Transform target)
    {
        transform.parent = null;
        transform.rotation = Quaternion.Euler(0, target.eulerAngles.y, -90);
        StartCoroutine(ThrowAnim(target));
    }

    public void ResetState()
    {
        transform.parent = parent;
        transform.localPosition = startPos;
        transform.localRotation = startRot;
        anim.SetTrigger("Reset");
    }

    private IEnumerator ThrowAnim(Transform target)
    {
        float p = 0f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position + offset;

        while(p < 1f)
        {
            p += Time.deltaTime;
            float value = 1 - Mathf.Pow(1 - p, 3f);

            Vector3 finalPos = Vector3.Lerp(startPos, targetPos, p);
            finalPos.y = Mathf.Lerp(startPos.y, targetPos.y, value);

            transform.position = finalPos;

            yield return null;
        }

        anim.SetTrigger("Sha");

        p = 0f;

        startPos = targetPos;
        targetPos = startPos - offset;

        while(p < 1f)
        {
            p += Time.deltaTime * 2f;
            float value = Mathf.Pow(p, 3f);

            transform.position = Vector3.Lerp(startPos, targetPos, value);

            yield return null;
        }
    }
}
