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

    public void OnSceneContextBuilt()
    {
        StartCoroutine(Move_co());
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

    public override void Jump()
    {
        moveDir.y = characterData.statusData.JumpForce;
    }
}
