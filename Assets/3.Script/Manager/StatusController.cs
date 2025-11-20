using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : SingletonBehaviour<StatusController>
{
    [SerializeField]
    private List<StatusData> statusDatas = new List<StatusData>();

    [SerializeField]
    private int maxExpRate;
    [SerializeField]
    private int maxJumpForce;

    public void AddStatusData(StatusData statusData)
    {
        if (statusDatas.Find(x => x.Key == statusData.Key) == null)
        {
            statusDatas.Add(statusData);
        }
    }

    public float GetTotalValueByType(StatusType statusType)
    {
        float acc = 0;
        switch(statusType)
        {
            case StatusType.Speed:
                statusDatas.ForEach(x => acc = x.MoveSpeed + acc);
                break;
            case StatusType.JumpForce:
                statusDatas.ForEach(x => acc = x.JumpForce + acc);
                break;
            case StatusType.ExpRate:
                statusDatas.ForEach(x => acc = x.EXPRate + acc);
                break;
            case StatusType.Atk:
                statusDatas.ForEach(x => acc = x.ATK + acc);
                break;
            case StatusType.MaxHP:
                statusDatas.ForEach(x => acc = x.MaxHP + acc);
                break;
            case StatusType.MaxMP:
                statusDatas.ForEach(x => acc = x.MaxMP + acc);
                break;
            case StatusType.STR:
                statusDatas.ForEach(x => acc = x.STR + acc);
                break;
            case StatusType.DEX:
                statusDatas.ForEach(x => acc = x.DEX + acc);
                break;
            case StatusType.INT:
                statusDatas.ForEach(x => acc = x.INT + acc);
                break;
            case StatusType.LUK:
                statusDatas.ForEach(x => acc = x.LUK + acc);
                break;
        }

        return acc;
    }
}