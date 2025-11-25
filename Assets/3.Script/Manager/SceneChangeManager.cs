using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    TitleScene,
    CharacterSelectScene,
    CharacterCreateScene,
    GameScene,
    None,
    ResultScene
}

public class SceneChangeManager :SingletonBehaviour<SceneChangeManager>
{
    public void ChangeScene(SceneType sceneType, PlayerCharacterData playerCharacterData = null)
    {
        StartCoroutine(ChangeScene_co(sceneType, playerCharacterData));
    }

    private IEnumerator ChangeScene_co(SceneType sceneType, PlayerCharacterData playerCharacterData)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync((int)sceneType);

        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            yield return null;

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
        }

        GameManager.Instance.Initialize(playerCharacterData);
    }
}
