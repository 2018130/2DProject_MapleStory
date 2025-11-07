using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    private void Awake()
    {
        T typeOfClass = GameObject.FindAnyObjectByType<T>();

        if (typeOfClass == null)
        {
            T targetTypeObj = gameObject.GetComponent<T>();
            instance = targetTypeObj;
            DontDestroyOnLoad(targetTypeObj.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
