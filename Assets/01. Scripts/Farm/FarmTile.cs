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

    public List<SkinnedMeshRenderer> lands = new();
    public Color unWaterColor;
    public Color waterColor;

    public GameObject binil;
    private bool isBinilActive;

    private void Start()
    {
        InvokeRepeating(nameof(DecreaseHumidity), 5f, 5f);
    }

    private void DecreaseHumidity()
    {
        humidity += GamePlayManager.Instance.humidity;
    }

    public void SetBinil()
    {
        isBinilActive = !isBinilActive;
        binil.SetActive(isBinilActive);
    }

    private void Update()
    {
        foreach (SkinnedMeshRenderer land in lands)
        {
            land.materials[1].SetColor("_Color", Color.Lerp(land.materials[1].GetColor("_Color"), humidity > 50f ? waterColor : unWaterColor, Time.deltaTime * 10f));
        }

    }
}
