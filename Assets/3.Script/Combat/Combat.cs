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

    private Action OnDead;

    public void Initialize(float maxHp)
    {
        hp = maxHp;
    }

    public void TakeDamage(float damage, Combat attacker)
    {
        hp -= damage;

        if(hp <= 0)
        {
            OnDead?.Invoke();
        }
    }

    public void BindDeadAction(Action callback)
    {
        OnDead -= callback;
        OnDead += callback;
    }
}
