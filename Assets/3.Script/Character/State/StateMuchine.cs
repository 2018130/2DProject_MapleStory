using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMuchine :MonoBehaviour
{
    private Character character;

    [SerializeField]
    private BaseState currnetState = new IdleState();
    public BaseState CurrentState => currnetState;

    public void Initialize(Character character)
    {
        this.character = character;
    }

    private void Update()
    {
        if (character == null)
            return;

        currnetState.OnStateStay(character);
    }

    public void ChangeState(BaseState newState)
    {
        if(newState.GetType() == currnetState.GetType())
        {
            //Debug.Log($"Change same state, do not anything : {newState.GetType()}");
            return;
        }

        currnetState.OnStateExit(character);

        currnetState = newState;

        newState.OnStateEnter(character);
    }
}
