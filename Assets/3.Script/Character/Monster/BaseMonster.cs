using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : Character, ISceneContextBuilt
{
    // 몬스터 고정 데이터 입력
    [SerializeField]
    protected CharacterDataSO characterData;

    [SerializeField]
    private float minMoveDirTime = 2f;
    [SerializeField]
    private float maxMoveDirTime = 7f;

    public int Priority { get; set; } = 0;

    public void OnSceneContextBuilt()
    {
        StartCoroutine(Move_co());
    }

    private IEnumerator Move_co()
    {
        yield return null;
    }

    public override void Jump()
    {
        moveDir.y = characterData.statusData.JumpForce;
    }
}
