using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimNotify : MonoBehaviour
{
    [SerializeField]
    private string key;

    public UnityEvent NotifyAnimEvent;

    public void Active()
    {
        NotifyAnimEvent?.Invoke();
    }
}
