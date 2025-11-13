using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable/Chracter/CharacterData")]
public class CharacterData : ScriptableObject
{
    public float ATK;
    public float MaxHP;
    public float MoveSpeed;
    public float JumpPower;
}
