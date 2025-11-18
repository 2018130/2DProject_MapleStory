using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class Item : MonoBehaviour
{
    [SerializeField]
    protected string itemCode;

    protected Character owner;
    public Character Owner => owner;

    protected virtual void Start()
    {
        if(owner == null)
        {
            Equip(GetComponentInParent<Character>());
        }
    }

    public void Equip(Character owner)
    {
        this.owner = owner;
    }
}
