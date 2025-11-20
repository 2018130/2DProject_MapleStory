using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
    protected int maxLV = 0;
    [SerializeField]
    protected int lv = 0;
    [SerializeField]
    protected int requireMPAmount;
    [SerializeField]
    protected string skillName;
    [SerializeField]
    protected string description;

    [SerializeField]
    protected string skillImageKey;
    protected Sprite skillImage;

    public string Description => description;
    public int LV => lv;
    public string SkillName => skillName;
    public Sprite SkillImage => skillImage;

    protected Weapon ownedWeapon;

    public Action<BaseSkill> OnUpgradeLV;

    private void Awake()
    {
        skillImage = Addressables.LoadAssetAsync<Sprite>(skillImageKey).WaitForCompletion();
    }

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

    public virtual void UpgradeLv(int amount)
    {
        lv = Mathf.Clamp(amount + lv, 0, maxLV);
        OnUpgradeLV?.Invoke(this);
    }
}
