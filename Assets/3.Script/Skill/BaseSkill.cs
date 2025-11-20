using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public enum SkillType
{
    None,
    Active,
    Passive
}

[Serializable]
public class BaseSkillData
{
    [SerializeField]
    public string SkillName;
    [SerializeField]
    public int LV;
}

public class BaseSkill : MonoBehaviour
{
    [Header("BaseSkill")]
    [SerializeField]
    protected SkillType skillType = SkillType.None;
    [SerializeField]
    protected int maxLV = 0;
    [SerializeField]
    protected int requireMPAmount;
    [SerializeField]
    protected string description;

    [SerializeField]
    protected string skillImageKey;
    protected Sprite skillImage;

    protected BaseSkillData baseSkillData;
    public BaseSkillData BaseSkillData => baseSkillData;

    public string Description => description;
    public int MaxLV => maxLV;
    public int LV => baseSkillData.LV;
    public string SkillName => baseSkillData.SkillName;
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

    public virtual void UpgradeLv()
    {
        if (GameManager.Instance.CurrentSceneContext.PlayerCharacter.PlayerCharacterData.RemainSkillLV < 1 ||
            1 + baseSkillData.LV > maxLV)
            return;

        GameManager.Instance.CurrentSceneContext.PlayerCharacter.PlayerCharacterData.RemainSkillLV -= 1;
        GameManager.Instance.CurrentSceneContext.MainUIManager.SkillInventory.
            SetSkillPoint(GameManager.Instance.CurrentSceneContext.PlayerCharacter.PlayerCharacterData.RemainSkillLV);
        baseSkillData.LV++;
        OnUpgradeLV?.Invoke(this);
    }

    public virtual void Copy(BaseSkillData baseSkill)
    {
        baseSkillData.LV = baseSkill.LV;
    }
}
