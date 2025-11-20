using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour, ISceneContextBuilt
{
    #region skill controller
    [SerializeField]
    private SkillInventory skillInventory;
    public SkillInventory SkillInventory => skillInventory;
    #endregion
    #region damageUI
    [SerializeField]
    private DamageUIController damageUIController;
    public DamageUIController DamageUIController => damageUIController;

    public void CreateDamageUI(int damage, Vector2 spawnPosition)
    {
        damageUIController.CreateDamageUI(damage, spawnPosition);
    }
    #endregion
    #region status
    [Header("Status")]
    [SerializeField]
    private Text lvText;

    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private Text mpText;
    [SerializeField]
    private Slider mpSlider;

    [SerializeField]
    private Text expText;
    [SerializeField]
    private Slider expSlider;

    public int Priority { get; set; } = 0;

    public void OnSceneContextBuilt()
    {
        PlayerCharacter pc = GameManager.Instance.CurrentSceneContext.PlayerCharacter;
        pc.OnChangedExp -= SetEXP;
        pc.OnChangedExp += SetEXP;

        pc.OnChangedLV -= SetLV;
        pc.OnChangedLV += SetLV;

        if (pc.TryGetComponent(out Combat combat))
        {
            combat.OnChangedHP -= SetHP;
            combat.OnChangedHP += SetHP;

            combat.OnChangedMP -= SetMP;
            combat.OnChangedMP += SetMP;
        }
    }

    public void SetLV(int lv)
    {
        lvText.text = lv.ToString();
    }
    public void SetHP(float value, float maxValue)
    {
        hpText.text = $"[{value.ToString("0")}/{maxValue.ToString("0")}]";
        hpSlider.value = value / maxValue;
    }
    public void SetMP(float value, float maxValue)
    {
        mpText.text = $"[{value.ToString("0")}/{maxValue.ToString("0")}]";
        mpSlider.value = value / maxValue;
    }
    public void SetEXP(float value, float maxValue)
    {
        expText.text = $"{value.ToString("0")}[{((value / maxValue) * 100).ToString("00.00")}%]";
        expSlider.value = value / maxValue;
    }
    #endregion

}
