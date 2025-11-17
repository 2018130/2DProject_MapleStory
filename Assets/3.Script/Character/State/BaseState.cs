using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public abstract void OnStateEnter(Character character);

    public abstract void OnStateStay(Character character);

    public abstract void OnStateExit(Character character);
}
