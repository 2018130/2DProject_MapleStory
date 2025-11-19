using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField]
    private ShootingArrowSkill shootingArrowSkill;

    private bool isPlayingAnySkill = false;
    public bool IsPlayingAnySkill
    {
        get => isPlayingAnySkill;
        set => isPlayingAnySkill = value;
    }

    private void Awake()
    {
        shootingArrowSkill = GetComponentInChildren<ShootingArrowSkill>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            shootingArrowSkill.StartSkill();
        }
    }
}
