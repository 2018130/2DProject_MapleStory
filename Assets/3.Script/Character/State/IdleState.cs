using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void OnStateEnter(Character character)
    {
        character.SetMoveDir(Vector3.zero);
    }

    public override void OnStateStay(Character character)
    {

    }
    public override void OnStateExit(Character character)
    {

    }
}
