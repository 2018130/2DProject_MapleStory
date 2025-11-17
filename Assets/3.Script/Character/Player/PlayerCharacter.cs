using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character, ISceneContextBuilt
{
    protected PlayerCharacterData playerCharacterData;
    public PlayerCharacterData PlayerCharacterData => playerCharacterData;

    public int Priority { get; set; }

    [Header("Physics Setting")]
    [SerializeField]
    private float gravityForce = 9.81f;
    [SerializeField]
    private LayerMask blockLayerMask;
    [SerializeField]
    private float groundCheckRayDistance = 0.3f;
    [SerializeField]
    private Transform groundCheckOffset;

    [Header("Physics Move")]
    [SerializeField]
    private bool isGrounded = false;
    [SerializeField]
    private int maxJumpCount = 1;
    [SerializeField]
    private int jumpCount = 0;
    [SerializeField]
    private Vector3 moveDir = Vector3.zero;
    [SerializeField]
    private bool downArrowJump = false;


    protected override void Awake()
    {
        base.Awake();
    }
    public void OnSceneContextBuilt()
    {
        playerCharacterData = GameManager.Instance.CurrentSceneContext.PlayerCharacterData;
    }

    protected override void Update()
    {
        base.Update();

        if (!isGrounded)
        {
            moveDir.y -= gravityForce * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
        }
        else
        {
            moveDir.y = 0;
        }
        // 움직임 관련 로직 정의
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                downArrowJump = true;
            }
            else
            {
                Jump();
            }
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

        //
        MoveTo(transform.position + moveDir * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime);
        CheckGround();
    }

    private void Jump()
    {
        if (jumpCount >= maxJumpCount)
            return;

        jumpCount++;
        moveDir.y = playerCharacterData.statusData.JumpForce;
    }

    private int firstTouchDir = 0;
    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheckOffset.position, Vector2.down, groundCheckRayDistance, blockLayerMask);

        if (hit.collider != null)
        {
            // 발판 아래에서 뚫고 온 경우 판별
            if (firstTouchDir == 0)
            {
                float targetToGround = (groundCheckOffset.position + Vector3.down * groundCheckRayDistance).y - hit.transform.position.y;
                Debug.Log(targetToGround);
                firstTouchDir = targetToGround > 0.2f ? 1 : -1;
            }

            if (hit.collider.CompareTag("OutOfMap") || (!downArrowJump && firstTouchDir == 1))
            {
                isGrounded = true;
                jumpCount = 0;
                Debug.Log("hit target up");
            }
            else
            {
                isGrounded = false;
                Debug.Log("hit target down");
            }
        }
        //한 발판을 완전히 벗어난 경우
        else
        {
            downArrowJump = false;
            firstTouchDir = 0;
            Debug.Log("dont have target");
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(groundCheckOffset.position, groundCheckOffset.position + Vector3.down * groundCheckRayDistance);
    }
}
