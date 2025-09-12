using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyButton : MonoBehaviour
{
    public ToolInfo toolInfo;
    public SeedInfo seedInfo;

    private Button button;
    private Text buttonText;

    private GamePlayManager gamePlayManager;

    private void Start()
    {
        gamePlayManager = GamePlayManager.Instance;
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<Text>();
        button.onClick.AddListener(OnClickBuyButton);
    }

    private void Update()
    {
        if (toolInfo != null)
        {
            bool isExist = gamePlayManager.inventory.IsExistItem(toolInfo);

            buttonText.text = isExist ? "보유중" : "구매";
            button.interactable = !isExist;
        }
    }

    private void OnClickBuyButton()
    {
        if (toolInfo != null)
            BuyTool();
        else if (seedInfo != null)
            BuySeed();
    }

    private void BuyTool()
    {
        if (gamePlayManager.inventory.IsExistItem(toolInfo))
        {
            UIManager.Instance.SetAlert("이미있음");
            return;
        }
        else if (toolInfo.buyPrice > gamePlayManager.money)
        {
            UIManager.Instance.SetAlert("돈없");
            return;
        }

        gamePlayManager.money -= toolInfo.buyPrice;
        gamePlayManager.inventory.AddList(toolInfo);
    }

    private void BuySeed()
    {
        if (seedInfo.buyPrice > gamePlayManager.money)
        {
            UIManager.Instance.SetAlert("돈없");
            return;
        }

        if (gamePlayManager.inventory.AddList(seedInfo))
        {
            gamePlayManager.money -= seedInfo.buyPrice;
        }
    }
}
