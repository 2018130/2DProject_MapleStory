using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
    public override void OnStateEnter(Character character)
    {

    }

    public override void OnStateExit(Character character)
    {

    }

    public override void OnStateStay(Character character)
    {
        character.MoveForward();
    }
}
