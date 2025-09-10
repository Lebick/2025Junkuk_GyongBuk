using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    private GamePlayManager gamePlayManager;
    private FadeManager fadeManager;

    private bool isInteraction;

    public Animator shopAnim;

    public Animator itemListAnim;
    public Animator upgradeAnim;
    public Animator buyAnim;
    public Animator sellAnim;

    public GameObject inventorySlotPrefab;
    public Transform toolInventoryTr;
    public Transform seedInventoryTr;
    public Transform cropInventoryTr;
    public Text inventoryCount;

    public Animator currentTabAnim;
    public Animator itemListTabAnim;
    public Animator buyTabAnim;
    public Animator sellTabAnim;

    public int maxInventoryCount;

    private bool isFocus;

    private void Awake()
    {
        gamePlayManager = GamePlayManager.Instance;
        fadeManager = FadeManager.Instance;

        InventoryUpdate();
    }

    private void Update()
    {
        CursorManager.Instance.CursorState = CursorState.Free;
    }

    private void OnEnable()
    {
        gamePlayManager.inventory.onItemListChanged.AddListener(InventoryUpdate);
    }

    private void OnDisable()
    {
        gamePlayManager.inventory.onItemListChanged.RemoveListener(InventoryUpdate);
    }

    public void OnClickListButton()
    {
        isFocus = true;
        SetFocus();
        currentTabAnim = itemListTabAnim;
        currentTabAnim.SetTrigger("Open");
        itemListAnim.SetTrigger("Full");
        itemListAnim.transform.SetAsLastSibling();
        itemListAnim.GetComponent<Button>().enabled = false;
    }

    public void OnClickUpgradeButton()
    {
        print("업글");
    }

    public void OnClickBuyButton()
    {
        isFocus = true;
        SetFocus();
        currentTabAnim = buyTabAnim;
        currentTabAnim.SetTrigger("Open");
        buyAnim.SetTrigger("Full");
        buyAnim.transform.SetAsLastSibling();
        buyAnim.GetComponent<Button>().enabled = false;
    }

    public void OnClickSellButton()
    {
        isFocus = true;
        SetFocus();
        currentTabAnim = sellTabAnim;
        currentTabAnim.SetTrigger("Open");
        sellAnim.SetTrigger("Full");
        sellAnim.transform.SetAsLastSibling();
        sellAnim.GetComponent<Button>().enabled = false;
    }

    public void OnClickMainButton()
    {
        isFocus = false;
        SetFocus();
        if (currentTabAnim != null)
            currentTabAnim.SetTrigger("Close");
        ResetMainButtonEnable();
    }

    public void OnClickShopExitButton()
    {
        isFocus = false;
        SetFocus();
        Time.timeScale = 1;
        shopAnim.SetTrigger("Close");
        isInteraction = false;

        ResetMainButtonEnable();
    }

    private void ResetMainButtonEnable()
    {
        itemListAnim.GetComponent<Button>().enabled = true;
        upgradeAnim.GetComponent<Button>().enabled = true;
        buyAnim.GetComponent<Button>().enabled = true;
        sellAnim.GetComponent<Button>().enabled = true;
    }

    private void SetFocus()
    {
        itemListAnim.SetBool("isFocus", isFocus);
        upgradeAnim.SetBool("isFocus", isFocus);
        buyAnim.SetBool("isFocus", isFocus);
        sellAnim.SetBool("isFocus", isFocus);
    }

    private void InventoryUpdate()
    {
        int toolUICount = toolInventoryTr.childCount;
        int toolCount = gamePlayManager.inventory.tools.Count;

        UpdateToolInventory(toolUICount, toolCount);

        int cropUICount = cropInventoryTr.childCount;
        int cropCount = gamePlayManager.inventory.crops.Count;

        UpdateCropInventory(cropUICount, cropCount);

        int seedUICount = seedInventoryTr.childCount;
        int seedCount = gamePlayManager.inventory.seeds.Count;

        UpdateSeedInventory(seedUICount, seedCount);

        inventoryCount.text = $"창고 용량  {gamePlayManager.inventory.GetItemsCount():D2} / {maxInventoryCount}";
    }

    private void UpdateToolInventory(int uiCount, int toolCount)
    {
        if (uiCount < toolCount)
        {
            for (int i = uiCount; i < toolCount; i++)
            {
                Instantiate(inventorySlotPrefab, toolInventoryTr);
            }
        }

        if (uiCount > toolCount)
        {
            for (int i = uiCount; i > toolCount; i--)
            {
                Destroy(toolInventoryTr.GetChild(i - 1).gameObject);
            }
        }

        for (int i = 0; i < toolCount; i++)
        {
            InventorySlotUI ui = toolInventoryTr.GetChild(i).GetComponent<InventorySlotUI>();
            ui.icon.sprite = gamePlayManager.inventory.tools[i].toolSprite;
            ui.count.text = string.Empty;
        }
    }

    private void UpdateCropInventory(int uiCount, int cropCount)
    {
        if (uiCount < cropCount)
        {
            for (int i = uiCount; i < cropCount; i++)
            {
                Instantiate(inventorySlotPrefab, cropInventoryTr);
            }
        }

        if (uiCount > cropCount)
        {
            for (int i = uiCount; i > cropCount; i--)
            {
                Destroy(cropInventoryTr.GetChild(i - 1).gameObject);
            }
        }

        for (int i = 0; i < cropCount; i++)
        {
            InventorySlotUI ui = cropInventoryTr.GetChild(i).GetComponent<InventorySlotUI>();
            ui.icon.sprite = gamePlayManager.inventory.crops[i].cropInfo.cropSprite;
            ui.count.text = $"{gamePlayManager.inventory.crops[i].count}";
        }
    }

    private void UpdateSeedInventory(int uiCount, int seedCount)
    {
        if (uiCount < seedCount)
        {
            for (int i = uiCount; i < seedCount; i++)
            {
                Instantiate(inventorySlotPrefab, seedInventoryTr);
            }
        }

        if (uiCount > seedCount)
        {
            for (int i = uiCount; i > seedCount; i--)
            {
                Destroy(seedInventoryTr.GetChild(i - 1).gameObject);
            }
        }

        for (int i = 0; i < seedCount; i++)
        {
            InventorySlotUI ui = seedInventoryTr.GetChild(i).GetComponent<InventorySlotUI>();
            ui.icon.sprite = gamePlayManager.inventory.seeds[i].seedInfo.seedSprite;
            ui.count.text = $"{gamePlayManager.inventory.seeds[i].count}";
        }
    }
}
