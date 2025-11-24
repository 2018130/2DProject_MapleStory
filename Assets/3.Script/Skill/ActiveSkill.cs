using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActiveSkillData : BaseSkillData
{
    [SerializeField]
    public int AtkCount;
    [SerializeField]
    public float AtkRate;
}

public class ActiveSkill : BaseSkill
{
    [Header("ActiveSkill")]
    [SerializeField]
    protected LayerMask skillHitTargetLayer;
    // 스킬 사용중에 움직일 수 있는지
    [SerializeField]
    protected bool canMoveWhileUsingSkill = false;
    // 점프중에 스킬이 사용가능한지
    [SerializeField]
    protected bool canUseSkillWhileJumping = false;

    protected SkillController skillController;

    [SerializeField]
    private ActiveSkillData activeSkillData;
    public ActiveSkillData ActiveSkillData => activeSkillData;

    protected DateTime preSkillPlayedTime = DateTime.MinValue;

    public int ATKCount => activeSkillData.AtkCount;

    protected override void Start()
    {
        base.Start();
        baseSkillData = activeSkillData;
        skillController = GetComponentInParent<SkillController>();
        skillType = SkillType.Active;
    }

    public virtual float CalculateDamage() { return 0; }

    public virtual float GetSkillDamagePercent() { return 0; }

    public virtual void StartSkill() { }

    // animNotify
    public virtual void EndSkill()
    {
        skillController.IsPlayingAnySkill = false;
    }

    public override void Copy(BaseSkillData activeSkill)
    {
        base.Copy(activeSkill);

        ActiveSkillData activeSkillData = (ActiveSkillData)activeSkill;
        this.activeSkillData.AtkCount = activeSkillData.AtkCount;
        this.activeSkillData.AtkRate = activeSkillData.AtkRate;
    }
}
