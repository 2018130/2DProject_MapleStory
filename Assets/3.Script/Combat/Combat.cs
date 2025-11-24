using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactionType
{
    None,
    Ally,
    Enemy,
}

public class Combat : MonoBehaviour
{
    [SerializeField]
    private FactionType factionType;
    public FactionType FactionType => factionType;

    [SerializeField]
    private float hp;
    private float maxHP;
    private float mp;
    private float maxMP;

    private Action OnDead;
    public Action<float, float> OnChangedHP;
    public Action<float, float> OnChangedMP;

    public void Initialize(float maxHP, float maxMP)
    {
        this.maxHP = maxHP;
        AddHP(maxHP);
        this.maxMP = maxMP;
        AddMP(maxMP);

    }

    public void AddMP(float mpAmount)
    {
        mp = Mathf.Clamp(mp + mpAmount, 0, maxMP);
        OnChangedMP?.Invoke(mp, maxMP);
    }
    public void AddHP(float hpAmount)
    {
        hp = Mathf.Clamp(hp + hpAmount, 0, maxHP);
        OnChangedHP?.Invoke(hp, maxHP);
    }

    public bool CheckMP(float mpAmount)
    {
        return mp >= mpAmount;
    }

    public void TakeDamage(float damage, Combat attacker)
    {
        if(TryGetComponent(out Character character))
        {
            if (character.IsInvincible)
                return;
        }

        AddHP(-damage);

        GameManager.Instance.CurrentSceneContext.MainUIManager.CreateDamageUI((int)damage, Camera.main.WorldToScreenPoint(transform.position));
        
        if (hp <= 0)
        {
            if(attacker != null && attacker.TryGetComponent(out PlayerCharacter pc))
            {
                int expAmount = 10;
                if(TryGetComponent(out BaseMonster m))
                {
                    expAmount = m.CharacterData.statusData.GetEXPAmount();
                }

                pc.AddExp(expAmount);
            }
            OnDead?.Invoke();
        }
    }

    public void BindDeadAction(Action callback)
    {
        OnDead -= callback;
        OnDead += callback;
    }
}
