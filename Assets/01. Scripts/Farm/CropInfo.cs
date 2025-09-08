using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropInfo", menuName = "CropInfo")]
public class CropInfo : ScriptableObject
{
    public string myName;

    [TextArea] public string myDescription;

    public GameObject myPrefab;

    public int buyPrice;

    public int minSellPrice;
    public int maxSellPrice;
    public int sellPrice;

    public List<Weather> allowWeathers = new();

    public int minHumidity;
    public int maxHumidity;

    public int minTime;
    public int maxTime;

    public int requireTime;
}
