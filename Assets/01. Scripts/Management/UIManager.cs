using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private GamePlayManager gamePlayManager;

    public Text alertMessage;

    public Text timeText;

    public Image fatigueFill;

    public Image weatherIcon;
    public Image nextWeatherIcon;

    public Sprite clearSprite;
    public Sprite cloudySprite;
    public Sprite rainSprite;
    public Sprite stormySprite;
    public Sprite hailSprite;
    public Sprite untifinedSprite;

    public GameObject inventorySlotPrefab;
    public Transform toolInventoryTr;
    public Transform seedInventoryTr;
    public Transform cropInventoryTr;

    private bool isInventoryOpen;
    public Animator inventoryAnim;

    public Image toolImage;
    public Text toolCount;

    public Transform timeCircle;

    private Dictionary<Weather, Sprite> weatherSprite = new();

    protected override void Awake()
    {
        base.Awake();

        gamePlayManager = GamePlayManager.Instance;

        weatherSprite = new()
        {
            {Weather.Clear, clearSprite},
            {Weather.Cloudy, cloudySprite},
            {Weather.Rain, rainSprite},
            {Weather.Stormy, stormySprite},
            {Weather.Hail, hailSprite}
        };

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        gamePlayManager.inventory.onItemListChanged.AddListener(InventoryUpdate);
    }

    private void OnDisable()
    {
        gamePlayManager.inventory.onItemListChanged.RemoveListener(InventoryUpdate);
    }

    private void LateUpdate()
    {
        int[] times = gamePlayManager.GetTime();

        timeText.text = $"{times[0]}d {times[1]}h {times[2]}m {times[3]}s";

        timeCircle.transform.localEulerAngles = new Vector3(0, 0, (times[1] + (times[2] / 60f)) / 24f * 360 - 180);

        weatherIcon.sprite = gamePlayManager.isKnowCurrentWeather ? weatherSprite[gamePlayManager.currentWeather] : untifinedSprite;
        nextWeatherIcon.sprite = gamePlayManager.isKnowNextWeather ? weatherSprite[gamePlayManager.nextWeather] : untifinedSprite;

        fatigueFill.fillAmount = Mathf.Lerp(fatigueFill.fillAmount, gamePlayManager.fatigue / 100, Time.deltaTime * 5f);

        ToolUpdate();

        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen;

            if (isInventoryOpen)
                inventoryAnim.SetTrigger("Open");
            else
                inventoryAnim.SetTrigger("Close");
        }
    }

    private void ToolUpdate()
    {
        PlayerController playerController = gamePlayManager.playerController;

        bool isTool = playerController.currentTool != null;
        bool isSeed = playerController.currentSeed != null;

        if (isTool)
        {
            toolImage.sprite = playerController.currentTool.toolSprite;
            toolCount.text = $"{(playerController.currentTool.maxUseCount == 0 ? "<size=96>¡Ä</size>" : $"{playerController.currentTool.maxUseCount - playerController.GetComponent<PlayerTool>().waterUseCount} / {playerController.currentTool.maxUseCount}")}";
        }
        else if (isSeed)
        {
            toolImage.sprite = playerController.currentSeed.seedSprite;
            foreach(var seed in gamePlayManager.inventory.seeds)
            {
                if(seed.seedInfo == playerController.currentSeed)
                    toolCount.text = $"{seed.count}";
            }
        }

        toolImage.enabled = isTool || isSeed;
        toolCount.enabled = isTool || isSeed;
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
                Destroy(toolInventoryTr.GetChild(i-1).gameObject);
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
                Destroy(cropInventoryTr.GetChild(i-1).gameObject);
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
                Destroy(seedInventoryTr.GetChild(i-1).gameObject);
            }
        }

        for (int i = 0; i < seedCount; i++)
        {
            InventorySlotUI ui = seedInventoryTr.GetChild(i).GetComponent<InventorySlotUI>();
            ui.icon.sprite = gamePlayManager.inventory.seeds[i].seedInfo.seedSprite;
            ui.count.text = $"{gamePlayManager.inventory.seeds[i].count}";
        }
    }

    public void SetAlert(string message)
    {
        alertMessage.text = message;
    }
}
