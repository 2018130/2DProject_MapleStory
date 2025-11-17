using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected SpriteRenderer model;

    protected Vector3 moveDir = Vector3.zero;

    protected StateMuchine stateMuchine;

    protected virtual void Awake()
    {
        model = transform.GetComponentInChildren<SpriteRenderer>();
        stateMuchine = new StateMuchine(this);
    }

    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            stateMuchine.ChangeState(new WalkState());
            stateMuchine.ChangeState(new WalkState());
        }
    }

    public virtual void MoveTo(Vector3 moveTo)
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

    public virtual void MoveForward()
    {
        MoveTo(transform.position + moveDir * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime);
    }

    public virtual void SetMoveDir(Vector3 newMoveDir)
    {
        moveDir = newMoveDir;
    }
}
