using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected CharacterData characterData;

    protected SpriteRenderer model;

    private void Awake()
    {
        model = transform.GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void MoveTo(Vector3 moveTo)
    {
        int dir = transform.position.x - moveTo.x > 0 ? 1 : -1;
        Flip(dir);

        transform.position = moveTo;
    }

    protected virtual void Flip(int dir)
    {
        int sign = 1;
        if(dir != 0)
        {
            sign = dir / Mathf.Abs(dir);
        }
        
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * sign, transform.localScale.y);
    }
}
