using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class StringUnityEventPair
{
    public string String;
    public UnityEvent Event;
}

public class AnimNotify : MonoBehaviour
{
    public List<StringUnityEventPair> NotifyAnimEvents;

    public void Active(string key)
    {
        for(int i = 0; i < NotifyAnimEvents.Count; i++)
        {
            if(NotifyAnimEvents[i].String == key)
            {
                NotifyAnimEvents[i].Event?.Invoke();
                return;
            }
        }
    }
}
