using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private List<ActiveSkill> activeSkills = new List<ActiveSkill>();
    private List<PassiveSkill> passiveSkills = new List<PassiveSkill>();

    private bool isPlayingAnySkill = false;
    public bool IsPlayingAnySkill
    {
        get => isPlayingAnySkill;
        set => isPlayingAnySkill = value;
    }

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform children = transform.GetChild(i);

            if(children.TryGetComponent(out ActiveSkill activeSkill))
            {
                activeSkills.Add(activeSkill);
            }
            else if (children.TryGetComponent(out PassiveSkill passiveSkill))
            {
                passiveSkills.Add(passiveSkill);
            }
        }
    }
}
