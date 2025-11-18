using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    public override void OnStateEnter(Character character)
    {
        character.Dead();
    }

    public override void OnStateExit(Character character)
    {

    }

    public override void OnStateStay(Character character)
    {

    }
}
