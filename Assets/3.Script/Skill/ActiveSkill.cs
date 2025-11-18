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

    protected SkillController skillController;

    protected override void Start()
    {
        base.Start();
        skillController = GetComponentInParent<SkillController>();
        skillType = SkillType.Active;
    }

    protected virtual float CalculateDamage() { return -1; }

    public virtual bool StartSkill() 
    {
        if (skillController.IsPlayingAnySkill)
            return false;

        skillController.IsPlayingAnySkill = true;
        return true;
    }
    public virtual void EndSkill()
    {
        skillController.IsPlayingAnySkill = false;
    }
}
