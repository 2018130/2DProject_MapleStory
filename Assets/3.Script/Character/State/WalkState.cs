using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
    public override void ActionState(Character character)
    {
        base.ActionState(character);

        character.MoveForward();
    }
}
