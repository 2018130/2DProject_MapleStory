using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    [SerializeField]
    private float gameDeltaTime = 1f;
    public float GameDeltaTime => gameDeltaTime;

    [SerializeField, Tooltip("Only view data")]
    private PlayerCharacterData playerCharacterData;
    public PlayerCharacterData PlayerCharacterData => playerCharacterData;

    private Canvas mainCanvas;
    public Canvas MainCanvas => mainCanvas;

    public void Initialize(PlayerCharacterData data = null)
    {
        playerCharacterData = data;
        mainCanvas = FindAnyObjectByType<Canvas>();
    }
}
