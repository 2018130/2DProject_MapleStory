using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager :SingletonBehaviour<SceneChangeManager>
{
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeScene_co(sceneName));
    }

    private IEnumerator ChangeScene_co(string sceneName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);

        ao.allowSceneActivation = false;

        while(!ao.isDone)
        {
            yield return null;

            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
        }
    }
}
