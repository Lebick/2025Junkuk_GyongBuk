using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingManager : Singleton<RankingManager>
{
    public List<RankData> rankDatas = new();

    protected override void Awake()
    {
        base.Awake();

        Load();
        DontDestroyOnLoad(gameObject);
    }

    public void Register(string name, float time)
    {
        rankDatas.Add(new RankData(name, time));
        Sort();
        Save();
    }

    public void Save()
    {
        for(int i=0; i<rankDatas.Count; i++)
        {
            PlayerPrefs.SetString($"{i}RankName", rankDatas[i].name);
            PlayerPrefs.SetFloat($"{i}RankTime", rankDatas[i].time);
        }
    }

    public void Sort()
    {
        rankDatas = rankDatas.OrderBy(a => a.time).ToList();
        rankDatas = rankDatas.GetRange(0, Mathf.Min(5, rankDatas.Count));
    }

    public void Load()
    {
        for (int i = 0; i<5; i++)
        {
            string name = PlayerPrefs.GetString($"{i}RankName", "------");
            float time = PlayerPrefs.GetInt($"{i}RankTime", 3599);

            rankDatas.Add(new RankData(name, time));
        }
    }
}

[System.Serializable]
public class RankData
{
    public string name;
    public float time;

    public RankData(string name, float time)
    {
        this.name = name;
        this.time = time;
    }
}