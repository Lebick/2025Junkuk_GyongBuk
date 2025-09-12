using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMove : MonoBehaviour
{
    public Vector3 center;
    public Vector3 range;

    private float randomTime;
    private float timer;

    private float rotateAngle;

    private void Awake()
    {
        transform.position = GetRandomPos();
        rotateAngle = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(0, rotateAngle, 0);
    }

    private void Start()
    {
        randomTime = Random.Range(2f, 8f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= randomTime)
        {
            randomTime = Random.Range(2f, 8f);
            timer = 0;
            StartCoroutine(MoveRandomPosition(GetRandomPos()));
        }

        transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(transform.eulerAngles.y, rotateAngle, Time.deltaTime * 5f), 0);
    }

    private IEnumerator MoveRandomPosition(Vector3 pos)
    {
        float p = 0f;

        Vector3 startPos = transform.position;
        Vector3 dir = pos - startPos;
        rotateAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        while(p < 1f)
        {
            p += Time.deltaTime / 2f;

            transform.position = Vector3.Lerp(startPos, pos, p);

            yield return null;
        }

        yield break;
    }

    private Vector3 GetRandomPos()
    {
        float x = Random.Range(center.x - range.x * 0.5f, center.x + range.x * 0.5f);
        float y = Random.Range(center.y - range.y * 0.5f, center.y + range.y * 0.5f);
        float z = Random.Range(center.z - range.z * 0.5f, center.z + range.z * 0.5f);

        return new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, range);
    }
}
