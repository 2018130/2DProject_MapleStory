using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMuchine
{
    private Character character;

    private BaseState currnetState = new BaseState();

    public StateMuchine(Character character)
    {
        this.character = character;
    }

    public void ChangeState(BaseState newState)
    {
        if(newState.GetType() == currnetState.GetType())
        {
            Debug.Log($"Change same state, do not anything");
            return;
        }
        newState.ActionState(character);
    }
}
