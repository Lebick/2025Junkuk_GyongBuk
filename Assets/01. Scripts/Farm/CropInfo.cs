using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropInfo", menuName = "CropInfo")]
public class CropInfo : ScriptableObject
{
    public string myName;

    [TextArea] public string myDescription;

    public Sprite cropSprite;

    public int minSellPrice;
    public int maxSellPrice;
    public int sellPrice;


    public void SetRandomPrice(bool isFirst)
    {
        if (isFirst)
        {
            sellPrice = (minSellPrice + maxSellPrice) / 2;
        }
        else
        {
            int minValue = Mathf.Clamp(sellPrice - 1000, minSellPrice, maxSellPrice);
            int maxValue = Mathf.Clamp(sellPrice + 2000, minSellPrice, maxSellPrice);
            sellPrice = Random.Range(minValue, maxValue + 1);
        }
    }
}
