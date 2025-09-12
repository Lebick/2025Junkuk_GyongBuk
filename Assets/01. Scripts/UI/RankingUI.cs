using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    private RankingManager rankingManager;

    public Transform[] rankUIs = new Transform[5];

    private void Awake()
    {
        rankingManager = RankingManager.Instance;
    }

    private void Update()
    {
        for(int i=0; i<rankUIs.Length; i++)
        {
            Text name = rankUIs[i].GetChild(1).GetComponent<Text>();
            Text time = rankUIs[i].GetChild(2).GetComponent<Text>();

            name.text = rankingManager.rankDatas[i].name;
            time.text = GetTimeFormat(rankingManager.rankDatas[i].time);
        }
    }

    private string GetTimeFormat(float time)
    {
        int mm = (int)(time / 60);
        int ss = (int)(time % 60);

        return $"{mm} : {ss}";
    }
}
