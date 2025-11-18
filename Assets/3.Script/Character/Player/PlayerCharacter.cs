using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character, ISceneContextBuilt
{
    protected PlayerCharacterData playerCharacterData;
    public PlayerCharacterData PlayerCharacterData => playerCharacterData;

    public int Priority { get; set; }

   
    [Header("Physics")]
    [SerializeField]
    private bool downArrowJump = false;
    [SerializeField]
    protected int maxJumpCount = 1;
    [SerializeField]
    protected int jumpCount = 0;
    private int firstTouchDir = 0;


    protected override void Awake()
    {
        base.Awake();
    }
    public void OnSceneContextBuilt()
    {
        playerCharacterData = GameManager.Instance.CurrentSceneContext.PlayerCharacterData;
        stateMuchine.ChangeState(new JumpState());
    }

    protected override void Update()
    {
        if (!isGrounded)
        {
            moveDir.y -= gravityForce * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
        }

        // 움직임 관련 로직 정의
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                downArrowJump = true;
            }
            stateMuchine.ChangeState(new JumpState());
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir.x = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir.x = -1;
        }
        else
        {
            moveDir.x = 0;
        }

        if(moveDir.x != 0 && stateMuchine.CurrentState.GetType() != new JumpState().GetType())
        {
            stateMuchine.ChangeState(new WalkState());
        }

        CheckGround();
    }

    

    protected override void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheckOffset.position, Vector2.down, groundCheckRayDistance, blockLayerMask);

        if (hit.collider != null)
        {
            // 발판 아래에서 뚫고 온 경우 판별
            if (firstTouchDir == 0)
            {
                float targetToGround = (groundCheckOffset.position + Vector3.down * groundCheckRayDistance).y - hit.transform.position.y;

                firstTouchDir = targetToGround > 0.2f ? 1 : -1;
            }

            if (hit.collider.CompareTag("OutOfMap") || (!downArrowJump && firstTouchDir == 1))
            {
                // 착지 후 판단
                if(moveDir.y < 0.5f && stateMuchine.CurrentState.GetType() == new JumpState().GetType())
                {
                    StateMuchine.ChangeState(new IdleState());
                }
            }
            else
            {
                isGrounded = false;
            }
        }
        //한 발판을 완전히 벗어난 경우
        else
        {
            downArrowJump = false;
            firstTouchDir = 0;
            isGrounded = false;
        }
    }

    public override void Jump()
    {
        if (jumpCount >= maxJumpCount)
            return;

        if(!downArrowJump)
        {
            jumpCount++;
            moveDir.y = playerCharacterData.statusData.JumpForce;
        }
    }

    public override void EndOfJump()
    {
        downArrowJump = false;
        isGrounded = true;
        jumpCount = 0;
        moveDir.y = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(groundCheckOffset.position, groundCheckOffset.position + Vector3.down * groundCheckRayDistance);
    }
}
