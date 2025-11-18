using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable/Skilldata/SkilldataSO")]
public class SkillDataSO : ScriptableObject
{
    public List<BaseSkill> skillList = new List<BaseSkill>();
}
