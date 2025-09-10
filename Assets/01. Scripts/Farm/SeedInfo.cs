using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedInfo", menuName = "SeedInfo")]
public class SeedInfo : ScriptableObject
{
    public string myName;

    [TextArea] public string myDescription;

    public CropInfo cropInfo;

    public GameObject myPrefab;

    public Sprite seedSprite;

    public int buyPrice;

    public List<Weather> allowWeathers = new();

    public int minHumidity;
    public int maxHumidity;

    public int minTime;
    public int maxTime;

    public int requireTime;
}
