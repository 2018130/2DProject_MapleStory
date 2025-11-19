using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    None,
    Active,
    Passive
}

[Serializable]
public class BaseSkill : MonoBehaviour
{
    [Header("BaseSkill")]
    [SerializeField]
    protected SkillType skillType = SkillType.None;
    [SerializeField]
    protected int lv = 0;
    [SerializeField]
    protected Weapon ownedWeapon;

    protected virtual void Start()
    {
        if(transform.parent != null)
        {
            Initialize(GetComponentInParent<Weapon>());
        }
    }

    public virtual void Initialize(Weapon ownedWeapon)
    {
        this.ownedWeapon = ownedWeapon;
    }
}
