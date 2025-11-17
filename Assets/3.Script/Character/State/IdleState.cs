using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void ActionState(Character character)
    {
        character.SetMoveDir(Vector3.zero);
    }
}
