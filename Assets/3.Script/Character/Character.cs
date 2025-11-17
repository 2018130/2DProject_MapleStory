using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMuchine))]
public class Character : MonoBehaviour
{
    protected SpriteRenderer model;

    protected Vector3 moveDir = Vector3.zero;

    protected StateMuchine stateMuchine;
    public StateMuchine StateMuchine => stateMuchine;

    [Header("Physics Setting")]
    [SerializeField]
    protected float gravityForce = 9.81f;
    [SerializeField]
    protected LayerMask blockLayerMask;
    [SerializeField]
    protected float groundCheckRayDistance = 0.3f;
    [SerializeField]
    protected Transform groundCheckOffset;

    [Header("Physics Move")]
    [SerializeField]
    protected bool isGrounded = false;

    protected virtual void Awake()
    {
        model = transform.GetComponentInChildren<SpriteRenderer>();
        stateMuchine = GetComponent<StateMuchine>();
        StateMuchine.Initialize(this);
    }

    protected virtual void Update()
    {
        CheckGround();
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
        if (dir != 0)
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

    /// <summary>
    /// Use must change to state muchine
    /// </summary>
    public virtual void Jump() { }

    protected virtual void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheckOffset.position, Vector2.down, groundCheckRayDistance, blockLayerMask);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        //한 발판을 완전히 벗어난 경우
        else
        {
            isGrounded = false;
        }
    }
}
