using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod]
    public static void GameInitializer()
    {
        SceneManager.LoadScene("BootStrapScene", LoadSceneMode.Additive);
    }
}
