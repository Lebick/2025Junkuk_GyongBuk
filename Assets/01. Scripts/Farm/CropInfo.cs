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
}
