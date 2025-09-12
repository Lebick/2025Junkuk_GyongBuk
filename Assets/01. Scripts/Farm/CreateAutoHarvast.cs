using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAutoHarvast : MonoBehaviour
{
    public float startHeight;
    public float endHeight;

    public ParticleSystem spawnParticle;

    public MeshRenderer render1;
    public SkinnedMeshRenderer render2;

    public MeshRenderer preview1;
    public SkinnedMeshRenderer preview2;
    public Gradient previewGradient;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(Test());
        }
    }

    public void Create()
    {
        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        float p = 0f;

        while(p < 1)
        {
            p += Time.deltaTime;
            preview1.material.SetColor("_Color", previewGradient.Evaluate(p));
            preview2.material.SetColor("_Color", previewGradient.Evaluate(p));
            yield return null;
        }

        p = 0f;

        float start = transform.position.y + startHeight;
        float end = transform.position.y + endHeight;

        spawnParticle.Play();

        while (p < 1)
        {
            p += Time.deltaTime / 5f;

            float height = Mathf.Lerp(start, end, p);

            render1.material.SetFloat("_Height", height);
            render2.material.SetFloat("_Height", height);

            Vector3 pos = transform.position;
            pos.y = height;

            spawnParticle.transform.position = pos;

            yield return null;
        }

        spawnParticle.Stop();

        p = 0f;

        while (p < 1)
        {
            p += Time.deltaTime * 4f;
            preview1.material.SetColor("_Color", previewGradient.Evaluate(1 - p));
            preview2.material.SetColor("_Color", previewGradient.Evaluate(1 - p));
            yield return null;
        }
    }
}
