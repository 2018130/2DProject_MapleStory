using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangState : BaseState
{
    public override void OnStateEnter(Character character)
    {
        PlayerCharacter pc = character as PlayerCharacter;
        if(pc != null)
        {
            pc.Hang();
        }
    }

    public override void OnStateExit(Character character)
    {
        PlayerCharacter pc = character as PlayerCharacter;
        if (pc != null)
        {
            pc.EndOfHang();
        }
    }

    public override void OnStateStay(Character character)
    {
        character.MoveForward();
    }
}
