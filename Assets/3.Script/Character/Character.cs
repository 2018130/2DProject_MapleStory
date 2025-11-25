using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMuchine))]
public class Character : MonoBehaviour
{
    protected SpriteRenderer model;
    public SpriteRenderer Model => model;

    protected Animator animator;

    protected Vector3 moveDir = Vector3.zero;
    public Vector3 MoveDir => moveDir;

    protected StateMuchine stateMuchine;
    public StateMuchine StateMuchine => stateMuchine;

    protected Collider2D collider;

    [Header("Physics Setting")]
    [SerializeField]
    protected float gravityForce = 9.81f;
    [SerializeField]
    protected LayerMask blockLayerMask;
    [SerializeField]
    protected float groundCheckRayDistance = 0.3f;
    [SerializeField]
    protected float forwardGroundCheckRayDistance = 0.3f;
    [SerializeField]
    protected Transform groundCheckOffset;
    [SerializeField]
    protected Transform forwardGroundCheckOffset;

    [Header("Physics Move")]
    [SerializeField]
    protected bool isGrounded = false;
    protected bool isGroundedForward = true;

    [Header("Combat")]
    [SerializeField]
    protected Combat combat;
    public Combat Combat => combat;

    [Header("State")]
    [SerializeField]
    protected bool isInvincible = false;
    protected float invincibleTime = 1.5f;
    protected Coroutine invincibilityCountDown_co;
    [SerializeField]
    protected bool isStuned = false;
    public bool IsInvincible => isInvincible;
    public bool IsStuned
    {
        get => isStuned;
        set => isStuned = value;
    }

    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();

        combat = GetComponent<Combat>();
        combat.BindDeadAction(Die);

        model = transform.GetComponentInChildren<SpriteRenderer>();
        animator = model.GetComponent<Animator>();

        stateMuchine = GetComponent<StateMuchine>();
        StateMuchine.Initialize(this);
    }

    protected virtual void Update()
    {
        if (!isGrounded)
        {
            moveDir.y -= gravityForce * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
        }

        CheckGround();
    }

    public virtual void MoveTo(Vector3 moveTo)
    {
        int dir = transform.position.x - moveTo.x > 0 ? 1 : -1;
        if (!Mathf.Approximately(transform.position.x - moveTo.x, 0))
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
        
    }

    public virtual void SetMoveDir(Vector3 newMoveDir)
    {
        moveDir = newMoveDir;
    }

    /// <summary>
    /// Use must change to state muchine
    /// </summary>
    public virtual void Jump() { }

    public virtual void EndOfJump()
    {
        isGrounded = true;
        moveDir.y = 0;
    }

    protected virtual void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheckOffset.position, Vector2.down, groundCheckRayDistance, blockLayerMask);

        if (hit.collider != null)
        {
            if (moveDir.y < 0.5f && stateMuchine.CurrentState.GetType() == new JumpState().GetType())
            {
                StateMuchine.ChangeState(new IdleState());
            }
        }
        //한 발판을 완전히 벗어난 경우
        else
        {
            isGrounded = false;
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " die");
        stateMuchine.ChangeState(new DeadState());
    }
    public virtual void Dead()
    {
        if(invincibilityCountDown_co != null)
            StopCoroutine(invincibilityCountDown_co);
        gameObject.SetActive(false);
    }

    public virtual void SetInvincible(bool active)
    {
        if (active && invincibilityCountDown_co == null)
        {
            invincibilityCountDown_co = StartCoroutine(InvincibilityCountDown_co());
        }

        isInvincible = active;
    }

    private IEnumerator InvincibilityCountDown_co()
    {
        float timer = 0;
        float cycleTime = 0f;
        float twinkleTime = 0.3f;
        Color color = model.color;
        color.a = 1f;

        while (timer < invincibleTime)
        {
            yield return null;

            timer += Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
            cycleTime += Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;

            if(cycleTime >= twinkleTime)
            {
                cycleTime = 0f;
                if(Mathf.Approximately(color.a, 0.6f))
                {
                    color.a = 1f;
                }
                else
                {
                    color.a = 0.6f;
                }
                model.color = color;
            }
        }

        color.a = 1f;
        model.color = color;
        invincibilityCountDown_co = null;

        SetInvincible(false);
    }

    public virtual void SetAnimation(string animationKey)
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            if (animator.parameters[i].name == animationKey)
            {
                animator.SetTrigger(animationKey);
            }
        }
    }
    public virtual void SetAnimation(string animationKey, bool value)
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            if (animator.parameters[i].name == animationKey)
            {
                animator.SetBool(animationKey, value);
            }
        }
    }
    public virtual void SetAnimation(string animationKey, float value)
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            if (animator.parameters[i].name == animationKey)
            {
                animator.SetFloat(animationKey, value);
            }
        }
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(this is not TitleCharacter)
        {
            Gizmos.DrawLine(groundCheckOffset.position, groundCheckOffset.position + Vector3.down * groundCheckRayDistance);

        }
    }
#endif
}
