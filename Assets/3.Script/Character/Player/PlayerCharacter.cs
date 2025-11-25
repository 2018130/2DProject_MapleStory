using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character, ISceneContextBuilt
{
    [SerializeField]
    protected PlayerCharacterData playerCharacterData;
    public PlayerCharacterData PlayerCharacterData => playerCharacterData;

    private bool isAuto = false;
    public bool IsAuto => isAuto;
    public int Priority { get; set; } = 1;


    [Header("Physics")]
    [SerializeField]
    public bool downArrowJump = false;
    [SerializeField]
    protected int maxJumpCount = 1;
    [SerializeField]
    protected int jumpCount = 0;
    private int firstTouchDir = 0;

    // event
    public Action<float, float> OnChangedExp;
    public Action<int> OnChangedLV;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnSceneContextBuilt()
    {
        playerCharacterData = GameManager.Instance.CurrentSceneContext.PlayerCharacterData;
        StatusController.Instance.AddStatusData(playerCharacterData.statusData);

        if (this is not TitleCharacter)
        {
            combat.Initialize(playerCharacterData.statusData.MaxHP, playerCharacterData.statusData.MaxMP);
            downArrowJump = true;
            stateMuchine.ChangeState(new JumpState());
        }

        AddExp(0);
    }

    protected override void Update()
    {
        if (!isGrounded && stateMuchine.CurrentState.GetType() != new HangState().GetType())
        {
            moveDir.y -= gravityForce * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
        }

        // 움직임 관련 로직 정의
        if(!isAuto)
        {
            if (!isStuned)
            {
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

                Transform hangTransform = CanHanging();
                if (hangTransform != null)
                {
                    float verticalInput = Input.GetAxisRaw("Vertical");

                    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                    {
                        transform.position = new Vector3(hangTransform.position.x, transform.position.y);
                        moveDir.y = verticalInput;
                        stateMuchine.ChangeState(new HangState());
                    }
                    else if (stateMuchine.CurrentState.GetType() == new HangState().GetType())
                    {
                        transform.position = new Vector3(hangTransform.position.x, transform.position.y);
                        moveDir.y = 0;
                    }
                }
                else if (stateMuchine.CurrentState.GetType() == new HangState().GetType())
                {
                    downArrowJump = true;
                    stateMuchine.ChangeState(new JumpState());
                }
            }
            else
            {
                moveDir.x = 0;
            }

            if (moveDir.x != 0 &&
                stateMuchine.CurrentState.GetType() != new JumpState().GetType() &&
                stateMuchine.CurrentState.GetType() != new HangState().GetType())
            {
                stateMuchine.ChangeState(new WalkState());
            }
        }
        

        CheckGround();
        SetAnimation("Speed", moveDir.magnitude);
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

            //Debug.Log($"{downArrowJump} {firstTouchDir} {moveDir.y} {stateMuchine.CurrentState.GetType()}");
            if (hit.collider.CompareTag("OutOfMap") || (!downArrowJump && firstTouchDir == 1))
            {
                // 착지 후 판단
                if (moveDir.y < 0.5f &&
                    (stateMuchine.CurrentState.GetType() == new JumpState().GetType() || stateMuchine.CurrentState.GetType() == new HangState().GetType()))
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

        if (!downArrowJump)
        {
            moveDir.y = playerCharacterData.statusData.JumpForce;
            jumpCount++;
        }
    }

    public override void EndOfJump()
    {
        base.EndOfJump();
        downArrowJump = false;
        jumpCount = 0;
    }
    public Transform CanHanging()
    {
        List<Collider2D> results = new List<Collider2D>();

        if (Physics2D.OverlapCollider(collider, results) > 0)
        {
            foreach(var result in results)
            {
                if(result.gameObject.layer == LayerMask.NameToLayer("Hangable"))
                {
                    return result.transform;
                }
            }
        }

        return null;
    }

    public void Hang()
    {
        moveDir.y = 0;
        jumpCount = 0;
        SetAnimation("IsHangging", true);
    }

    public void EndOfHang()
    {
        moveDir.y = 0;
        jumpCount = 0;
        SetAnimation("IsHangging", false);
    }

    public override void MoveForward()
    {
        float speed = playerCharacterData.statusData.MoveSpeed;
        MoveTo(transform.position + speed * moveDir * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime);
    }
    public void AddExp(int expAmount)
    {
        int lv = playerCharacterData.GetLevel();

        playerCharacterData.EXP += (int)((StatusController.Instance.GetTotalValueByType(StatusType.ExpRate) + 1) * expAmount);

        if (playerCharacterData.GetLevel() >= lv)
        {
            LevelUp(playerCharacterData.GetLevel());
        }
        OnChangedExp?.Invoke(playerCharacterData.GetRemainExp(), playerCharacterData.GetTotalRequiredExpForNextLevel());
    }

    public void LevelUp(int lv)
    {
        playerCharacterData.RemainSkillLV += 3;
        OnChangedLV?.Invoke(lv);
    }

    public void AttackDefaultSkill()
    {
        SkillController sc = GetComponentInChildren<SkillController>();
        sc.UseDefaultSkill();
    }

    public void ToggleAutoMode()
    {
        isAuto = !isAuto;
    }
}
