using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMuchine))]
public class Character : MonoBehaviour
{
    protected SpriteRenderer model;
    public SpriteRenderer Model => model;

    protected Vector3 moveDir = Vector3.zero;
    public Vector3 MoveDir => moveDir;

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

    [Header("Combat")]
    [SerializeField]
    protected Combat combat;
    public Combat Combat => combat;

    protected virtual void Awake()
    {
        combat = GetComponent<Combat>();
        combat.BindDeadAction(Die);

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
        if(!Mathf.Approximately(transform.position.x - moveTo.x, 0))
        {
            Flip(dir);
        }

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

    public int GetFrontDirX()
    {
        return transform.localScale.x > 0 ? -1 : 1;
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

    public virtual void EndOfJump() { }

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

    public virtual void Die()
    {
        stateMuchine.ChangeState(new DeadState());
    }
    public virtual void Dead() 
    {
        gameObject.SetActive(false);
    }
}
