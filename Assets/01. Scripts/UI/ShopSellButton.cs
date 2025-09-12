using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellButton : MonoBehaviour
{
    private GamePlayManager gamePlayManager;

    public CropInfo crop;
    public Text itemCount;
    public Text itemPrice;
    public Button sellButton;

    private void Awake()
    {
        gamePlayManager = GamePlayManager.Instance;
        sellButton.onClick.AddListener(Sell);
    }

    private void Update()
    {
        bool isExist = gamePlayManager.inventory.IsExistItem(crop);

        sellButton.interactable = isExist;
        itemCount.text = isExist ? $"{gamePlayManager.inventory.GetCropItem(crop).count}" : "0";
        itemPrice.text = gamePlayManager.isCheckSellPrice ? $"{crop.sellPrice:N0} $" : "??? $";
    }

    private void Sell()
    {
        if (gamePlayManager.inventory.UseCrop(crop))
        {
            gamePlayManager.money += crop.sellPrice;
        }
    }
}
