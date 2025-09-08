using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    private float _humidity;
    public float humidity
    {
        get => _humidity;
        set => _humidity = Mathf.Clamp(value, 0f, 100f);
    }

    public List<MeshFilter> lands = new();
    public Mesh notWaterLandMesh;
    public Mesh waterLandMesh;

    private void Start()
    {
        InvokeRepeating(nameof(DecreaseHumidity), 5f, 5f);
    }

    private void DecreaseHumidity()
    {
        humidity += GamePlayManager.Instance.humidity;
    }

    private void Update()
    {
        foreach (MeshFilter land in lands)
        {
            land.mesh = humidity > 50f ? waterLandMesh : notWaterLandMesh;
        }

    }
}
