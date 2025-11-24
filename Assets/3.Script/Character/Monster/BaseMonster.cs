using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseMonster : Character, ISceneContextBuilt
{
    // 몬스터 고정 데이터 입력
    [SerializeField]
    protected CharacterDataSO characterData;
    public CharacterDataSO CharacterData => characterData;

    [SerializeField]
    private float minTurnTime = 2f;
    [SerializeField]
    private float maxTurnTime = 7f;

    public int Priority { get; set; } = 0;

    protected override void Awake()
    {
        base.Awake();

        combat.Initialize(characterData.statusData.MaxHP, characterData.statusData.MaxMP);
    }

    public void OnSceneContextBuilt()
    {
        StartCoroutine(Move_co());
    }

    protected override void Update()
    {
        base.Update();

        ProcessBodyHit();
        CheckGroundForward();
    }

    private IEnumerator Move_co()
    {
        stateMuchine.ChangeState(new JumpState());

        while (true)
        {
            if (stateMuchine.CurrentState.GetType() == new JumpState().GetType())
            {
                yield return null;
                continue;
            }

            float turnTime = Random.Range(minTurnTime, maxTurnTime);
            int dir = Random.Range(0, 2) == 0 ? 1 : -1;
            moveDir.x = dir;

            stateMuchine.ChangeState(new WalkState());

            yield return new WaitForSeconds(turnTime);

            stateMuchine.ChangeState(new IdleState());

            yield return new WaitForSeconds(1f);
        }
    }

    public override void MoveForward()
    {
        if (!isGroundedForward)
            return;

        float speed = characterData.statusData.MoveSpeed;
        MoveTo(transform.position + speed * moveDir * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime);
    }

    public override void Jump()
    {
        moveDir.y = characterData.statusData.JumpForce;
    }

    private void ProcessBodyHit()
    {
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.SetLayerMask(LayerMask.NameToLayer("Character"));
        List<Collider2D> colliders = new List<Collider2D>();

        if (Physics2D.OverlapCollider(collider, colliders) > 0)
        {
            foreach (var overlapTarget in colliders)
            {
                if(overlapTarget.TryGetComponent(out Combat hit) && hit.FactionType == FactionType.Ally)
                {
                    hit.TakeDamage(characterData.statusData.ATK,combat);
                    hit.GetComponent<Character>().SetInvincible(true);
                }
            }
        }
    }

    private void CheckGroundForward()
    {
        Vector2 direction = new Vector3(moveDir.x / Mathf.Abs(moveDir.x), -1f).normalized;
        RaycastHit2D hit = Physics2D.Raycast(forwardGroundCheckOffset.position, direction, forwardGroundCheckRayDistance, blockLayerMask);

        if (hit.collider != null)
        {
            isGroundedForward = true;
        }
        else
        {
            isGroundedForward = false;
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Vector3 direction = new Vector3(GetFrontDirX() / Mathf.Abs(GetFrontDirX()), -1f).normalized;
        Gizmos.DrawLine(forwardGroundCheckOffset.position, forwardGroundCheckOffset.position + direction * forwardGroundCheckRayDistance);
    }
}
