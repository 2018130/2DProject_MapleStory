using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    private void LateUpdate()
    {
        if(GameManager.Instance.CurrentSceneContext.PlayerCharacter != null)
        {
            Vector3 newPosition = GameManager.Instance.CurrentSceneContext.PlayerCharacter.transform.position + offset;
            newPosition.z = transform.position.z;

            transform.position = newPosition;
        }
    }
}
