using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTool : MonoBehaviour
{
    public Transform rake; //°¥Äû
    public Transform water; //¹°»Ñ¸®°³

    public ParticleSystem spawnParticle;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.Particle[] particles;

    public ParticleSystem removeParticle;

    private Transform currentTransform;

    private void Start()
    {
        mainModule = spawnParticle.main;
        emissionModule = spawnParticle.emission;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            SpawningTool(rake);

        if (Input.GetKeyDown(KeyCode.J))
            rake.gameObject.SetActive(false);


        if(currentTransform != null)
        {
            spawnParticle.transform.position = currentTransform.position;
            spawnParticle.transform.rotation = currentTransform.rotation;
        }
    }

    public void SpawningTool(Transform tr)
    {
        Mesh mesh = tr.GetComponent<MeshFilter>().mesh;

        emissionModule.SetBurst(0, new ParticleSystem.Burst(0, mesh.vertexCount));
        mainModule.maxParticles = mesh.vertexCount;
        particles = new ParticleSystem.Particle[mesh.vertexCount];
        spawnParticle.Play();

        currentTransform = tr;

        StartCoroutine(Test(tr));
    }

    public void RemoveTool(Transform tr)
    {
        tr.gameObject.SetActive(false);
        removeParticle.Play();
    }

    private IEnumerator Test(Transform tr)
    {
        Mesh mesh = tr.GetComponent<MeshFilter>().mesh;

        yield return null;
        spawnParticle.GetParticles(particles);

        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 pos = mesh.vertices[i];
            particles[i].position = pos + (pos - Vector3.zero).normalized;
        }

        spawnParticle.SetParticles(particles);

        yield return new WaitForSeconds(1.5f);

        tr.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        currentTransform = null;
    }
}
