using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    [SerializeField]
    private float gameDeltaTime = 1f;
    public float GameDeltaTime => gameDeltaTime;

    [SerializeField]
    private PlayerCharacter playerCharacter;
    public PlayerCharacter PlayerCharacter => playerCharacter;

    [SerializeField]
    private PlayerCharacterData playerCharacterData;
    public PlayerCharacterData PlayerCharacterData => playerCharacterData;

    private Canvas mainCanvas;
    public Canvas MainCanvas => mainCanvas;

    public void Initialize(PlayerCharacterData data = null)
    {
        if(data != null)
        {
            playerCharacterData = data;
        }

        playerCharacter = FindAnyObjectByType<PlayerCharacter>();
        mainCanvas = FindAnyObjectByType<Canvas>();
    }
}
