using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public List<CropItem> crops = new();
    public List<SeedItem> seeds = new();
    public List<ToolInfo> tools = new();

    public int maxItemCount = 20;

    public UnityEvent onItemListChanged = new();

    public bool AddList(CropInfo info)
    {
        if (GetItemsCount() >= maxItemCount)
        {
            UIManager.Instance.SetAlert("공간이 부족하여 아이템을 획득할 수 없습니다.");
            return false;
        }

        bool flag = false;

        foreach (var crop in crops)
        {
            if (crop.cropInfo == info)
            {
                crop.count++;
                flag = true;
            }
        }

        if (!flag)
        {
            crops.Add(new CropItem(info, 1));
        }

        onItemListChanged?.Invoke();

        return true;
    }

    public bool AddList(SeedInfo info)
    {
        if (GetItemsCount() >= maxItemCount)
        {
            UIManager.Instance.SetAlert("공간이 부족하여 아이템을 획득할 수 없습니다.");
            return false;
        }

        bool flag = false;

        foreach (var seed in seeds)
        {
            if (seed.seedInfo == info)
            {
                seed.count++;
                flag = true;
            }
        }

        if (!flag)
        {
            seeds.Add(new SeedItem(info, 1));
        }

        onItemListChanged?.Invoke();

        return true;
    }

    public void AddList(ToolInfo info)
    {
        onItemListChanged?.Invoke();

        tools.Add(info);
    }

    public bool IsExistItem(SeedInfo info)
    {
        foreach (var seed in seeds)
            if (seed.seedInfo == info)
                return true;

        return false;
    }

    public bool IsExistItem(ToolInfo info)
    {
        foreach (var tool in tools)
            if (tool == info)
                return true;

        return false;
    }

    public bool UseSeed(SeedInfo info)
    {
        foreach (var seed in seeds)
        {
            if (seed.seedInfo == info)
            {
                seed.count--;

                if (seed.count <= 0)
                    seeds.Remove(seed);

                onItemListChanged?.Invoke();

                return true;
            }
        }

        return false;
    }

    public int GetItemsCount()
    {
        int count = 0;

        foreach(var crop in crops)
            count += crop.count;

        foreach(var seed in seeds)
            count += seed.count;

        return count;
    }
}

[System.Serializable]
public class CropItem
{
    public CropInfo cropInfo;
    public int count;

    public CropItem(CropInfo cropInfo, int count)
    {
        this.cropInfo = cropInfo;
        this.count = count;
    }
}

[System.Serializable]
public class SeedItem
{
    public SeedInfo seedInfo;
    public int count;

    public SeedItem(SeedInfo seedInfo, int count)
    {
        this.seedInfo = seedInfo;
        this.count = count;
    }
}