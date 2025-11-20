using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SkillInventory : MonoBehaviour,ISceneContextBuilt
{
    private GameObject skillBoxPrefab;

    [SerializeField]
    private string skillBoxKey;

    [Header("UI")]
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject contents;
    [SerializeField]
    private GameObject tooltip;

    // SkillData
    private SkillController skillController;

    public int Priority { get; set; }

    public void OnSceneContextBuilt()
    {
        skillController = GameManager.Instance.CurrentSceneContext.PlayerCharacter.GetComponentInChildren<SkillController>();
        Initialize();
    }

    private void Awake()
    {
        skillBoxPrefab = Addressables.LoadAssetAsync<GameObject>(skillBoxKey).WaitForCompletion();
        panel.gameObject.SetActive(false);
        SetTooltip(false, Vector2.zero);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            TogglePanel();
        }
    }

    private void TogglePanel()
    {
        panel.gameObject.SetActive(!panel.gameObject.activeSelf);
    }
    private void Initialize()
    {
        RectTransform contentsRect = contents.GetComponent<RectTransform>();
        float boxSize = skillBoxPrefab.GetComponent<RectTransform>().sizeDelta.y;

        for (int i = 0; i < skillController.ActiveSkillList.Count; i++)
        {
            SkillBox skillBox = Instantiate(skillBoxPrefab, contents.transform).GetComponent<SkillBox>();
            skillBox.SetSkill(skillController.ActiveSkillList[i], SetTooltip);
            skillController.ActiveSkillList[i].OnUpgradeLV += skillBox.UpdateSkillLV;

            contentsRect.sizeDelta += new Vector2(0, boxSize);
        }
        for (int i = 0; i < skillController.PassiveSkillList.Count; i++)
        {
            SkillBox skillBox = Instantiate(skillBoxPrefab, contents.transform).GetComponent<SkillBox>();
            skillBox.SetSkill(skillController.PassiveSkillList[i], SetTooltip);
            skillController.PassiveSkillList[i].OnUpgradeLV += skillBox.UpdateSkillLV;

            contentsRect.sizeDelta += new Vector2(0, boxSize);
        }
    }

    private void SetTooltip(bool active, Vector2 postion, BaseSkill skillData = null)
    {
        if(skillData == null)
        {
            tooltip.SetActive(active);
        }
        else
        {
            tooltip.GetComponent<RectTransform>().transform.position = postion;
            tooltip.GetComponentInChildren<Text>().text = ReplaceFormat(skillData);
            tooltip.SetActive(active);
        }
    }
    private string ReplaceFormat(BaseSkill skillData)
    {
        string str = skillData.Description;
        if (skillData is ActiveSkill)
        {
            ActiveSkill activeSkillData = (ActiveSkill)skillData;
            str = str.Replace("{atkCount}", activeSkillData.ATKCount.ToString());
            str = str.Replace("{skillDamagePercent}", activeSkillData.GetSkillDamagePercent().ToString() + "%");
        }
        else if (skillData is PassiveSkill)
        {
            PassiveSkill passiveSkillData = (PassiveSkill)skillData;
            str = str.Replace("{statusType}", passiveSkillData.StatusType.ToString());
            str = str.Replace("{statusValuePerLV}", passiveSkillData.StatValuePerLV.ToString());
            str = str.Replace("{statusValue}", (passiveSkillData.StatValuePerLV * passiveSkillData.LV).ToString());
        }

        return str;
    }
}
