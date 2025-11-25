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
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ActiveSkill activeSkill))
            {
                activeSkillList.Add(activeSkill);
            }
            if (transform.GetChild(i).TryGetComponent(out PassiveSkill passiveSkill))
            {
                passiveSkillList.Add(passiveSkill);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveSkillJsonData();
    }
    public void OnSceneContextBuilt()
    {
        LoadSkillJsonData();
        StatusController.Instance.AddStatusData(skillStatusData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GetActiveSkill("¿¡·Î¿ì ¼¦").StartSkill();
        }
    }

    public ActiveSkill GetActiveSkill(string skillName)
    {
        return activeSkillList.Find(x => x.SkillName == skillName);
    }

    private void LoadSkillJsonData()
    {
        SkillDataJson sdj = PersistentDataManager.Instance.LoadSkillDataFromJson();

        if(sdj != null)
        {
            sdj.activeSkillList.ForEach(x =>
            {
                ActiveSkill ac = ActiveSkillList.Find(y => y.SkillName == x.SkillName);
                if (ac != null)
                {
                    ac.Copy(x);
                }
            });
            sdj.passiveSkillList.ForEach(x =>
            {
                PassiveSkill ps = passiveSkillList.Find(y => y.SkillName == x.SkillName);
                if (ps != null)
                {
                    ps.Copy(x);

                }
            });
        }
    }
    private void SaveSkillJsonData()
    {
        SkillDataJson skillDataJson = new SkillDataJson();
        activeSkillList.ForEach(x => skillDataJson.activeSkillList.Add(x.ActiveSkillData));
        passiveSkillList.ForEach(x => skillDataJson.passiveSkillList.Add(x.BaseSkillData));

        PersistentDataManager.Instance.SaveToJson(skillDataJson);
    }

    public void UseDefaultSkill()
    {
        GetActiveSkill("¿¡·Î¿ì ¼¦").StartSkill();
    }
}
