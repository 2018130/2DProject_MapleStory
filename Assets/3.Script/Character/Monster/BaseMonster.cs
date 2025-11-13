using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : Character
{
    [SerializeField]
    private float minMoveDirTime = 2f;
    [SerializeField]
    private float maxMoveDirTime = 7f;

    private bool isGrounded = true;

    // 임시 sceneContext에서 실행 예정
    private void Start()
    {
        StartCoroutine(Move_co());
    }

    private IEnumerator Move_co()
    {
        float jumpPower = characterData.JumpPower;
        float moveDirectionTime = UnityEngine.Random.Range(minMoveDirTime, maxMoveDirTime);
        float timer = 0f;
        int moveDirX = 1;
        float changeDirLagTime = 2f;

        while (true)
        {
            yield return null;
            //timer += Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
            timer += Time.deltaTime;
            if(moveDirectionTime <= timer && isGrounded)
            {
                moveDirX = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
                timer = 0;

                yield return new WaitForSeconds(changeDirLagTime);
            }

            //float nextPosX = transform.position.x + characterData.MoveSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
            float nextPosX = transform.position.x + moveDirX * characterData.MoveSpeed * Time.deltaTime;
            float nextPosY = 0f;
            if(!Mathf.Approximately(jumpPower, 0))
            {
                isGrounded = false;
                float weight = 200f;
                nextPosY = jumpPower * Mathf.Sin((timer * weight / jumpPower) * (Mathf.PI / 180));

                if(nextPosY <= 0)
                {
                    isGrounded = true;
                    nextPosY = transform.position.y;
                }
            }

            MoveTo(new Vector2(nextPosX, nextPosY));
        }
    }
}
