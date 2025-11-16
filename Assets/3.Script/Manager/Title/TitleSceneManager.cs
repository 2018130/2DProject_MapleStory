using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneChangeManager.Instance.ChangeScene(SceneType.CharacterSelectScene);
    }
}
