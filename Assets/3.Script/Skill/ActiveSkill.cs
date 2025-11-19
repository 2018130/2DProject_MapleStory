using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActiveSkill : BaseSkill
{
    [Header("ActiveSkill")]
    [SerializeField]
    protected float atkRate;
    [SerializeField]
    protected float baseCooltime;
    [SerializeField]
    protected float baseRequireMP;
    [SerializeField]
    protected LayerMask skillHitTargetLayer;
    // 스킬 사용중에 움직일 수 있는지
    [SerializeField]
    protected bool canMoveWhileUsingSkill = false;
    // 점프중에 스킬이 사용가능한지
    [SerializeField]
    protected bool canUseSkillWhileJumping = false;

    protected SkillController skillController;

    protected override void Start()
    {
        base.Start();
        skillController = GetComponentInParent<SkillController>();
        skillType = SkillType.Active;
    }

    protected virtual float CalculateDamage() { return -1; }

    public virtual void StartSkill() { }

    // animNotify
    public virtual void EndSkill()
    {
        skillController.IsPlayingAnySkill = false;
    }
}
