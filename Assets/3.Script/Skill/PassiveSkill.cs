using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PassiveSkill : BaseSkill
{
    [SerializeField]
    private StatusType statusType = StatusType.None;
    // 레벨당 스텟 증가 개수
    [SerializeField]
    private int statValuePerLV = 0;

    public StatusType StatusType => statusType;
    public int StatValuePerLV => statValuePerLV;

    public override void UpgradeLv(int amount)
    {
        int preLv = lv;

        base.UpgradeLv(amount);

        ChangeSkillStatus(preLv);
    }

    public void ChangeSkillStatus(int preLV)
    {
        StatusData statusData = GameManager.Instance.CurrentSceneContext.PlayerCharacter.GetComponentInChildren<SkillController>().SkillStatusData;
        float gap = (lv - preLV) * statValuePerLV;

        if (statusData == null)
            return;

        switch (statusType)
        {
            case StatusType.Speed:
                statusData.MoveSpeed += gap;
                break;
            case StatusType.JumpForce:
                statusData.JumpForce += gap;
                break;
            case StatusType.ExpRate:
                statusData.EXPRate += gap;
                break;
            case StatusType.Atk:
                statusData.ATK += (int)gap;
                break;
            case StatusType.MaxHP:
                statusData.MaxHP += (int)gap;
                break;
            case StatusType.MaxMP:
                statusData.MaxMP += (int)gap;
                break;
            case StatusType.STR:
                statusData.STR += (int)gap;
                break;
            case StatusType.DEX:
                statusData.DEX += (int)gap;
                break;
            case StatusType.INT:
                statusData.INT += (int)gap;
                break;
            case StatusType.LUK:
                statusData.LUK += (int)gap;
                break;
        }
    }
}