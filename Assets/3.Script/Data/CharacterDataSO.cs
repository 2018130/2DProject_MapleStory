using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable/Chracter/CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    [SerializeField]
    public StatusData statusData;
}
