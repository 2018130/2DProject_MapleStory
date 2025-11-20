using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour, ISceneContextBuilt
{
    private List<ActiveSkill> activeSkillList = new List<ActiveSkill>();
    private List<PassiveSkill> passiveSkillList = new List<PassiveSkill>();

    public List<ActiveSkill> ActiveSkillList => activeSkillList;
    public List<PassiveSkill> PassiveSkillList => passiveSkillList;

    [SerializeField]
    private StatusData skillStatusData = new StatusData(false);
    public StatusData SkillStatusData => skillStatusData;

    private bool isPlayingAnySkill = false;
    public bool IsPlayingAnySkill
    {
        get => isPlayingAnySkill;
        set => isPlayingAnySkill = value;
    }
    public int Priority { get; set; }

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent(out ActiveSkill activeSkill))
            {
                activeSkillList.Add(activeSkill);
            }
            if (transform.GetChild(i).TryGetComponent(out PassiveSkill passiveSkill))
            {
                passiveSkillList.Add(passiveSkill);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            GetActiveSkill("¿¡·Î¿ì ¼¦").StartSkill();
        }
    }

    public ActiveSkill GetActiveSkill(string skillName)
    {
        return activeSkillList.Find(x => x.SkillName == skillName);
    }

    public void OnSceneContextBuilt()
    {
        StatusController.Instance.AddStatusData(skillStatusData);
    }
}
