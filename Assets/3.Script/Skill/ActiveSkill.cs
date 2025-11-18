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

    protected override void Start()
    {
        base.Start();
        skillType = SkillType.Active;
    }

    protected virtual float CalculateDamage() { return -1; }
}
