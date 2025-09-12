using System.Collections;
using UnityEngine;

public class SpawnAnimal : MonoBehaviour
{
    public ParticleSystem spawnParticle;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.Particle[] particles;

    public SkinnedMeshRenderer animalMesh;

    private void Awake()
    {
        mainModule = spawnParticle.main;
        emissionModule = spawnParticle.emission;
    }

    private void Start()
    {
        Mesh mesh = new();
        animalMesh.BakeMesh(mesh);

        emissionModule.SetBurst(0, new ParticleSystem.Burst(0, mesh.vertexCount));
        mainModule.maxParticles = mesh.vertexCount;
        particles = new ParticleSystem.Particle[mesh.vertexCount];
        spawnParticle.Play();

        StartCoroutine(Spawn(mesh));
    }

    private IEnumerator Spawn(Mesh mesh)
    {
        yield return null;

        spawnParticle.GetParticles(particles);

        for (int i=0; i<particles.Length; i++)
        {
            Vector3 pos = mesh.vertices[i];
            particles[i].position = pos + (pos - Vector3.zero).normalized;
        }

        spawnParticle.SetParticles(particles);

        yield return new WaitForSeconds(1.5f);
        animalMesh.enabled = true;

        yield break;
    }
}
