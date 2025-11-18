using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActiveSkill : BaseSkill
{
    [Header("ActiveSkill")]
    [SerializeField]
    protected float baseCooltime;
    [SerializeField]
    protected float baseRequireMP;

    protected override void Awake()
    {
        skillType = SkillType.Active;
    }
}
