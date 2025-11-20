using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActiveSkill : BaseSkill
{
    [Header("ActiveSkill")]
    [SerializeField]
    protected int atkCount;
    [SerializeField]
    protected float atkRate;
    [SerializeField]
    protected LayerMask skillHitTargetLayer;
    // 스킬 사용중에 움직일 수 있는지
    [SerializeField]
    protected bool canMoveWhileUsingSkill = false;
    // 점프중에 스킬이 사용가능한지
    [SerializeField]
    protected bool canUseSkillWhileJumping = false;

    protected SkillController skillController;

    public int ATKCount => atkCount;

    protected override void Start()
    {
        base.Start();
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
}
