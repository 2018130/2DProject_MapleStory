using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillBox : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField]
    private Image skillImage;
    [SerializeField]
    private Text skillName;
    [SerializeField]
    private Text skillLv;

    [SerializeField]
    private Button LevelupBtn;

    private BaseSkill skillData;

    public Action<bool, Vector2, BaseSkill> OnMouseHover;

    public void SetSkill(BaseSkill skillData, Action<bool, Vector2, BaseSkill> mouseHoverCallback)
    {
        this.skillData = skillData;
        skillImage.sprite = skillData.SkillImage;
        skillName.text = skillData.SkillName;
        UpdateSkillLV(skillData);

        SetUpgradeBtnImage();
        OnMouseHover -= mouseHoverCallback;
        OnMouseHover += mouseHoverCallback;
        LevelupBtn.onClick.AddListener(skillData.UpgradeLv);
    }

    public void UpdateSkillLV(BaseSkill skillData)
    {
        skillLv.text = skillData.LV.ToString();
    }

    public void SetUpgradeBtnImage()
    {
        if (GameManager.Instance.CurrentSceneContext.PlayerCharacter.PlayerCharacterData.RemainSkillLV > 0 &&
               skillData.LV < skillData.MaxLV)
        {
            LevelupBtn.GetComponent<Image>().color = Color.orange;
        }
        else
        {
            LevelupBtn.GetComponent<Image>().color = Color.gray;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseHover?.Invoke(true, eventData.position, skillData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseHover?.Invoke(false, eventData.position, skillData);
    }


}
